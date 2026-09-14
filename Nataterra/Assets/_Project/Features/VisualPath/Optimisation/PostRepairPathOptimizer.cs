using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace VisualPath
{
    public sealed class PostRepairPathOptimizer
    {
        private readonly VisualPathSettings settings;

        private const float PointTolerance = 0.001f;

        public PostRepairPathOptimizer(
            VisualPathSettings settings)
        {
            this.settings =
                settings;
        }

        // =============================================================
        // OPTIMIZE
        // =============================================================

        public bool TryOptimize(
            IReadOnlyList<Vector3> sourcePath,
            IReadOnlyList<VisualHex> corridor,
            out List<Vector3> result)
        {
            result = null;

            if (sourcePath == null ||
                sourcePath.Count < 2)
            {
                return false;
            }

            if (corridor == null ||
                corridor.Count == 0)
            {
                return false;
            }

            // ---------------------------------------------------------
            // Start with a copy so the original repaired path is not
            // modified if optimization fails.
            // ---------------------------------------------------------

            var optimized =
                new List<Vector3>(
                    sourcePath.Count);

            for (int i = 0;
                 i < sourcePath.Count;
                 i++)
            {
                AddUnique(
                    optimized,
                    sourcePath[i]);
            }

            if (optimized.Count < 2)
            {
                return false;
            }

            // =========================================================
            // REMOVE REDUNDANT / REPAIR POINTS
            // =========================================================
            //
            // For every point, look as far ahead as possible and find
            // the furthest point that can be reached directly.
            //
            // This is intentionally more aggressive than simply
            // removing points where three consecutive points are
            // collinear.
            //
            // Example:
            //
            // A -> N1 -> N2 -> N3 -> N4 -> B
            //
            // If A -> N4 is both corridor-valid and NavMesh-valid,
            // the intermediate points are unnecessary.
            // =========================================================

            bool changed = true;

            while (changed)
            {
                changed = false;

                if (optimized.Count <= 2)
                {
                    break;
                }

                for (int i = 0;
                     i < optimized.Count - 2;
                     i++)
                {
                    int bestTarget =
                        FindFurthestReachablePoint(
                            optimized,
                            i,
                            corridor);

                    if (bestTarget <= i + 1)
                    {
                        continue;
                    }

                    // -------------------------------------------------
                    // Remove everything between the current point and
                    // the furthest directly reachable point.
                    // -------------------------------------------------

                    int removeCount =
                        bestTarget - i - 1;

                    optimized.RemoveRange(
                        i + 1,
                        removeCount);

                    changed = true;

                    break;
                }
            }

            // =========================================================
            // FINAL VALIDATION
            // =========================================================

            if (optimized.Count < 2)
            {
                return false;
            }

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    optimized,
                    corridor))
            {
                return false;
            }

            if (!IsPathWalkable(
                    optimized))
            {
                return false;
            }

            result =
                optimized;

            return true;
        }

        // =============================================================
        // FIND FURTHEST REACHABLE POINT
        // =============================================================

        private int FindFurthestReachablePoint(
            IReadOnlyList<Vector3> path,
            int startIndex,
            IReadOnlyList<VisualHex> corridor)
        {
            int furthest =
                startIndex + 1;

            // ---------------------------------------------------------
            // Search backwards from the furthest possible point.
            //
            // This means we always prefer the longest valid shortcut.
            // ---------------------------------------------------------

            for (int targetIndex =
                     path.Count - 1;
                 targetIndex > startIndex + 1;
                 targetIndex--)
            {
                Vector3 start =
                    path[startIndex];

                Vector3 target =
                    path[targetIndex];

                if (!IsSegmentWalkable(
                        start,
                        target))
                {
                    continue;
                }

                if (!IsSegmentInsideCorridor(
                        start,
                        target,
                        corridor))
                {
                    continue;
                }

                furthest =
                    targetIndex;

                break;
            }

            return furthest;
        }

        // =============================================================
        // SEGMENT CORRIDOR VALIDATION
        // =============================================================

        private bool IsSegmentInsideCorridor(
            Vector3 start,
            Vector3 end,
            IReadOnlyList<VisualHex> corridor)
        {
            if (corridor == null ||
                corridor.Count == 0)
            {
                return false;
            }

            // ---------------------------------------------------------
            // The existing validator is the authoritative corridor
            // test. Use a two-point path so this segment is tested as
            // an actual path segment rather than only testing its
            // endpoints.
            // ---------------------------------------------------------

            var segment =
                new List<Vector3>(2)
                {
                    start,
                    end
                };

            return
                CorridorPathValidator.IsPathInsideCorridor(
                    segment,
                    corridor);
        }

        // =============================================================
        // PATH WALKABILITY
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

        // =============================================================
        // SEGMENT WALKABILITY
        // =============================================================

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

        // =============================================================
        // NAVMESH SAMPLE
        // =============================================================

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
                    xzDistance *
                    xzDistance +
                    heightTolerance *
                    heightTolerance);

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
        // ADD UNIQUE
        // =============================================================

        private static void AddUnique(
            List<Vector3> path,
            Vector3 point)
        {
            if (path.Count == 0)
            {
                path.Add(
                    point);

                return;
            }

            if (PathMath.DistanceXZ(
                    path[path.Count - 1],
                    point) >
                PointTolerance)
            {
                path.Add(
                    point);
            }
        }
    }
}