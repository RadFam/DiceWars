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

        public ControlSequenceOfActions CSoA;
        public Grid tileGrid;
        public List<Tile> allTiles;
        public List<Tile> borderTiles;
        //public Tile redTile;
        public Tilemap gameTilemap;
        public List<Tilemap> borderTilemaps;
        public Tilemap diceTilemap;

        private BattleUnitGraphics graphData;

        public int playersCount; // Number of all players
        public int initArmy; // Initial value of dices for each player

        public bool humanMove;

        private Vector3Int position;
        private Camera cam;

        private float timer;
        private RegionMap RM;
        private RegionMap RM_D;
        private List<int> darkenedRegions;

        private bool canPickOnTiles;
        private UISequenceScript uiSS;

        [SerializeField]
        private int initPlayerNum; // number of player which starts the game
        public int CurrPlayerNum
        {
            get { return initPlayerNum; }
            set { initPlayerNum = value; }
        }

        public RegionMap GetRM
        {
            get { return RM; }
        }

        public bool CanTilePicking
        {
            get { return canPickOnTiles; }
            set { canPickOnTiles = value; }
        }

        public List<int> GetDarkenedRegions
        {
            get { return darkenedRegions; }
        }

        // Start is called before the first frame update
        void Start()
        {
            //CSoA = gameObject.GetComponent<ControlSequenceOfActions>();
            //graphData = CommonControl.instance.allGraphics;
            //borderTilemaps = new List<Tilemap>();
            //darkenedRegions = new List<int>();

            //cam = Camera.main;

            //position = new Vector3Int(-1, 1, 0);

            //timer = 0.0f;
            //RM = gameObject.GetComponent<RegionMap>();

            //humanMove = false;

            //RM.GenerateRegionsMap();
            //InitiatePlayerDistribution();
            //InitiateArmyDistribution();
        }

        public void ActionsAfterSceneLoad()
        {
            CSoA = gameObject.GetComponent<ControlSequenceOfActions>();
            graphData = CommonControl.instance.allGraphics;
            borderTilemaps = new List<Tilemap>();
            darkenedRegions = new List<int>();

            position = new Vector3Int(-1, 1, 0);

            timer = 0.0f;

            RM = null;
            RM = gameObject.GetComponent<RegionMap>();
            RM.CreateNewZerofication();

            humanMove = false;

            // Restore camera
            cam = Camera.main;

            // Make connections of local and global variables
            uiSS = FindObjectOfType<UISequenceScript>();
            tileGrid = FindObjectOfType<Grid>(); Debug.Log("tileGrid: " + tileGrid + "   Region Map: " + RM);
            gameTilemap = tileGrid.transform.GetChild(0).gameObject.GetComponent<Tilemap>();
            borderTilemaps.Add(tileGrid.transform.GetChild(2).gameObject.GetComponent<Tilemap>());
            borderTilemaps.Add(tileGrid.transform.GetChild(3).gameObject.GetComponent<Tilemap>());
            borderTilemaps.Add(tileGrid.transform.GetChild(4).gameObject.GetComponent<Tilemap>());
            borderTilemaps.Add(tileGrid.transform.GetChild(5).gameObject.GetComponent<Tilemap>());
            borderTilemaps.Add(tileGrid.transform.GetChild(6).gameObject.GetComponent<Tilemap>());
            borderTilemaps.Add(tileGrid.transform.GetChild(1).gameObject.GetComponent<Tilemap>());
            diceTilemap = tileGrid.transform.GetChild(7).gameObject.GetComponent<Tilemap>();

            canPickOnTiles = false;

            // Clear all tilemaps
            gameTilemap.ClearAllTiles();
            diceTilemap.ClearAllTiles();
            foreach (Tilemap tm in borderTilemaps)
            {
                tm.ClearAllTiles();
            }

            // Generate map
            RM.GenerateRegionsMap();
            InitiatePlayerDistribution();
            InitiateArmyDistribution();

            DrawTestRegions();
            DrawBattleUnits();

            // Start game routine
            //ControlSequenceOfActions CSoA = gameObject.GetComponent<ControlSequenceOfActions>();
            //Debug.Log("Suddenly invoke GoAhead from CS AfterSceneLoad");
            CSoA.GoAhead();
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (canPickOnTiles)
                {
                    Vector3 mV = cam.ScreenToWorldPoint(Input.mousePosition);
                    Debug.Log("mV: " + mV + "  tileGrid: " + tileGrid + "  RM: " + RM);
                    Vector3Int tV = tileGrid.WorldToCell(mV);

                    int ind = RM.GetIndexByCoord(tV);
                    int reg = RM.GetRegion[ind];
                    int regDBL = RM.GetRegionDBL[ind];
                    int pl = RM.GetPlayerByCoord(tV);
                    
                    OnRegionClick();
                }
            }

            
            if (Input.GetKeyUp("space")) // Скрипт должен срабатывать, когда игровая сцена загрузилась
            {
                if (canPickOnTiles)
                {
                    //Debug.Log("Suddenly invoke GoAhead from CS mouse 2");
                    CSoA.GoAhead();
                }
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
            playersCount = CSoA.GetAllGamerNums.Count;

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
                    RM.GetAccRegions[tmpNmbrs[tmpInd]].myPlayer = CSoA.GetAllGamerNums[pl];
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

                var regInd = RM.GetAccRegions.Select((val, ind) => new { val, ind }).Where(x => x.val.myPlayer == CSoA.GetAllGamerNums[i]).Select(x => x.ind).ToList();

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

            RM_D = RM;

            UISequenceScript uiSS = FindObjectOfType<UISequenceScript>();
            uiSS.SetupSequeneceList();
        }

        public void DrawBattleUnits()
        {
            foreach (Region reg in RM.GetAccRegions)
            {
                Vector3Int placeUnit = new Vector3Int(reg.RegCenter.x, reg.RegCenter.y, -8);
                int player = reg.myPlayer;
                int diceCount = reg.GetArmy(ArmyTypes.Dice_d6);
                diceTilemap.SetTile(placeUnit, graphData.unitTile_class_01[player]);
                DrawDiceReserve(placeUnit, player, diceCount, ArmyTypes.Dice_d6);
            }
        }

        public void OnRegionClick()
        {
            // Определяем на какой тайл нажали - какому региону принадлежит
            Vector3 mV = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tV = tileGrid.WorldToCell(mV);

            // Если это левый тайл (его нет, вышли за экран, прочеее...)
            //
            if (RM.GetIndexByCoord(tV) == -1)
            {
                return;
            }

            // Первым надо выделить регион, принадлежащий игроку
            if (darkenedRegions.Count == 1)
            {
                DarkRegion(tV);
            }
            else if (darkenedRegions.Count == 0)
            {
                int indd = RM.GetIndexByCoord(tV);
                int regg = RM.GetRegion[indd];
                int ind_1 = RM.GetAccRegions.FindIndex(x => x.RegNum == regg);

                if (ind_1 != -1 && RM.GetAccRegions[ind_1].myPlayer == initPlayerNum)
                {
                    DarkRegion(tV);
                }
            }
            
            if (darkenedRegions.Count == 2)
            {
                // Теперь, clash regions и смотрим, что из этого получится

                // Start Attack procedure
                canPickOnTiles = false;

                int checkArmy = RM.GetRegionByCoords(RM.GetCoordByRegion(darkenedRegions[0])).GetArmyUnit(ArmyTypes.Dice_d6).myCount;

                if (checkArmy > 1)
                {
                    ClashScript CS = gameObject.GetComponent<ClashScript>();
                    bool res = CS.OnClash(RM.GetCoordByRegion(darkenedRegions[0]), RM.GetCoordByRegion(darkenedRegions[1]));

                    GameUIViewController gameUIVC = FindObjectOfType<GameUIViewController>();
                    gameUIVC.ShowClashAttack();

                    ChangeArmyDistribution(res);
                }

                UndarkRegions();

                canPickOnTiles = true;
            }
        }

        public void ChangeArmyDistribution(bool successAttack)
        {
            // reg_1 and reg_2 - numbers of regions!!!
            int reg_1 = darkenedRegions[0];
            int reg_2 = darkenedRegions[1];

            if (successAttack)
            {
                //Debug.Log("Success change distribution");
                int ind_1 = RM.GetAccRegions.FindIndex(x => x.RegNum == reg_1);
                int ind_2 = RM.GetAccRegions.FindIndex(x => x.RegNum == reg_2);

                int player_a = RM.GetAccRegions[ind_1].myPlayer;
                int player_b = RM.GetAccRegions[ind_2].myPlayer;

                int army = RM.GetAccRegions[ind_1].GetArmy(ArmyTypes.Dice_d6);
                RM.GetAccRegions[ind_1].DefeatArmy(ArmyTypes.Dice_d6);
                RM.GetAccRegions[ind_2].SetArmy(army - 1, ArmyTypes.Dice_d6);
                RM.GetAccRegions[ind_2].myPlayer = RM.GetAccRegions[ind_1].myPlayer;

                uiSS.UpdatePlayerFiledsVol(player_a);
                uiSS.UpdatePlayerFiledsVol(player_b);
            }
            else
            {
                //Debug.Log("Unsuccess change distribution");
                int ind_1 = RM.GetAccRegions.FindIndex(x => x.RegNum == reg_1);
                RM.GetAccRegions[ind_1].DefeatArmy(ArmyTypes.Dice_d6);
            }
        }

        public void DarkRegion(int regionNum)
        {
            // Get coordinate of the region
            int ind = RM.GetAccRegions.FindIndex(x => x.RegNum == regionNum);
            if (ind != -1)
            {
                Vector3Int vct = RM.GetAccRegions[ind].RegCenter;
                DarkRegion(vct);
            }
        }

        public void DarkRegion(Vector3Int crd)
        { 
            // Get Number of region
            int ind = RM.GetIndexByCoord(crd);
            int reg = RM.GetRegion[ind];

            if (reg != -1)
            {
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
                        }
                    }
                }
            }
 
        }

        public void UndarkRegions()
        {
            foreach (int nm in darkenedRegions)
            {
                SubdrawRegion(nm, false);
            }
            darkenedRegions.Clear();

            // Check the result of battle
            if (humanMove)
            {
                if (CheckWinCase(initPlayerNum)) // Win case
                {
                    canPickOnTiles = false;
                    GameUIViewController gameUIVC = FindObjectOfType<GameUIViewController>();
                    UIMessageScript uiMS = gameUIVC.ShowMessageFrame();
                    uiMS.SetStatus(true, initPlayerNum);
                }
            }
            else
            {
                if (CheckLooseCase()) // Loose case
                {
                    // Stop AI
                    // ....
                    CSoA.EmergencyStop();
                    

                    GameUIViewController gameUIVC = FindObjectOfType<GameUIViewController>();
                    UIMessageScript uiMS = gameUIVC.ShowMessageFrame();
                    uiMS.SetStatus(false);
                }
            }
        }

        public void SubdrawRegion(int regNum, bool darken)
        {
            //Debug.Log("Subdraw region: " + regNum.ToString());
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

                // Redraw tiles in the case? when owner of the region is changed
                gameTilemap.SetTile(RM.GetAccRegions[ind].RegTiles[i], allTiles[RM.GetAccRegions[ind].myPlayer]);
            }

            // Redraw dice over the region (in the case of player owenrship change)
            Vector3Int placeUnit = new Vector3Int(RM.GetAccRegions[ind].RegCenter.x, RM.GetAccRegions[ind].RegCenter.y, -8);
            //diceTilemap.SetTile(placeUnit, graphData.unitTile_class_01[RM.GetAccRegions[ind].myPlayer]);
            DrawDiceReserve(placeUnit, RM.GetAccRegions[ind].myPlayer, RM.GetAccRegions[ind].GetArmy(ArmyTypes.Dice_d6), ArmyTypes.Dice_d6);
        }

        private void DrawDiceReserve(Vector3Int placeUnit, int playerNum, int diceCount, ArmyTypes type)
        {
            if (type == ArmyTypes.Dice_d6)
            {
                switch (playerNum)
                {
                    case 0:
                        diceTilemap.SetTile(placeUnit, graphData.aquaDice_Reserve_d6[diceCount - 1]);
                        break;
                    case 1:
                        diceTilemap.SetTile(placeUnit, graphData.blueDice_Reserve_d6[diceCount - 1]);
                        break;
                    case 2:
                        diceTilemap.SetTile(placeUnit, graphData.greenDice_Reserve_d6[diceCount - 1]);
                        break;
                    case 3:
                        diceTilemap.SetTile(placeUnit, graphData.orangeDice_Reserve_d6[diceCount - 1]);
                        break;
                    case 4:
                        diceTilemap.SetTile(placeUnit, graphData.purpleDice_Reserve_d6[diceCount - 1]);
                        break;
                    case 5:
                        diceTilemap.SetTile(placeUnit, graphData.redDice_Reserve_d6[diceCount - 1]);
                        break;
                    case 6:
                        diceTilemap.SetTile(placeUnit, graphData.whiteDice_Reserve_d6[diceCount - 1]);
                        break;
                    case 7:
                        diceTilemap.SetTile(placeUnit, graphData.yellowDice_Reserve_d6[diceCount - 1]);
                        break;
                }
            }
        }

        public bool CheckWinCase(int playerNum)
        {
            int playerTerr = Enumerable.Range(0, RM.GetAccRegions.Count)
                    .Where(x => RM.GetAccRegions[x].myPlayer == playerNum)
                    .ToList().Count;
            int allTerr = RM.GetAccRegions.Count;
            if (playerTerr == allTerr)
            {
                canPickOnTiles = false;
                return true;
            }

            return false;
        }

        public bool CheckLooseCase()
        {
            List<int> humans = CSoA.GetAllPlayerNums;
            int summ = 0;
            int playerTerr = 0;
            foreach (int person in humans)
            {
                playerTerr = Enumerable.Range(0, RM.GetAccRegions.Count)
                    .Where(x => RM.GetAccRegions[x].myPlayer == person)
                    .ToList().Count;
                summ += playerTerr;
            }
            if (summ == 0)
            {
                canPickOnTiles = false;
                return true;
            }

            return false;
        }

        public void RestoreInitialGame()
        {
            RM = RM_D;

            CSoA.RestartSequence();

            UISequenceScript uiSS = FindObjectOfType<UISequenceScript>();
            uiSS.SetupSequeneceList();

            DrawTestRegions();
            DrawBattleUnits();

            CSoA.GoAhead();
        }

        public void RestoreBeforeReload()
        {
            //RM = null;
            //RM_D = null;

            CSoA.RestoreBeforeReload();
        }
    }
}