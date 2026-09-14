using UnityEngine;

namespace VisualPath
{
    public enum PathPriorityMode
    {
        OptimizedPathFirst,
        PreferRoads
    }

    public enum EdgeAvoidanceMode
    {
        PortalCenterBias,
        StayAwayFromEdge
    }

    public sealed class VisualPathSettings
    {
        public PathPriorityMode PathPriority =
            PathPriorityMode.OptimizedPathFirst;

        public EdgeAvoidanceMode EdgeAvoidanceMode =
            EdgeAvoidanceMode.PortalCenterBias;

        public float EdgeAvoidanceAmount =
            0.5f;

        public float EdgeMargin =
            0.5f;

        public int PortalRelaxationPasses =
            4;

        public float StraightAngleTolerance =
            6f;

        public int SmoothingSamples =
            2;

        public float SmoothingRatio =
            0.22f;

        public float RoadPortalTolerance =
            1f;

        public int RoadExitSamplesPerSegment =
            10;

        public float RoadExitAngleRange =
            30f;

        // =============================================================
        // NAVMESH
        // =============================================================

        public bool UseNavMesh =
            false;

        public UnityEngine.AI.NavMeshData NavMeshData;

        public int NavMeshAreaMask =
            UnityEngine.AI.NavMesh.AllAreas;

        public float NavMeshSampleDistance =
            0.5f;

        public float NavMeshHeightTolerance =
            1f;

        public float NavMeshValidationSampleDistance =
            0.5f;

        public float NavMeshEdgeAvoidanceDistance =
            0.5f;

        public int NavMeshEdgeAvoidanceIterations =
            2;

        public float NavMeshEdgeAvoidanceStrength =
            1f;

        public bool ValidateSmoothedPath =
            true;

        // =============================================================
        // TEMPORARY NAVMESH BLOCKERS
        // =============================================================

        /// <summary>
        /// Maximum number of temporary corridor-edge blockers that may
        /// be created during a single generation attempt.
        /// </summary>
        public int NavMeshMaxBlockerRetries =
            10;

        /// <summary>
        /// Amount added to the blocker width beyond the actual shared
        /// hex edge.
        ///
        /// A small value helps ensure the carved obstacle fully closes
        /// the entrance.
        /// </summary>
        public float NavMeshBlockerEdgePadding =
            0.05f;

        /// <summary>
        /// Multiplier applied to the prefab's local X scale when sizing
        /// the blocker to the shared edge.
        ///
        /// The prefab's local X axis is assumed to be its width.
        /// </summary>
        public float NavMeshBlockerWidthMultiplier =
            1f;
    }
}