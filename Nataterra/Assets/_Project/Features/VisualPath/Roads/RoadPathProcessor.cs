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

        public RoadPathProcessor(VisualPathSettings settings)
        {
            if (settings == null)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Settings cannot be null.");

                this.settings = new VisualPathSettings();
            }
            else
            {
                this.settings = settings;
            }
        }

        // ============================================================
        // ROAD SELECTION
        // ============================================================

        public VisualRoad FindBestRoad(
            IReadOnlyList<VisualHex> corridor,
            IReadOnlyList<VisualRoad> roads)
        {
            if (corridor == null || corridor.Count < 2)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Cannot find road: corridor is invalid.");

                return null;
            }

            if (roads == null || roads.Count == 0)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Cannot find road: no roads supplied.");

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
            {
                Debug.LogError(
                    "[RoadPathProcessor] Cannot build road path: invalid road.");

                return false;
            }

            if (corridor == null || corridor.Count < 2)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Cannot build road path: invalid corridor.");

                return false;
            }

            if (optimizedPath == null || optimizedPath.Count < 2)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Cannot build road path: invalid optimized path.");

                return false;
            }

            if (optimizedPortalIndices == null ||
                optimizedPortalIndices.Count != optimizedPath.Count)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Optimized portal indices do not match optimized path.");

                return false;
            }

            if (portals == null || portals.Count == 0)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Cannot build road path: no portals.");

                return false;
            }

            if (!TryFindBestRoadSection(
                    corridor,
                    road,
                    out RoadSection section))
            {
                Debug.LogError(
                    "[RoadPathProcessor] Could not find a continuous road section in corridor.");

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
                Debug.LogError(
                    "[RoadPathProcessor] Calculated road portal range is invalid.");

                return false;
            }

            if (!TryFindRoadPointRange(
                    road,
                    corridor,
                    section,
                    out int firstRoadPoint,
                    out int lastRoadPoint))
            {
                Debug.LogError(
                    "[RoadPathProcessor] Could not find road points covering the road section.");

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

            bool forwardValid = forward.IsValid;
            bool reverseValid = reverse.IsValid;

            if (!forwardValid && !reverseValid)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Neither forward nor reverse road traversal is valid.");

                return false;
            }

            RoadTraversal selected;

            if (forwardValid && reverseValid)
            {
                selected =
                    forward.TotalCost <= reverse.TotalCost
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

            for (int i = 0; i < corridor.Count; i++)
            {
                int hexId = corridor[i].Id;

                bool covered =
                    road.HexIds.Contains(hexId);

                bool hasRoadPoint =
                    HasRoadPointInHex(
                        road,
                        hexId);

                if (covered && hasRoadPoint)
                {
                    if (currentStart < 0)
                        currentStart = i;

                    currentLength++;

                    if (currentLength > bestLength)
                    {
                        bestLength = currentLength;
                        bestStart = currentStart;
                        bestEnd = i;
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
            for (int i = 0; i < road.PointHexIds.Count; i++)
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

            for (int i = 0; i < road.PointHexIds.Count; i++)
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
            for (int i = start; i <= end; i++)
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
            RoadSection section,
            int firstRoadPoint,
            int lastRoadPoint,
            Vector3 entryTarget,
            Vector3 exitTarget)
        {
            VisualHex entryHex =
                corridor[section.StartHexIndex];

            VisualHex exitHex =
                corridor[section.EndHexIndex];

            RoadPoint entry =
                TryFindBestEntryPointForward(
                    road,
                    firstRoadPoint,
                    entryHex,
                    entryTarget);

            if (!entry.IsValid)
                return RoadTraversal.Invalid;

            RoadPoint exit =
                FindBestExitPointForward(
                    road,
                    entry,
                    lastRoadPoint,
                    exitHex,
                    exitTarget);

            if (!exit.IsValid)
                return RoadTraversal.Invalid;

            if (exit.DistanceAlongRoad <=
                entry.DistanceAlongRoad)
            {
                return RoadTraversal.Invalid;
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
                corridor[section.EndHexIndex];

            VisualHex exitHex =
                corridor[section.StartHexIndex];

            RoadPoint entry =
                TryFindBestEntryPointReverse(
                    road,
                    lastRoadPoint,
                    entryHex,
                    entryTarget);

            if (!entry.IsValid)
                return RoadTraversal.Invalid;

            RoadPoint exit =
                FindBestExitPointReverse(
                    road,
                    entry,
                    firstRoadPoint,
                    exitHex,
                    exitTarget);

            if (!exit.IsValid)
                return RoadTraversal.Invalid;

            if (exit.DistanceAlongRoad >=
                entry.DistanceAlongRoad)
            {
                return RoadTraversal.Invalid;
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

        private RoadPoint TryFindBestEntryPointForward(
            VisualRoad road,
            int firstRoadPoint,
            VisualHex entryHex,
            Vector3 target)
        {
            return FindBestEntryPointInHex(
                road,
                firstRoadPoint,
                entryHex,
                target,
                true);
        }

        private RoadPoint TryFindBestEntryPointReverse(
            VisualRoad road,
            int lastRoadPoint,
            VisualHex entryHex,
            Vector3 target)
        {
            return FindBestEntryPointInHex(
                road,
                lastRoadPoint,
                entryHex,
                target,
                false);
        }

        private RoadPoint FindBestEntryPointInHex(
            VisualRoad road,
            int referenceRoadPoint,
            VisualHex hex,
            Vector3 target,
            bool forward)
        {
            RoadPoint best =
                RoadPoint.Invalid;

            float bestDistance =
                float.MaxValue;

            // --------------------------------------------------------
            // ROAD VERTICES
            // --------------------------------------------------------

            for (int i = 0; i < road.Points.Count; i++)
            {
                if (road.PointHexIds[i] != hex.Id)
                    continue;

                float distance =
                    PathMath.DistanceXZ(
                        road.Points[i],
                        target);

                if (distance < bestDistance)
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

            // --------------------------------------------------------
            // ROAD SEGMENTS
            // --------------------------------------------------------

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

                if (distance < bestDistance)
                {
                    bestDistance = distance;

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

            if (!best.IsValid)
            {
                Debug.LogError(
                    $"[RoadPathProcessor] Could not find entry point in hex {hex.Id}. " +
                    $"Forward: {forward}, ReferenceRoadPoint: {referenceRoadPoint}");
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
                return RoadPoint.Invalid;

            if (best.DistanceAlongRoad <=
                entry.DistanceAlongRoad)
            {
                Debug.LogError(
                    $"[RoadPathProcessor] Forward exit is behind entry. " +
                    $"Entry distance: {entry.DistanceAlongRoad}, " +
                    $"Exit distance: {best.DistanceAlongRoad}, " +
                    $"FinalHex: {finalHex.Id}, " +
                    $"LastRoadPoint: {lastRoadPoint}");

                return RoadPoint.Invalid;
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
                return RoadPoint.Invalid;

            if (best.DistanceAlongRoad >=
                entry.DistanceAlongRoad)
            {
                return RoadPoint.Invalid;
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
                Debug.LogError(
                    $"[RoadPathProcessor] Final hex {finalHex.Id} has invalid corners.");

                return RoadPoint.Invalid;
            }

            var samples =
                new List<ExitSample>();

            int sampleCount =
                Mathf.Max(
                    1,
                    settings.RoadExitSamplesPerSegment);

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

                if (!TryGetSegmentPortionInsideHex(
                        a,
                        b,
                        finalHex,
                        out float minT,
                        out float maxT))
                {
                    continue;
                }

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

                    roadDirection.y = 0f;

                    if (roadDirection.sqrMagnitude <=
                        GeometryTolerance)
                    {
                        continue;
                    }

                    roadDirection.Normalize();

                    if (!forward)
                        roadDirection = -roadDirection;

                    Vector3 toTarget =
                        target - position;

                    toTarget.y = 0f;

                    float distance =
                        PathMath.DistanceXZ(
                            position,
                            target);

                    if (toTarget.sqrMagnitude <=
                        GeometryTolerance)
                    {
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
                                0f));

                        continue;
                    }

                    toTarget.Normalize();

                    float angle =
                        Vector3.Angle(
                            roadDirection,
                            toTarget);

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
            {
                Debug.LogError(
                    $"[RoadPathProcessor] No exit samples found in final hex {finalHex.Id}.");

                return RoadPoint.Invalid;
            }

            float angleRange =
                Mathf.Clamp(
                    settings.RoadExitAngleRange,
                    0f,
                    MaximumExitAngle);

            while (true)
            {
                bool found =
                    false;

                ExitSample best =
                    ExitSample.Invalid;

                float bestDistance =
                    float.MaxValue;

                for (int i = 0;
                     i < samples.Count;
                     i++)
                {
                    ExitSample sample =
                        samples[i];

                    if (sample.Angle > angleRange)
                        continue;

                    found = true;

                    if (sample.DistanceToTarget <
                        bestDistance)
                    {
                        bestDistance =
                            sample.DistanceToTarget;

                        best =
                            sample;
                    }
                }

                if (found)
                {
                    return new RoadPoint(
                        best.Position,
                        best.SegmentIndex,
                        best.DistanceAlongRoad);
                }

                if (angleRange >= MaximumExitAngle)
                    break;

                angleRange =
                    Mathf.Min(
                        MaximumExitAngle,
                        Mathf.Max(
                            angleRange * 2f,
                            angleRange + 5f));
            }

            Debug.LogError(
                $"[RoadPathProcessor] Could not find a valid exit sample " +
                $"in final hex {finalHex.Id}.");

            return RoadPoint.Invalid;
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
                Debug.LogError(
                    $"[RoadPathProcessor] Hex {hex.Id} has invalid corners.");

                return false;
            }

            bool insideA =
                IsPointInsideHex(a, hex);

            bool insideB =
                IsPointInsideHex(b, hex);

            if (insideA && insideB)
                return true;

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

                return maxT >= minT;
            }

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

                return maxT >= minT;
            }

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
                        values[i] - value) <=
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
                    hasPositive = true;

                if (cross < -0.0001f)
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

        private float FindClosestT(
            List<float> values,
            float reference)
        {
            float best = values[0];

            float bestDistance =
                Mathf.Abs(
                    values[0] - reference);

            for (int i = 1;
                 i < values.Count;
                 i++)
            {
                float distance =
                    Mathf.Abs(
                        values[i] - reference);

                if (distance < bestDistance)
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
            float best = values[0];

            float bestDistance =
                Mathf.Abs(
                    values[0] - reference);

            for (int i = 1;
                 i < values.Count;
                 i++)
            {
                float distance =
                    Mathf.Abs(
                        values[i] - reference);

                if (distance > bestDistance)
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
            result = null;

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

            if (points.Count < 2)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Forward road path produced fewer than two points.");

                return false;
            }

            result =
                new RoadPathResult(
                    points,
                    roadFlags);

            return true;
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
            result = null;

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
                    road.Points[
                        endSegment + 1],
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

            if (points.Count < 2)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Reverse road path produced fewer than two points.");

                return false;
            }

            result =
                new RoadPathResult(
                    points,
                    roadFlags);

            return true;
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
            {
                return;
            }

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
            {
                return;
            }

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
                if (portalIndices[i] == portalIndex)
                    return i;
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
                Debug.LogError(
                    "[RoadPathProcessor] Road has null point data.");

                return false;
            }

            if (road.Points.Count < 2)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Road must contain at least two points.");

                return false;
            }

            if (road.PointHexIds.Count !=
                road.Points.Count)
            {
                Debug.LogError(
                    "[RoadPathProcessor] PointHexIds count must match Points count.");

                return false;
            }

            if (road.HexIds == null ||
                road.HexIds.Count == 0)
            {
                Debug.LogError(
                    "[RoadPathProcessor] Road contains no hex IDs.");

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
                StartHexIndex = startHexIndex;
                EndHexIndex = endHexIndex;
            }
        }

        private readonly struct RoadPoint
        {
            public readonly Vector3 Position;
            public readonly int SegmentIndex;
            public readonly float DistanceAlongRoad;

            public bool IsValid =>
                SegmentIndex >= 0;

            public static RoadPoint Invalid =>
                new RoadPoint(
                    Vector3.zero,
                    -1,
                    0f);

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

            public static RoadTraversal Invalid =>
                new RoadTraversal(
                    RoadPoint.Invalid,
                    RoadPoint.Invalid,
                    float.MaxValue,
                    false);

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

            public static ExitSample Invalid =>
                new ExitSample(
                    Vector3.zero,
                    -1,
                    0f,
                    0f,
                    float.MaxValue,
                    MaximumExitAngle);

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