using PurrNet;
using System.Collections.Generic;

public struct AC_UnitMoveCommand : IActionCommand
{
    public PlayerID ID {  get; set; }

    public List<UnitType> ListOfUnitType;
    public List<string> ListOfUnitGUID;

    public int SelectedIndex;
    public int Destination;
}
