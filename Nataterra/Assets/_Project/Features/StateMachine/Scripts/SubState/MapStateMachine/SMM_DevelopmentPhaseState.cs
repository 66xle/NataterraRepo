using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SMM_DevelopmentPhaseState : GameplayBaseState
{
    DevelopmentCart _cart = new();

    int TotalFoodCost;
    int TotalWoodCost;
    int TotalMetalCost;

    public SMM_DevelopmentPhaseState(StateMachineManager context, GameplayStateFactory combatStateFactory) : base(context, combatStateFactory) { }
    public override void EnterState()
    {
        Debug.Log("Entered Development Phase");

        TotalFoodCost = 0;
        TotalWoodCost = 0;
        TotalMetalCost = 0;
        _cart.Units = new();

        MapCtx.OnUnitPurchase += AddUnitToCart;
        MapCtx.OnEndPhase += SwitchToWaitingForTurn;
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AC_PhaseEndPhaseCommand command = new AC_PhaseEndPhaseCommand()
            {
                CurrentState = GameplayState.DevelopmentPhase,
                Cart = _cart
            };

            MapCtx.SendEndPhaseCommand(command);
        }
    }

    public override void FixedUpdateState() { }
    public override void ExitState() 
    {
        _cart.Units.Clear();

        MapCtx.OnEndPhase -= SwitchToWaitingForTurn;
    }

    public override void CheckSwitchState() { }

    public override void InitializeSubState() { }

    private void SwitchToWaitingForTurn()
    {
        SwitchState(Factory.WaitingForTurn());
    }

    public void AddUnitToCart(UnitType unitType)
    {
        // Check Unit Upgraded version

        

        if (!IsResourceEnough(unitType))
        {
            Debug.Log("Not enough resources");
            return;
        }

        if (_cart.Units.ContainsKey(unitType))
            _cart.Units[unitType]++;
        else
            _cart.Units.Add(unitType, 1);

        Debug.Log($"Added {unitType}");


        int food = MapCtx.FactionState.Food;
        int wood = MapCtx.FactionState.Wood;
        int metal = MapCtx.FactionState.Metal;
        MapCtx.GS.UISystem.SetResourceUI(food - TotalFoodCost, wood - TotalMetalCost, metal - TotalMetalCost);
    }

    public bool IsResourceEnough(UnitType type)
    {
        int FoodCost = MapCtx.GetFoodCost(type);
        int WoodCost = MapCtx.GetWoodCost(type);
        int MetalCost = MapCtx.GetMetalCost(type);

        int CurrentFood = MapCtx.FactionState.Food;
        int CurrentWood = MapCtx.FactionState.Wood;
        int CurrentMetal = MapCtx.FactionState.Metal;

        if (CurrentFood >= TotalFoodCost + FoodCost && CurrentWood >= TotalWoodCost + WoodCost && CurrentMetal >= TotalMetalCost + MetalCost)
        {
            TotalFoodCost += FoodCost;
            TotalWoodCost += WoodCost;
            TotalMetalCost += MetalCost;

            return true;
        }

        return false;
    }
}
