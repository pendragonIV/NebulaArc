using DG.Tweening;
using UnityEngine;

public class MatchBlock : MonoBehaviour
{
    [SerializeField]
    private bool _isMatched = false;
    [SerializeField]
    private ColorType _colorType;
    private SpriteRenderer _colorSpriteRenderer;
    private SpriteRenderer _ringSpriteRenderer;

    [SerializeField]
    private Color _defaultColor;
    [SerializeField]
    private Color _matchedColor;

    private void Start()
    {
        _colorSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _ringSpriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        CheckStarBlock();
    }

    public bool IsThisMatchedStarBlock()
    {
        return _isMatched;
    }

    public void SetMatchedStarBlock(bool isMatched)
    {
        _isMatched = isMatched;
        if(_isMatched)
        {
            _colorSpriteRenderer.DOColor(_matchedColor, 0.5f);
            _ringSpriteRenderer.DOColor(Color.white, 0.5f);
        }
        else
        {
            _colorSpriteRenderer.DOColor(_defaultColor, 0.5f);
            _ringSpriteRenderer.DOColor(Color.gray, 0.5f);
        }
    }

    public void CheckStarBlock()
    {
        Vector3Int[] adjacentDirections = new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        Vector3Int startDir = GridCellManager.instance.GetCellPositionOfGivenPosition(transform.position);

        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            Vector3Int dir = startDir + adjacentDirections[i];
            Vector3 posToCheck = GridCellManager.instance.GetWordPositionOfGivenCellPosition(dir);

            Collider2D checkObj = Physics2D.OverlapPoint(posToCheck, LayerMask.GetMask("Parent"));
            if (checkObj != null)
            {
                if (checkObj.gameObject.GetComponent<StarBlock>().GetColorType() == _colorType)
                {
                    SetMatchedStarBlock(true);
                    return;
                }
            }
        }
        SetMatchedStarBlock(false);
    }
}
