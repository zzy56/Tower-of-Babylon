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
    Vector3 size;

    private Camera cam;
    private TileManager tileManager;

    public enum Direction { up, left, down, right }
    
    // Start is called before the first frame update
    void Awake()
    {
        cam = gameObject.GetComponentInChildren<Camera>();
        tileManager = gameObject.GetComponent<TileManager>();
        currentTileType = tileManager.allTileTypes[0];
        currentInstance = currentTileType.obj;
        size = currentTileType.dimension;
        //Direction dir = Direction.up;
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
            //rotate obj by 90 degrees;
        }


        if (Input.GetMouseButtonDown(0) && inBound)
        {
            PlaceTile(tileManager.currentIndex);
            Debug.Log(canBuild);
        }
    }

    void PlaceTile(int index)
    {
        
        Vector3 pos = controlCube.transform.position;
        Vector3 gridPos = gdManager.GetGridPostion(pos);
        List<Vector3> gridPosList = new List<Vector3>();
        for (int x=0; x< size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    gridPosList.Add(gridPos + new Vector3(x, y, z));
                }
            }
        }

        canBuild = true;
        foreach(Vector3 gPos in gridPosList)
        {

            if (gdManager.grid.GetValueOfCell(gPos) != 0)
            {
                canBuild = false;
                Debug.Log("invaild position");
            }


        }

        if(canBuild)
        {
            Instantiate(currentInstance, pos, Quaternion.Euler(0, 0, 0));
            foreach(Vector3 gPos in gridPosList)
            {
                gdManager.grid.SetText(1, gPos);
            }
        }
        Debug.Log(gdManager.dimension);
    }

    void SwitchCurrentTile()
    {
        tileManager.currentIndex = (tileManager.currentIndex + 1) % tileManager.allTileTypes.Length;
        currentTileType = tileManager.allTileTypes[tileManager.currentIndex];
        currentInstance = currentTileType.obj;
        size = currentTileType.dimension;
        //controlCube.transform.localScale = currentTileType.dimension;
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


        Vector3 MouseHitWorldPosition()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            return new Vector3(hit.point.x, hit.point.y, hit.point.z);
        }
    }

}
