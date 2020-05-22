﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BattleUnitStructure
{
    public class BattleUnit
    {
        public ControlScript.ArmyTypes myType;

        public int lowerDamage;
        public int upperDamage;

        public int myCount;
        public int maxUnitOnTerritory;
        //public BattleUnitGraphics graphData;

        private List<int> attackRes; 

        public BattleUnit()
        {
            attackRes = new List<int>();
        }

        public List<int> GetOnceDamage()
        {
            attackRes.Clear();
            for (int i = 0; i < myCount; ++i)
            {
                attackRes.Add(Random.Range(lowerDamage, upperDamage + 1));
            }
            return attackRes;
        }

        public virtual Sprite GetSpriteData(int player)
        {
            return null;
        }

        public virtual Tile GetTileData(int player)
        {
            return null;
        }

        public int AddUnits(int value)
        {
            int add = Mathf.Min(value, maxUnitOnTerritory - myCount);
            myCount += add;
            return Mathf.Max(0, value - add);
        }
    }
}