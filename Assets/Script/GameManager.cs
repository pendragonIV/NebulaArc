using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ColorType
{
    Biege,
    Green,
    Pink,
    Purple,
    Red,
    Yellow,
    DarkGreen,
    DeepPurple
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;

    #region Game status
    private Level currentLevelData;
    private bool isGameWin = false;
    private bool isGameLose = false;
    private float timeLeft;
    #endregion

    private void Start()
    {
        currentLevelData = LevelManager.instance.levelData.GetTheLevelAtThisIndex(LevelManager.instance.currentLevelIndex);
        GameObject map = Instantiate(currentLevelData.map);
        MovementManager.instance.SetMatchColorContainer(map.transform.GetChild(0));
        timeLeft = currentLevelData.timeLimit;
        gameScene.UpdatePlayerTimeLeftRainBowBar(timeLeft);
    }

    public void PlayerWinTheLevelNowPopup()
    {
        if (isGameWin || isGameLose)
        {
            return;
        }
        LevelManager.instance.levelData.SetDataForGivenLevelIndex(LevelManager.instance.currentLevelIndex, true, true);
        if (LevelManager.instance.levelData.GetAllLevelSaved().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetTheLevelAtThisIndex(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetDataForGivenLevelIndex(LevelManager.instance.currentLevelIndex + 1, true, false);
            }
        }
        isGameWin = true;

        StartCoroutine(WaitToShowWinUIPanelForPlayer());
        LevelManager.instance.levelData.SaveDataAsJSONFomat();
    }

    private void Update()
    {
        DecreaseTimeLeftAndShowUI();
        CheckTimeLeftToCheckLose();
    }

    private IEnumerator WaitToShowWinUIPanelForPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        gameScene.PopupWinPanelInGameSceneForPlayer();
    }

    public void DecreaseTimeLeftAndShowUI()
    {
        if (isGameWin || isGameLose)
        {
            return;
        }
        timeLeft -= Time.deltaTime;
        gameScene.UpdatePlayerTimeLeftRainBowBar(timeLeft);
    }

    public void CheckTimeLeftToCheckLose()
    {
        if (timeLeft <= 0)
        {
            if (isGameWin || isGameLose)
            {
                return;
            }
            PlayerLoseTheLevelNowPopupUI();
        }
    }

    public void PlayerLoseTheLevelNowPopupUI()
    {
        isGameLose = true;
        gameScene.PopupLosePanelInGameSceneForPlayer();
    }

    public bool IsThisGameWin()
    {
        return isGameWin;
    }

    public bool IsThisGameLose()
    {
        return isGameLose;
    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }
}
