using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private const string MENU = "MainMenu";
    private const string GAME = "GameScene";
    private const string LEVEL_CHOOSE = "LevelScene";

    [SerializeField]
    private Transform sceneTransition;

    private void Start()
    {
        ActiveSceneTransitionAnimation();
    }

    public void ActiveSceneTransitionAnimation()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransition");
    }

    public void ToMainScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToGivenScene(MENU));
    }

    public void ToGamePlay()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToGivenScene(GAME));
    }

    public void ToLevelChossingStateScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeToGivenScene(LEVEL_CHOOSE));
    }

    public void ToNextStarBlockChallenge()
    {
        StopAllCoroutines();
        if (LevelManager.instance.currentLevelIndex < LevelManager.instance.levelData.GetAllLevelSaved().Count - 1)
        {
            LevelManager.instance.currentLevelIndex++;
            StartCoroutine(ChangeToGivenScene(GAME));
        }
        else
        {
            StartCoroutine(ChangeToGivenScene(LEVEL_CHOOSE));
        }
    }


    private IEnumerator ChangeToGivenScene(string sceneName)
    {
        DOTween.KillAll();
        //Optional: Add animation here
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadSceneAsync(sceneName);

    }
}
