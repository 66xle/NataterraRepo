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
            // First try the original path.
            // ---------------------------------------------------------

            if (IsPathWalkable(
                    sourcePath))
            {
                result =
                    new List<Vector3>(
                        sourcePath);

                // Match the path to the actual NavMesh height.
                if (!ProjectPathToNavMeshHeight(
                        result))
                {
                    result = null;

                    return false;
                }

                return true;
            }

            // ---------------------------------------------------------
            // Original path is not completely walkable.
            //
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

                // -----------------------------------------------------
                // Invalid segment.
                //
                // Generate a NavMesh fallback between the two points.
                // -----------------------------------------------------

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
            // Validate repaired path.
            // ---------------------------------------------------------

            if (!IsPathWalkable(
                    repaired))
            {
                result = null;

                return false;
            }

            // The repaired path must remain inside the original
            // corridor.
            if (!CorridorPathValidator.IsPathInsideCorridor(
                    repaired,
                    corridor))
            {
                result = null;

                return false;
            }

            // ---------------------------------------------------------
            // Match every point to NavMesh height.
            // ---------------------------------------------------------

            if (!ProjectPathToNavMeshHeight(
                    repaired))
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

            // Every point must correspond to the NavMesh.
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

            // Every segment must be traversable.
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
        /// Samples the NavMesh using:
        ///
        /// XZ <= NavMeshSampleDistance
        /// Y  <= NavMeshHeightTolerance
        ///
        /// Unity's SamplePosition uses a spherical 3D radius, so the
        /// search radius encompasses both tolerances. The resulting
        /// candidate is then explicitly checked using separate XZ/Y
        /// limits.
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

            if (actualXZDistance >
                xzDistance)
            {
                return false;
            }

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
        // NAVMESH HEIGHT
        // =============================================================

        /// <summary>
        /// Changes only the Y coordinate of each point so that it
        /// matches the NavMesh height.
        ///
        /// X and Z are deliberately preserved.
        /// </summary>
        private bool ProjectPathToNavMeshHeight(
            List<Vector3> path)
        {
            if (path == null ||
                path.Count == 0)
            {
                return false;
            }

            for (int i = 0;
                 i < path.Count;
                 i++)
            {
                Vector3 point =
                    path[i];

                if (!TrySampleNavMesh(
                        point,
                        out NavMeshHit hit))
                {
                    return false;
                }

                // Preserve X/Z exactly.
                point.y =
                    hit.position.y;

                path[i] =
                    point;
            }

            return true;
        }

        // =============================================================
        // FALLBACK PATH
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

            // Keep original X/Z for the endpoints.
            // Their Y will be projected to the NavMesh below.
            fallback.Add(
                start);

            // NavMesh corners already contain their NavMesh height.
            for (int i = 1;
                 i < navMeshPath.corners.Length - 1;
                 i++)
            {
                fallback.Add(
                    navMeshPath.corners[i]);
            }

            fallback.Add(
                end);

            // Ensure all points use the actual NavMesh height.
            if (!ProjectPathToNavMeshHeight(
                    fallback))
            {
                return false;
            }

            // ---------------------------------------------------------
            // Apply edge avoidance.
            // ---------------------------------------------------------

            ApplyEdgeAvoidance(
                fallback);

            // Edge avoidance changes X/Z, so sample the NavMesh again
            // to make sure every modified point has the correct height.
            if (!ProjectPathToNavMeshHeight(
                    fallback))
            {
                return false;
            }

            // ---------------------------------------------------------
            // Fallback must remain inside the original corridor.
            // Corridor validation is intentionally XZ-only.
            // ---------------------------------------------------------

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    fallback,
                    corridor))
            {
                return false;
            }

            // ---------------------------------------------------------
            // Fallback must still be walkable.
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