using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class TileSprite
{
    public string Name;
    public Sprite TileImage;
    public Tiles TileType;

    public TileSprite()
    {
        Name = "Unset";
        TileImage = new Sprite();
        TileType = Tiles.Unset;
    }

    public TileSprite(string name, Sprite image, Tiles tile)
    {
        Name = name;
        TileImage = image;
        TileType = tile;
    }
}
