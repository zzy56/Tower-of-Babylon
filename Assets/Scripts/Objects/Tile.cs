using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public int tileIndex;
    public string tileName;
    public Material tileMat;
    public Vector3Int tileDimension;
    public Vector3 tileOrigin;
    public GameObject tileAppearance;

    public Tile(int index, string name, Material mat, Vector3Int dimension, GameObject appeal)
    {
        tileIndex = index;
        tileName = name;
        tileMat = mat;
        tileDimension = dimension;
        tileOrigin = new Vector3(Mathf.Round(dimension.x / 2), Mathf.Round(dimension.y / 2), 1);
        tileAppearance = appeal;
    }

}
