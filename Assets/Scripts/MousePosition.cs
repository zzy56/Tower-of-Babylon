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
            //rotate obj by 90 degrees;
        }


        if (Input.GetMouseButtonDown(0) && inBound)
        {
            PlaceBlock(tileManager.currentIndex);
        }

        if (Input.GetMouseButtonDown(1) && inBound)
        {

            EraseBlock();
        }
    }

    void PlaceBlock(int index)
    {
        
        Vector3 pos = controlCube.transform.position;
        Vector3 gridPos = gdManager.GetGridPostion(pos);
        List<Vector3> gridPosList = new List<Vector3>(); //list of spaces that this block will take
        for (int x=0; x< size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    gridPosList.Add(gridPos + Direction(rotation, new Vector3Int(x, y, z))); //add each to list
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
                gdManager.grid.SetOccupied(true, gPos);
                gdManager.grid.SetBlockType(currentTileType.blockType, gPos);
            }
        }
    }

    void EraseBlock()
    {
        RaycastHit mouseHit = MouseHit();

        if (mouseHit.collider != null)
        {
            GameObject obj = mouseHit.collider.gameObject.transform.parent.gameObject;
            if(obj.tag != "Floor" && obj != null)
            {
                Vector3 gridPos = gdManager.GetGridPostion(obj.transform.position);
                gdManager.grid.SetOccupied(false, gridPos);
                Destroy(obj);
                Debug.Log("erase");
            }
   
        }

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
        RaycastHit mouseHit = MouseHit();
        Vector3 hitWorldPos = new Vector3(mouseHit.point.x, mouseHit.point.y, mouseHit.point.z);
        if (hitWorldPos != new Vector3(0, 0, 0))
        {
            Vector3 GridPos = gdManager.GetGridPostion(hitWorldPos);
            controlCube.transform.position = GridPos * gdManager.cellSize + new Vector3(gdManager.cellSize, gdManager.cellSize, gdManager.cellSize) / 2;
            return true;
        }
        else
        {
            return false;
        }

    }

    RaycastHit MouseHit()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
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
