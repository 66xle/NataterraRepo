using DrawXXL;
using SerializeReferenceEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VisualPath;

public class VisualPathManager : MonoBehaviour
{
    [Header("Path Settings")]
    public PathPriorityMode PathPriority;
    public EdgeAvoidanceMode EdgeAvoidance;
    [Range(0, 1)] public float EdgeAvoidanceAmount = 0.35f;
    [Range(0, 1)] public float EdgeMargin = 0.25f;
    public int PortalRelaxationPasses = 4;
    public float StraightAngleTolerance = 6f;
    public int SmoothingSamples = 8;
    [Range(0, 1)] public float SmoothingRatio = 0.2f;

    [Header("Road Exit")]
    public int RoadExitSamplesPerSegment = 10;
    public float RoadExitAngleRange = 30f;

    [Header("NavMesh")]
    public bool UseNavMesh = false;
    public NavMeshData NavMeshData;
    public int NavMeshAreaMask = NavMesh.AllAreas;
    public float NavMeshSampleDistance = 0.5f;
    public float NavMeshHeightTolerance = 1f;
    public float NavMeshValidationSampleDistance = 0.5f;
    public float NavMeshEdgeAvoidanceDistance = 0.5f;
    public int NavMeshEdgeAvoidanceIterations = 2;
    [Range(0, 1)] public float NavMeshEdgeAvoidanceStrength = 1f;
    public bool ValidateSmoothedPath = true;

    [Header("Blockers")]
    public int NavMeshMaxBlockerRetries = 10;
    public float NavMeshBlockerEdgePadding = 0.05f;
    public float NavMeshBlockerWidthMultiplier = 1f;

    [Header("Other")]
    public bool UpdatePath = false;
    public GameObject PrefabBlocker;
    public List<Transform> RoadPoints;

    [SerializeReference, SR]
    public List<RoadWrapper> ListOfRoads;

    readonly List<GameObject> _temporaryBlockers =
        new();

    Coroutine _generationRoutine;

    bool _generationRunning;

    VisualPathGenerator _generator;
    VisualPathRequest _request;

    LineRenderer _lineRenderer;

    List<Vector3> _path =
        new();

    List<VisualRoad> _roads =
        new();

    public void Awake()
    {
        _lineRenderer =
            GetComponent<LineRenderer>();
    }

    public void Update()
    {
        if (UpdatePath &&
            !_generationRunning)
        {
            GeneratePath();
        }
    }

    // =============================================================
    // REQUEST
    // =============================================================

    /// <summary>
    /// Creates a VisualPath request.
    ///
    /// allHexes must contain every hex on the map, not only the
    /// corridor. It is required when NavMesh blocker generation is
    /// enabled because the processor needs to identify the actual
    /// outside hex entered by a NavMesh fallback.
    /// </summary>
    public void CreateRequest(
        List<VisualHex> corridor,
        Vector3 start,
        Vector3 destination,
        List<VisualRoad> roads,
        List<VisualHex> allHexes)
    {
        _roads =
            roads;

        _request =
            new VisualPathRequest(
                corridor,
                start,
                destination,
                roads,
                allHexes);
    }

    // =============================================================
    // GENERATE
    // =============================================================

    public List<Vector3> GeneratePath()
    {
        if (_generationRunning)
            return _path;

        if (_generationRoutine != null)
        {
            StopCoroutine(
                _generationRoutine);
        }

        _generationRoutine =
            StartCoroutine(
                GeneratePathRoutine());

        return _path;
    }

