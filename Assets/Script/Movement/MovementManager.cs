using DG.Tweening;
using DG.Tweening.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static MovementManager instance;

    #region Cast Variables
    [SerializeField]
    private LayerMask _childLayer;
    [SerializeField]
    private LayerMask _parentLayer;
    private GameObject _hitObject;
    #endregion

    [SerializeField]
    private XYAxisMovementInput _movementInput;
    [SerializeField]
    private List<GameObject> _starBlocks;
    [SerializeField]
    private Transform _matchColorContainer;

    private bool _isSwitching = false;

    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        _starBlocks = new List<GameObject>();
    }

    private void Update()
    {
        if (_isSwitching)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _hitObject = RayToCheckClickedStarBlock();
        }

        if(Input.GetMouseButtonUp(0))
        {
            Vector2 dir = _movementInput.GetStarBlockMoveDirection();
            if(dir == Vector2.zero || !_hitObject)
            {
                return;
            }
            StarBlockMovementController(_hitObject);
            if(_starBlocks.Count > 0)
            {
                CheckStarBlockToSwitch(_hitObject, _starBlocks, dir);
            }
        }
    }

    public void SetMatchColorContainer(Transform matchColorContainer)
    {
        _matchColorContainer = matchColorContainer;
    }

    private void CheckMatchColors()
    {
        foreach(Transform child in _matchColorContainer)
        {
            child.GetComponent<MatchBlock>().CheckStarBlock();
        }
        if(CheckMatchBlocksToWin())
        {
            GameManager.instance.PlayerWinTheLevelNowPopup();
        }
    }

    private bool CheckMatchBlocksToWin()
    {
        foreach(Transform child in _matchColorContainer)
        {
            if(child.GetComponent<MatchBlock>().IsThisMatchedStarBlock() == false)
            {
                return false;
            }
        }
        return true;
    }

    private void CheckStarBlockToSwitch(GameObject switchBlock,List<GameObject> blocksToSwitch, Vector2 dir)
    {
        if(dir == Vector2.zero || !switchBlock || blocksToSwitch.Count <= 0)
        {
            return;
        } 

        if(dir == Vector2.down || dir == Vector2.up)
        {
            SwitchStarBlockVerticalManager(switchBlock, blocksToSwitch, dir);
        }
        else if(dir == Vector2.left || dir == Vector2.right)
        {
            SwitchStarBlockHorizontalManager(switchBlock, blocksToSwitch, dir);
        }
    }

    #region Switch Star Block Horizontal

    private void SwitchStarBlockHorizontalManager(GameObject switchBlock, List<GameObject> blocksToSwitch, Vector2 dir)
    {
        int heightRequired = switchBlock.GetComponent<StarBlock>().GetStarBlockHeight();
        int switchBlocksHeight = 0;
        foreach(GameObject block in blocksToSwitch)
        {
            switchBlocksHeight += block.GetComponent<StarBlock>().GetStarBlockHeight();
        }

        if(switchBlocksHeight != heightRequired)
        {
            return;
        }

        int widthRequired = blocksToSwitch[0].GetComponent<StarBlock>().GetStarBlockWidth();
        foreach(GameObject block in blocksToSwitch)
        {
            if(block.GetComponent<StarBlock>().GetStarBlockWidth() != widthRequired)
            {
                return;
            }
        }
        _isSwitching = true;
        Vector3Int switchBlockCell = GridCellManager.instance.GetCellPositionOfGivenPosition(switchBlock.transform.position);
        MoveStarBlocksHorizontal(switchBlockCell, switchBlock, blocksToSwitch, dir, widthRequired);
    }

    private void MoveStarBlocksHorizontal(Vector3Int switchBlockCell, GameObject switchBlock, List<GameObject> movingBlocks, Vector2 dir, int widthRequired)
    {
        foreach(GameObject block in movingBlocks)
        {
            Vector3Int blockCell = GridCellManager.instance.GetCellPositionOfGivenPosition(block.transform.position);
            Vector3Int moveTo = GetCellToMoveStarBlockHorizontal(blockCell, switchBlockCell, switchBlock, dir);
            block.transform.DOMove(GridCellManager.instance.GetWordPositionOfGivenCellPosition(moveTo), 0.5f).SetEase(Ease.OutElastic);
        }

        Vector3Int startBlockDestination = switchBlockCell;
        startBlockDestination.x += (int)dir.x * widthRequired;
        switchBlock.transform.DOMove(GridCellManager.instance.GetWordPositionOfGivenCellPosition(startBlockDestination), 0.5f).SetEase(Ease.OutElastic)
            .OnComplete(() =>
        {
            _isSwitching = false;
            CheckMatchColors();
        });
    }

    private Vector3Int GetCellToMoveStarBlockHorizontal(Vector3Int startCell, Vector3Int destinationCell, GameObject switchBlock, Vector2 dir)
    {
        int width = switchBlock.GetComponent<StarBlock>().GetStarBlockWidth();

        dir = -dir;

        Vector3Int cellToMove = startCell;
        cellToMove.x += (int)dir.x * width;

        return cellToMove;
    }

    #endregion

    #region Switch Star Block Vertical
    private void SwitchStarBlockVerticalManager(GameObject switchBlock, List<GameObject> blocksToSwitch, Vector2 dir)
    {
        int widthRequired = switchBlock.GetComponent<StarBlock>().GetStarBlockWidth();

        int switchBlocksWidth = 0;
        foreach(GameObject block in blocksToSwitch)
        {
            switchBlocksWidth += block.GetComponent<StarBlock>().GetStarBlockWidth();
        }

        if(switchBlocksWidth != widthRequired)
        {
            return;
        }

        int heightRequired = blocksToSwitch[0].GetComponent<StarBlock>().GetStarBlockHeight();
        foreach(GameObject block in blocksToSwitch)
        {
            if(block.GetComponent<StarBlock>().GetStarBlockHeight() != heightRequired)
            {
                return;
            }
        }
        _isSwitching = true;
        Vector3Int switchBlockCell = GridCellManager.instance.GetCellPositionOfGivenPosition(switchBlock.transform.position);
        MoveStarBlocksVertical(switchBlockCell, switchBlock, blocksToSwitch, dir, heightRequired);
    }

    private void MoveStarBlocksVertical(Vector3Int switchBlockCell, GameObject switchBlock, List<GameObject> movingBlocks, Vector2 dir, int heightRequired)
    {
        foreach(GameObject block in movingBlocks)
        {
            Vector3Int blockCell = GridCellManager.instance.GetCellPositionOfGivenPosition(block.transform.position);
            Vector3Int moveTo = GetCellToMoveStarBlockVertical(blockCell, switchBlockCell, switchBlock, dir);
            block.transform.DOMove(GridCellManager.instance.GetWordPositionOfGivenCellPosition(moveTo), 0.5f).SetEase(Ease.OutElastic);
        }

        Vector3Int startBlockDestination = switchBlockCell;
        startBlockDestination.y += (int)dir.y * heightRequired;
        switchBlock.transform.DOMove(GridCellManager.instance.GetWordPositionOfGivenCellPosition(startBlockDestination), 0.5f).SetEase(Ease.OutElastic)
            .OnComplete(() =>
        {
            _isSwitching = false;
            CheckMatchColors();
        });
    }

    private Vector3Int GetCellToMoveStarBlockVertical(Vector3Int startCell, Vector3Int destinationCell, GameObject switchBlock, Vector2 dir)
    {
        int height = switchBlock.GetComponent<StarBlock>().GetStarBlockHeight();

        dir = -dir;

        Vector3Int cellToMove = startCell;
        cellToMove.y += (int)dir.y * height;

        return cellToMove;
    }

    #endregion
    private void StarBlockMovementController(GameObject starBlock)
    {
        if (starBlock != null)
        {
            _starBlocks.Clear();
            Vector2 moveDir = _movementInput.GetStarBlockMoveDirection();
            foreach (Transform child in starBlock.transform)
            {
                GameObject nextStarBlock = CheckNearByStarBlock(child.gameObject, moveDir);
                if(nextStarBlock != null) 
                {
                    if (!_starBlocks.Contains(nextStarBlock.transform.parent.gameObject))
                    {
                        _starBlocks.Add(nextStarBlock.transform.parent.gameObject);
                    }
                }
                else
                {
                    _starBlocks.Clear();
                    return;
                }
            }
        }
    }


    #region Block Checker

    private GameObject CheckNearByStarBlock(GameObject starBlockToCheck, Vector2 dir)
    {
        Vector2 startPosition = starBlockToCheck.transform.position;
        Vector3Int starBlockCell = GridCellManager.instance.GetCellPositionOfGivenPosition(startPosition);

        Vector3Int nextCell = starBlockCell + Vector3Int.FloorToInt(dir);
        Vector3 nextPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(nextCell);
        Collider2D checkingObj = Physics2D.OverlapPoint(nextPos, _childLayer);

        while(checkingObj != null && checkingObj.transform.parent == starBlockToCheck.transform.parent)
        {
            nextCell += Vector3Int.FloorToInt(dir);
            nextPos = GridCellManager.instance.GetWordPositionOfGivenCellPosition(nextCell);
            checkingObj = Physics2D.OverlapPoint(nextPos, _childLayer);
        }

        if(checkingObj == null)
        {
            return null;
        }
        return checkingObj.gameObject;
    }

    private GameObject RayToCheckClickedStarBlock()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _parentLayer);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    #endregion
}
