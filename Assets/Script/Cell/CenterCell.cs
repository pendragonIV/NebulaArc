using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCell : MonoBehaviour
{
    private void Start()
    {
        SetCenterCell();
    }
    private void SetCenterCell()
    {
        Vector3Int cellPos = GridCellManager.instance.GetCellPositionOfGivenPosition(transform.position);
        this.transform.position = GridCellManager.instance.GetWordPositionOfGivenCellPosition(cellPos);
    }
}