    public IEnumerator GeneratePathRoutine()
    {
        _generationRunning =
            true;

        // ---------------------------------------------------------
        // Remove blockers from the previous generation.
        // ---------------------------------------------------------

        ClearTemporaryBlockers();

        if (_request == null)
        {
            Debug.LogError(
                "VisualPathManager has no path request.");

            _generationRunning =
                false;

            _generationRoutine =
                null;

            yield break;
        }

        // =========================================================
        // SETTINGS
        // =========================================================

        VisualPathSettings settings =
            new VisualPathSettings
            {
                PathPriority =
                    PathPriority,

                EdgeAvoidanceMode =
                    EdgeAvoidance,

                EdgeAvoidanceAmount =
                    EdgeAvoidanceAmount,

                EdgeMargin =
                    EdgeMargin,

                PortalRelaxationPasses =
                    PortalRelaxationPasses,

                RoadExitSamplesPerSegment =
                    RoadExitSamplesPerSegment,

                StraightAngleTolerance =
                    StraightAngleTolerance,

                SmoothingSamples =
                    SmoothingSamples,

                SmoothingRatio =
                    SmoothingRatio,

                RoadExitAngleRange =
                    RoadExitAngleRange,

                UseNavMesh =
                    UseNavMesh,

                NavMeshData =
                    NavMeshData,

                NavMeshAreaMask =
                    NavMeshAreaMask,

                NavMeshSampleDistance =
                    NavMeshSampleDistance,

                NavMeshHeightTolerance =
                    NavMeshHeightTolerance,

                NavMeshValidationSampleDistance =
                    NavMeshValidationSampleDistance,

                NavMeshEdgeAvoidanceDistance =
                    NavMeshEdgeAvoidanceDistance,

                NavMeshEdgeAvoidanceIterations =
                    NavMeshEdgeAvoidanceIterations,

                NavMeshEdgeAvoidanceStrength =
                    NavMeshEdgeAvoidanceStrength,

                ValidateSmoothedPath =
                    ValidateSmoothedPath,

                NavMeshMaxBlockerRetries =
                    NavMeshMaxBlockerRetries,

                NavMeshBlockerEdgePadding =
                    NavMeshBlockerEdgePadding,

                NavMeshBlockerWidthMultiplier =
                    NavMeshBlockerWidthMultiplier
            };

        _generator =
            new VisualPathGenerator(
                settings,
                NavMeshData);
            
        // =========================================================
        // GENERATION / BLOCKER RETRIES
        // =========================================================

        for (int attempt = 0;
             attempt <= NavMeshMaxBlockerRetries;
             attempt++)
        {
            VisualPathResult result =
                _generator.Generate(
                    _request,
                    out List<NavMeshBlockerRequest> blockerRequests);

            // -----------------------------------------------------
            // SUCCESS
            // -----------------------------------------------------

            if (result.Success)
            {
                _path =
                    new List<Vector3>(
                        result.FinalPath);

                DisplayPath();

                _generationRunning =
                    false;

                _generationRoutine =
                    null;

                yield break;
            }

            // -----------------------------------------------------
            // NO BLOCKERS REQUIRED
            // -----------------------------------------------------

            if (blockerRequests == null ||
                blockerRequests.Count == 0)
            {
                Debug.LogError(
                    result.Error);

                _generationRunning =
                    false;

                _generationRoutine =
                    null;

                yield break;
            }

            // -----------------------------------------------------
            // BLOCKER PREFAB REQUIRED
            // -----------------------------------------------------

            if (PrefabBlocker == null)
            {
                Debug.LogError(
                    result.Error +
                    " PrefabBlocker is not assigned.");

                _generationRunning =
                    false;

                _generationRoutine =
                    null;

                yield break;
            }

            // -----------------------------------------------------
            // CREATE ALL BLOCKERS
            // -----------------------------------------------------

            bool createdAny =
                false;

            for (int i = 0;
                 i < blockerRequests.Count;
                 i++)
            {
                GameObject blocker =
                    CreateBlocker(
                        blockerRequests[i]);

                if (blocker == null)
                {
                    continue;
                }

                _temporaryBlockers.Add(
                    blocker);

                createdAny =
                    true;
            }

            if (!createdAny)
            {
                Debug.LogError(
                    "Failed to create any temporary NavMesh blockers.");

                _generationRunning =
                    false;

                _generationRoutine =
                    null;

                yield break;
            }

            // -----------------------------------------------------
            // NavMeshObstacle carving does not update immediately.
            //
            // Wait one frame so Unity can update the NavMesh before
            // the next CalculatePath call.
            // -----------------------------------------------------

            yield return null;
        }

        // =========================================================
        // EXHAUSTED
        // =========================================================

        Debug.LogError(
            "Visual path generation exhausted the maximum number of temporary NavMesh blockers.");

        _generationRunning =
            false;

        _generationRoutine =
            null;
    }

