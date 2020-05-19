using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RegionStructure
{
    public class RegionMap : MonoBehaviour
    {

        public int deltaXTile;
        public int deltaYTile;
        public int regionsNum;
        public int rarePercent;

        private List<Region> allRegions;
        private int[] regionAssign;
        private List<int> growthFront;
        private Vector3Int[] tileCoords;
        private int[,] regionAdjacency;

        private bool[] visited;
        private int sparceAttempts = 100;

        public Vector3Int[] GetCoords
        {
            get { return tileCoords; }
        }

        public int[] GetRegion
        {
            get { return regionAssign; }
        }

        // Start is called before the first frame update
        void Awake()
        {
            allRegions = new List<Region>();
            visited = new bool[regionsNum];
            regionAdjacency = new int[regionsNum, regionsNum];
            regionAssign = new int[(2 * deltaYTile + 1) * (2 * deltaXTile + 1)];
            growthFront = new List<int>();
            tileCoords = new Vector3Int[(2 * deltaYTile + 1) * (2 * deltaXTile + 1)];
            for (int i = 0; i < (2 * deltaYTile + 1) * (2 * deltaXTile + 1); ++i)
            {
                regionAssign[i] = -1;
                growthFront.Add(0);
                int x = i % (2 * deltaXTile + 1) - deltaXTile;
                int y = (i - (i % (2 * deltaXTile + 1))) / (2 * deltaXTile + 1) - deltaYTile;
                tileCoords[i] = new Vector3Int(x, y, 0);
            }
        }

        public void GenerateRegionsMap()
        {
            allRegions.Clear();

            // Create seed points
            List<Vector3Int> seedTiles = new List<Vector3Int>();

            int x = 0;
            int y = 0;
            bool canProceed;
            for (int i = 0; i < regionsNum; ++i)
            {
                canProceed = true;
                while (canProceed)
                {
                    canProceed = false;
                    x = Random.Range(-deltaXTile, deltaXTile + 1);
                    y = Random.Range(-deltaYTile, deltaYTile + 1);
                    for (int j = 0; j < seedTiles.Count; ++j)
                    {
                        if (seedTiles[j].x == x && seedTiles[j].y == y)
                        {
                            canProceed = true;
                        }
                    }
                }
                seedTiles.Add(new Vector3Int(x, y, 0));
                regionAssign[(x + deltaXTile) + (y + deltaYTile) * (2 * deltaXTile + 1)] = i;
                growthFront[(x + deltaXTile) + (y + deltaYTile) * (2 * deltaXTile + 1)] = 1;
            }

            // Create regions with by seedPoints
            for (int i = 0; i < regionsNum; ++i)
            {
                Region rg = new Region(i);
                rg.AddTile(seedTiles[i]);
                allRegions.Add(rg);
            }

            // Start to upbuild regions
            bool canGrowth = true;
            int probab, myX, myY, neibX, neibY, neibInd;
            while (canGrowth)
            {
                var front = Enumerable.Range(0, growthFront.Count).Where(i => growthFront[i] == 1).ToList();
                //Debug.Log("Number of front tiles: " + front.Count.ToString());

                Region tmpReg;

                foreach (int ind in front)
                {
                    // Решаем, разрастись ему, или нет
                    probab = Random.Range(1, 5) % 4;
                    if (probab != 0)
                    {
                        growthFront[ind] = 2;

                        // Обходим соседей
                        myX = tileCoords[ind].x;
                        myY = tileCoords[ind].y;

                        neibX = myX - 1;
                        neibY = myY - 1;
                        if (neibX >= -deltaXTile && neibY >= -deltaYTile)
                        {
                            neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                            if (growthFront[neibInd] == 0 && regionAssign[neibInd] == -1)
                            {
                                growthFront[neibInd] = 1;
                                regionAssign[neibInd] = regionAssign[ind];

                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.AddTile(new Vector3Int(neibX, neibY, 0));
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                            if (regionAssign[neibInd] != 1 && regionAssign[neibInd] != regionAssign[ind])
                            {
                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.SetNeighbour(regionAssign[neibInd]);
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                        }

                        neibX = myX;
                        neibY = myY - 1;
                        if (neibY >= -deltaYTile)
                        {
                            neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                            if (growthFront[neibInd] == 0 && regionAssign[neibInd] == -1)
                            {
                                growthFront[neibInd] = 1;
                                regionAssign[neibInd] = regionAssign[ind];

                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.AddTile(new Vector3Int(neibX, neibY, 0));
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                            if (regionAssign[neibInd] != 1 && regionAssign[neibInd] != regionAssign[ind])
                            {
                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.SetNeighbour(regionAssign[neibInd]);
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                        }

                        neibX = myX + 1;
                        neibY = myY;
                        if (neibX <= deltaXTile)
                        {
                            neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                            if (growthFront[neibInd] == 0 && regionAssign[neibInd] == -1)
                            {
                                growthFront[neibInd] = 1;
                                regionAssign[neibInd] = regionAssign[ind];

                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.AddTile(new Vector3Int(neibX, neibY, 0));
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                            if (regionAssign[neibInd] != 1 && regionAssign[neibInd] != regionAssign[ind])
                            {
                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.SetNeighbour(regionAssign[neibInd]);
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                        }

                        neibX = myX;
                        neibY = myY + 1;
                        if (neibY <= deltaYTile)
                        {
                            neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                            if (growthFront[neibInd] == 0 && regionAssign[neibInd] == -1)
                            {
                                growthFront[neibInd] = 1;
                                regionAssign[neibInd] = regionAssign[ind];

                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.AddTile(new Vector3Int(neibX, neibY, 0));
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                            if (regionAssign[neibInd] != 1 && regionAssign[neibInd] != regionAssign[ind])
                            {
                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.SetNeighbour(regionAssign[neibInd]);
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                        }

                        neibX = myX - 1;
                        neibY = myY + 1;
                        if (neibX >= -deltaXTile && neibY <= deltaYTile)
                        {
                            neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                            if (growthFront[neibInd] == 0 && regionAssign[neibInd] == -1)
                            {
                                growthFront[neibInd] = 1;
                                regionAssign[neibInd] = regionAssign[ind];

                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.AddTile(new Vector3Int(neibX, neibY, 0));
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                            if (regionAssign[neibInd] != 1 && regionAssign[neibInd] != regionAssign[ind])
                            {
                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.SetNeighbour(regionAssign[neibInd]);
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                        }

                        neibX = myX - 1;
                        neibY = myY;
                        if (neibX >= -deltaXTile)
                        {
                            neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                            if (growthFront[neibInd] == 0 && regionAssign[neibInd] == -1)
                            {
                                growthFront[neibInd] = 1;
                                regionAssign[neibInd] = regionAssign[ind];

                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.AddTile(new Vector3Int(neibX, neibY, 0));
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                            if (regionAssign[neibInd] != 1 && regionAssign[neibInd] != regionAssign[ind])
                            {
                                tmpReg = allRegions[regionAssign[ind]];
                                tmpReg.SetNeighbour(regionAssign[neibInd]);
                                allRegions[regionAssign[ind]] = tmpReg;
                            }
                        }
                    }
                }

                var checkFront = Enumerable.Range(0, growthFront.Count).Where(i => growthFront[i] == 1).ToList();
                if (checkFront.Count == 0)
                {
                    canGrowth = false;
                }
            }

            // Make RegionAdjacency matrix
            for (int i = 0; i < regionsNum; ++i)
            {
                List<int> neighbours = allRegions[i].RegBorder;
                foreach (int j in neighbours)
                {
                    regionAdjacency[i, j] = 1;
                    regionAdjacency[j, i] = 1;
                }
            }


            // Rarefact the adjacency matrix
            List<int> rared = new List<int>();
            List<int> tmpAdj = new List<int>();
            for (int k = 0; k < regionsNum; ++k)
            {
                tmpAdj.Add(0);
            }
            bool nextDeletion = true;
            int counter = 0;

            while (nextDeletion)
            {
                int regToDel = Random.Range(0, regionsNum);
                bool itWas = rared.Any(item => item == regToDel);
                if (!itWas)
                {
                    // Try to zeroficate all edges
                    for (int k = 0; k < regionsNum; ++k)
                    {
                        visited[k] = false;
                        tmpAdj[k] = regionAdjacency[regToDel, k];
                        regionAdjacency[regToDel, k] = 0;
                        regionAdjacency[k, regToDel] = 0;
                    }

                    int start = 0;
                    for (int k = 0; k < regionsNum; ++k)
                    {
                        if (!rared.Contains(k))
                        {
                            start = k;
                            break;
                        }
                    }
                    DFS(start);

                    bool canDel = true;
                    for (int k = 0; k < regionsNum; ++k)
                    {
                        if (!visited[k])
                        {
                            if (k != regToDel && !rared.Contains(k))
                            {
                                canDel = false;
                            }
                        }
                    }

                    if (canDel)
                    {
                        rared.Add(regToDel);
                    }
                    else
                    {
                        for (int k = 0; k < regionsNum; ++k)
                        {
                            regionAdjacency[regToDel, k] = tmpAdj[k];
                            regionAdjacency[k, regToDel] = tmpAdj[k];
                        }
                    }

                }

                counter++;
                if (counter >= sparceAttempts)
                {
                    nextDeletion = false;
                }
                if (rared.Count >= regionsNum * rarePercent / 100) ;
                {
                    nextDeletion = false;
                }
            }


            //Debug.Log("End of region growth");
        }

        private void DFS(int num)
        {
            visited[num] = true;
            for (int i = 0; i < regionsNum; ++i)
            {
                if (regionAdjacency[num, i] == 1 && !visited[i])
                {
                    DFS(i);
                }
            }
        }

        public void HasTileBorder(int tileNum, ref int[] neibs)
        {


            int myX, myY, neibX, neibY, neibInd;

            myX = tileNum % (2 * deltaXTile + 1) - deltaXTile;
            myY = (tileNum - (tileNum % (2 * deltaXTile + 1))) / (2 * deltaXTile + 1) - deltaYTile;

            // Check for neibours

            // Common check
            neibs[2] = 0;
            neibX = myX + 1;
            neibY = myY;
            if (neibX <= deltaXTile)
            {
                neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                if (regionAssign[tileNum] != regionAssign[neibInd])
                {
                    neibs[2] = 1;
                }
            }
            else
            {
                neibs[2] = 1;
            }

            neibs[5] = 0;
            neibX = myX - 1;
            neibY = myY;
            if (neibX >= -deltaXTile)
            {
                neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                if (regionAssign[tileNum] != regionAssign[neibInd])
                {
                    neibs[5] = 1;
                }
            }
            else
            {
                neibs[5] = 1;
            }

            // Check, based on height
            if (Mathf.Abs(myY) % 2 == 0) // Четная строка
            {
                neibs[4] = 0;
                neibX = myX - 1;
                neibY = myY - 1;
                if (neibX >= -deltaXTile && neibY >= -deltaYTile)
                {
                    neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                    if (regionAssign[tileNum] != regionAssign[neibInd])
                    {
                        neibs[4] = 1;
                    }
                }
                else
                {
                    neibs[4] = 1;
                }

                neibs[3] = 0;
                neibX = myX;
                neibY = myY - 1;
                if (neibY >= -deltaYTile)
                {
                    neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                    if (regionAssign[tileNum] != regionAssign[neibInd])
                    {
                        neibs[3] = 1;
                    }
                }
                else
                {
                    neibs[3] = 1;
                }

                neibs[1] = 0;
                neibX = myX;
                neibY = myY + 1;
                if (neibY <= deltaYTile)
                {
                    neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                    if (regionAssign[tileNum] != regionAssign[neibInd])
                    {
                        neibs[1] = 1;
                    }
                }
                else
                {
                    neibs[1] = 1;
                }

                neibs[0] = 0;
                neibX = myX - 1;
                neibY = myY + 1;
                if (neibX >= -deltaXTile && neibY <= deltaYTile)
                {
                    neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                    if (regionAssign[tileNum] != regionAssign[neibInd])
                    {
                        neibs[0] = 1;
                    }
                }
                else
                {
                    neibs[0] = 1;
                }
            }

            if (Mathf.Abs(myY) % 2 == 1) // Нечетная строка
            {
                neibs[4] = 0;
                neibX = myX;
                neibY = myY - 1;
                if (neibY >= -deltaYTile)
                {
                    neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                    if (regionAssign[tileNum] != regionAssign[neibInd])
                    {
                        neibs[4] = 1;
                    }
                }
                else
                {
                    neibs[4] = 1;
                }

                neibs[3] = 0;
                neibX = myX + 1;
                neibY = myY - 1;
                if (neibX <= deltaXTile && neibY >= -deltaYTile)
                {
                    neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                    if (regionAssign[tileNum] != regionAssign[neibInd])
                    {
                        neibs[3] = 1;
                    }
                }
                else
                {
                    neibs[3] = 1;
                }

                neibs[1] = 0;
                neibX = myX + 1;
                neibY = myY + 1;
                if (neibX <= deltaXTile && neibY <= deltaYTile)
                {
                    neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                    if (regionAssign[tileNum] != regionAssign[neibInd])
                    {
                        neibs[1] = 1;
                    }
                }
                else
                {
                    neibs[1] = 1;
                }

                neibs[0] = 0;
                neibX = myX;
                neibY = myY + 1;
                if (neibY <= deltaYTile)
                {
                    neibInd = (neibX + deltaXTile) + (neibY + deltaYTile) * (2 * deltaXTile + 1);
                    if (regionAssign[tileNum] != regionAssign[neibInd])
                    {
                        neibs[0] = 1;
                    }
                }
                else
                {
                    neibs[0] = 1;
                }
            }

            return;
        }

        public int GetIndexByCoord(Vector3Int crd)
        {
            int ans = 0;

            for (int i = 0; i < tileCoords.Length; ++i)
            {
                if (tileCoords[i] == crd)
                {
                    return i;
                }
            }

            return ans;
        }
    }
}

