using PurrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGS;
using UnityEngine;

public class MapStateMachine : NetworkBehaviour
{
    [Header("References")]
    public GameplaySystem GS;
    [SerializeField] LayerMask TerrainLayer;

    TerrainGridSystem _tgs;
    ServerMap _serverMap;
    List<HexCellState> _state;
    FactionState _factionState;
    Dictionary<string, GameObject> _unitObjects;
    Dictionary<UnitType, UnitData> _dictOfUnits;

    Cell _selectedCell;
    List<Unit> _selectedUnits;
    GameObject _movementBorder;
    DijkstraResult _movementResult;

    public FactionState FactionState { get { return _factionState; } }

    public Cell SelectedCell { get { return _selectedCell; } set { _selectedCell = value; } }
    public List<Unit> SelectedUnits { get { return _selectedUnits; } set { _selectedUnits = value; } }
    public GameObject MovementBorder { get { return _movementBorder; } set { _movementBorder = value; } }
    public DijkstraResult MovementResult { get { return _movementResult; } set { _movementResult = value; } }


    public Action<UnitType> OnUnitPurchase;

    public Action<List<Unit>> OnSelectUnit;
    public Action OnEndPhase;


    private void Awake()
    {
        InstanceHandler.RegisterInstance(this);
    }

    private void OnDestory()
    {
        InstanceHandler.UnregisterInstance<MapStateMachine>();
    }

    public void SetupServer(Dictionary<UnitType, UnitData> dictUnits, List<HexCellState> state)
    {
        MapData mapData = GameManager.Instance.MapData;
        SetupGrid(mapData);

        SetupServerMap(mapData, state, dictUnits);

        GS.Setup();
    }

    public async Task SetupClient(Dictionary<UnitType, UnitData> dictUnits)
    {
        _dictOfUnits = dictUnits;
        _unitObjects = new();

        ClientRuntime client = await SetupClientOnServer();
        _factionState = client.FactionState;
        _state = client.MapState;

        if (!isHost)
        {
            MapData mapData = GameManager.Instance.MapData;
            SetupGrid(mapData);

            GS.Setup();
        }

        await LoadClientMap();
    }

    void SetupServerMap(MapData mapData, List<HexCellState> state, Dictionary<UnitType, UnitData> dictOfUnits)
    {
        List<FactionData> factionData = GameManager.Instance.ListOfFactions;

        ServerMapWrapper wrapper = new ServerMapWrapper(state, _tgs.cells, mapData.bases, factionData, dictOfUnits);
        _serverMap = new ServerMap();
        _serverMap.Init(wrapper, GS);
    }

    void SetupGrid(MapData mapData)
    {
        // Get terrain with layer
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            if ((1 << terrain.gameObject.layer & TerrainLayer.value) == 0) continue;

            _tgs = terrain.gameObject.AddTerrainGridSystem();
            break;
        }

        _tgs.gridTopology = GridTopology.Hexagonal;
        _tgs.SetGridSize(mapData.row, mapData.column);
        _tgs.SetNumTerritories(0, false);
        _tgs.highlightMode = HighlightMode.None;
        _tgs.overlayMode = OverlayMode.Overlay;
        _tgs.cellBorderThickness = 10f;
        _tgs.gridLinesIgnoreDepth = true;

        _tgs.Redraw();

