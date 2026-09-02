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

        /// <summary>
        /// Enables NavMesh walkability validation and fallback routing.
        /// </summary>
        public bool UseNavMesh =
            false;

        /// <summary>
        /// NavMeshData associated with this path request.
        ///
        /// The NavMeshData must already be registered with Unity's
        /// NavMesh system before path generation.
        /// </summary>
        public UnityEngine.AI.NavMeshData NavMeshData;

        /// <summary>
        /// Areas considered walkable.
        /// </summary>
        public int NavMeshAreaMask =
            UnityEngine.AI.NavMesh.AllAreas;

        /// <summary>
        /// Maximum distance used when projecting a visual path point
        /// onto the NavMesh.
        /// </summary>
        public float NavMeshSampleDistance =
            0.5f;

        /// <summary>
        /// Maximum allowed vertical difference between the visual path
        /// point and the NavMesh.
        ///
        /// This is evaluated independently from NavMeshSampleDistance.
        /// </summary>
        public float NavMeshHeightTolerance =
            1f;

        /// <summary>
        /// Desired distance between fallback path points when additional
        /// validation samples are required.
        /// </summary>
        public float NavMeshValidationSampleDistance =
            0.5f;

        /// <summary>
        /// Desired physical clearance from NavMesh boundaries for
        /// fallback path corners.
        /// </summary>
        public float NavMeshEdgeAvoidanceDistance =
            0.5f;

        /// <summary>
        /// Number of times NavMesh fallback corners may be pushed away
        /// from nearby NavMesh edges.
        /// </summary>
        public int NavMeshEdgeAvoidanceIterations =
            2;

        /// <summary>
        /// Strength of NavMesh edge avoidance.
        ///
        /// 0 = no movement.
        /// 1 = full requested clearance.
        /// </summary>
        public float NavMeshEdgeAvoidanceStrength =
            1f;

        /// <summary>
        /// If true, the smoothed path is also validated against the
        /// NavMesh and corridor.
        /// </summary>
        public bool ValidateSmoothedPath =
            true;
    }
}