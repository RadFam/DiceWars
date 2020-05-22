using BattleUnitStructure;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Dice : BattleUnit
{
    public Dice() : base()
    {     
        myType = ControlScript.ArmyTypes.Dice_d6;

        lowerDamage = 1;
        upperDamage = 6;
        myCount = 0;

        maxUnitOnTerritory = 8;
    }

    public override Sprite GetSpriteData(int player)
    {
        return null;
    }

    public override Tile GetTileData(int player)
    {
        return null;
    }

}
