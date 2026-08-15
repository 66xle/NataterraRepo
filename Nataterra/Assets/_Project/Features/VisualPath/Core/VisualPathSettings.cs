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
        /// <summary>
        /// Determines whether the normal optimised path
        /// or a valid road path gets priority.
        /// </summary>
        public PathPriorityMode PathPriority = PathPriorityMode.OptimizedPathFirst;

        /// <summary>
        /// Determines how portal edge avoidance works.
        /// </summary>
        public EdgeAvoidanceMode EdgeAvoidanceMode = EdgeAvoidanceMode.PortalCenterBias;

        /// <summary>
        /// Amount used by the selected edge avoidance mode.
        ///
        /// PortalCenterBias:
        /// 0 = no bias
        /// 1 = completely centre
        ///
        /// StayAwayFromEdge:
        /// 0.0 = can use entire portal
        /// 0.2 = stay at least 20% from either end
        /// 0.5 = only centre point is available
        /// </summary>
        public float EdgeAvoidanceAmount = 0.5f;

        /// <summary>
        /// Physical margin from the ends of the shared edge.
        /// This is applied before the edge avoidance mode.
        /// </summary>
        public float EdgeMargin = 0.5f;

        /// <summary>
        /// Number of portal optimisation passes.
        /// </summary>
        public int PortalRelaxationPasses = 4;

        /// <summary>
        /// Maximum angle considered effectively straight.
        /// </summary>
        public float StraightAngleTolerance = 6f;

        /// <summary>
        /// Number of smoothing samples.
        /// </summary>
        public int SmoothingSamples = 2;

        /// <summary>
        /// How aggressively turns are rounded.
        /// </summary>
        public float SmoothingRatio = 0.22f;

        /// <summary>
        /// Distance within which a road is considered
        /// to intersect a portal.
        /// </summary>
        public float RoadPortalTolerance = 1f;

        public int RoadExitSamplesPerSegment = 10;

        public float RoadExitAngleRange = 30f;
    }
}