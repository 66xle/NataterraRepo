using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class RoadPathProcessor
    {
        private readonly VisualPathSettings settings;

        private const float PositionTolerance = 0.001f;
        private const float GeometryTolerance = 0.0001f;
        private const float MaximumExitAngle = 180f;

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
             * The entry target is the normal path point before
             * the road begins.
             */
            Vector3 entryTarget =
                FindEntryTarget(
                    optimizedPath,
                    optimizedPortalIndices,
                    firstPortal,
                    start);

            /*
             * The exit target is the normal path point after
             * the final road hex.
             */
            Vector3 exitTarget =
                FindExitTarget(
                    optimizedPath,
                    optimizedPortalIndices,
                    lastPortal,
                    end);

            /*
             * Try both possible directions along the road.
             */
            RoadTraversal forward =
                FindForwardTraversal(
                    road,
                    corridor,
                    section,
                    firstRoadPoint,
                    lastRoadPoint,
                    entryTarget,
                    exitTarget);

            RoadTraversal reverse =
                FindReverseTraversal(
                    road,
                    corridor,
                    section,
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

                bool hasRoadPoint =
                    HasRoadPointInHex(
                        road,
                        hexId);

                if (covered &&
                    hasRoadPoint)
                {
                    if (currentStart < 0)
                    {
                        currentStart = i;
                    }

                    currentLength++;

                    if (currentLength >
                        bestLength)
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
             * A road needs to span at least two corridor hexes
             * to be useful as a road path.
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
                if (road.PointHexIds[i] ==
                    hexId)
                {
                    return true;
                }
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
                if (corridor[i].Id ==
                    hexId)
                {
                    return true;
                }
            }

            return false;
        }

        // ============================================================
        // FORWARD TRAVERSAL
        // ============================================================

        private RoadTraversal FindForwardTraversal(
            VisualRoad road,
            IReadOnlyList<VisualHex> corridor,
            RoadSection section,
            int firstRoadPoint,
            int lastRoadPoint,
            Vector3 entryTarget,
            Vector3 exitTarget)
        {
            VisualHex entryHex =
                corridor[
                    section.StartHexIndex];

            VisualHex exitHex =
                corridor[
                    section.EndHexIndex];

            RoadPoint entry =
                FindBestEntryPointForward(
                    road,
                    firstRoadPoint,
                    entryHex,
                    entryTarget);

            if (!entry.IsValid)
                return default;

            /*
             * The exit is ALWAYS calculated inside the final
             * corridor hex occupied by the road.
             */
            RoadPoint exit =
                FindBestExitPointForward(
                    road,
                    entry,
                    lastRoadPoint,
                    exitHex,
                    exitTarget);

            if (!exit.IsValid)
                return default;

            /*
             * The exit must still be ahead of the entry.
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
        // REVERSE TRAVERSAL
        // ============================================================

        private RoadTraversal FindReverseTraversal(
            VisualRoad road,
            IReadOnlyList<VisualHex> corridor,
            RoadSection section,
            int firstRoadPoint,
            int lastRoadPoint,
            Vector3 entryTarget,
            Vector3 exitTarget)
        {
            VisualHex entryHex =
                corridor[
                    section.EndHexIndex];

            VisualHex exitHex =
                corridor[
                    section.StartHexIndex];

            RoadPoint entry =
                FindBestEntryPointReverse(
                    road,
                    lastRoadPoint,
                    entryHex,
                    entryTarget);

            if (!entry.IsValid)
                return default;

            RoadPoint exit =
                FindBestExitPointReverse(
                    road,
                    entry,
                    firstRoadPoint,
                    exitHex,
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
        // ENTRY POINT
        // ============================================================

        private RoadPoint FindBestEntryPointForward(
            VisualRoad road,
            int firstRoadPoint,
            VisualHex entryHex,
            Vector3 target)
        {
            return FindBestEntryPointInHex(
                road,
                entryHex,
                target,
                true);
        }

        private RoadPoint FindBestEntryPointReverse(
            VisualRoad road,
            int lastRoadPoint,
            VisualHex entryHex,
            Vector3 target)
        {
            return FindBestEntryPointInHex(
                road,
                entryHex,
                target,
                false);
        }

        private RoadPoint FindBestEntryPointInHex(
            VisualRoad road,
            VisualHex hex,
            Vector3 target,
            bool forward)
        {
            RoadPoint best =
                default;

            float bestDistance =
                float.MaxValue;

            /*
             * Check road points that belong to this hex.
             */
            for (int i = 0;
                 i < road.Points.Count;
                 i++)
            {
                if (road.PointHexIds[i] !=
                    hex.Id)
                {
                    continue;
                }

                float distance =
                    PathMath.DistanceXZ(
                        road.Points[i],
                        target);

                if (distance <
                    bestDistance)
                {
                    bestDistance = distance;

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
             * Check every segment touching this hex.
             *
             * This is important because an entry may happen
             * somewhere along A -> B rather than at the B vertex.
             */
            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                int hexA =
                    road.PointHexIds[i];

                int hexB =
                    road.PointHexIds[i + 1];

                if (hexA != hex.Id &&
                    hexB != hex.Id)
                {
                    continue;
                }

                Vector3 a =
                    road.Points[i];

                Vector3 b =
                    road.Points[i + 1];

                if (!TryGetSegmentPortionInsideHex(
                        a,
                        b,
                        hex,
                        out float minT,
                        out float maxT))
                {
                    continue;
                }

                Vector3 clippedA =
                    Vector3.Lerp(
                        a,
                        b,
                        minT);

                Vector3 clippedB =
                    Vector3.Lerp(
                        a,
                        b,
                        maxT);

                Vector3 candidate =
                    PathMath.ProjectOnSegment(
                        target,
                        clippedA,
                        clippedB,
                        out float localT);

                float t =
                    Mathf.Lerp(
                        minT,
                        maxT,
                        localT);

                float distance =
                    PathMath.DistanceXZ(
                        candidate,
                        target);

                if (distance <
                    bestDistance)
                {
                    bestDistance =
                        distance;

                    best =
                        new RoadPoint(
                            candidate,
                            i,
                            CalculateDistanceAlongRoad(
                                road,
                                i,
                                t));
                }
            }

            return best;
        }

        // ============================================================
        // EXIT POINT
        // ============================================================

        private RoadPoint FindBestExitPointForward(
            VisualRoad road,
            RoadPoint entry,
            int lastRoadPoint,
            VisualHex finalHex,
            Vector3 target)
        {
            RoadPoint best =
                FindBestExitPointInHex(
                    road,
                    finalHex,
                    target,
                    true);

            if (!best.IsValid)
                return default;

            /*
             * The sample selection itself does NOT consider
             * road distance from the entry.
             *
             * This check only makes sure the selected sample
             * is actually reachable while travelling forward.
             */
            if (best.DistanceAlongRoad <=
                entry.DistanceAlongRoad)
            {
                return default;
            }

            return best;
        }

        private RoadPoint FindBestExitPointReverse(
            VisualRoad road,
            RoadPoint entry,
            int firstRoadPoint,
            VisualHex finalHex,
            Vector3 target)
        {
            RoadPoint best =
                FindBestExitPointInHex(
                    road,
                    finalHex,
                    target,
                    false);

            if (!best.IsValid)
                return default;

            /*
             * Same rule for reverse traversal.
             */
            if (best.DistanceAlongRoad >=
                entry.DistanceAlongRoad)
            {
                return default;
            }

            return best;
        }

        // ============================================================
        // FINAL HEX EXIT SAMPLING
        // ============================================================

        private RoadPoint FindBestExitPointInHex(
            VisualRoad road,
            VisualHex finalHex,
            Vector3 target,
            bool forward)
        {
            if (finalHex.Corners == null ||
                finalHex.Corners.Length < 3)
            {
                return default;
            }

            var samples =
                new List<ExitSample>();

            int sampleCount =
                Mathf.Max(
                    1,
                    settings.RoadExitSamplesPerSegment);

            /*
             * Check every road segment that touches the final
             * road hex.
             */
            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                int hexA =
                    road.PointHexIds[i];

                int hexB =
                    road.PointHexIds[i + 1];

                if (hexA != finalHex.Id &&
                    hexB != finalHex.Id)
                {
                    continue;
                }

                Vector3 a =
                    road.Points[i];

                Vector3 b =
                    road.Points[i + 1];

                /*
                 * Find the actual portion of the road segment
                 * that lies inside the final hex.
                 */
                if (!TryGetSegmentPortionInsideHex(
                        a,
                        b,
                        finalHex,
                        out float minT,
                        out float maxT))
                {
                    continue;
                }

                /*
                 * Sample along the clipped portion.
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

                    Vector3 roadDirection =
                        b - a;

                    /*
                     * Ignore the Y axis.
                     */
                    roadDirection.y = 0f;

                    if (roadDirection.sqrMagnitude <=
                        GeometryTolerance)
                    {
                        continue;
                    }

                    roadDirection.Normalize();

                    if (!forward)
                    {
                        roadDirection = -roadDirection;
                    }

                    Vector3 toTarget =
                        target - position;

                    /*
                     * Ignore the Y axis for direction
                     * comparison.
                     */
                    toTarget.y = 0f;

                    if (toTarget.sqrMagnitude <=
                        GeometryTolerance)
                    {
                        /*
                         * The sample is essentially already
                         * at the destination.
                         */
                        samples.Add(
                            new ExitSample(
                                position,
                                i,
                                t,
                                CalculateDistanceAlongRoad(
                                    road,
                                    i,
                                    t),
                                0f));

                        continue;
                    }

                    toTarget.Normalize();

                    float angle =
                        Vector3.Angle(
                            roadDirection,
                            toTarget);

                    float distance =
                        PathMath.DistanceXZ(
                            position,
                            target);

                    samples.Add(
                        new ExitSample(
                            position,
                            i,
                            t,
                            CalculateDistanceAlongRoad(
                                road,
                                i,
                                t),
                            distance,
                            angle));
                }
            }

            if (samples.Count == 0)
                return default;

            /*
             * --------------------------------------------------------
             * ANGLE FILTER
             * --------------------------------------------------------
             *
             * First look for samples whose road direction is
             * reasonably aligned with the direction to the next
             * normal path point.
             */
            float angleRange =
                Mathf.Clamp(
                    settings.RoadExitAngleRange,
                    0f,
                    MaximumExitAngle);

            while (true)
            {
                bool foundValidSample = false;

                ExitSample best =
                    default;

                float bestDistance =
                    float.MaxValue;

                for (int i = 0;
                     i < samples.Count;
                     i++)
                {
                    ExitSample sample =
                        samples[i];

                    if (sample.Angle >
                        angleRange)
                    {
                        continue;
                    }

                    foundValidSample = true;

                    /*
                     * IMPORTANT:
                     *
                     * Once a sample has passed the direction
                     * filter, selection is based ONLY on the
                     * shortest direct distance to the next
                     * normal path point.
                     */
                    if (sample.DistanceToTarget <
                        bestDistance)
                    {
                        bestDistance =
                            sample.DistanceToTarget;

                        best =
                            sample;
                    }
                }

                if (foundValidSample)
                {
                    return new RoadPoint(
                        best.Position,
                        best.SegmentIndex,
                        best.DistanceAlongRoad);
                }

                /*
                 * No samples matched the current angle range.
                 *
                 * Expand the range and try again.
                 */
                if (angleRange >=
                    MaximumExitAngle)
                {
                    break;
                }

                angleRange =
                    Mathf.Min(
                        MaximumExitAngle,
                        Mathf.Max(
                            angleRange * 2f,
                            angleRange + 5f));
            }

            /*
             * This should only be reached if something unusual
             * happened with the sample data.
             *
             * Fall back to the globally shortest sample.
             */
            ExitSample fallback =
                samples[0];

            for (int i = 1;
                 i < samples.Count;
                 i++)
            {
                if (samples[i].DistanceToTarget <
                    fallback.DistanceToTarget)
                {
                    fallback =
                        samples[i];
                }
            }

            return new RoadPoint(
                fallback.Position,
                fallback.SegmentIndex,
                fallback.DistanceAlongRoad);
        }

        // ============================================================
        // SEGMENT / HEX CLIPPING
        // ============================================================

        private bool TryGetSegmentPortionInsideHex(
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

            /*
             * Entire segment is inside.
             */
            if (insideA &&
                insideB)
            {
                minT = 0f;
                maxT = 1f;

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
                    AddUniqueT(
                        intersections,
                        t);
                }
            }

            /*
             * A is inside, so the portion begins at A.
             */
            if (insideA)
            {
                minT = 0f;

                if (intersections.Count > 0)
                {
                    maxT =
                        FindFarthestT(
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
             * B is inside, so the portion ends at B.
             */
            if (insideB)
            {
                maxT = 1f;

                if (intersections.Count > 0)
                {
                    minT =
                        FindClosestT(
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
             * If the segment crosses the hex, there should be
             * two intersections.
             */
            if (intersections.Count >= 2)
            {
                intersections.Sort();

                minT =
                    Mathf.Clamp01(
                        intersections[0]);

                maxT =
                    Mathf.Clamp01(
                        intersections[
                            intersections.Count - 1]);

                return maxT >= minT;
            }

            return false;
        }

        private void AddUniqueT(
            List<float> values,
            float value)
        {
            for (int i = 0;
                 i < values.Count;
                 i++)
            {
                if (Mathf.Abs(
                        values[i] -
                        value) <=
                    0.0001f)
                {
                    return;
                }
            }

            values.Add(
                Mathf.Clamp01(value));
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

                if (cross > 0.0001f)
                {
                    hasPositive = true;
                }

                if (cross < -0.0001f)
                {
                    hasNegative = true;
                }

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
                Mathf.Clamp01(
                    tValue);

            return true;
        }

        private float FindClosestT(
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

                if (distance <
                    bestDistance)
                {
                    bestDistance = distance;
                    best = values[i];
                }
            }

            return best;
        }

        private float FindFarthestT(
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

                if (distance >
                    bestDistance)
                {
                    bestDistance = distance;
                    best = values[i];
                }
            }

            return best;
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
             * Do not add the point after the exit segment.
             *
             * This is particularly important when the final
             * road hex is B and the road continues:
             *
             * A -> B -> C
             *
             * If the exit is somewhere along B -> C, C must
             * never be added to the path.
             */
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
                    road.Points[
                        segmentIndex + 1]) *
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
                    flags[
                        flags.Count - 1] = true;
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

        private readonly struct ExitSample
        {
            public readonly Vector3 Position;
            public readonly int SegmentIndex;
            public readonly float SegmentT;
            public readonly float DistanceAlongRoad;
            public readonly float DistanceToTarget;
            public readonly float Angle;

            public ExitSample(
                Vector3 position,
                int segmentIndex,
                float segmentT,
                float distanceAlongRoad,
                float distanceToTarget)
            {
                Position = position;
                SegmentIndex = segmentIndex;
                SegmentT = segmentT;
                DistanceAlongRoad =
                    distanceAlongRoad;
                DistanceToTarget =
                    distanceToTarget;
                Angle = 0f;
            }

            public ExitSample(
                Vector3 position,
                int segmentIndex,
                float segmentT,
                float distanceAlongRoad,
                float distanceToTarget,
                float angle)
            {
                Position = position;
                SegmentIndex = segmentIndex;
                SegmentT = segmentT;
                DistanceAlongRoad =
                    distanceAlongRoad;
                DistanceToTarget =
                    distanceToTarget;
                Angle = angle;
            }
        }
    }
}