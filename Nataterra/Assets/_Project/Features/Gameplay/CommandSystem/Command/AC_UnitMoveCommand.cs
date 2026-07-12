using System.Collections.Generic;

public struct AC_UnitMoveCommand : IActionCommand
{
    public List<UnitType> ListOfUnitType;
    public List<string> ListOfUnitGUID;

    public int SelectedIndex;
    public int Destination;
}
