using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BattleUnitGraphics", menuName = "ScriptableObjects/Battle unit graphics", order = 0)]
public class BattleUnitGraphics : ScriptableObject
{
    public List<Sprite> unitSprite_class01;
    public List<Tile> unitTile_class_01;
}
