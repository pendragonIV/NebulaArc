using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorHand : MonoBehaviour
{
    private void Start()
    {
        if(LevelManager.instance.currentLevelIndex == 0)
        {
            Vector3 destination = transform.localPosition + new Vector3(2f, 0, 0);
            this.transform.DOLocalMove(destination, 1).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Restart).SetUpdate(true);
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && Time.timeScale == 1)
        {
            this.transform.DOKill();
            Destroy(this.gameObject);
        }
    }
}
