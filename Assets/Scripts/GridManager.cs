using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    internal Grid grid;

    [SerializeField]
    Vector3Int dimension;

    [SerializeField]
    float cellSize;

    [SerializeField]
    GameObject floorObj;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid(dimension.x, dimension.y, dimension.z, 5f);
        PlaceFloor();
    }

    void PlaceFloor()
    {
        GameObject floor = Instantiate(floorObj, GetWorldPosition(0, 0, 0, grid.GetCellSize()), Quaternion.identity);
        //floor.transform.localScale = new Vector3(dimension.x*cellSize, 1, dimension.z*cellSize);
    }

    public Vector3 GetWorldPosition(int x, int y, int z, float cellSize)
    {
        Vector3 pos = new Vector3(x, y, z) * cellSize;
        return pos;
    }

    public Vector3 GetGridPostion(Vector3 worldPosition)
    {
        return new Vector3Int(Mathf.FloorToInt(worldPosition.x / cellSize),
                              Mathf.FloorToInt(worldPosition.y / cellSize),
                              Mathf.FloorToInt(worldPosition.z / cellSize));
    }

}
