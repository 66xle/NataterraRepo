using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class RoadPathProcessor
    {
        private readonly VisualPathSettings settings;

        private const float PositionTolerance = 0.001f;
        private const float HexTolerance = 0.0001f;

        public RoadPathProcessor(
            VisualPathSettings settings)
        {
            this.settings = settings;
        }

        // ============================================================
        // ROAD SELECTION
        // ============================================================

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

                if (sectionLength > bestSectionLength)
                {
                    bestSectionLength = sectionLength;
                    bestRoad = road;
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
                optimizedPortalIndices.Count != optimizedPath.Count)
            {
                return false;
            }

            if (portals == null ||
                portals.Count == 0)
            {
                return false;
            }

            /*
             * Find a road section by matching actual road
             * transitions against consecutive corridor hexes.
             *
             * Example:
             *
             * Corridor:
             *     A -> B -> C -> E
             *
             * Road:
             *         B -> C -> D
             *
             * Matching transition:
             *         B -> C
             *
             * Therefore:
             *
             *     Entry = B
             *     Exit  = C
             */
            if (!TryFindBestRoadSection(
                    corridor,
                    road,
                    out RoadSection section))
            {
                return false;
            }

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

            /*
             * The road entry and exit hexes come directly from
             * the corridor section.
             *
             * This is important.
             *
             * We do NOT derive these from arbitrary road points.
             */
            int entryHexId =
                corridor[
                    section.StartHexIndex].Id;

            int exitHexId =
                corridor[
                    section.EndHexIndex].Id;

            if (!TryFindRoadPointRange(
                    road,
                    corridor,
                    section,
                    out int firstRoadPoint,
                    out int lastRoadPoint))
            {
                return false;
            }

            Vector3 entryTarget =
                FindEntryTarget(
                    optimizedPath,
                    optimizedPortalIndices,
                    firstPortal,
                    start);

            Vector3 exitTarget =
                FindExitTarget(
                    optimizedPath,
                    optimizedPortalIndices,
                    lastPortal,
                    end);

            /*
             * The corridor determines the direction.
             *
             * We therefore only attempt forward traversal.
             *
             * This prevents cases such as:
             *
             *     A -> C -> B -> C
             *
             * where the road was entered from the wrong
             * direction simply because its calculated cost
             * happened to be lower.
             */
            RoadTraversal selected =
                FindForwardTraversal(
                    road,
                    corridor,
                    firstRoadPoint,
                    lastRoadPoint,
                    entryHexId,
                    exitHexId,
                    entryTarget,
                    exitTarget);

            if (!selected.IsValid)
                return false;

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

            if (corridor == null ||
                corridor.Count < 2)
            {
                return false;
            }

            int bestStart = -1;
            int bestEnd = -1;
            int bestLength = 0;

            int currentStart = -1;
            int currentEnd = -1;

            /*
             * We examine each transition in the corridor.
             *
             *     corridor[i] -> corridor[i + 1]
             *
             * A road transition is valid only when the road
             * contains the same transition in the same direction.
             */
            for (int i = 0;
                 i < corridor.Count - 1;
                 i++)
            {
                int currentHex =
                    corridor[i].Id;

                int nextHex =
                    corridor[i + 1].Id;

                bool matches =
                    HasRoadTransition(
                        road,
                        currentHex,
                        nextHex);

                if (matches)
                {
                    if (currentStart < 0)
                    {
                        currentStart = i;
                    }

                    currentEnd = i + 1;

                    int length =
                        currentEnd -
                        currentStart +
                        1;

                    if (length > bestLength)
                    {
                        bestLength = length;

                        bestStart =
                            currentStart;

                        bestEnd =
                            currentEnd;
                    }
                }
                else
                {
                    currentStart = -1;
                    currentEnd = -1;
                }
            }

            /*
             * A valid road section needs at least one transition.
             *
             * Example:
             *
             *     B -> C
             *
             * means:
             *
             *     StartHexIndex = B
             *     EndHexIndex   = C
             */
            if (bestStart < 0 ||
                bestEnd <= bestStart)
            {
                return false;
            }

            section =
                new RoadSection(
                    bestStart,
                    bestEnd);

            return true;
        }

        private bool HasRoadTransition(
            VisualRoad road,
            int fromHexId,
            int toHexId)
        {
            if (road.PointHexIds == null ||
                road.PointHexIds.Count < 2)
            {
                return false;
            }

            for (int i = 0;
                 i < road.PointHexIds.Count - 1;
                 i++)
            {
                int roadHexA =
                    road.PointHexIds[i];

                int roadHexB =
                    road.PointHexIds[i + 1];

                /*
                 * Direction matters.
                 *
                 * B -> C is valid for corridor B -> C.
                 *
                 * C -> B is NOT considered the same transition.
                 */
                if (roadHexA == fromHexId &&
                    roadHexB == toHexId)
                {
                    return true;
                }
            }

            return false;
        }

        // ============================================================
        // ROAD POINT RANGE
        // ============================================================

        private bool TryFindRoadPointRange(
            VisualRoad road,
            IReadOnlyList<VisualHex> corridor,
            RoadSection section,
            out int firstRoadPoint,
            out int lastRoadPoint)
        {
            firstRoadPoint = -1;
            lastRoadPoint = -1;

            /*
             * The section includes:
             *
             *     B -> C
             *
             * Therefore both B and C are part of the usable
             * road section.
             */
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
                {
                    firstRoadPoint = i;
                }

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
            IReadOnlyList<VisualHex> corridor,
            int firstRoadPoint,
            int lastRoadPoint,
            int entryHexId,
            int exitHexId,
            Vector3 entryTarget,
            Vector3 exitTarget)
        {
            /*
             * Entry is explicitly restricted to the FIRST
             * corridor hex of the road section.
             *
             * For:
             *
             *     A -> B -> C -> E
             *
             *     Road: B -> C -> D
             *
             * entryHexId is B.
             */
            RoadPoint entry =
                FindBestEntryPointForward(
                    road,
                    firstRoadPoint,
                    entryHexId,
                    entryTarget);

            if (!entry.IsValid)
                return default;

            /*
             * Exit is explicitly restricted to the LAST
             * corridor hex of the road section.
             *
             * Here that is C.
             */
            RoadPoint exit =
                FindBestExitPointForward(
                    road,
                    entry,
                    lastRoadPoint,
                    exitHexId,
                    corridor,
                    exitTarget);

            if (!exit.IsValid)
                return default;

            /*
             * The exit must be ahead of the entry on the road.
             */
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
        // FORWARD ENTRY
        // ============================================================

        private RoadPoint FindBestEntryPointForward(
            VisualRoad road,
            int firstRoadPoint,
            int entryHexId,
            Vector3 target)
        {
            RoadPoint best =
                default;

            float bestCost =
                float.MaxValue;

            /*
             * IMPORTANT:
             *
             * Only consider road geometry belonging to the
             * actual entry hex.
             */
            for (int i = 0;
                 i < road.Points.Count;
                 i++)
            {
                if (road.PointHexIds[i] !=
                    entryHexId)
                {
                    continue;
                }

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
             * Also check segments completely inside the
             * entry hex.
             */
            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                if (road.PointHexIds[i] != entryHexId ||
                    road.PointHexIds[i + 1] != entryHexId)
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
        // FINAL HEX EXIT
        // ============================================================

        private RoadPoint FindBestExitPointForward(
            VisualRoad road,
            RoadPoint entry,
            int lastRoadPoint,
            int exitHexId,
            IReadOnlyList<VisualHex> corridor,
            Vector3 target)
        {
            /*
             * The exit hex comes from the corridor, NOT from
             * whichever road point happens to be last.
             */
            VisualHex exitHex =
                FindHexById(
                    corridor,
                    exitHexId);

            if (exitHex.Corners == null ||
                exitHex.Corners.Length < 3)
            {
                return default;
            }

            /*
             * Find the best sample inside the final road hex.
             *
             * The score is:
             *
             *     remaining road distance
             *     +
             *     distance to next normal path point
             *
             * We do NOT use the distance from the entry to
             * select the sample independently.
             */
            RoadPoint best =
                default;

            float bestCost =
                float.MaxValue;

            int sampleCount =
                Mathf.Max(
                    1,
                    settings.RoadExitSamplesPerSegment);

            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                Vector3 a =
                    road.Points[i];

                Vector3 b =
                    road.Points[i + 1];

                int hexA =
                    road.PointHexIds[i];

                int hexB =
                    road.PointHexIds[i + 1];

                bool aInside =
                    hexA == exitHexId;

                bool bInside =
                    hexB == exitHexId;

                if (!aInside &&
                    !bInside)
                {
                    continue;
                }

                float minT;
                float maxT;

                if (aInside &&
                    bInside)
                {
                    /*
                     * Entire segment is inside the final hex.
                     */
                    minT = 0f;
                    maxT = 1f;
                }
                else
                {
                    /*
                     * This segment crosses the boundary of
                     * the final hex.
                     *
                     * Example:
                     *
                     *     B -------- C
                     *                 \
                     *                  E
                     *
                     * The segment B->C may contain the portion
                     * that is actually inside C.
                     */
                    if (!TryClipSegmentToHex(
                            a,
                            b,
                            exitHex,
                            out minT,
                            out maxT))
                    {
                        /*
                         * Do NOT blindly use a road vertex here.
                         *
                         * If clipping fails, only use the endpoint
                         * that is explicitly known to belong to C.
                         */
                        if (aInside)
                        {
                            minT = 0f;
                            maxT = 0f;
                        }
                        else if (bInside)
                        {
                            minT = 1f;
                            maxT = 1f;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                if (maxT < minT)
                {
                    float temp = minT;
                    minT = maxT;
                    maxT = temp;
                }

                /*
                 * Sample the complete portion of the road segment
                 * that lies inside the final road hex.
                 */
                for (int sample = 0;
                     sample <= sampleCount;
                     sample++)
                {
                    float normalized =
                        sample /
                        (float)sampleCount;

                    float t =
                        Mathf.Lerp(
                            minT,
                            maxT,
                            normalized);

                    Vector3 position =
                        Vector3.Lerp(
                            a,
                            b,
                            t);

                    float distanceToTarget =
                        PathMath.DistanceXZ(
                            position,
                            target);

                    float distanceAlongRoad =
                        CalculateDistanceAlongRoad(
                            road,
                            i,
                            t);

                    /*
                     * This is the remaining road distance from
                     * the selected entry point to this sample.
                     */
                    float remainingRoadDistance =
                        distanceAlongRoad -
                        entry.DistanceAlongRoad;

                    /*
                     * A sample behind the entry cannot be an exit.
                     */
                    if (remainingRoadDistance <=
                        PositionTolerance)
                    {
                        continue;
                    }

                    /*
                     * The actual score used to select the exit.
                     *
                     * Example:
                     *
                     * Sample 1:
                     *     8m remaining road
                     *     2m to D
                     *     = 10m
                     *
                     * Sample 2:
                     *     4m remaining road
                     *     5m to D
                     *     = 9m
                     *
                     * Sample 2 wins.
                     */
                    float totalCost =
                        remainingRoadDistance +
                        distanceToTarget;

                    if (totalCost <
                        bestCost)
                    {
                        bestCost = totalCost;

                        best =
                            new RoadPoint(
                                position,
                                i,
                                distanceAlongRoad);
                    }
                }
            }

            return best;
        }

        // ============================================================
        // HEX SEGMENT CLIPPING
        // ============================================================

        private bool TryClipSegmentToHex(
            Vector3 a,
            Vector3 b,
            VisualHex hex,
            out float minT,
            out float maxT)
        {
            minT = 0f;
            maxT = 1f;

            if (hex.Corners == null ||
                hex.Corners.Length < 3)
            {
                return false;
            }

            bool insideA =
                IsPointInsideHex(
                    a,
                    hex);

            bool insideB =
                IsPointInsideHex(
                    b,
                    hex);

            if (insideA &&
                insideB)
            {
                return true;
            }

            var intersections =
                new List<float>();

            for (int i = 0;
                 i < hex.Corners.Length;
                 i++)
            {
                Vector3 edgeA =
                    hex.Corners[i];

                Vector3 edgeB =
                    hex.Corners[
                        (i + 1) %
                        hex.Corners.Length];

                if (TryGetSegmentIntersectionT(
                        a,
                        b,
                        edgeA,
                        edgeB,
                        out float t))
                {
                    intersections.Add(t);
                }
            }

            /*
             * If A is inside the hex, the segment starts inside
             * the desired region.
             */
            if (insideA)
            {
                minT = 0f;

                if (intersections.Count > 0)
                {
                    maxT =
                        GetFarthestValidT(
                            intersections,
                            0f);
                }
                else
                {
                    maxT = 0f;
                }

                return true;
            }

            /*
             * If B is inside the hex, the segment ends inside
             * the desired region.
             */
            if (insideB)
            {
                maxT = 1f;

                if (intersections.Count > 0)
                {
                    minT =
                        GetClosestValidT(
                            intersections,
                            1f);
                }
                else
                {
                    minT = 1f;
                }

                return true;
            }

            /*
             * Neither endpoint is inside.
             *
             * The segment must enter and leave the hex.
             */
            if (intersections.Count >= 2)
            {
                float first =
                    float.MaxValue;

                float second =
                    float.MinValue;

                for (int i = 0;
                     i < intersections.Count;
                     i++)
                {
                    float value =
                        Mathf.Clamp01(
                            intersections[i]);

                    if (value < first)
                        first = value;

                    if (value > second)
                        second = value;
                }

                if (first != float.MaxValue &&
                    second != float.MinValue &&
                    second >= first)
                {
                    minT = first;
                    maxT = second;

                    return true;
                }
            }

            return false;
        }

        private bool IsPointInsideHex(
            Vector3 point,
            VisualHex hex)
        {
            bool hasPositive = false;
            bool hasNegative = false;

            for (int i = 0;
                 i < hex.Corners.Length;
                 i++)
            {
                Vector3 a =
                    hex.Corners[i];

                Vector3 b =
                    hex.Corners[
                        (i + 1) %
                        hex.Corners.Length];

                float cross =
                    (b.x - a.x) *
                    (point.z - a.z)
                    -
                    (b.z - a.z) *
                    (point.x - a.x);

                if (cross > HexTolerance)
                    hasPositive = true;

                if (cross < -HexTolerance)
                    hasNegative = true;

                if (hasPositive &&
                    hasNegative)
                {
                    return false;
                }
            }

            return true;
        }

        private bool TryGetSegmentIntersectionT(
            Vector3 a,
            Vector3 b,
            Vector3 c,
            Vector3 d,
            out float t)
        {
            float denominator =
                (b.x - a.x) *
                (d.z - c.z)
                -
                (b.z - a.z) *
                (d.x - c.x);

            if (Mathf.Abs(denominator) <
                0.00001f)
            {
                t = 0f;
                return false;
            }

            float tValue =
                ((c.x - a.x) *
                    (d.z - c.z)
                 -
                 (c.z - a.z) *
                    (d.x - c.x))
                /
                denominator;

            float u =
                ((c.x - a.x) *
                    (b.z - a.z)
                 -
                 (c.z - a.z) *
                    (b.x - a.x))
                /
                denominator;

            if (tValue < -0.00001f ||
                tValue > 1f + 0.00001f ||
                u < -0.00001f ||
                u > 1f + 0.00001f)
            {
                t = 0f;
                return false;
            }

            t =
                Mathf.Clamp01(tValue);

            return true;
        }

        private float GetClosestValidT(
            List<float> values,
            float reference)
        {
            float best =
                values[0];

            float bestDistance =
                Mathf.Abs(
                    values[0] -
                    reference);

            for (int i = 1;
                 i < values.Count;
                 i++)
            {
                float distance =
                    Mathf.Abs(
                        values[i] -
                        reference);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = values[i];
                }
            }

            return best;
        }

        private float GetFarthestValidT(
            List<float> values,
            float reference)
        {
            float best =
                values[0];

            float bestDistance =
                Mathf.Abs(
                    values[0] -
                    reference);

            for (int i = 1;
                 i < values.Count;
                 i++)
            {
                float distance =
                    Mathf.Abs(
                        values[i] -
                        reference);

                if (distance > bestDistance)
                {
                    bestDistance = distance;
                    best = values[i];
                }
            }

            return best;
        }

        private VisualHex FindHexById(
            IReadOnlyList<VisualHex> corridor,
            int hexId)
        {
            if (corridor == null)
                return default;

            for (int i = 0;
                 i < corridor.Count;
                 i++)
            {
                if (corridor[i].Id == hexId)
                    return corridor[i];
            }

            return default;
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
             * Actual road entry.
             */
            AddUnique(
                points,
                roadFlags,
                entry.Position,
                false);

            AddRoadPointsForward(
                points,
                roadFlags,
                road,
                entry,
                exit);

            /*
             * Actual road exit.
             *
             * This is now a sampled point inside the final
             * road hex, rather than necessarily being the road
             * vertex.
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
             * If there is a road vertex before the selected exit
             * sample, include it.
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
                segmentIndex >= road.Points.Count - 1)
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