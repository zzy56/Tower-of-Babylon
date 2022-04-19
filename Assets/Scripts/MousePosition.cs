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

    private Camera cam;
    private TileManager tileManager;


    
    // Start is called before the first frame update
    void Awake()
    {
        cam = gameObject.GetComponentInChildren<Camera>();
        tileManager = gameObject.GetComponent<TileManager>();
        currentTileType = tileManager.allTileTypes[0];
        currentInstance = currentTileType.obj;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateControlCube();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCurrentTile();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //rotate obj by 90 degrees;
        }


        if (Input.GetMouseButtonDown(0) && canBuild)
        {
            PlaceTile(tileManager.currentIndex);
        }
    }

    void PlaceTile(int index)
    {
        Vector3 size = currentTileType.dimension;
        Vector3 pos = new Vector3(controlCube.transform.position.x, controlCube.transform.position.y, controlCube.transform.position.z);
        Instantiate(currentInstance, pos, Quaternion.Euler(0, 0, 0));
        Vector3 GridPos = gdManager.GetGridPostion(pos);
        gdManager.grid.SetText(1, currentTileType.TileName, Mathf.RoundToInt(GridPos.x), Mathf.RoundToInt(GridPos.y), Mathf.RoundToInt(GridPos.z));
    }

    void SwitchCurrentTile()
    {
        tileManager.currentIndex = (tileManager.currentIndex + 1) % tileManager.allTileTypes.Length;
        currentTileType = tileManager.allTileTypes[tileManager.currentIndex];
        currentInstance = currentTileType.obj;
        //controlCube.transform.localScale = currentTileType.dimension;
    }

    void UpdateControlCube()
    {
        //Debug.Log("This hit at " + hit.point);
        Vector3 MouseHit = MouseHitWorldPosition();
        if (MouseHit != new Vector3(0,0,0))
        {
            Vector3 GridPos = gdManager.GetGridPostion(MouseHit);
            controlCube.transform.position = GridPos * gdManager.grid.GetCellSize();
            canBuild = true;
        }
        else
        {
            canBuild = false;
        }
        
        
        //new Vector3(Mathf.Round(hit.point.x) - 0.5f + (currentTileType.dimension.x * 0.5f),Mathf.Round(hit.point.y) - 0.5f + (currentTileType.dimension.y * 0.5f),Mathf.Round(hit.point.z) - 0.5f + (currentTileType.dimension.z * 0.5f));
    }

    Vector3 MouseHitWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return new Vector3(hit.point.x, hit.point.y, hit.point.z);
    }
}
