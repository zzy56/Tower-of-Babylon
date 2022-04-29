using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    public TileType[] allTileTypes;

    public int currentIndex;

    private void Awake()
    {
        for (int i=0; i<allTileTypes.Length; i++)
        {
            TileType newTileType = allTileTypes[i];
        }
        currentIndex = 0;
    }

}


[Serializable]
public struct TileType
{
    public string TileName;
    public BlockTypes blockType;
    public Material mat;
    public Vector3Int dimension;
    public GameObject obj;
}
