using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using RegionStructure;

public class ControlScript : MonoBehaviour
{
    public Grid tileGrid;
    public List<Tile> allTiles;
    public List<Tile> borderTiles;
    public Tile redTile;
    public Tilemap gameTilemap;
    public List<Tilemap> borderTilemaps;
    private Vector3Int position;
    private Camera cam;

    private float timer;
    private RegionMap RM;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        position = new Vector3Int(-1, 1, 0);
        //ShowData();

        timer = 0.0f;
        RM = gameObject.GetComponent<RegionMap>();
        RM.GenerateRegionsMap();
    }

    public void ShowData()
    {
        Tile tile = gameTilemap.GetTile(position) as Tile;
        TileData td = new TileData();
        //tile.GetTileData(position, gameTilemap, ref td);
        //Debug.Log("Tile.sprite: " + tile.sprite.name);
        gameTilemap.SetTile(position, redTile);
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

            Debug.Log("Cell coord: " + tV + "   region num: " + reg.ToString());
        }

        if (Input.GetMouseButtonDown(1))
        {
            DrawTestRegions();
        }
    }

    public void DrawTestRegions()
    {
        int[] regType = RM.GetRegion;
        Vector3Int[] regCoord = RM.GetCoords;

        int type;
        float type2;
        Color hexColor;
        for (int i = 0; i < regType.Length; ++i)
        {
            type = regType[i];
            type2 = (float)type;
            hexColor = new Color(type2 / RM.regionsNum, 1.0f, 1.0f);            
            gameTilemap.SetTile(regCoord[i], allTiles[6]);
            gameTilemap.SetTileFlags(regCoord[i], TileFlags.None);
            gameTilemap.SetColor(regCoord[i], hexColor);
            //gameTilemap.RefreshTile(regCoord[i]);
        }

        int[] border = new int[6];
        Vector3Int tmpVct;
        for (int i = 0; i < regCoord.Length; ++i)
        {
            border[0] = 0; border[1] = 0; border[2] = 0; border[3] = 0; border[4] = 0; border[5] = 0;

            bool toStop = false;


            RM.HasTileBorder(i, ref border);
            for (int j = 0; j < 6; ++j)
            {
                if (border[j] == 1)
                {
                    tmpVct = new Vector3Int(regCoord[i].x, regCoord[i].y, -1-j);
                    //gameTilemap.SetTile(tmpVct, borderTiles[j]);
                    borderTilemaps[j].SetTile(regCoord[i], borderTiles[j]);

                    toStop = true;
                    //Debug.Log("Neib: " + j.ToString() + "  tile coord: " + regCoord[i]);
                }
            }
        }
    }
}
