using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RegionStructure
{

    public class Region
    {

        private int myNum;
        private List<Vector3Int> tileCoords;
        private List<int> borderRegions;

        public int myPlayer;
        public int myArmy;
        public Color myColor;

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

        public Region()
        {
            myNum = 0;
            tileCoords = new List<Vector3Int>();
            borderRegions = new List<int>();

            myPlayer = -1;
            myArmy = -1;
            myColor = new Color(1.0f, 1.0f, 1.0f);
        }

        public Region(int nm)
        {
            myNum = nm;
            tileCoords = new List<Vector3Int>();
            borderRegions = new List<int>();

            myPlayer = -1;
            myArmy = -1;
            myColor = new Color(1.0f, 1.0f, 1.0f);
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
    }
}
