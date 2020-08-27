using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BattleUnitStructure;
using GameControls;

namespace RegionStructure
{

    public class Region
    {

        private int myNum;
        private List<Vector3Int> tileCoords;
        private List<int> borderRegions;
        private Vector3Int regionCenter;

        public int myPlayer;
        public int myArmy;
        public Color myColor;
        public List<BattleUnit> myArmyOnRegion;

        public int RegNum
        {
            get { return myNum; }
            set { myNum = value; }
        }

        public List<Vector3Int> RegTiles
        {
            get { return tileCoords; }
            set { tileCoords = value; }
        }

        public List<int> RegBorder
        {
            get { return borderRegions; }
            set { borderRegions = value; }
        }

        public Vector3Int RegCenter
        {
            get { return regionCenter; }
        }

        public Region()
        {
            myNum = 0;
            tileCoords = new List<Vector3Int>();
            borderRegions = new List<int>();

            myPlayer = -1;
            myArmy = -1;

            myColor = new Color(1.0f, 1.0f, 1.0f);
            myArmyOnRegion = new List<BattleUnit>();
        }

        public Region(int nm)
        {
            myNum = nm;
            tileCoords = new List<Vector3Int>();
            borderRegions = new List<int>();

            myPlayer = -1;
            myArmy = -1;
            myColor = new Color(1.0f, 1.0f, 1.0f);
            myArmyOnRegion = new List<BattleUnit>();
        }

        public void AddTile(Vector3Int tileCrd)
        {
            tileCoords.Add(tileCrd);
        }

        public void SetNeighbour(int reg)
        {
            int ind = borderRegions.FindIndex(x => x == reg);
            if (!borderRegions.Contains(reg))
            {
                borderRegions.Add(reg);
            }
        }

        public int AddArmy(int newArmy, ControlScript.ArmyTypes type)
        {
            int ind = myArmyOnRegion.FindIndex(x => x.myType == type);
            if (ind != -1)
            {
                return myArmyOnRegion[ind].AddUnits(newArmy);
            }

            return newArmy;
        }

        public void DefeatArmy(ControlScript.ArmyTypes type)
        {
            int ind = myArmyOnRegion.FindIndex(x => x.myType == type);
            if (ind != -1)
            {
                myArmyOnRegion[ind].DefeatUnit();
            }
        }

        public void SetArmy(int val, ControlScript.ArmyTypes type)
        {
            int ind = myArmyOnRegion.FindIndex(x => x.myType == type);
            if (ind != -1 && val > 0)
            {
                myArmyOnRegion[ind].SetUnits(val);
            }
        }

        public int GetArmy(ControlScript.ArmyTypes type)
        {
            int army = 0;

            int ind = myArmyOnRegion.FindIndex(x => x.myType == type);
            if (ind != -1)
            {
                return myArmyOnRegion[ind].myCount;
            }

            return army;
        }

        public bool CheckArmyFullfilled(ControlScript.ArmyTypes type)
        {
            int ind = myArmyOnRegion.FindIndex(x => x.myType == type);
            if (ind != -1)
            {
                return myArmyOnRegion[ind].IsFullFilled();
            }

            return false;
        }

        public BattleUnit GetArmyUnit(ControlScript.ArmyTypes type)
        {
            int ind = myArmyOnRegion.FindIndex(x => x.myType == type);
            if (ind != -1)
            {
                return myArmyOnRegion[ind];
            } 
            return null;
        }

        public void EvaluateRegionCenter()
        {
            int xCenter = 0;
            int yCenter = 0;

            foreach (Vector3Int tileCrd in tileCoords)
            {
                xCenter += tileCrd.x;
                yCenter += tileCrd.y;
            }
            xCenter /= tileCoords.Count;
            yCenter /= tileCoords.Count;

            Vector3Int prefCenter = new Vector3Int(xCenter, yCenter+3, 0);

            var sortedTileCoords = tileCoords.OrderByDescending(x => Vector3Int.Distance(x, prefCenter)).Reverse().ToList();

            bool stopFinding = false;
            int ind = 0;

            int xNeib, yNeib;
            Vector3Int tmpCenterNeib;
            while (!stopFinding)
            {
                xCenter = sortedTileCoords[ind].x;
                yCenter = sortedTileCoords[ind].y;

                xNeib = xCenter;
                yNeib = yCenter;

                /*
                yNeib = yCenter - 2; //- 1;
                if (Mathf.Abs(yCenter) % 2 == 0)
                {
                    xNeib = xCenter - 1;
                }
                else
                {
                    xNeib = xCenter;
                }
                */

                tmpCenterNeib = new Vector3Int(xNeib, yNeib, 0);

                if (tileCoords.Contains(tmpCenterNeib))
                {
                    regionCenter = sortedTileCoords[ind];
                    stopFinding = true;
                }

                ind++;
                if (ind == tileCoords.Count)
                {
                    regionCenter = sortedTileCoords[0];
                    stopFinding = true;
                }

            }
        }
    }
}
