using PurrNet;
using UnityEngine;
using System.Collections.Generic;

public class ServerManager : Singleton<NetworkBehaviour>
{
    List<HexCellData> _hexCells;

    public void SetHexData(List<HexCellData> hexCells)
    {
        _hexCells = hexCells;
    }
}
