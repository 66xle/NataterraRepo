using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class PortalPathSolver
    {
        private readonly VisualPathSettings settings;

        public PortalPathSolver(
            VisualPathSettings settings)
        {
            this.settings = settings;
        }

        public PortalPathSolution Solve(
            Vector3 start,
            Vector3 end,
            IReadOnlyList<PathPortal> portals)
        {
            if (portals == null ||
                portals.Count == 0)
            {
                var directPoints =
                    new List<PortalPathPoint>
                    {
                        new PortalPathPoint(
                            start,
                            -1),

                        new PortalPathPoint(
                            end,
                            -1)
                    };

                return new PortalPathSolution(
                    directPoints);
            }

            var crossings =
                new Vector3[portals.Count];

            // ---------------------------------------------------------
            // Start all crossings at the centre of their portals.
            // ---------------------------------------------------------

            for (int i = 0;
                 i < portals.Count;
                 i++)
            {
                crossings[i] =
                    portals[i].Center;
            }

            // ---------------------------------------------------------
            // Relax portal crossing positions.
            // ---------------------------------------------------------

            int relaxationPasses =
                Mathf.Max(
                    0,
                    settings.PortalRelaxationPasses);

            for (int pass = 0;
                 pass < relaxationPasses;
                 pass++)
            {
                for (int i = 0;
                     i < portals.Count;
                     i++)
                {
                    Vector3 previous =
                        i == 0
                            ? start
                            : crossings[i - 1];

                    Vector3 next =
                        i == portals.Count - 1
                            ? end
                            : crossings[i + 1];

                    Vector3 shortest =
                        FindShortestPortalPoint(
                            portals[i],
                            previous,
                            next);

                    crossings[i] =
                        ApplyEdgeAvoidance(
                            portals[i],
                            shortest);
                }
            }

            // ---------------------------------------------------------
            // Build solution.
            // ---------------------------------------------------------

            var points =
                new List<PortalPathPoint>(
                    portals.Count + 2);

            points.Add(
                new PortalPathPoint(
                    start,
                    -1));

            for (int i = 0;
                 i < crossings.Length;
                 i++)
            {
                points.Add(
                    new PortalPathPoint(
                        crossings[i],
                        i));
            }

            points.Add(
                new PortalPathPoint(
                    end,
                    -1));

            return new PortalPathSolution(
                points);
        }

        // =============================================================
        // PORTAL OPTIMISATION
        // =============================================================

        private Vector3 FindShortestPortalPoint(
            PathPortal portal,
            Vector3 previous,
            Vector3 next)
        {
            float low = 0f;
            float high = 1f;

            /*
             * Golden-section search.
             *
             * The portal is represented by a single parameter:
             *
             * 0 = PointA
             * 1 = PointB
             */
            const int iterations = 30;

            for (int i = 0;
                 i < iterations;
                 i++)
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

                float costFirst =
                    CalculateCost(
                        portal,
                        first,
                        previous,
                        next);

                float costSecond =
                    CalculateCost(
                        portal,
                        second,
                        previous,
                        next);

                if (costFirst <
                    costSecond)
                {
                    high = second;
                }
                else
                {
                    low = first;
                }
            }

            float t =
                (low + high) * 0.5f;

            return portal.GetPoint(t);
        }

        private float CalculateCost(
            PathPortal portal,
            float t,
            Vector3 previous,
            Vector3 next)
        {
            Vector3 point =
                portal.GetPoint(t);

            return
                Vector3.Distance(
                    previous,
                    point)
                +
                Vector3.Distance(
                    point,
                    next);
        }

        // =============================================================
        // EDGE AVOIDANCE
        // =============================================================

        private Vector3 ApplyEdgeAvoidance(
            PathPortal portal,
            Vector3 shortest)
        {
            float t =
                GetPortalT(
                    portal,
                    shortest);

            switch (
                settings.EdgeAvoidanceMode)
            {
                case EdgeAvoidanceMode.PortalCenterBias:
                    {
                        float bias =
                            Mathf.Clamp01(
                                settings.EdgeAvoidanceAmount);

                        t =
                            Mathf.Lerp(
                                t,
                                0.5f,
                                bias);

                        break;
                    }

                case EdgeAvoidanceMode.StayAwayFromEdge:
                    {
                        float minimum =
                            Mathf.Clamp(
                                settings.EdgeAvoidanceAmount,
                                0f,
                                0.5f);

                        t =
                            Mathf.Clamp(
                                t,
                                minimum,
                                1f - minimum);

                        break;
                    }
            }

            return portal.GetPoint(t);
        }

        private float GetPortalT(
            PathPortal portal,
            Vector3 point)
        {
            Vector3 direction =
                portal.PointB -
                portal.PointA;

            float lengthSq =
                direction.sqrMagnitude;

            if (lengthSq <=
                Mathf.Epsilon)
            {
                return 0.5f;
            }

            float t =
                Vector3.Dot(
                    point - portal.PointA,
                    direction)
                /
                lengthSq;

            return Mathf.Clamp01(t);
        }
    }

    // =============================================================
    // PORTAL PATH POINT
    // =============================================================

    public readonly struct PortalPathPoint
    {
        public Vector3 Position { get; }

        /// <summary>
        /// -1 for start/end.
        ///
        /// Otherwise this is the index of the portal
        /// that generated this point.
        /// </summary>
        public int PortalIndex { get; }

        public PortalPathPoint(
            Vector3 position,
            int portalIndex)
        {
            Position = position;
            PortalIndex = portalIndex;
        }
    }

    // =============================================================
    // PORTAL PATH SOLUTION
    // =============================================================

    public sealed class PortalPathSolution
    {
        public IReadOnlyList<PortalPathPoint> Points { get; }

        public PortalPathSolution(
            IReadOnlyList<PortalPathPoint> points)
        {
            Points = points;
        }

        /// <summary>
        /// Returns only the Vector3 positions.
        /// </summary>
        public List<Vector3> GetPositions()
        {
            if (Points == null ||
                Points.Count == 0)
            {
                return new List<Vector3>();
            }

            var result =
                new List<Vector3>(
                    Points.Count);

            foreach (
                PortalPathPoint point
                in Points)
            {
                result.Add(
                    point.Position);
            }

            return result;
        }
    }
}