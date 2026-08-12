using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class RoadPathProcessor
    {
        private readonly VisualPathSettings settings;

        private const float PositionTolerance = 0.001f;

        public RoadPathProcessor(
            VisualPathSettings settings)
        {
            this.settings = settings;
        }

        // ============================================================
        // ROAD SELECTION
        // ============================================================

        /// <summary>
        /// Finds the road with the longest continuous section
        /// inside the requested corridor.
        ///
        /// Example:
        ///
        /// Corridor:
        /// A -> B -> C -> D -> E
        ///
        /// Road:
        /// A -> B -> C -> E
        ///
        /// Valid continuous section:
        /// A -> B -> C
        ///
        /// The road therefore covers the A/B/C section.
        /// </summary>
        public VisualRoad FindBestRoad(
            IReadOnlyList<VisualHex> corridor,
            IReadOnlyList<VisualRoad> roads)
        {
            if (corridor == null ||
                corridor.Count < 2 ||
                roads == null ||
                roads.Count == 0)
            {
                return null;
            }

            VisualRoad bestRoad = null;

            int bestSectionLength = 0;

            foreach (VisualRoad road in roads)
            {
                if (!IsValidRoad(road))
                    continue;

                if (!TryFindBestRoadSection(
                        corridor,
                        road,
                        out RoadSection section))
                {
                    continue;
                }

                int sectionLength =
                    section.EndHexIndex -
                    section.StartHexIndex +
                    1;

                /*
                 * Prefer the road that covers the largest
                 * continuous portion of the corridor.
                 */
                if (sectionLength > bestSectionLength)
                {
                    bestSectionLength =
                        sectionLength;

                    bestRoad =
                        road;
                }
            }

            return bestRoad;
        }

        // ============================================================
        // BUILD ROAD PATH
        // ============================================================

        public bool TryBuildRoadPath(
            VisualRoad road,
            Vector3 start,
            Vector3 end,
            IReadOnlyList<VisualHex> corridor,
            IReadOnlyList<Vector3> optimizedPath,
            IReadOnlyList<int> optimizedPortalIndices,
            IReadOnlyList<PathPortal> portals,
            out RoadPathResult result)
        {
            result = null;

            if (!IsValidRoad(road))
                return false;

            if (corridor == null ||
                corridor.Count < 2)
            {
                return false;
            }

            if (optimizedPath == null ||
                optimizedPath.Count < 2)
            {
                return false;
            }

            if (optimizedPortalIndices == null ||
                optimizedPortalIndices.Count !=
                optimizedPath.Count)
            {
                return false;
            }

            if (portals == null ||
                portals.Count == 0)
            {
                return false;
            }

            if (!TryFindBestRoadSection(
                    corridor,
                    road,
                    out RoadSection section))
            {
                return false;
            }

            /*
             * A road section containing N hexes crosses
             * N - 1 corridor portals.
             *
             * Example:
             *
             * A -> B -> C
             *
             * portals:
             * A/B = 0
             * B/C = 1
             */
            int firstPortal =
                section.StartHexIndex;

            int lastPortal =
                section.EndHexIndex - 1;

            if (firstPortal < 0 ||
                lastPortal < firstPortal ||
                lastPortal >= portals.Count)
            {
                return false;
            }

            if (!TryFindRoadPointRange(
                    road,
                    corridor,
                    section,
                    out int firstRoadPoint,
                    out int lastRoadPoint))
            {
                return false;
            }

            /*
             * Find the normal visual path point immediately
             * before the road begins.
             */
            Vector3 entryTarget =
                FindEntryTarget(
                    optimizedPath,
                    optimizedPortalIndices,
                    firstPortal,
                    start);

            /*
             * Find the normal visual path point immediately
             * after the road ends.
             *
             * IMPORTANT:
             *
             * This is the portal-solver result and therefore
             * already respects the normal edge avoidance rules.
             */
            Vector3 exitTarget =
                FindExitTarget(
                    optimizedPath,
                    optimizedPortalIndices,
                    lastPortal,
                    end);

            /*
             * Evaluate both possible directions along the
             * supplied road.
             *
             * Forward:
             *
             * road[0] -> road[1] -> road[2] ...
             *
             * Reverse:
             *
             * road[last] -> road[last - 1] -> ...
             */
            RoadTraversal forward =
                FindForwardTraversal(
                    road,
                    firstRoadPoint,
                    lastRoadPoint,
                    entryTarget,
                    exitTarget);

            RoadTraversal reverse =
                FindReverseTraversal(
                    road,
                    firstRoadPoint,
                    lastRoadPoint,
                    entryTarget,
                    exitTarget);

            bool forwardValid =
                forward.IsValid;

            bool reverseValid =
                reverse.IsValid;

            if (!forwardValid &&
                !reverseValid)
            {
                return false;
            }

            RoadTraversal selected;

            if (forwardValid &&
                reverseValid)
            {
                selected =
                    forward.TotalCost <=
                    reverse.TotalCost
                        ? forward
                        : reverse;
            }
            else
            {
                selected =
                    forwardValid
                        ? forward
                        : reverse;
            }

            if (selected.IsReverse)
            {
                return BuildReverseRoadPath(
                    road,
                    start,
                    end,
                    optimizedPath,
                    optimizedPortalIndices,
                    firstPortal,
                    lastPortal,
                    selected.Entry,
                    selected.Exit,
                    out result);
            }

            return BuildForwardRoadPath(
                road,
                start,
                end,
                optimizedPath,
                optimizedPortalIndices,
                firstPortal,
                lastPortal,
                selected.Entry,
                selected.Exit,
                out result);
        }

        // ============================================================
        // ROAD SECTION
        // ============================================================

        private bool TryFindBestRoadSection(
            IReadOnlyList<VisualHex> corridor,
            VisualRoad road,
            out RoadSection section)
        {
            section = default;

            int bestStart = -1;
            int bestEnd = -1;
            int bestLength = 0;

            int currentStart = -1;
            int currentLength = 0;

            for (int i = 0;
                 i < corridor.Count;
                 i++)
            {
                int hexId =
                    corridor[i].Id;

                bool covered =
                    road.HexIds.Contains(hexId);

                /*
                 * The road must also actually have a road point
                 * associated with this hex.
                 */
                if (covered &&
                    HasRoadPointInHex(
                        road,
                        hexId))
                {
                    if (currentStart < 0)
                        currentStart = i;

                    currentLength++;

                    if (currentLength > bestLength)
                    {
                        bestLength =
                            currentLength;

                        bestStart =
                            currentStart;

                        bestEnd =
                            i;
                    }
                }
                else
                {
                    currentStart = -1;
                    currentLength = 0;
                }
            }

            if (bestStart < 0)
                return false;

            /*
             * We need at least two hexes to form an actual
             * road section.
             */
            if (bestEnd <= bestStart)
                return false;

            section =
                new RoadSection(
                    bestStart,
                    bestEnd);

            return true;
        }

        private bool HasRoadPointInHex(
            VisualRoad road,
            int hexId)
        {
            for (int i = 0;
                 i < road.PointHexIds.Count;
                 i++)
            {
                if (road.PointHexIds[i] == hexId)
                    return true;
            }

            return false;
        }

        private bool TryFindRoadPointRange(
            VisualRoad road,
            IReadOnlyList<VisualHex> corridor,
            RoadSection section,
            out int firstRoadPoint,
            out int lastRoadPoint)
        {
            firstRoadPoint = -1;
            lastRoadPoint = -1;

            for (int i = 0;
                 i < road.PointHexIds.Count;
                 i++)
            {
                int pointHex =
                    road.PointHexIds[i];

                if (!IsHexInsideSection(
                        corridor,
                        pointHex,
                        section.StartHexIndex,
                        section.EndHexIndex))
                {
                    continue;
                }

                if (firstRoadPoint < 0)
                    firstRoadPoint = i;

                lastRoadPoint = i;
            }

            return
                firstRoadPoint >= 0 &&
                lastRoadPoint >= firstRoadPoint;
        }

        private bool IsHexInsideSection(
            IReadOnlyList<VisualHex> corridor,
            int hexId,
            int start,
            int end)
        {
            for (int i = start;
                 i <= end;
                 i++)
            {
                if (corridor[i].Id == hexId)
                    return true;
            }

            return false;
        }

        // ============================================================
        // FORWARD TRAVERSAL
        // ============================================================

        private RoadTraversal FindForwardTraversal(
            VisualRoad road,
            int firstRoadPoint,
            int lastRoadPoint,
            Vector3 entryTarget,
            Vector3 exitTarget)
        {
            RoadPoint entry =
                FindBestEntryPointForward(
                    road,
                    firstRoadPoint,
                    entryTarget);

            if (!entry.IsValid)
                return default;

            RoadPoint exit =
                FindBestExitPointForward(
                    road,
                    entry,
                    lastRoadPoint,
                    exitTarget);

            if (!exit.IsValid)
                return default;

            if (exit.DistanceAlongRoad <=
                entry.DistanceAlongRoad)
            {
                return default;
            }

            float roadDistance =
                exit.DistanceAlongRoad -
                entry.DistanceAlongRoad;

            float transitionDistance =
                PathMath.DistanceXZ(
                    exit.Position,
                    exitTarget);

            /*
             * The exit decision is based on:
             *
             * distance travelled along the road
             * +
             * distance from the road exit to the next
             * normal path point.
             */
            float totalCost =
                roadDistance +
                transitionDistance;

            return new RoadTraversal(
                entry,
                exit,
                totalCost,
                false);
        }

        // ============================================================
        // REVERSE TRAVERSAL
        // ============================================================

        private RoadTraversal FindReverseTraversal(
            VisualRoad road,
            int firstRoadPoint,
            int lastRoadPoint,
            Vector3 entryTarget,
            Vector3 exitTarget)
        {
            RoadPoint entry =
                FindBestEntryPointReverse(
                    road,
                    lastRoadPoint,
                    entryTarget);

            if (!entry.IsValid)
                return default;

            RoadPoint exit =
                FindBestExitPointReverse(
                    road,
                    entry,
                    firstRoadPoint,
                    exitTarget);

            if (!exit.IsValid)
                return default;

            if (exit.DistanceAlongRoad >=
                entry.DistanceAlongRoad)
            {
                return default;
            }

            float roadDistance =
                entry.DistanceAlongRoad -
                exit.DistanceAlongRoad;

            float transitionDistance =
                PathMath.DistanceXZ(
                    exit.Position,
                    exitTarget);

            float totalCost =
                roadDistance +
                transitionDistance;

            return new RoadTraversal(
                entry,
                exit,
                totalCost,
                true);
        }

        // ============================================================
        // FORWARD ENTRY
        // ============================================================

        private RoadPoint FindBestEntryPointForward(
            VisualRoad road,
            int firstRoadPoint,
            Vector3 target)
        {
            /*
             * The entry must occur inside the first road hex.
             *
             * Start by considering road points in that hex.
             */
            RoadPoint best =
                default;

            float bestCost =
                float.MaxValue;

            int entryHex =
                road.PointHexIds[firstRoadPoint];

            for (int i = 0;
                 i < road.Points.Count;
                 i++)
            {
                if (road.PointHexIds[i] != entryHex)
                    continue;

                float distance =
                    PathMath.DistanceXZ(
                        road.Points[i],
                        target);

                if (distance < bestCost)
                {
                    bestCost = distance;

                    best =
                        new RoadPoint(
                            road.Points[i],
                            Mathf.Clamp(
                                i,
                                0,
                                road.Points.Count - 2),
                            CalculateDistanceAlongRoad(
                                road,
                                i));
                }
            }

            /*
             * Also test segments where both endpoints are
             * inside the entry hex.
             *
             * This gives us a continuous entry position rather
             * than forcing the transition to a road point.
             */
            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                if (road.PointHexIds[i] != entryHex ||
                    road.PointHexIds[i + 1] != entryHex)
                {
                    continue;
                }

                RoadPoint candidate =
                    FindBestPointOnSegment(
                        road,
                        i,
                        target);

                if (!candidate.IsValid)
                    continue;

                float distance =
                    PathMath.DistanceXZ(
                        candidate.Position,
                        target);

                if (distance < bestCost)
                {
                    bestCost = distance;
                    best = candidate;
                }
            }

            return best;
        }

        // ============================================================
        // REVERSE ENTRY
        // ============================================================

        private RoadPoint FindBestEntryPointReverse(
            VisualRoad road,
            int lastRoadPoint,
            Vector3 target)
        {
            int entryHex =
                road.PointHexIds[lastRoadPoint];

            RoadPoint best =
                default;

            float bestCost =
                float.MaxValue;

            for (int i = 0;
                 i < road.Points.Count;
                 i++)
            {
                if (road.PointHexIds[i] != entryHex)
                    continue;

                float distance =
                    PathMath.DistanceXZ(
                        road.Points[i],
                        target);

                if (distance < bestCost)
                {
                    bestCost = distance;

                    best =
                        new RoadPoint(
                            road.Points[i],
                            Mathf.Clamp(
                                i,
                                0,
                                road.Points.Count - 2),
                            CalculateDistanceAlongRoad(
                                road,
                                i));
                }
            }

            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                if (road.PointHexIds[i] != entryHex ||
                    road.PointHexIds[i + 1] != entryHex)
                {
                    continue;
                }

                RoadPoint candidate =
                    FindBestPointOnSegment(
                        road,
                        i,
                        target);

                if (!candidate.IsValid)
                    continue;

                float distance =
                    PathMath.DistanceXZ(
                        candidate.Position,
                        target);

                if (distance < bestCost)
                {
                    bestCost = distance;
                    best = candidate;
                }
            }

            return best;
        }

        // ============================================================
        // FORWARD EXIT
        // ============================================================

        private RoadPoint FindBestExitPointForward(
            VisualRoad road,
            RoadPoint entry,
            int lastRoadPoint,
            Vector3 target)
        {
            int exitHex =
                road.PointHexIds[lastRoadPoint];

            RoadPoint best =
                default;

            float bestCost =
                float.MaxValue;

            /*
             * --------------------------------------------------------
             * Test every road point inside the final road hex.
             * --------------------------------------------------------
             *
             * This is the important part.
             *
             * We do NOT simply find the road point closest to
             * the portal.
             *
             * Instead we calculate:
             *
             * road distance from ENTRY
             * +
             * distance from candidate to next normal path point.
             *
             * This allows a farther point such as RoadC to win.
             */
            for (int i = 0;
                 i < road.Points.Count;
                 i++)
            {
                if (road.PointHexIds[i] != exitHex)
                    continue;

                float distanceAlongRoad =
                    CalculateDistanceAlongRoad(
                        road,
                        i);

                if (distanceAlongRoad <=
                    entry.DistanceAlongRoad)
                {
                    continue;
                }

                float roadDistance =
                    distanceAlongRoad -
                    entry.DistanceAlongRoad;

                float transitionDistance =
                    PathMath.DistanceXZ(
                        road.Points[i],
                        target);

                float cost =
                    roadDistance +
                    transitionDistance;

                if (cost < bestCost)
                {
                    bestCost = cost;

                    best =
                        new RoadPoint(
                            road.Points[i],
                            Mathf.Clamp(
                                i,
                                0,
                                road.Points.Count - 2),
                            distanceAlongRoad);
                }
            }

            /*
             * --------------------------------------------------------
             * Test segments completely contained in the final hex.
             * --------------------------------------------------------
             *
             * This allows the exit to occur between road points.
             */
            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                if (road.PointHexIds[i] != exitHex ||
                    road.PointHexIds[i + 1] != exitHex)
                {
                    continue;
                }

                float segmentStartDistance =
                    CalculateDistanceAlongRoad(
                        road,
                        i);

                float segmentLength =
                    PathMath.DistanceXZ(
                        road.Points[i],
                        road.Points[i + 1]);

                if (segmentLength <=
                    Mathf.Epsilon)
                {
                    continue;
                }

                /*
                 * Minimise:
                 *
                 * entry -> candidate along road
                 * +
                 * candidate -> target
                 *
                 * along this segment.
                 *
                 * A small one-dimensional search is enough
                 * because the candidate lies on one straight
                 * road segment.
                 */
                RoadPoint candidate =
                    FindBestExitOnSegment(
                        road,
                        i,
                        entry.DistanceAlongRoad,
                        target);

                if (!candidate.IsValid)
                    continue;

                if (candidate.DistanceAlongRoad <=
                    entry.DistanceAlongRoad)
                {
                    continue;
                }

                float roadDistance =
                    candidate.DistanceAlongRoad -
                    entry.DistanceAlongRoad;

                float transitionDistance =
                    PathMath.DistanceXZ(
                        candidate.Position,
                        target);

                float cost =
                    roadDistance +
                    transitionDistance;

                if (cost < bestCost)
                {
                    bestCost = cost;
                    best = candidate;
                }
            }

            return best;
        }

        // ============================================================
        // REVERSE EXIT
        // ============================================================

        private RoadPoint FindBestExitPointReverse(
            VisualRoad road,
            RoadPoint entry,
            int firstRoadPoint,
            Vector3 target)
        {
            int exitHex =
                road.PointHexIds[firstRoadPoint];

            RoadPoint best =
                default;

            float bestCost =
                float.MaxValue;

            for (int i = 0;
                 i < road.Points.Count;
                 i++)
            {
                if (road.PointHexIds[i] != exitHex)
                    continue;

                float distanceAlongRoad =
                    CalculateDistanceAlongRoad(
                        road,
                        i);

                if (distanceAlongRoad >=
                    entry.DistanceAlongRoad)
                {
                    continue;
                }

                float roadDistance =
                    entry.DistanceAlongRoad -
                    distanceAlongRoad;

                float transitionDistance =
                    PathMath.DistanceXZ(
                        road.Points[i],
                        target);

                float cost =
                    roadDistance +
                    transitionDistance;

                if (cost < bestCost)
                {
                    bestCost = cost;

                    best =
                        new RoadPoint(
                            road.Points[i],
                            Mathf.Clamp(
                                i,
                                0,
                                road.Points.Count - 2),
                            distanceAlongRoad);
                }
            }

            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                if (road.PointHexIds[i] != exitHex ||
                    road.PointHexIds[i + 1] != exitHex)
                {
                    continue;
                }

                RoadPoint candidate =
                    FindBestExitOnSegment(
                        road,
                        i,
                        entry.DistanceAlongRoad,
                        target);

                if (!candidate.IsValid)
                    continue;

                if (candidate.DistanceAlongRoad >=
                    entry.DistanceAlongRoad)
                {
                    continue;
                }

                float roadDistance =
                    entry.DistanceAlongRoad -
                    candidate.DistanceAlongRoad;

                float transitionDistance =
                    PathMath.DistanceXZ(
                        candidate.Position,
                        target);

                float cost =
                    roadDistance +
                    transitionDistance;

                if (cost < bestCost)
                {
                    bestCost = cost;
                    best = candidate;
                }
            }

            return best;
        }

        // ============================================================
        // EXIT SEGMENT OPTIMISATION
        // ============================================================

        private RoadPoint FindBestExitOnSegment(
            VisualRoad road,
            int segmentIndex,
            float entryDistance,
            Vector3 target)
        {
            Vector3 a =
                road.Points[segmentIndex];

            Vector3 b =
                road.Points[segmentIndex + 1];

            float segmentLength =
                PathMath.DistanceXZ(a, b);

            if (segmentLength <=
                Mathf.Epsilon)
            {
                return default;
            }

            float segmentStartDistance =
                CalculateDistanceAlongRoad(
                    road,
                    segmentIndex);

            /*
             * We only want positions after the entry point.
             */
            float minimumT =
                Mathf.Clamp01(
                    (entryDistance -
                     segmentStartDistance) /
                    segmentLength);

            /*
             * The cost along this segment is:
             *
             * |candidate - entry| along road
             * +
             * distance(candidate, target)
             *
             * Since the first term is linear and the second
             * is smooth, a golden-section search works well.
             */
            float low =
                minimumT;

            float high =
                1f;

            for (int iteration = 0;
                 iteration < 24;
                 iteration++)
            {
                float first =
                    Mathf.Lerp(
                        low,
                        high,
                        0.382f);

                float second =
                    Mathf.Lerp(
                        low,
                        high,
                        0.618f);

                float firstCost =
                    CalculateExitSegmentCost(
                        a,
                        b,
                        segmentStartDistance,
                        segmentLength,
                        entryDistance,
                        target,
                        first);

                float secondCost =
                    CalculateExitSegmentCost(
                        a,
                        b,
                        segmentStartDistance,
                        segmentLength,
                        entryDistance,
                        target,
                        second);

                if (firstCost <
                    secondCost)
                {
                    high = second;
                }
                else
                {
                    low = first;
                }
            }

            float bestT =
                (low + high) * 0.5f;

            Vector3 bestPosition =
                Vector3.Lerp(
                    a,
                    b,
                    bestT);

            float bestDistance =
                segmentStartDistance +
                segmentLength * bestT;

            return new RoadPoint(
                bestPosition,
                segmentIndex,
                bestDistance);
        }

        private float CalculateExitSegmentCost(
            Vector3 a,
            Vector3 b,
            float segmentStartDistance,
            float segmentLength,
            float entryDistance,
            Vector3 target,
            float t)
        {
            Vector3 point =
                Vector3.Lerp(a, b, t);

            float distanceAlongRoad =
                segmentStartDistance +
                segmentLength * t;

            float roadDistance =
                Mathf.Abs(
                    distanceAlongRoad -
                    entryDistance);

            float transitionDistance =
                PathMath.DistanceXZ(
                    point,
                    target);

            return
                roadDistance +
                transitionDistance;
        }

        // ============================================================
        // FORWARD PATH BUILD
        // ============================================================

        private bool BuildForwardRoadPath(
            VisualRoad road,
            Vector3 start,
            Vector3 end,
            IReadOnlyList<Vector3> optimizedPath,
            IReadOnlyList<int> optimizedPortalIndices,
            int firstPortal,
            int lastPortal,
            RoadPoint entry,
            RoadPoint exit,
            out RoadPathResult result)
        {
            var points =
                new List<Vector3>();

            var roadFlags =
                new List<bool>();

            AddUnique(
                points,
                roadFlags,
                start,
                false);

            AddNormalPathBeforeRoad(
                points,
                roadFlags,
                optimizedPath,
                optimizedPortalIndices,
                firstPortal);

            /*
             * Transition onto the road.
             */
            AddUnique(
                points,
                roadFlags,
                entry.Position,
                false);

            /*
             * Add original road points.
             *
             * They are marked as road points so the smoothing
             * stage can preserve their geometry if required.
             */
            AddRoadPointsForward(
                points,
                roadFlags,
                road,
                entry,
                exit);

            /*
             * Transition off the road.
             */
            AddUnique(
                points,
                roadFlags,
                exit.Position,
                false);

            AddNormalPathAfterRoad(
                points,
                roadFlags,
                optimizedPath,
                optimizedPortalIndices,
                lastPortal);

            AddUnique(
                points,
                roadFlags,
                end,
                false);

            result =
                new RoadPathResult(
                    points,
                    roadFlags);

            return points.Count >= 2;
        }

        // ============================================================
        // REVERSE PATH BUILD
        // ============================================================

        private bool BuildReverseRoadPath(
            VisualRoad road,
            Vector3 start,
            Vector3 end,
            IReadOnlyList<Vector3> optimizedPath,
            IReadOnlyList<int> optimizedPortalIndices,
            int firstPortal,
            int lastPortal,
            RoadPoint entry,
            RoadPoint exit,
            out RoadPathResult result)
        {
            var points =
                new List<Vector3>();

            var roadFlags =
                new List<bool>();

            AddUnique(
                points,
                roadFlags,
                start,
                false);

            AddNormalPathBeforeRoad(
                points,
                roadFlags,
                optimizedPath,
                optimizedPortalIndices,
                firstPortal);

            AddUnique(
                points,
                roadFlags,
                entry.Position,
                false);

            int startSegment =
                Mathf.Clamp(
                    entry.SegmentIndex,
                    0,
                    road.Points.Count - 2);

            int endSegment =
                Mathf.Clamp(
                    exit.SegmentIndex,
                    0,
                    road.Points.Count - 2);

            /*
             * Walk the supplied road backwards.
             */
            for (int i = startSegment;
                 i >= endSegment;
                 i--)
            {
                AddUnique(
                    points,
                    roadFlags,
                    road.Points[i],
                    true);
            }

            /*
             * The final road point may need to be included.
             */
            if (endSegment + 1 <
                road.Points.Count)
            {
                AddUnique(
                    points,
                    roadFlags,
                    road.Points[endSegment + 1],
                    true);
            }

            AddUnique(
                points,
                roadFlags,
                exit.Position,
                false);

            AddNormalPathAfterRoad(
                points,
                roadFlags,
                optimizedPath,
                optimizedPortalIndices,
                lastPortal);

            AddUnique(
                points,
                roadFlags,
                end,
                false);

            result =
                new RoadPathResult(
                    points,
                    roadFlags);

            return points.Count >= 2;
        }

        // ============================================================
        // ROAD POINTS
        // ============================================================

        private void AddRoadPointsForward(
            List<Vector3> points,
            List<bool> flags,
            VisualRoad road,
            RoadPoint entry,
            RoadPoint exit)
        {
            int start =
                Mathf.Clamp(
                    entry.SegmentIndex + 1,
                    0,
                    road.Points.Count - 1);

            int end =
                Mathf.Clamp(
                    exit.SegmentIndex,
                    0,
                    road.Points.Count - 1);

            for (int i = start;
                 i <= end;
                 i++)
            {
                float distance =
                    CalculateDistanceAlongRoad(
                        road,
                        i);

                if (distance <=
                    entry.DistanceAlongRoad +
                    PositionTolerance)
                {
                    continue;
                }

                if (distance >=
                    exit.DistanceAlongRoad -
                    PositionTolerance)
                {
                    continue;
                }

                AddUnique(
                    points,
                    flags,
                    road.Points[i],
                    true);
            }

            /*
             * If the exit occurs after the final original road
             * point, preserve that final point.
             */
            if (exit.SegmentIndex + 1 <
                road.Points.Count)
            {
                int finalIndex =
                    exit.SegmentIndex + 1;

                float finalDistance =
                    CalculateDistanceAlongRoad(
                        road,
                        finalIndex);

                if (finalDistance <
                    exit.DistanceAlongRoad -
                    PositionTolerance)
                {
                    AddUnique(
                        points,
                        flags,
                        road.Points[finalIndex],
                        true);
                }
            }
        }

        // ============================================================
        // NORMAL PATH
        // ============================================================

        private void AddNormalPathBeforeRoad(
            List<Vector3> points,
            List<bool> flags,
            IReadOnlyList<Vector3> path,
            IReadOnlyList<int> portalIndices,
            int firstPortal)
        {
            int index =
                FindPortalIndex(
                    portalIndices,
                    firstPortal);

            if (index < 0)
                return;

            /*
             * Do not add the portal crossing itself.
             *
             * The road entry point replaces it.
             */
            for (int i = 1;
                 i < index;
                 i++)
            {
                AddUnique(
                    points,
                    flags,
                    path[i],
                    false);
            }
        }

        private void AddNormalPathAfterRoad(
            List<Vector3> points,
            List<bool> flags,
            IReadOnlyList<Vector3> path,
            IReadOnlyList<int> portalIndices,
            int lastPortal)
        {
            int index =
                FindPortalIndex(
                    portalIndices,
                    lastPortal);

            if (index < 0)
                return;

            /*
             * Skip the portal crossing itself.
             *
             * The road exit point replaces it.
             *
             * The next point remains the normal portal-solver
             * point, including all edge avoidance.
             */
            for (int i = index + 1;
                 i < path.Count - 1;
                 i++)
            {
                AddUnique(
                    points,
                    flags,
                    path[i],
                    false);
            }
        }

        private int FindPortalIndex(
            IReadOnlyList<int> portalIndices,
            int portalIndex)
        {
            for (int i = 0;
                 i < portalIndices.Count;
                 i++)
            {
                if (portalIndices[i] ==
                    portalIndex)
                {
                    return i;
                }
            }

            return -1;
        }

        private Vector3 FindEntryTarget(
            IReadOnlyList<Vector3> path,
            IReadOnlyList<int> portalIndices,
            int portal,
            Vector3 fallback)
        {
            int index =
                FindPortalIndex(
                    portalIndices,
                    portal);

            if (index <= 0)
                return fallback;

            return path[index - 1];
        }

        private Vector3 FindExitTarget(
            IReadOnlyList<Vector3> path,
            IReadOnlyList<int> portalIndices,
            int portal,
            Vector3 fallback)
        {
            int index =
                FindPortalIndex(
                    portalIndices,
                    portal);

            if (index < 0 ||
                index >= path.Count - 1)
            {
                return fallback;
            }

            return path[index + 1];
        }

        // ============================================================
        // ROAD GEOMETRY
        // ============================================================

        private RoadPoint FindBestPointOnSegment(
            VisualRoad road,
            int segmentIndex,
            Vector3 target)
        {
            if (segmentIndex < 0 ||
                segmentIndex >=
                road.Points.Count - 1)
            {
                return default;
            }

            Vector3 a =
                road.Points[segmentIndex];

            Vector3 b =
                road.Points[segmentIndex + 1];

            Vector3 point =
                PathMath.ProjectOnSegment(
                    target,
                    a,
                    b,
                    out float t);

            float distance =
                CalculateDistanceAlongRoad(
                    road,
                    segmentIndex,
                    t);

            return new RoadPoint(
                point,
                segmentIndex,
                distance);
        }

        private float CalculateDistanceAlongRoad(
            VisualRoad road,
            int pointIndex)
        {
            pointIndex =
                Mathf.Clamp(
                    pointIndex,
                    0,
                    road.Points.Count - 1);

            float distance = 0f;

            for (int i = 0;
                 i < pointIndex;
                 i++)
            {
                distance +=
                    PathMath.DistanceXZ(
                        road.Points[i],
                        road.Points[i + 1]);
            }

            return distance;
        }

        private float CalculateDistanceAlongRoad(
            VisualRoad road,
            int segmentIndex,
            float t)
        {
            segmentIndex =
                Mathf.Clamp(
                    segmentIndex,
                    0,
                    road.Points.Count - 2);

            t =
                Mathf.Clamp01(t);

            float distance =
                CalculateDistanceAlongRoad(
                    road,
                    segmentIndex);

            distance +=
                PathMath.DistanceXZ(
                    road.Points[segmentIndex],
                    road.Points[segmentIndex + 1]) *
                t;

            return distance;
        }

        // ============================================================
        // VALIDATION
        // ============================================================

        private bool IsValidRoad(
            VisualRoad road)
        {
            if (road == null)
                return false;

            if (road.Points == null ||
                road.PointHexIds == null)
            {
                return false;
            }

            if (road.Points.Count < 2)
                return false;

            if (road.PointHexIds.Count !=
                road.Points.Count)
            {
                return false;
            }

            if (road.HexIds == null ||
                road.HexIds.Count == 0)
            {
                return false;
            }

            return true;
        }

        // ============================================================
        // HELPERS
        // ============================================================

        private void AddUnique(
            List<Vector3> points,
            List<bool> flags,
            Vector3 point,
            bool isRoadPoint)
        {
            if (points.Count == 0)
            {
                points.Add(point);
                flags.Add(isRoadPoint);
                return;
            }

            if (PathMath.DistanceXZ(
                    points[points.Count - 1],
                    point) <=
                PositionTolerance)
            {
                /*
                 * If a point is already present but is also
                 * an original road point, preserve that fact.
                 */
                if (isRoadPoint)
                {
                    flags[flags.Count - 1] = true;
                }

                return;
            }

            points.Add(point);
            flags.Add(isRoadPoint);
        }

        // ============================================================
        // STRUCTS
        // ============================================================

        private readonly struct RoadSection
        {
            public readonly int StartHexIndex;
            public readonly int EndHexIndex;

            public RoadSection(
                int startHexIndex,
                int endHexIndex)
            {
                StartHexIndex =
                    startHexIndex;

                EndHexIndex =
                    endHexIndex;
            }
        }

        private readonly struct RoadPoint
        {
            public readonly Vector3 Position;
            public readonly int SegmentIndex;
            public readonly float DistanceAlongRoad;

            public bool IsValid =>
                SegmentIndex >= 0;

            public RoadPoint(
                Vector3 position,
                int segmentIndex,
                float distanceAlongRoad)
            {
                Position = position;
                SegmentIndex = segmentIndex;
                DistanceAlongRoad =
                    distanceAlongRoad;
            }
        }

        private readonly struct RoadTraversal
        {
            public readonly RoadPoint Entry;
            public readonly RoadPoint Exit;
            public readonly float TotalCost;
            public readonly bool IsReverse;

            public bool IsValid =>
                Entry.IsValid &&
                Exit.IsValid;

            public RoadTraversal(
                RoadPoint entry,
                RoadPoint exit,
                float totalCost,
                bool isReverse)
            {
                Entry = entry;
                Exit = exit;
                TotalCost = totalCost;
                IsReverse = isReverse;
            }
        }
    }
}