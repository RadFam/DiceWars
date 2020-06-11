using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using RegionStructure;
using UIControls;

namespace GameControls
{
    public class ControlScript : MonoBehaviour
    {
        public enum ArmyTypes { Dice_d6, Dice_d12, Dice_d20 };

        public Grid tileGrid;
        public List<Tile> allTiles;
        public List<Tile> borderTiles;
        //public Tile redTile;
        public Tilemap gameTilemap;
        public List<Tilemap> borderTilemaps;
        public Tilemap diceTilemap;

        private BattleUnitGraphics graphData;

        public int playersCount;
        public int initArmy;

        private Vector3Int position;
        private Camera cam;

        private float timer;
        private RegionMap RM;
        private List<int> darkenedRegions;

        [SerializeField]
        private int initPlayerNum;

        public RegionMap GetRM
        {
            get { return RM; }
        }

        // Start is called before the first frame update
        void Start()
        {
            graphData = CommonControl.instance.allGraphics;
            darkenedRegions = new List<int>();

            cam = Camera.main;
            position = new Vector3Int(-1, 1, 0);
            //ShowData();

            timer = 0.0f;
            RM = gameObject.GetComponent<RegionMap>();
            RM.GenerateRegionsMap();
            InitiatePlayerDistribution();
            InitiateArmyDistribution();
        }

        public void ShowData()
        {
            Tile tile = gameTilemap.GetTile(position) as Tile;
            TileData td = new TileData();
            //tile.GetTileData(position, gameTilemap, ref td);
            //Debug.Log("Tile.sprite: " + tile.sprite.name);
            //gameTilemap.SetTile(position, redTile);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                Vector3 mV = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tV = tileGrid.WorldToCell(mV);

                //gameTilemap.SetTile(tV, redTile);

                int ind = RM.GetIndexByCoord(tV);
                int reg = RM.GetRegion[ind];
                int regDBL = RM.GetRegionDBL[ind];
                int pl = RM.GetPlayerByCoord(tV);

                Debug.Log("Cell coord: " + tV + "   region num: " + reg.ToString() + "   region INITnum: " + regDBL.ToString() + "  PLAYER: " + pl.ToString());

                OnRegionClick();
            }

            if (Input.GetMouseButtonDown(1))
            {
                DrawTestRegions();
                DrawBattleUnits();
            }

            if (Input.GetMouseButtonDown(2))
            {
                Vector3 mV = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tV = tileGrid.WorldToCell(mV);
                RM.GetAdjacency(tV);
            }
        }

        // Отрисовка всего игрового поля 
        public void DrawTestRegions()
        {
            int[] regType = RM.GetRegion;
            Vector3Int[] regCoord = RM.GetCoords;

            int type;
            int player;
            float type2;
            Color hexColor;
            for (int i = 0; i < regType.Length; ++i)
            {
                type = regType[i];
                player = RM.GetPlayerByCoord(i);
                type2 = (float)type;
                if (type != -1)
                {
                    hexColor = new Color(type2 / RM.regionsNum, 1.0f, 1.0f);
                    //Debug.Log("My current player is: " + player.ToString());
                    gameTilemap.SetTile(regCoord[i], allTiles[player]);
                }
                else
                {
                    hexColor = new Color(0.0f, 0.0f, 1.0f);
                }

                //gameTilemap.SetTile(regCoord[i], allTiles[6]);
                //gameTilemap.SetTileFlags(regCoord[i], TileFlags.None);
                //gameTilemap.SetColor(regCoord[i], hexColor);
                //gameTilemap.RefreshTile(regCoord[i]);
            }

            int[] border = new int[6];
            Vector3Int tmpVct;
            for (int i = 0; i < regCoord.Length; ++i)
            {
                if (regType[i] != -1)
                {
                    border[0] = 0; border[1] = 0; border[2] = 0; border[3] = 0; border[4] = 0; border[5] = 0;

                    RM.HasTileBorder(i, ref border);
                    for (int j = 0; j < 6; ++j)
                    {
                        if (border[j] == 1)
                        {
                            tmpVct = new Vector3Int(regCoord[i].x, regCoord[i].y, -1);
                            borderTilemaps[j].SetTile(regCoord[i], borderTiles[j]);
                        }
                    }
                }
            }
        }

        public void InitiatePlayerDistribution()
        {
            int accReg = RM.GetAccRegions.Count;
            int fullReg = accReg / playersCount;
            int resReg = accReg % playersCount;

            List<int> tmpNmbrs = new List<int>();
            for (int i = 0; i < accReg; ++i)
            {
                tmpNmbrs.Add(i);
            }

            for (int pl = 0; pl < playersCount; ++pl)
            {
                int smplCounter = 0;
                int regCounter = fullReg;
                if (pl < resReg)
                {
                    regCounter = fullReg + 1;
                }

                while (smplCounter < regCounter)
                {
                    int tmpInd = Random.Range(0, tmpNmbrs.Count);
                    RM.GetAccRegions[tmpNmbrs[tmpInd]].myPlayer = pl;
                    smplCounter++;
                    tmpNmbrs.RemoveAt(tmpInd);
                }
            }
        }