        _tgs.SetGridType(GridTopology.Irregular);
        _tgs.RegenerateFlatToppedHexagonalGrid(mapData.tgsCells);
        _tgs.CellsUpdateBounds();
        _tgs.CellsUpdateNeighbours();
        _tgs.RedrawCells(_tgs.cells);
    }

    public void AddFaction(PlayerID playerID, Base facton)
    {
        _serverMap.AddFaction(playerID, facton);
    }

    [ServerRpc]
    async Task<ClientRuntime> SetupClientOnServer(RPCInfo info = default)
    {
        ClientRuntime client = new();

        AssignPlayerToFaction(info.sender);
        client.MapState = _serverMap.GetMap();
        client.FactionState = _serverMap.GetFactionState(info.sender);

        return client;
    }

    [ServerRpc]
    async Task LoadClientMap(RPCInfo info = default)
    {
        _serverMap.LoadClientMap(info.sender);
    }

    void AssignPlayerToFaction(PlayerID playerID)
    {
        List<FactionData> factions = GameManager.Instance.ListOfFactions;
        FactionData data = factions[(int)playerID.id.value - 1];

        Debug.Log($"Player {playerID} Faction: {data.Settings.Faction}");

        _serverMap.AddFaction(playerID, data.Settings.Faction);
    }

    [ServerRpc]
    public void SendCommandToServer(IActionCommand command, RPCInfo info = default)
    {
        if (command == null)
            return;

        command.ID = info.sender;

        _serverMap.HandleCommand(command);
    }

    [TargetRpc]
    public void EndPhaseForClient(PlayerID playerID)
    {
        OnEndPhase?.Invoke();
    }



    public void SetFactionState(FactionState state)
    {
        _factionState = state;
    }

    public void SetCellState(HexCellState cellState, int cellIndex)
    {
        _state[cellIndex] = cellState;
    }

    public void AddUnitObject(string guid, GameObject obj)
    {
        _unitObjects.Add(guid, obj);
    }


    public GameObject GetUnitObject(string guid)
    {
        return _unitObjects[guid];
    }


    public GameObject GetUnitPrefab(UnitType type)
    {
        return _dictOfUnits[type].Prefab;
    }

    public int GetFoodCost(UnitType type)
    {
        return _dictOfUnits[type].FoodCost;
    }

    public int GetWoodCost(UnitType type)
    {
        return _dictOfUnits[type].WoodCost;
    }

    public int GetMetalCost(UnitType type)
    {
        return _dictOfUnits[type].MetalCost;
    }


    public bool UnitExistOnCell(int cellIndex)
    {
        return _state[cellIndex].DictOfGroups.Count == 0 ? false : true;
    }

    public List<Unit> GetUnitList(int cellIndex)
    {
        List<UnitType> unitTypes = _state[cellIndex].DictOfGroups.Keys.ToList();

        return _state[cellIndex].DictOfGroups[unitTypes[0]].ListOfUnits;
    }


    public int GetOrigin(List<Unit> units, int selectedIndex, out bool IsOrigin)
    {
        int origin = units[0].CellOrigin;
        IsOrigin = true;

        for (int i = 1; i < units.Count; i++)
        {
            if (units[i].CellOrigin == origin)
                continue;

            IsOrigin = false;
            return selectedIndex;
        }

        return origin;
    }

    public int GetLowestMovement(List<Unit> units, bool isOrigin)
    {
        if (isOrigin)
            return units[0].Movement;

        int currentMovement = units[0].CurrentMovement;

        // Get the lowest avaliable movement
        for (int i = 1; i < units.Count; i++)
        {
            if (units[i].CurrentMovement < currentMovement)
                currentMovement = units[i].CurrentMovement;
        }

        return currentMovement;
    }


    public void RemoveMovementBorder()
    {
        Destroy(MovementBorder);
    }


    public DijkstraResult CalculateMovementRange(int startCell, int maxMovement, List<Cell> cells)
    {
        DijkstraResult result = new DijkstraResult();

        List<int> open = new();

        open.Add(startCell);

        result.Cost[startCell] = 0;
        result.Parent[startCell] = -1;

        while (open.Count > 0)
        {
            // Find the node with the lowest cost
            int current = open[0];

            for (int i = 1; i < open.Count; i++)
            {
                if (result.Cost[open[i]] < result.Cost[current])
                    current = open[i];
            }

            open.Remove(current);

            int currentCost = result.Cost[current];

            Cell cell = cells[current];

            foreach (Cell neighbour in cell.neighbours)
            {
                if (!CanEnter(neighbour))
                    continue;

                int newCost = currentCost + GetMovementCost(neighbour);

                if (newCost > maxMovement)
                    continue;

                if (!result.Cost.TryGetValue(neighbour.index, out int oldCost) ||
                    newCost < oldCost)
                {
                    result.Cost[neighbour.index] = newCost;
                    result.Parent[neighbour.index] = current;

                    if (!open.Contains(neighbour.index))
                        open.Add(neighbour.index);
                }
            }
        }

        return result;
    }

    bool CanEnter(Cell cell)
    {
        if (!cell.canCross)
            return false;

        return true;
    }

    int GetMovementCost(Cell cell)
    {
        // Check if ground or flying type

        // If ground check if cell is a mountain

        return 1;
    }

}
