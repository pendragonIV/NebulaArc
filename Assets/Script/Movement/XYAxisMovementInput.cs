using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class XYAxisMovementInput : MonoBehaviour
{
    #region Variables
    private Vector2 _mouseDownPos;
    private Vector2 _mouseUpPos;
    private Vector2 _moveDirection;
    #endregion

    private void Update()
    {
        PlayerInputHandler();
    }

    #region Getters
    public Vector2 GetStarBlockMoveDirection()
    {
        return _moveDirection;
    }
    #endregion

    #region Setters

    public void SetDefaultMoveDirection()
    {
        _moveDirection = Vector2.zero;
    }

    #endregion

    #region Movement
    private void PlayerInputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseUpPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(_mouseDownPos, _mouseUpPos) > 1f)
            {
                CalculateStarBlockMoveDirection(_mouseDownPos, _mouseUpPos);
            }
            else
            {
                _moveDirection = Vector2.zero;
            }
        }
    }
    private void CalculateStarBlockMoveDirection(Vector2 startPos, Vector2 endPos)
    {
        if (Mathf.Abs(startPos.x - endPos.x) > Mathf.Abs(startPos.y - endPos.y))
        {
            if (startPos.x > endPos.x)
            {
                _moveDirection = Vector2.left;
            }
            else
            {
                _moveDirection = Vector2.right;
            }
        }
        else
        {
            if (startPos.y > endPos.y)
            {
                _moveDirection = Vector2.down;
            }
            else
            {
                _moveDirection = Vector2.up;
            }
        }
    }
    #endregion
}
