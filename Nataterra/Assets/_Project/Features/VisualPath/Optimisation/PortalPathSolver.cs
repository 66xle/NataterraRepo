using System.Collections.Generic;
using UnityEngine;

namespace VisualPath
{
    public sealed class PortalPathSolver
    {
        private readonly VisualPathSettings settings;

        public PortalPathSolver(VisualPathSettings settings)
        {
            this.settings = settings;
        }

        public List<Vector3> Solve(Vector3 start,Vector3 end, IReadOnlyList<PathPortal> portals)
        {
            var crossings = new Vector3[portals.Count];

            // Start at portal centres.
            for (int i = 0; i < portals.Count; i++)
            {
                crossings[i] = portals[i].Center;
            }

            // Gradually optimise the crossing positions.
            for (int pass = 0; pass < settings.PortalRelaxationPasses; pass++)
            {
                for (int i = 0; i < portals.Count; i++)
                {
                    Vector3 previous = i == 0 ? start : crossings[i - 1];
                    Vector3 next = i == portals.Count - 1 ? end : crossings[i + 1];
                    Vector3 shortest = FindShortestPortalPoint(portals[i], previous, next);

                    crossings[i] = ApplyEdgeAvoidance(portals[i], shortest);
                }
            }

            var result = new List<Vector3>(portals.Count + 2);

            result.Add(start);

            for (int i = 0; i < crossings.Length; i++)
            {
                result.Add(crossings[i]);
            }

            result.Add(end);

            return result;
        }

        private Vector3 FindShortestPortalPoint(PathPortal portal, Vector3 previous, Vector3 next)
        {
            float low = 0f;
            float high = 1f;

            // Golden-section style search.
            for (int i = 0; i < 30; i++)
            {
                float first = Mathf.Lerp(low, high, 0.382f);
                float second = Mathf.Lerp(low, high, 0.618f);

                float costFirst = CalculateCost(portal, first, previous, next);
                float costSecond = CalculateCost(portal, second, previous, next);

                if (costFirst < costSecond)
                    high = second;
                else
                    low = first;
            }

            float t = (low + high) * 0.5f;

            return portal.GetPoint(t);
        }

        private float CalculateCost(PathPortal portal, float t, Vector3 previous, Vector3 next)
        {
            Vector3 point = portal.GetPoint(t);

            return Vector3.Distance(previous, point) + Vector3.Distance(point, next);
        }

        private Vector3 ApplyEdgeAvoidance(PathPortal portal, Vector3 shortest)
        {
            float t = GetPortalT(portal, shortest);

            switch (settings.EdgeAvoidanceMode)
            {
                case EdgeAvoidanceMode.PortalCenterBias:
                    float bias = Mathf.Clamp01(settings.EdgeAvoidanceAmount);
                    t = Mathf.Lerp(t, 0.5f, bias);
                    break;

                case EdgeAvoidanceMode.StayAwayFromEdge:
                    float minimum = Mathf.Clamp(settings.EdgeAvoidanceAmount, 0f, 0.5f);
                    t = Mathf.Clamp(t, minimum, 1f - minimum);
                    break;
            }

            return portal.GetPoint(t);
        }

        private float GetPortalT(PathPortal portal, Vector3 point)
        {
            Vector3 direction =portal.PointB - portal.PointA;
            float lengthSq = direction.sqrMagnitude;

            if (lengthSq <= Mathf.Epsilon)
                return 0.5f;

            return Mathf.Clamp01(Vector3.Dot(point - portal.PointA, direction) / lengthSq);
        }
    }
}