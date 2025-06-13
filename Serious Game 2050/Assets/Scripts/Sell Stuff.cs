using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellStuff : MonoBehaviour

{
    public ResourceManager resourceManager;
    public int WoodSell;
    public int StoneSell;
    public int GrainSell;
    public int EnergySell;
    public int CrudeOilSell;
    public int OilSell;
    public int FishSell;
    public int CoalSell;
    public int SteelSell;
    public int WoodPrice;
    public int StonePrice;
    public int GrainPrice;
    public int EnergyPrice;
    public int CrudeOilPrice;
    public int OilPrice;
    public int FishPrice;
    public int CoalPrice;
    public int SteelPrice;
     //all for selling resources

    
    public void SellWood()
{
    int amountToSell = Mathf.Min(100, resourceManager.Wood);
    if (amountToSell > 0)
    {
        resourceManager.Wood -= amountToSell;
        WoodSell += amountToSell;
        resourceManager.GildedBanana += amountToSell * WoodPrice;
    }
}

public void SellStone()
{
    int amountToSell = Mathf.Min(100, resourceManager.Stone);
    if (amountToSell > 0)
    {
        resourceManager.Stone -= amountToSell;
        StoneSell += amountToSell;
        resourceManager.GildedBanana += amountToSell * StonePrice;
    }
}

public void SellGrain()
{
    int amountToSell = Mathf.Min(100, resourceManager.Grain);
    if (amountToSell > 0)
    {
        resourceManager.Grain -= amountToSell;
        GrainSell += amountToSell;
        resourceManager.GildedBanana += amountToSell * GrainPrice;
    }
}

void SellEnergy()
{
    int amountToSell = Mathf.Min(100, resourceManager.Energy);
    if (amountToSell > 0)
    {
        resourceManager.Energy -= amountToSell;
        EnergySell += amountToSell;
        resourceManager.GildedBanana += amountToSell * EnergyPrice;
    }
}

void SellCrudeOil()
{
    int amountToSell = Mathf.Min(100, resourceManager.CrudeOil);
    if (amountToSell > 0)
    {
        resourceManager.CrudeOil -= amountToSell;
        CrudeOilSell += amountToSell;
        resourceManager.GildedBanana += amountToSell * CrudeOilPrice;
    }
}

void SellOil()
{
    int amountToSell = Mathf.Min(100, resourceManager.Oil);
    if (amountToSell > 0)
    {
        resourceManager.Oil -= amountToSell;
        OilSell += amountToSell;
        resourceManager.GildedBanana += amountToSell * OilPrice;
    }
}

public void SellFish()
{
    int amountToSell = Mathf.Min(100, resourceManager.Fish);
    if (amountToSell > 0)
    {
        resourceManager.Fish -= amountToSell;
        FishSell += amountToSell;
        resourceManager.GildedBanana += amountToSell * FishPrice;
    }
}

public void SellCoal()
{
    int amountToSell = Mathf.Min(100, resourceManager.Coal);
    if (amountToSell > 0)
    {
        resourceManager.Coal -= amountToSell;
        CoalSell += amountToSell;
        resourceManager.GildedBanana += amountToSell * CoalPrice;
    }
}
    // all for buying resources

public void BuyWood()
{
    int maxAffordable = resourceManager.GildedBanana / WoodPrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        WoodSell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * WoodPrice;
        resourceManager.Wood += amountToBuy;
    }
}

public void BuyStone()
{
    int maxAffordable = resourceManager.GildedBanana / StonePrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        StoneSell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * StonePrice;
        resourceManager.Stone += amountToBuy;
    }
}

public void BuyGrain()
{
    int maxAffordable = resourceManager.GildedBanana / GrainPrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        GrainSell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * GrainPrice;
        resourceManager.Grain += amountToBuy;
    }
}

void BuyEnergy()
{
    int maxAffordable = resourceManager.GildedBanana / EnergyPrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        EnergySell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * EnergyPrice;
        resourceManager.Energy += amountToBuy;
    }
}

void BuyCrudeOil()
{
    int maxAffordable = resourceManager.GildedBanana / CrudeOilPrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        CrudeOilSell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * CrudeOilPrice;
        resourceManager.CrudeOil += amountToBuy;
    }
}

void BuyOil()
{
    int maxAffordable = resourceManager.GildedBanana / OilPrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        OilSell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * OilPrice;
        resourceManager.Oil += amountToBuy;
    }
}

void BuyFish()
{
    int maxAffordable = resourceManager.GildedBanana / FishPrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        FishSell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * FishPrice;
        resourceManager.Fish += amountToBuy;
    }
}

void BuyCoal()
{
    int maxAffordable = resourceManager.GildedBanana / CoalPrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        CoalSell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * CoalPrice;
        resourceManager.Coal += amountToBuy;
    }
}

void BuySteel()
{
    int maxAffordable = resourceManager.GildedBanana / SteelPrice;
    int amountToBuy = Mathf.Min(100, maxAffordable);
    if (amountToBuy > 0)
    {
        SteelSell -= amountToBuy;
        resourceManager.GildedBanana -= amountToBuy * SteelPrice;
        resourceManager.Steel += amountToBuy;
    }
}

}