    // =============================================================
    // CREATE BLOCKER
    // =============================================================

    private GameObject CreateBlocker(
        NavMeshBlockerRequest request)
    {
        Vector3 edgeA =
            request.EdgeA;

        Vector3 edgeB =
            request.EdgeB;

        Vector3 edge =
            edgeB -
            edgeA;

        edge.y =
            0f;

        float edgeLength =
            edge.magnitude;

        if (edgeLength <=
            0.001f)
        {
            return null;
        }

        Vector3 midpoint =
            (edgeA + edgeB) *
            0.5f;

        Vector3 direction =
            edge.normalized;

        // ---------------------------------------------------------
        // Local X = actual shared edge direction.
        //
        // This means the blocker spans the actual irregular hex
        // edge rather than assuming any regular hex geometry.
        // ---------------------------------------------------------

        Vector3 right =
            direction;

        Vector3 up =
            Vector3.up;

        Vector3 forward =
            Vector3.Cross(
                right,
                up).normalized;

        Quaternion rotation =
            Quaternion.LookRotation(
                forward,
                up);

        GameObject blocker =
            Instantiate(
                PrefabBlocker,
                midpoint,
                rotation);

        blocker.name =
            "Temporary NavMesh Blocker - Corridor Hex " +
            request.CorridorHex.Id;

        // ---------------------------------------------------------
        // Scale local X to the shared edge length.
        // ---------------------------------------------------------

        Vector3 scale =
            blocker.transform.localScale;

        float prefabWidth =
            Mathf.Abs(
                scale.x);

        if (prefabWidth <=
            0.001f)
        {
            prefabWidth =
                1f;
        }

        float targetWidth =
            (edgeLength +
             NavMeshBlockerEdgePadding) *
            NavMeshBlockerWidthMultiplier;

        scale.x =
            targetWidth /
            prefabWidth *
            Mathf.Sign(
                blocker.transform.localScale.x);

        blocker.transform.localScale =
            scale;

        // ---------------------------------------------------------
        // Configure carving.
        // ---------------------------------------------------------

        NavMeshObstacle obstacle =
            blocker.GetComponent<
                NavMeshObstacle>();

        if (obstacle != null)
        {
            obstacle.carving =
                true;

            obstacle.carveOnlyStationary =
                false;
        }

        return blocker;
    }

    // =============================================================
    // CLEAR BLOCKERS
    // =============================================================

    private void ClearTemporaryBlockers()
    {
        for (int i = 0;
             i < _temporaryBlockers.Count;
             i++)
        {
            GameObject blocker =
                _temporaryBlockers[i];

            if (blocker != null)
            {
                Destroy(
                    blocker);
            }
        }

        _temporaryBlockers.Clear();
    }

    // =============================================================
    // DISPLAY
    // =============================================================

    public void DisplayPath()
    {
        if (_lineRenderer == null)
            return;

        _lineRenderer.positionCount =
            _path.Count;

        for (int i = 0;
             i < _path.Count;
             i++)
        {
            _lineRenderer.SetPosition(
                i,
                _path[i]);
        }
    }

    // =============================================================
    // GIZMOS
    // =============================================================

    private void OnDrawGizmos()
    {
        DrawBasics.usedUnityLineDrawingMethod =
            DrawBasics.UsedUnityLineDrawingMethod.gizmoLines;

        if (ListOfRoads == null)
            return;

        foreach (RoadWrapper road in ListOfRoads)
        {
            if (road == null ||
                road.Points == null)
            {
                continue;
            }

            for (int i = 0;
                 i < road.Points.Count - 1;
                 i++)
            {
                DrawBasics.Line(
                    road.Points[i].position,
                    road.Points[i + 1].position);
            }
        }
    }
}