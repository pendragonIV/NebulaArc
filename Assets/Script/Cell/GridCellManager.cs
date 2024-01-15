using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void SetMapForGridCellManager(Tilemap tilemap)
    {
        if (tilemap != null)
        {
            this.tileMap = tilemap;
        }
    }
    public bool IsThisAreaValidToPlaceBlock(Vector3Int cellPos)
    {
        if (tileMap.GetTile(cellPos) == null)
        {
            return false;
        }
        return true;
    }

    public Vector3Int GetCellPositionOfGivenPosition(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 GetWordPositionOfGivenCellPosition(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }
}
