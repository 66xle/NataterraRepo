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
            this.settings =
                settings;
        }

        // =============================================================
        // PUBLIC
        // =============================================================

        public bool TryValidateAndRepairPath(
            IReadOnlyList<Vector3> sourcePath,
            IReadOnlyList<VisualHex> corridor,
            IReadOnlyList<VisualHex> allHexes,
            out List<Vector3> result,
            out bool usedFallback,
            out List<NavMeshBlockerRequest> blockerRequests)
        {
            result = null;
            usedFallback = false;
            blockerRequests = null;

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

            List<Vector3> workingPath =
                new List<Vector3>();

            List<bool> repairedSegments =
                new List<bool>();

            if (!TryValidateAndRepairPoints(
                    sourcePath,
                    corridor,
                    allHexes,
                    workingPath,
                    repairedSegments,
                    out blockerRequests))
            {
                usedFallback =
                    blockerRequests != null &&
                    blockerRequests.Count > 0;

                return false;
            }

            if (!TryValidateAndRepairSegments(
                    workingPath,
                    repairedSegments,
                    corridor,
                    allHexes,
                    out blockerRequests))
            {
                usedFallback =
                    blockerRequests != null &&
                    blockerRequests.Count > 0;

                return false;
            }

            if (workingPath.Count < 2)
            {
                return false;
            }

            if (!ProjectPathToNavMeshHeight(
                    workingPath))
            {
                return false;
            }

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    workingPath,
                    corridor))
            {
                return false;
            }

            if (!IsPathWalkable(
                    workingPath))
            {
                return false;
            }

            result =
                workingPath;

            usedFallback =
                ContainsRepairedSegments(
                    repairedSegments);

            return true;
        }

        // =============================================================
        // NAVMESH EDGE AVOIDANCE
        // =============================================================

        public bool TryApplyEdgeAvoidance(
            List<Vector3> path,
            IReadOnlyList<VisualHex> corridor)
        {
            if (path == null ||
                path.Count < 2)
            {
                return false;
            }

            if (corridor == null ||
                corridor.Count == 0)
            {
                return false;
            }

            bool modified =
                ApplyEdgeAvoidance(
                    path,
                    corridor);

            if (!modified)
            {
                return true;
            }

            if (!ProjectPathToNavMeshHeight(
                    path))
            {
                return false;
            }

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    path,
                    corridor))
            {
                return false;
            }

            if (!IsPathWalkable(
                    path))
            {
                return false;
            }

            return true;
        }

        // =============================================================
        // POINT VALIDATION / REPAIR
        // =============================================================

        private bool TryValidateAndRepairPoints(
            IReadOnlyList<Vector3> sourcePath,
            IReadOnlyList<VisualHex> corridor,
            IReadOnlyList<VisualHex> allHexes,
            List<Vector3> workingPath,
            List<bool> repairedSegments,
            out List<NavMeshBlockerRequest> blockerRequests)
        {
            blockerRequests = null;

            int currentIndex = 0;

            while (currentIndex <
                   sourcePath.Count)
            {
                Vector3 point =
                    sourcePath[currentIndex];

                if (TrySampleNavMesh(
                        point,
                        out NavMeshHit hit))
                {
                    Vector3 validPoint =
                        hit.position;

                    AddPoint(
                        workingPath,
                        repairedSegments,
                        validPoint,
                        false);

                    currentIndex++;

                    continue;
                }

                if (!TryFindPointRepairBounds(
                        sourcePath,
                        currentIndex,
                        corridor,
                        out int beforeIndex,
                        out int afterIndex,
                        out Vector3 beforePoint,
                        out Vector3 afterPoint))
                {
                    return false;
                }

                if (!TryBuildFallback(
                        beforePoint,
                        afterPoint,
                        corridor,
                        allHexes,
                        out List<Vector3> fallbackPath,
                        out List<NavMeshBlockerRequest> localBlockers))
                {
                    blockerRequests =
                        localBlockers;

                    return false;
                }

                if (fallbackPath == null ||
                    fallbackPath.Count < 2)
                {
                    return false;
                }

                for (int i = 0;
                     i < fallbackPath.Count;
                     i++)
                {
                    Vector3 fallbackPoint =
                        fallbackPath[i];

                    if (i == 0)
                    {
                        if (workingPath.Count == 0 ||
                            PathMath.DistanceXZ(
                                workingPath[
                                    workingPath.Count - 1],
                                fallbackPoint) > 0.01f)
                        {
                            AddPoint(
                                workingPath,
                                repairedSegments,
                                fallbackPoint,
                                true);
                        }

                        continue;
                    }

                    AddPoint(
                        workingPath,
                        repairedSegments,
                        fallbackPoint,
                        true);
                }

                currentIndex =
                    afterIndex + 1;
            }

            return true;
        }

        // =============================================================
        // FIND POINT REPAIR BOUNDS
        // =============================================================

        private bool TryFindPointRepairBounds(
            IReadOnlyList<Vector3> sourcePath,
            int invalidIndex,
            IReadOnlyList<VisualHex> corridor,
            out int beforeIndex,
            out int afterIndex,
            out Vector3 beforePoint,
            out Vector3 afterPoint)
        {
            beforeIndex = -1;
            afterIndex = -1;

            beforePoint = default;
            afterPoint = default;

            for (int i = invalidIndex - 1;
                 i >= 0;
                 i--)
            {
                if (!TrySampleNavMesh(
                        sourcePath[i],
                        out NavMeshHit hit))
                {
                    continue;
                }

                Vector3 candidate =
                    hit.position;

                if (!IsPointInsideCorridor(
                        candidate,
                        corridor))
                {
                    continue;
                }

                beforeIndex =
                    i;

                beforePoint =
                    candidate;

                break;
            }

            if (beforeIndex < 0)
            {
                return false;
            }

            for (int i = invalidIndex + 1;
                 i < sourcePath.Count;
                 i++)
            {
                if (!TrySampleNavMesh(
                        sourcePath[i],
                        out NavMeshHit hit))
                {
                    continue;
                }

                Vector3 candidate =
                    hit.position;

                if (!IsPointInsideCorridor(
                        candidate,
                        corridor))
                {
                    continue;
                }

                afterIndex =
                    i;

                afterPoint =
                    candidate;

                break;
            }

            if (afterIndex < 0)
            {
                return false;
            }

            if (afterIndex <= beforeIndex)
            {
                return false;
            }

            if (!TrySampleNavMesh(
                    beforePoint,
                    out NavMeshHit verifiedBefore))
            {
                return false;
            }

            if (!TrySampleNavMesh(
                    afterPoint,
                    out NavMeshHit verifiedAfter))
            {
                return false;
            }

            beforePoint =
                verifiedBefore.position;

            afterPoint =
                verifiedAfter.position;

            if (!IsPointInsideCorridor(
                    beforePoint,
                    corridor))
            {
                return false;
            }

            if (!IsPointInsideCorridor(
                    afterPoint,
                    corridor))
            {
                return false;
            }

            return true;
        }

        // =============================================================
        // SEGMENT VALIDATION / REPAIR
        // =============================================================

        private bool TryValidateAndRepairSegments(
            List<Vector3> path,
            List<bool> repairedSegments,
            IReadOnlyList<VisualHex> corridor,
            IReadOnlyList<VisualHex> allHexes,
            out List<NavMeshBlockerRequest> blockerRequests)
        {
            blockerRequests = null;

            int segmentIndex = 0;

            while (segmentIndex <
                   path.Count - 1)
            {
                if (repairedSegments[segmentIndex])
                {
                    segmentIndex++;

                    continue;
                }

                Vector3 start =
                    path[segmentIndex];

                Vector3 end =
                    path[segmentIndex + 1];

                if (IsSegmentWalkable(
                        start,
                        end))
                {
                    segmentIndex++;

                    continue;
                }

                if (!TryBuildFallback(
                        start,
                        end,
                        corridor,
                        allHexes,
                        out List<Vector3> fallbackPath,
                        out List<NavMeshBlockerRequest> localBlockers))
                {
                    blockerRequests =
                        localBlockers;

                    return false;
                }

                if (fallbackPath == null ||
                    fallbackPath.Count < 2)
                {
                    return false;
                }

                int oldSegmentEndIndex =
                    segmentIndex + 1;

                path.RemoveAt(
                    oldSegmentEndIndex);

                repairedSegments.RemoveAt(
                    segmentIndex);

                for (int i = fallbackPath.Count - 1;
                     i >= 1;
                     i--)
                {
                    path.Insert(
                        oldSegmentEndIndex,
                        fallbackPath[i]);
                }

                int numberOfNewSegments =
                    fallbackPath.Count - 1;

                for (int i = 0;
                     i < numberOfNewSegments;
                     i++)
                {
                    repairedSegments.Insert(
                        segmentIndex + i,
                        true);
                }

                segmentIndex +=
                    numberOfNewSegments;
            }

            return true;
        }

        // =============================================================
        // SHARED NAVMESH REPAIR
        // =============================================================

        private bool TryBuildFallback(
            Vector3 beforePoint,
            Vector3 afterPoint,
            IReadOnlyList<VisualHex> corridor,
            IReadOnlyList<VisualHex> allHexes,
            out List<Vector3> fallbackPath,
            out List<NavMeshBlockerRequest> blockerRequests)
        {
            fallbackPath = null;
            blockerRequests = null;

            List<Vector3> anchors =
                new List<Vector3>
                {
                    beforePoint,
                    afterPoint
                };

            if (!ProjectPathToNavMeshHeight(
                    anchors))
            {
                return false;
            }

            beforePoint =
                anchors[0];

            afterPoint =
                anchors[1];

            NavMeshPath navMeshPath =
                new NavMeshPath();

            bool calculated =
                NavMesh.CalculatePath(
                    beforePoint,
                    afterPoint,
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

            List<Vector3> path =
                new List<Vector3>(
                    navMeshPath.corners);

            if (!ProjectPathToNavMeshHeight(
                    path))
            {
                return false;
            }

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    path,
                    corridor))
            {
                blockerRequests =
                    TryFindOutsideHexBlockers(
                        path,
                        corridor,
                        allHexes);

                return false;
            }

            fallbackPath =
                path;

            return true;
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
            NavMeshPath path =
                new NavMeshPath();

            bool calculated =
                NavMesh.CalculatePath(
                    start,
                    end,
                    settings.NavMeshAreaMask,
                    path);

            if (!calculated)
            {
                return false;
            }

            if (path.status !=
                NavMeshPathStatus.PathComplete)
            {
                return false;
            }

            if (path.corners == null ||
                path.corners.Length != 2)
            {
                return false;
            }

            return true;
        }

        // =============================================================
        // NAVMESH SAMPLING
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
        // PROJECT PATH TO NAVMESH HEIGHT
        // =============================================================

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
                if (!TrySampleNavMesh(
                        path[i],
                        out NavMeshHit hit))
                {
                    return false;
                }

                Vector3 point =
                    path[i];

                point.y =
                    hit.position.y;

                path[i] =
                    point;
            }

            return true;
        }

        // =============================================================
        // EDGE AVOIDANCE
        // =============================================================

        private bool ApplyEdgeAvoidance(
            List<Vector3> path,
            IReadOnlyList<VisualHex> corridor)
        {
            if (path == null ||
                path.Count < 3)
            {
                return false;
            }

            if (corridor == null ||
                corridor.Count == 0)
            {
                return false;
            }

            if (settings.NavMeshEdgeAvoidanceStrength <= 0f ||
                settings.NavMeshEdgeAvoidanceDistance <= 0f ||
                settings.NavMeshEdgeAvoidanceIterations <= 0)
            {
                return false;
            }

            float strength =
                Mathf.Clamp01(
                    settings.NavMeshEdgeAvoidanceStrength);

            float avoidanceDistance =
                settings.NavMeshEdgeAvoidanceDistance;

            bool modified =
                false;

            for (int iteration = 0;
                 iteration <
                 settings.NavMeshEdgeAvoidanceIterations;
                 iteration++)
            {
                bool changedThisIteration =
                    false;

                for (int i = 1;
                     i < path.Count - 1;
                     i++)
                {
                    Vector3 point =
                        path[i];

                    if (!NavMesh.SamplePosition(
                            point,
                            out NavMeshHit pointHit,
                            settings.NavMeshSampleDistance,
                            settings.NavMeshAreaMask))
                    {
                        continue;
                    }

                    point =
                        pointHit.position;

                    if (!NavMesh.FindClosestEdge(
                            point,
                            out NavMeshHit edgeHit,
                            settings.NavMeshAreaMask))
                    {
                        continue;
                    }

                    float currentDistance =
                        PathMath.DistanceXZ(
                            point,
                            edgeHit.position);

                    if (currentDistance >=
                        avoidanceDistance)
                    {
                        path[i] =
                            point;

                        continue;
                    }

                    float requiredOffset =
                        avoidanceDistance -
                        currentDistance;

                    float offset =
                        requiredOffset *
                        strength;

                    if (offset <=
                        0.0001f)
                    {
                        continue;
                    }

                    // Determine the direction away from the edge.
                    //
                    // If the point is not on the edge, the direction
                    // comes directly from edge to point.
                    //
                    // If the point is effectively on the edge, the
                    // direction cannot be determined from those two
                    // positions, so candidate directions are generated.

                    if (!TryGetEdgeAvoidanceDirection(
                            path,
                            corridor,
                            i,
                            point,
                            edgeHit,
                            offset,
                            out Vector3 away))
                    {
                        continue;
                    }

                    Vector3 candidate =
                        point +
                        away *
                        offset;

                    // Validate the candidate against the NavMesh.
                    //
                    // The candidate itself is retained. We do not
                    // replace it with the SamplePosition result.

                    if (!NavMesh.SamplePosition(
                            candidate,
                            out _,
                            settings.NavMeshSampleDistance,
                            settings.NavMeshAreaMask))
                    {
                        continue;
                    }

                    if (!IsPointInsideCorridor(
                            candidate,
                            corridor))
                    {
                        continue;
                    }

                    // Compare against the original edge position.

                    float candidateDistance =
                        PathMath.DistanceXZ(
                            candidate,
                            edgeHit.position);

                    if (candidateDistance <=
                        currentDistance + 0.0001f)
                    {
                        continue;
                    }

                    // Make sure moving this waypoint does not break
                    // either connected segment.

                    if (!IsEdgeAvoidanceCandidateValid(
                            path,
                            i,
                            candidate))
                    {
                        continue;
                    }

                    path[i] =
                        candidate;

                    modified =
                        true;

                    changedThisIteration =
                        true;
                }

                if (!changedThisIteration)
                {
                    break;
                }
            }

            return modified;
        }

        // =============================================================
        // GET EDGE AVOIDANCE DIRECTION
        //
        // First try to determine the direction directly from:
        //
        // point - edge position
        //
        // If that vector is effectively zero, generate candidates to
        // determine which direction moves the point away from the edge.
        // =============================================================

        private bool TryGetEdgeAvoidanceDirection(
            List<Vector3> path,
            IReadOnlyList<VisualHex> corridor,
            int index,
            Vector3 point,
            NavMeshHit edgeHit,
            float offset,
            out Vector3 direction)
        {
            direction =
                point -
                edgeHit.position;

            direction.y =
                0f;

            float magnitude =
                direction.magnitude;

            // We already know the direction away from the edge.

            if (magnitude > 0.001f)
            {
                direction /=
                    magnitude;

                return true;
            }

            // We cannot determine a direction from the point and edge
            // position because they are effectively the same position.
            //
            // Generate candidate positions to determine the direction.

            return TryGetDirectionFromCandidates(
                path,
                corridor,
                index,
                point,
                edgeHit,
                offset,
                out direction);
        }

        // =============================================================
        // GET DIRECTION FROM CANDIDATES
        //
        // Candidate positions are generated around the current point.
        //
        // The best valid candidate is the one that:
        //
        // 1. Is on the NavMesh.
        // 2. Is inside the corridor.
        // 3. Preserves both connected path segments.
        // 4. Produces the greatest distance from the original edge hit.
        //
        // The returned value is only the direction.
        // The caller performs the actual movement.
        // =============================================================

        private bool TryGetDirectionFromCandidates(
            List<Vector3> path,
            IReadOnlyList<VisualHex> corridor,
            int index,
            Vector3 point,
            NavMeshHit edgeHit,
            float offset,
            out Vector3 direction)
        {
            direction =
                default;

            List<Vector3> candidates =
                new List<Vector3>();

            Vector3 normal =
                edgeHit.normal;

            normal.y =
                0f;

            if (normal.sqrMagnitude >
                0.000001f)
            {
                normal.Normalize();

                candidates.Add(
                    point +
                    normal *
                    offset);

                candidates.Add(
                    point -
                    normal *
                    offset);
            }

            // If the edge normal cannot provide a usable direction,
            // try horizontal directions around the point.

            if (candidates.Count == 0)
            {
                const int radialCount = 8;

                for (int i = 0;
                     i < radialCount;
                     i++)
                {
                    float angle =
                        (360f / radialCount) *
                        i;

                    float radians =
                        angle *
                        Mathf.Deg2Rad;

                    Vector3 radial =
                        new Vector3(
                            Mathf.Cos(radians),
                            0f,
                            Mathf.Sin(radians));

                    candidates.Add(
                        point +
                        radial *
                        offset);
                }
            }

            float bestDistance =
                PathMath.DistanceXZ(
                    point,
                    edgeHit.position);

            bool found =
                false;

            Vector3 bestCandidate =
                point;

            for (int i = 0;
                 i < candidates.Count;
                 i++)
            {
                Vector3 candidate =
                    candidates[i];

                if (!NavMesh.SamplePosition(
                        candidate,
                        out _,
                        settings.NavMeshSampleDistance,
                        settings.NavMeshAreaMask))
                {
                    continue;
                }

                if (!IsPointInsideCorridor(
                        candidate,
                        corridor))
                {
                    continue;
                }

                float candidateDistance =
                    PathMath.DistanceXZ(
                        candidate,
                        edgeHit.position);

                if (candidateDistance <=
                    bestDistance + 0.0001f)
                {
                    continue;
                }

                if (!IsEdgeAvoidanceCandidateValid(
                        path,
                        index,
                        candidate))
                {
                    continue;
                }

                bestDistance =
                    candidateDistance;

                bestCandidate =
                    candidate;

                found =
                    true;
            }

            if (!found)
            {
                return false;
            }

            Vector3 result =
                bestCandidate -
                point;

            result.y =
                0f;

            float resultMagnitude =
                result.magnitude;

            if (resultMagnitude <=
                0.001f)
            {
                return false;
            }

            direction =
                result /
                resultMagnitude;

            return true;
        }

        // =============================================================
        // EDGE AVOIDANCE CANDIDATE VALIDATION
        // =============================================================

        private bool IsEdgeAvoidanceCandidateValid(
            List<Vector3> path,
            int index,
            Vector3 candidate)
        {
            if (path == null ||
                index <= 0 ||
                index >= path.Count - 1)
            {
                return false;
            }

            if (!TrySampleNavMesh(
                    candidate,
                    out _))
            {
                return false;
            }

            if (!IsSegmentWalkable(
                    path[index - 1],
                    candidate))
            {
                return false;
            }

            if (!IsSegmentWalkable(
                    candidate,
                    path[index + 1]))
            {
                return false;
            }

            return true;
        }

        // =============================================================
        // ADD PATH POINT
        // =============================================================

        private void AddPoint(
            List<Vector3> path,
            List<bool> repairedSegments,
            Vector3 point,
            bool repaired)
        {
            if (path.Count == 0)
            {
                path.Add(point);

                return;
            }

            Vector3 last =
                path[path.Count - 1];

            if (PathMath.DistanceXZ(
                    last,
                    point) <= 0.01f)
            {
                path[path.Count - 1] =
                    point;

                return;
            }

            path.Add(point);

            repairedSegments.Add(
                repaired);
        }

        // =============================================================
        // REPAIRED SEGMENT CHECK
        // =============================================================

        private bool ContainsRepairedSegments(
            IReadOnlyList<bool> repairedSegments)
        {
            if (repairedSegments == null)
            {
                return false;
            }

            for (int i = 0;
                 i < repairedSegments.Count;
                 i++)
            {
                if (repairedSegments[i])
                {
                    return true;
                }
            }

            return false;
        }

        // =============================================================
        // CORRIDOR POINT TEST
        // =============================================================

        private bool IsPointInsideCorridor(
            Vector3 point,
            IReadOnlyList<VisualHex> corridor)
        {
            if (corridor == null ||
                corridor.Count == 0)
            {
                return false;
            }

            for (int i = 0;
                 i < corridor.Count;
                 i++)
            {
                if (IsPointInsideHex(
                        point,
                        corridor[i]))
                {
                    return true;
                }
            }

            return false;
        }

        // =============================================================
        // POINT IN HEX
        // =============================================================

        private bool IsPointInsideHex(
            Vector3 point,
            VisualHex hex)
        {
            if (hex.Corners == null ||
                hex.Corners.Length < 3)
            {
                return false;
            }

            bool inside =
                false;

            float px =
                point.x;

            float pz =
                point.z;

            for (int i = 0,
                     j = hex.Corners.Length - 1;
                 i < hex.Corners.Length;
                 j = i++)
            {
                float xi =
                    hex.Corners[i].x;

                float zi =
                    hex.Corners[i].z;

                float xj =
                    hex.Corners[j].x;

                float zj =
                    hex.Corners[j].z;

                bool intersects =
                    ((zi > pz) != (zj > pz)) &&
                    (px <
                     (xj - xi) *
                     (pz - zi) /
                     ((zj - zi) == 0f
                         ? 0.000001f
                         : (zj - zi)) +
                     xi);

                if (intersects)
                {
                    inside =
                        !inside;
                }
            }

            return inside;
        }

        // =============================================================
        // OUTSIDE-CORRIDOR BLOCKERS
        // =============================================================

        private List<NavMeshBlockerRequest> TryFindOutsideHexBlockers(
            IReadOnlyList<Vector3> fallbackPath,
            IReadOnlyList<VisualHex> corridor,
            IReadOnlyList<VisualHex> allHexes)
        {
            if (fallbackPath == null ||
                fallbackPath.Count == 0 ||
                corridor == null ||
                corridor.Count == 0 ||
                allHexes == null ||
                allHexes.Count == 0)
            {
                return null;
            }

            Vector3 outsidePoint =
                default;

            bool foundOutsidePoint =
                false;

            for (int i = 0;
                 i < fallbackPath.Count;
                 i++)
            {
                if (!IsPointInsideCorridor(
                        fallbackPath[i],
                        corridor))
                {
                    outsidePoint =
                        fallbackPath[i];

                    foundOutsidePoint =
                        true;

                    break;
                }
            }

            if (!foundOutsidePoint)
            {
                return null;
            }

            if (!TryFindContainingHex(
                    outsidePoint,
                    allHexes,
                    out VisualHex outsideHex))
            {
                return null;
            }

            var requests =
                new List<NavMeshBlockerRequest>();

            for (int i = 0;
                 i < corridor.Count;
                 i++)
            {
                VisualHex corridorHex =
                    corridor[i];

                if (!HexPathGeometry.TryGetSharedEdge(
                        outsideHex,
                        corridorHex,
                        out Vector3 edgeA,
                        out Vector3 edgeB))
                {
                    continue;
                }

                AddBlockerRequestUnique(
                    requests,
                    new NavMeshBlockerRequest(
                        corridorHex,
                        edgeA,
                        edgeB));
            }

            return requests;
        }

        // =============================================================
        // FIND CONTAINING HEX
        // =============================================================

        private bool TryFindContainingHex(
            Vector3 point,
            IReadOnlyList<VisualHex> allHexes,
            out VisualHex result)
        {
            result = default;

            for (int i = 0;
                 i < allHexes.Count;
                 i++)
            {
                VisualHex hex =
                    allHexes[i];

                if (!IsPointInsideHex(
                        point,
                        hex))
                {
                    continue;
                }

                result =
                    hex;

                return true;
            }

            return false;
        }

        // =============================================================
        // UNIQUE BLOCKER REQUEST
        // =============================================================

        private void AddBlockerRequestUnique(
            List<NavMeshBlockerRequest> requests,
            NavMeshBlockerRequest request)
        {
            for (int i = 0;
                 i < requests.Count;
                 i++)
            {
                NavMeshBlockerRequest existing =
                    requests[i];

                if (existing.CorridorHex.Id !=
                    request.CorridorHex.Id)
                {
                    continue;
                }

                bool sameForward =
                    PathMath.DistanceXZ(
                        existing.EdgeA,
                        request.EdgeA) <= 0.01f &&
                    PathMath.DistanceXZ(
                        existing.EdgeB,
                        request.EdgeB) <= 0.01f;

                bool sameReverse =
                    PathMath.DistanceXZ(
                        existing.EdgeA,
                        request.EdgeB) <= 0.01f &&
                    PathMath.DistanceXZ(
                        existing.EdgeB,
                        request.EdgeA) <= 0.01f;

                if (sameForward ||
                    sameReverse)
                {
                    return;
                }
            }

            requests.Add(request);
        }
    }
}