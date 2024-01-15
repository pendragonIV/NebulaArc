using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Transform tutorPanel;
    [SerializeField]
    private Transform guideLine;
    [SerializeField]
    private Transform sceneComponents;


    private void Start()
    {
        tutorPanel.gameObject.SetActive(false);
        gameLogo.GetComponent<Image>().fillAmount = 0;

        gameLogo.GetComponent<CanvasGroup>().alpha = 0f;
        gameLogo.GetComponent<CanvasGroup>().DOFade(1, 2f).SetUpdate(true);
        gameLogo.DOShakeRotation(5, new Vector3(0,0,50), 0, 10, false).SetLoops(-1, LoopType.Yoyo).SetUpdate(true).SetDelay(2f);
    }

    private void Update()
    {
        if(gameLogo.GetComponent<Image>().fillAmount < 1)
        {
            gameLogo.GetComponent<Image>().fillAmount += Time.deltaTime/1.2f;
        }
    }

    public void ShowStarBlockTutorPanel()
    {
        tutorPanel.gameObject.SetActive(true);
        guideLine.gameObject.SetActive(true);
        FadeSBTutorIn(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>());
        sceneComponents.gameObject.SetActive(false);
    }

    public void HideStarBlockTutorPanel()
    {
        StartCoroutine(FadeSBTutorOut(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>()));
        sceneComponents.gameObject.SetActive(true);
    }   

    private void FadeSBTutorIn(CanvasGroup canvasGroup ,RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 700, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    private IEnumerator FadeSBTutorOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 700), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        guideLine.gameObject.SetActive(true);
        tutorPanel.gameObject.SetActive(false);

    }

}
