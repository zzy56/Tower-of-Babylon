using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int height;
    private int width;
    private int[,] m_gridArray;

    public Grid(int height, int width)
    {
        this.height = height;
        this.width = width;

        m_gridArray = new int[height, width];

        for (int x=0; x<m_gridArray.GetLength(0); x++)
        {
            for (int y=0; y< m_gridArray.GetLength(1); y++)
            {
                Debug.Log(x + " " + y);
            }
        }

        
    }
}
