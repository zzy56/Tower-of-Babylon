using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{

    public GameObject controlCube;
    public TileType currentTileType;
    public GameObject currentInstance;
    public GridManager gdManager;
    public bool canBuild = false;
    public bool inBound = false;

    int rotation = 0;
    Vector3 size;
    private Camera cam;
    private TileManager tileManager;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        cam = gameObject.GetComponentInChildren<Camera>();
        tileManager = gameObject.GetComponent<TileManager>();
        currentTileType = tileManager.allTileTypes[0];
        currentInstance = currentTileType.obj;
        size = currentTileType.dimension;
    }

    // Update is called once per frame
    void Update()
    {
        inBound = UpdateControlCube();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            
            SwitchCurrentTile();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rotation += 1;
            rotation %= 4;
            Debug.Log(rotation);
            //rotate obj by 90 degrees;
        }


        if (Input.GetMouseButtonDown(0) && inBound)
        {
            PlaceBlock(tileManager.currentIndex);
            Debug.Log(canBuild);
        }

        if (Input.GetMouseButtonDown(2) && inBound)
        {
            EraseBlock();
        }
    }

    void PlaceBlock(int index)
    {
        
        Vector3 pos = controlCube.transform.position;
        Vector3 gridPos = gdManager.GetGridPostion(pos);
        List<Vector3> gridPosList = new List<Vector3>(); //list of spaces that this block will take
        //Vector3 rotationVector = RotateClockWise(rotation, size);
        for (int x=0; x< size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    gridPosList.Add(gridPos + Direction(rotation, new Vector3Int(x, y, z))); //add each to list
                    Debug.Log(gridPos);
                    Debug.Log(gridPos + Direction(rotation, new Vector3Int(x, y, z)));
                }
            }
        }


        canBuild = true;
        foreach (Vector3 gPos in gridPosList) // check if any block is occupied
        {

            if (gdManager.grid.isCellOccupied(gPos))
            {
                canBuild = false;
                Debug.Log("invaild position");
            }


        }

        if (canBuild)
        {
            Instantiate(currentInstance, pos, Quaternion.Euler(0, rotation * 90, 0)); // build with given roation
            foreach (Vector3 gPos in gridPosList)
            {
                gdManager.grid.SetText(true, gPos);
            }
        }
        Debug.Log(gdManager.dimension);
    }

    void EraseBlock()
    {
        //erase block
    }

    void SwitchCurrentTile()
    {
        tileManager.currentIndex = (tileManager.currentIndex + 1) % tileManager.allTileTypes.Length;
        currentTileType = tileManager.allTileTypes[tileManager.currentIndex];
        currentInstance = currentTileType.obj;
        size = currentTileType.dimension;
        rotation = 0;
    }

    bool UpdateControlCube()
    {
        //Debug.Log("This hit at " + hit.point);
        Vector3 MouseHit = MouseHitWorldPosition();
        if (MouseHit != new Vector3(0, 0, 0))
        {
            Vector3 GridPos = gdManager.GetGridPostion(MouseHit);
            controlCube.transform.position = GridPos * gdManager.cellSize + new Vector3(gdManager.cellSize, gdManager.cellSize, gdManager.cellSize) / 2;
            return true;
        }
        else
        {
            return false;
        }

    }

    Vector3 MouseHitWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return new Vector3(hit.point.x, hit.point.y, hit.point.z);
    }

    Vector3 Direction(int direction, Vector3Int size)
    {
        int x = size.x;
        int y = size.y;
        int z = size.z;

        switch (direction)
        {
            case 0:
                return (new Vector3(x, y, z));
            case 1:
                return (new Vector3(z, y, -x));
            case 2:
                return (new Vector3(-x, y, -z));
            case 3:
                return (new Vector3(-z, y, x));
            default:
                return (size);

        }
    }

}
