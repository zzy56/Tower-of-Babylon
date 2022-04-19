using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    int xSize;
    int ySize;
    int zSize;
    float cellSize;
    int[,,] cells;
    TextMesh[,,] texts;

    public Grid(Vector3Int dimension, float cellSize)
    {
        this.xSize = dimension.x;
        this.ySize = dimension.y;
        this.zSize = dimension.z;
        this.cellSize = cellSize;

        cells = new int[xSize, ySize, zSize];
        texts = new TextMesh[xSize, ySize, zSize];

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    GameObject gb = new GameObject("gb", typeof(TextMesh));
                    gb.transform.localPosition = GetWorldPosition(x, y, z, cellSize)+new Vector3(cellSize/2, cellSize/2, cellSize/2);
                    gb.transform.GetComponent<TextMesh>().text = cells[x, y, z].ToString();
                    texts[x, y, z] = gb.transform.GetComponent<TextMesh>();
                    texts[x, y, z].color = Color.black;
                    texts[x, y, z].fontSize = 8;
                    texts[x, y, z].alignment = TextAlignment.Center;
                }
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y, int z, float cellSize)
    {
        Vector3 pos = new Vector3(x, y, z) * cellSize;
        return pos;
    }

    public void SetText(int value, Vector3 gridPosition)
    {
        cells[Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y), Mathf.RoundToInt(gridPosition.z)] = value;
        texts[Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y), Mathf.RoundToInt(gridPosition.z)].text = value.ToString();
    }

    public int GetValueOfCell(Vector3 gridPosition)
    {
        if(gridPosition.x < 0 || gridPosition.x > xSize-1 ||
           gridPosition.y < 0 || gridPosition.y > ySize-1 ||
           gridPosition.z < 0 || gridPosition.z > zSize-1)
        {
            return 1;
        }
        return cells[Mathf.RoundToInt(gridPosition.x), Mathf.RoundToInt(gridPosition.y), Mathf.RoundToInt(gridPosition.z)];
    }

}