        public void InitiateArmyDistribution()
        {
            for (int i = 0; i < playersCount; ++i)
            {
                int armyCount = initArmy;

                var regInd = RM.GetAccRegions.Select((val, ind) => new { val, ind }).Where(x => x.val.myPlayer == i).Select(x => x.ind).ToList();

                foreach (int index in regInd)
                {
                    Dice dc = new Dice();
                    dc.AddUnits(1);
                    armyCount--;
                    Region reg = RM.GetAccRegions[index];
                    reg.myArmyOnRegion.Add(dc);
                    RM.GetAccRegions[index] = reg;
                }

                // А теперь, оставшиеся войска рандомно расставляем
                int cntr = 0;
                int addArmyNum = 0;
                int res = 0;
                while (armyCount > 0)
                {
                    addArmyNum = Random.Range(1, 4);
                    addArmyNum = Mathf.Min(addArmyNum, armyCount);
                    res = RM.GetAccRegions[regInd[cntr]].AddArmy(addArmyNum, ArmyTypes.Dice_d6);
                    armyCount = armyCount + res - addArmyNum;
                    cntr++;
                    if (cntr == regInd.Count)
                    {
                        cntr = 0;
                    }
                }
            }
        }

        public void DrawBattleUnits()
        {
            foreach (Region reg in RM.GetAccRegions)
            {
                Vector3Int placeUnit = new Vector3Int(reg.RegCenter.x, reg.RegCenter.y, -8);
                int player = reg.myPlayer;
                diceTilemap.SetTile(placeUnit, graphData.unitTile_class_01[player]);
            }
        }

        public void OnRegionClick()
        {
            // Определяем на какой тайл нажали - какому региону принадлежит
            Vector3 mV = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tV = tileGrid.WorldToCell(mV);

            // Get Number of region
            int ind = RM.GetIndexByCoord(tV);
            int reg = RM.GetRegion[ind];

            // Проверяем, единственный ли этот регион в списке
            if (darkenedRegions.Count == 0)
            {
                darkenedRegions.Add(reg);
                SubdrawRegion(reg, true);
            }
            if (darkenedRegions.Count == 1)
            {
                int firstReg = darkenedRegions[0];

                // Проверяем, соседние ли это враждебные территории

                // Не соседние
                if (RM.GetAdjMatrix[firstReg, reg] == 0)
                {
                    SubdrawRegion(firstReg, false);
                    darkenedRegions.Clear();
                    darkenedRegions.Add(reg);
                    SubdrawRegion(reg, true);
                }

                if (RM.GetAdjMatrix[firstReg, reg] == 1)
                {
                    int ind_1 = RM.GetAccRegions.FindIndex(x => x.RegNum == firstReg);
                    int ind_2 = RM.GetAccRegions.FindIndex(x => x.RegNum == reg);

                    // Один и тот же игрок
                    if (RM.GetAccRegions[ind_1].myPlayer == RM.GetAccRegions[ind_2].myPlayer)
                    {
                        SubdrawRegion(firstReg, false);
                        darkenedRegions.Clear();
                        darkenedRegions.Add(reg);
                        SubdrawRegion(reg, true);
                    }
                    else
                    {
                        darkenedRegions.Add(reg);
                        SubdrawRegion(reg, true);

                        // Start Attack procedure
                        ClashScript CS = gameObject.GetComponent<ClashScript>();
                        CS.OnClash(RM.GetCoordByRegion(firstReg), RM.GetCoordByRegion(reg));

                        // If player was an attacker - show it
                        if (RM.GetAccRegions[ind_1].myPlayer == initPlayerNum)
                        {
                            GameUIViewController gameUIVC = FindObjectOfType<GameUIViewController>();
                            gameUIVC.ShowClashAttack();
                            UndarkRegions();


                        }
                    }
                }
            }
        }

        public void ChangeArmyDistribution(int reg_1, int reg_2, bool successAttack)
        {
            if (successAttack)
            {

            }
            else
            {
                RM.GetAllRegions[reg_1].DefeatArmy(ArmyTypes.Dice_d6);
            }
        }

        public void UndarkRegions()
        {
            foreach (int nm in darkenedRegions)
            {
                SubdrawRegion(nm, false);
            }
            darkenedRegions.Clear();
        }

        private void SubdrawRegion(int regNum, bool darken)
        {
            int ind = RM.GetAccRegions.FindIndex(x => x.RegNum == regNum);

            Color hexColor;
            if (darken)
            {
                hexColor = new Color(0.2f, 0.2f, 0.2f);
            }
            else
            {
                hexColor = new Color(1.0f, 1.0f, 1.0f);
            }

            for (int i = 0; i < RM.GetAccRegions[ind].RegTiles.Count; ++i)
            {
                gameTilemap.SetTileFlags(RM.GetAccRegions[ind].RegTiles[i], TileFlags.None);
                gameTilemap.SetColor(RM.GetAccRegions[ind].RegTiles[i], hexColor);
            }
        }
    }
}