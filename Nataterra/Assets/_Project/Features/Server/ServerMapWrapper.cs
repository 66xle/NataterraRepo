using System.Collections.Generic;
using UnityEngine;

public class ServerMapWrapper
{
    private List<HexCellState> _state;
    private int[] _basesPlaced;

    public ServerMapWrapper(List<HexCellState> state, int[] basesPlaced)
    {
        _state = state;
        _basesPlaced = basesPlaced;
    }


    public int GetBaseCellIndex(Base faction)
    {
        return _basesPlaced[(int)faction];
    }
}
