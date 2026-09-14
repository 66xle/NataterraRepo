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

            // =========================================================
            // WORKING PATH
            //
            // repairedSegments[i] tells us whether the segment:
            //
            // path[i] -> path[i + 1]
            //
            // was already produced by a NavMesh repair.
            //
            // Those segments do not need to be checked again during
            // the later segment-validation pass.
            // =========================================================

            List<Vector3> workingPath =
                new List<Vector3>();

            List<bool> repairedSegments =
                new List<bool>();

            // =========================================================
            // POINT VALIDATION / REPAIR
            // =========================================================

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

            // =========================================================
            // SEGMENT VALIDATION / REPAIR
            //
            // This happens AFTER point repair.
            //
            // Any segment created by a point repair is already marked
            // as repaired and is therefore skipped.
            // =========================================================

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

            // =========================================================
            // FINAL VALIDATION
            // =========================================================

            if (workingPath.Count < 2)
            {
                return false;
            }

            // Project the repaired/validated path onto the NavMesh
            // before the final NavMesh checks.
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
        //
        // This is intentionally public because edge avoidance must be
        // applied to ALL final paths, not only fallback paths.
        //
        // The caller should pass a complete path after all NavMesh
        // repair/post-repair optimization has finished.
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

            // ---------------------------------------------------------
            // Edge avoidance modifies only interior points.
            //
            // Start and end therefore remain untouched.
            // ---------------------------------------------------------

            bool modified =
                ApplyEdgeAvoidance(
                    path);

            // ---------------------------------------------------------
            // If nothing moved, the path already satisfies the current
            // edge avoidance settings.
            //
            // There is no need to project or recalculate it.
            // ---------------------------------------------------------

            if (!modified)
            {
                return true;
            }

            // ---------------------------------------------------------
            // Edge avoidance moved points horizontally.
            //
            // Project the modified points back onto the NavMesh only
            // AFTER all edge avoidance iterations have completed.
            // ---------------------------------------------------------

            if (!ProjectPathToNavMeshHeight(
                    path))
            {
                return false;
            }

            // ---------------------------------------------------------
            // Edge avoidance can potentially move a point outside the
            // supplied corridor.
            // ---------------------------------------------------------

            if (!CorridorPathValidator.IsPathInsideCorridor(
                    path,
                    corridor))
            {
                return false;
            }

            // ---------------------------------------------------------
            // The modified path must now be directly walkable.
            //
            // Every segment must produce exactly two CalculatePath
            // corners.
            // ---------------------------------------------------------

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

                // -----------------------------------------------------
                // POINT IS VALID
                // -----------------------------------------------------

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

                // -----------------------------------------------------
                // POINT IS INVALID
                //
                // Find the closest valid point before and after it.
                // -----------------------------------------------------

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

                // -----------------------------------------------------
                // Calculate the NavMesh repair between the two valid
                // anchor points.
                // -----------------------------------------------------

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

                // -----------------------------------------------------
                // The previous valid source point should already be
                // represented by workingPath.
                //
                // The fallback contains beforePoint and afterPoint,
                // so add the fallback starting at point 1.
                // -----------------------------------------------------

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

                // -----------------------------------------------------
                // Everything from beforeIndex through afterIndex has
                // been replaced by the NavMesh solution.
                // -----------------------------------------------------

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

            // =========================================================
            // SEARCH BACKWARD
            // =========================================================

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

            // =========================================================
            // SEARCH FORWARD
            // =========================================================

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

            // =========================================================
            // VERIFY BOTH ANCHORS AGAIN
            // =========================================================

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
                // -----------------------------------------------------
                // Already repaired.
                // -----------------------------------------------------

                if (repairedSegments[segmentIndex])
                {
                    segmentIndex++;

                    continue;
                }

                Vector3 start =
                    path[segmentIndex];

                Vector3 end =
                    path[segmentIndex + 1];

                // -----------------------------------------------------
                // Segment is directly walkable.
                // -----------------------------------------------------

                if (IsSegmentWalkable(
                        start,
                        end))
                {
                    segmentIndex++;

                    continue;
                }

                // -----------------------------------------------------
                // Segment is invalid.
                // -----------------------------------------------------

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

                // -----------------------------------------------------
                // Replace:
                //
                // start -> end
                //
                // with:
                //
                // start -> fallback... -> end
                // -----------------------------------------------------

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

                // These segments were already generated by
                // TryBuildFallback(), so they are not checked again.
                segmentIndex +=
                    numberOfNewSegments;
            }

            return true;
        }

        // =============================================================
        // SHARED NAVMESH REPAIR
        //
        // No edge avoidance happens here.
        //
        // Edge avoidance is applied once to the complete path later,
        // after all repairs and post-repair optimization are complete.
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

            // =========================================================
            // PREPARE ANCHORS
            // =========================================================

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

            // =========================================================
            // CALCULATE PATH
            // =========================================================

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

            // =========================================================
            // PROJECT CALCULATED CORNERS
            // =========================================================

            if (!ProjectPathToNavMeshHeight(
                    path))
            {
                return false;
            }

            // =========================================================
            // CORRIDOR VALIDATION
            // =========================================================

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

            // =========================================================
            // FALLBACK IS VALID
            //
            // Edge avoidance is deliberately NOT applied here.
            // The complete path will receive edge avoidance after all
            // point and segment repairs have finished.
            // =========================================================

            fallbackPath =
                path;

            return true;
        }

        // =============================================================
        // PATH WALKABILITY
        //
        // Every point must be on the NavMesh.
        //
        // Every segment must produce exactly TWO CalculatePath
        // corners.
        //
        // More than two corners means CalculatePath needed to route
        // around something, so the original straight segment is not
        // considered directly walkable.
        // =============================================================

        private bool IsPathWalkable(
            IReadOnlyList<Vector3> path)
        {
            if (path == null ||
                path.Count < 2)
            {
                return false;
            }

            // ---------------------------------------------------------
            // POINT VALIDATION
            // ---------------------------------------------------------

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

            // ---------------------------------------------------------
            // SEGMENT VALIDATION
            // ---------------------------------------------------------

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
        //
        // IMPORTANT:
        //
        // corners.Length == 2 is required.
        //
        // 2 corners:
        //
        // A ---------------- B
        //
        // means the NavMesh can travel directly between the points.
        //
        // 3+ corners:
        //
        // A --------+
        //            |
        //            +------- B
        //
        // means CalculatePath had to route around something, so the
        // original segment is considered invalid.
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
        //
        // Returns true only if at least one point was actually moved.
        //
        // Start and end are intentionally excluded.
        // =============================================================

        private bool ApplyEdgeAvoidance(
            List<Vector3> path)
        {
            if (path == null ||
                path.Count < 2)
            {
                return false;
            }

            if (settings.NavMeshEdgeAvoidanceStrength <= 0f ||
                settings.NavMeshEdgeAvoidanceDistance <= 0f ||
                settings.NavMeshEdgeAvoidanceIterations <= 0)
            {
                return false;
            }

            bool modified =
                false;

            for (int iteration = 0;
                 iteration <
                 settings.NavMeshEdgeAvoidanceIterations;
                 iteration++)
            {
                for (int i = 1;
                     i < path.Count - 1;
                     i++)
                {
                    Vector3 point =
                        path[i];

                    if (!NavMesh.FindClosestEdge(
                            point,
                            out NavMeshHit edgeHit,
                            settings.NavMeshAreaMask))
                    {
                        continue;
                    }

                    float distance =
                        PathMath.DistanceXZ(
                            point,
                            edgeHit.position);

                    if (distance >=
                        settings.NavMeshEdgeAvoidanceDistance)
                    {
                        continue;
                    }

                    Vector3 away =
                        point -
                        edgeHit.position;

                    away.y =
                        0f;

                    float magnitude =
                        away.magnitude;

                    if (magnitude <=
                        0.001f)
                    {
                        continue;
                    }

                    away /=
                        magnitude;

                    float normalized =
                        1f -
                        Mathf.Clamp01(
                            distance /
                            settings.NavMeshEdgeAvoidanceDistance);

                    float offset =
                        normalized *
                        settings.NavMeshEdgeAvoidanceStrength;

                    Vector3 newPoint =
                        point +
                        away *
                        offset;

                    // Do not project here.
                    //
                    // Projection happens once after ALL edge avoidance
                    // iterations have completed.
                    path[i] =
                        newPoint;

                    modified =
                        true;
                }
            }

            return modified;
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

            // ---------------------------------------------------------
            // Find the first point outside the corridor.
            // ---------------------------------------------------------

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

            // ---------------------------------------------------------
            // Find the actual map hex containing the outside point.
            // ---------------------------------------------------------

            if (!TryFindContainingHex(
                    outsidePoint,
                    allHexes,
                    out VisualHex outsideHex))
            {
                return null;
            }

            var requests =
                new List<NavMeshBlockerRequest>();

            // ---------------------------------------------------------
            // Find every corridor hex sharing an actual edge with the
            // outside hex.
            // ---------------------------------------------------------

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