using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform winPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Image timeLeftBar;
    [SerializeField]
    private Transform cat;
    [SerializeField]
    private Transform tutorPanel;

    private void Start()
    {
        cat.DOShakeRotation(5, new Vector3(0, 0, 20), 1, 10, false).SetLoops(-1, LoopType.Yoyo);
        if(LevelManager.instance.currentLevelIndex == 0)
        {
            ShowStarBlockTutorPanel();
            Time.timeScale = 0;
        }
    }

    public void ShowStarBlockTutorPanel()
    {
        overlayPanel.gameObject.SetActive(true);
        tutorPanel.gameObject.SetActive(true);
        FadePanelInToScene(overlayPanel.GetComponent<CanvasGroup>(), tutorPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
    }

    public void HideStarBlockTutorPanel()
    {
        StartCoroutine(FadeSBTutorOut(overlayPanel.GetComponent<CanvasGroup>(), tutorPanel.GetComponent<RectTransform>()));
        homeButton.interactable = true;
        replayButton.interactable = true;
    }

    public void UpdatePlayerTimeLeftRainBowBar(float playerTimeLeft)
    {
        float total = LevelManager.instance.levelData.GetTheLevelAtThisIndex(LevelManager.instance.currentLevelIndex).timeLimit;
        float percent = playerTimeLeft / total;
        timeLeftBar.DOFillAmount(percent, 0.2f);
    }

    public void PopupWinPanelInGameSceneForPlayer()
    {
        overlayPanel.gameObject.SetActive(true);
        winPanel.gameObject.SetActive(true);
        FadePanelInToScene(overlayPanel.GetComponent<CanvasGroup>(), winPanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
    }

    public void PopupLosePanelInGameSceneForPlayer()
    {
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadePanelInToScene(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
    }

    private void FadePanelInToScene(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(1, .3f).SetEase(Ease.OutBack).SetUpdate(true);
    }

    private IEnumerator FadeSBTutorOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 700), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        tutorPanel.gameObject.SetActive(false);
        overlayPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
