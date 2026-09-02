using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VisualPath
{
    public sealed class NavMeshPathProcessor
    {
        private readonly VisualPathSettings settings;

        public NavMeshPathProcessor(
            VisualPathSettings settings)
        {
            this.settings = settings;
        }

        public bool TryValidateAndRepairPath(
            IReadOnlyList<Vector3> sourcePath,
            IReadOnlyList<VisualHex> corridor,
            out List<Vector3> result,
            out bool usedFallback)
        {
            result = null;

            usedFallback = false;

            if (sourcePath == null ||
                sourcePath.Count < 2)
            {
                return false;
            }

            if (!settings.UseNavMesh)
            {
                result =
                    new List<Vector3>(
                        sourcePath);

                return true;
            }

            // ---------------------------------------------------------
            // Validate the complete path first.
            // ---------------------------------------------------------

            if (IsPathWalkable(
                    sourcePath))
            {
                result =
                    new List<Vector3>(
                        sourcePath);

                return true;
            }

            // ---------------------------------------------------------
            // Repair invalid segments individually.
            // ---------------------------------------------------------

            var repaired =
                new List<Vector3>();

            repaired.Add(
                sourcePath[0]);

            for (int i = 0;
                 i < sourcePath.Count - 1;
                 i++)
            {
                Vector3 start =
                    sourcePath[i];

                Vector3 end =
                    sourcePath[i + 1];

                if (IsSegmentWalkable(
                        start,
                        end))
                {
                    AddUnique(
                        repaired,
                        end);

                    continue;
                }

                if (!TryBuildFallback(
                        start,
                        end,
                        corridor,
                        out List<Vector3> fallback))
                {
                    result = null;

                    return false;
                }

                usedFallback = true;

                for (int j = 1;
                     j < fallback.Count;
                     j++)
                {
                    AddUnique(
                        repaired,
                        fallback[j]);
                }
            }

            // ---------------------------------------------------------
            // Final validation.
            // ---------------------------------------------------------

            if (!IsPathWalkable(
                    repaired))
            {
                result = null;

                return false;
            }

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    repaired,
                    corridor))
            {
                result = null;

                return false;
            }

            result =
                repaired;

            return true;
        }

        // =============================================================
        // NAVMESH VALIDATION
        // =============================================================

        private bool IsPathWalkable(
            IReadOnlyList<Vector3> path)
        {
            if (path == null ||
                path.Count < 2)
            {
                return false;
            }

            for (int i = 0;
                 i < path.Count;
                 i++)
            {
                if (!TrySampleNavMesh(
                        path[i],
                        out _))
                {
                    return false;
                }
            }

            for (int i = 0;
                 i < path.Count - 1;
                 i++)
            {
                if (!IsSegmentWalkable(
                        path[i],
                        path[i + 1]))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsSegmentWalkable(
            Vector3 start,
            Vector3 end)
        {
            if (!TrySampleNavMesh(
                    start,
                    out NavMeshHit startHit))
            {
                return false;
            }

            if (!TrySampleNavMesh(
                    end,
                    out NavMeshHit endHit))
            {
                return false;
            }

            return !NavMesh.Raycast(
                startHit.position,
                endHit.position,
                out _,
                settings.NavMeshAreaMask);
        }

        /// <summary>
        /// Finds a NavMesh position using an XZ radius and an
        /// independent Y tolerance.
        ///
        /// Unity's SamplePosition uses a 3D radius internally, so we
        /// search using a radius large enough to encompass the desired
        /// XZ/Y cylinder and then explicitly reject candidates that
        /// fall outside either constraint.
        /// </summary>
        private bool TrySampleNavMesh(
            Vector3 position,
            out NavMeshHit hit)
        {
            hit = default;

            float xzDistance =
                Mathf.Max(
                    0.01f,
                    settings.NavMeshSampleDistance);

            float heightTolerance =
                Mathf.Max(
                    0.01f,
                    settings.NavMeshHeightTolerance);

            // The cylinder defined by:
            //
            // XZ <= xzDistance
            // Y  <= heightTolerance
            //
            // is contained inside a sphere whose radius is:
            //
            // sqrt(xzDistance^2 + heightTolerance^2)
            //
            // This ensures SamplePosition does not miss a valid point
            // simply because its vertical difference is larger than
            // the XZ radius.
            float searchRadius =
                Mathf.Sqrt(
                    xzDistance * xzDistance +
                    heightTolerance * heightTolerance);

            if (!NavMesh.SamplePosition(
                    position,
                    out NavMeshHit candidate,
                    searchRadius,
                    settings.NavMeshAreaMask))
            {
                return false;
            }

            float actualXZDistance =
                PathMath.DistanceXZ(
                    position,
                    candidate.position);

            float actualHeightDifference =
                Mathf.Abs(
                    position.y -
                    candidate.position.y);

            // ---------------------------------------------------------
            // XZ constraint
            // ---------------------------------------------------------

            if (actualXZDistance >
                xzDistance)
            {
                return false;
            }

            // ---------------------------------------------------------
            // Y constraint
            // ---------------------------------------------------------

            if (actualHeightDifference >
                heightTolerance)
            {
                return false;
            }

            hit =
                candidate;

            return true;
        }

        // =============================================================
        // FALLBACK
        // =============================================================

        private bool TryBuildFallback(
            Vector3 start,
            Vector3 end,
            IReadOnlyList<VisualHex> corridor,
            out List<Vector3> result)
        {
            result = null;

            if (!TrySampleNavMesh(
                    start,
                    out NavMeshHit startHit))
            {
                return false;
            }

            if (!TrySampleNavMesh(
                    end,
                    out NavMeshHit endHit))
            {
                return false;
            }

            var navMeshPath =
                new NavMeshPath();

            bool calculated =
                NavMesh.CalculatePath(
                    startHit.position,
                    endHit.position,
                    settings.NavMeshAreaMask,
                    navMeshPath);

            if (!calculated ||
                navMeshPath.status !=
                NavMeshPathStatus.PathComplete ||
                navMeshPath.corners == null ||
                navMeshPath.corners.Length < 2)
            {
                return false;
            }

            var fallback =
                new List<Vector3>(
                    navMeshPath.corners.Length);

            // ---------------------------------------------------------
            // Preserve the original visual path height.
            // ---------------------------------------------------------

            fallback.Add(
                start);

            for (int i = 1;
                 i < navMeshPath.corners.Length - 1;
                 i++)
            {
                Vector3 corner =
                    navMeshPath.corners[i];

                corner.y =
                    GetInterpolatedHeight(
                        start,
                        end,
                        corner);

                fallback.Add(
                    corner);
            }

            fallback.Add(
                end);

            // ---------------------------------------------------------
            // Apply NavMesh edge avoidance.
            // ---------------------------------------------------------

            ApplyEdgeAvoidance(
                fallback);

            // ---------------------------------------------------------
            // Fallback must remain inside corridor.
            // ---------------------------------------------------------

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    fallback,
                    corridor))
            {
                return false;
            }

            // ---------------------------------------------------------
            // Edge avoidance must not make the path unwalkable.
            // ---------------------------------------------------------

            if (!IsPathWalkable(
                    fallback))
            {
                return false;
            }

            result =
                fallback;

            return true;
        }

        // =============================================================
        // EDGE AVOIDANCE
        // =============================================================

        private void ApplyEdgeAvoidance(
            List<Vector3> path)
        {
            if (path == null ||
                path.Count <= 2)
            {
                return;
            }

            float strength =
                Mathf.Clamp01(
                    settings.NavMeshEdgeAvoidanceStrength);

            float desiredDistance =
                Mathf.Max(
                    0f,
                    settings.NavMeshEdgeAvoidanceDistance);

            int iterations =
                Mathf.Max(
                    0,
                    settings.NavMeshEdgeAvoidanceIterations);

            if (strength <= 0f ||
                desiredDistance <= 0f ||
                iterations == 0)
            {
                return;
            }

            for (int iteration = 0;
                 iteration < iterations;
                 iteration++)
            {
                for (int i = 1;
                     i < path.Count - 1;
                     i++)
                {
                    Vector3 original =
                        path[i];

                    if (!NavMesh.FindClosestEdge(
                            original,
                            out NavMeshHit edgeHit,
                            settings.NavMeshAreaMask))
                    {
                        continue;
                    }

                    if (edgeHit.distance >=
                        desiredDistance)
                    {
                        continue;
                    }

                    float requiredOffset =
                        desiredDistance -
                        edgeHit.distance;

                    float offset =
                        requiredOffset *
                        strength;

                    if (offset <= 0.001f)
                    {
                        continue;
                    }

                    Vector3 candidateA =
                        original +
                        edgeHit.normal *
                        offset;

                    Vector3 candidateB =
                        original -
                        edgeHit.normal *
                        offset;

                    candidateA.y =
                        original.y;

                    candidateB.y =
                        original.y;

                    bool validA =
                        IsPointWalkable(
                            candidateA);

                    bool validB =
                        IsPointWalkable(
                            candidateB);

                    Vector3 candidate =
                        original;

                    float bestDistance =
                        edgeHit.distance;

                    if (validA)
                    {
                        float distanceA =
                            GetEdgeDistance(
                                candidateA);

                        if (distanceA >
                            bestDistance)
                        {
                            candidate =
                                candidateA;

                            bestDistance =
                                distanceA;
                        }
                    }

                    if (validB)
                    {
                        float distanceB =
                            GetEdgeDistance(
                                candidateB);

                        if (distanceB >
                            bestDistance)
                        {
                            candidate =
                                candidateB;

                            bestDistance =
                                distanceB;
                        }
                    }

                    if (candidate != original)
                    {
                        path[i] =
                            candidate;
                    }
                }
            }
        }

        private bool IsPointWalkable(
            Vector3 point)
        {
            return TrySampleNavMesh(
                point,
                out _);
        }

        private float GetEdgeDistance(
            Vector3 point)
        {
            if (!NavMesh.FindClosestEdge(
                    point,
                    out NavMeshHit hit,
                    settings.NavMeshAreaMask))
            {
                return 0f;
            }

            return hit.distance;
        }

        // =============================================================
        // HEIGHT
        // =============================================================

        private float GetInterpolatedHeight(
            Vector3 start,
            Vector3 end,
            Vector3 point)
        {
            Vector3 direction =
                end - start;

            direction.y = 0f;

            float lengthSq =
                direction.sqrMagnitude;

            if (lengthSq <=
                Mathf.Epsilon)
            {
                return Mathf.Lerp(
                    start.y,
                    end.y,
                    0.5f);
            }

            float t =
                Vector3.Dot(
                    new Vector3(
                        point.x - start.x,
                        0f,
                        point.z - start.z),
                    direction)
                /
                lengthSq;

            t =
                Mathf.Clamp01(t);

            return Mathf.Lerp(
                start.y,
                end.y,
                t);
        }

        // =============================================================
        // UTILITY
        // =============================================================

        private static void AddUnique(
            List<Vector3> path,
            Vector3 point)
        {
            if (path.Count == 0)
            {
                path.Add(point);

                return;
            }

            if (PathMath.DistanceXZ(
                    path[path.Count - 1],
                    point) >
                0.001f)
            {
                path.Add(point);
            }
        }
    }
}