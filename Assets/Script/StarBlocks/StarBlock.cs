using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBlock : MonoBehaviour
{
    [SerializeField]
    private int _verticalBlock;
    [SerializeField]
    private int _horizontalBlock;
    [SerializeField]
    private ColorType _colorType;

    public int GetStarBlockHeight()
    {
        return _verticalBlock;
    }

    public int GetStarBlockWidth()
    {
        return _horizontalBlock;
    }
    public ColorType GetColorType()
    {
        return _colorType;
    }
}
