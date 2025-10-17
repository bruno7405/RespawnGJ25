using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] BlackScreenUI blackScreenUI;

    private void Start()
    {
        AudioManager.Instance.PlayBackgroundMusic("StartMenuSong");
    }

    public void PlayGame()
	{
        StartCoroutine(FadeToBlackTransition());

    }

	public void QuitGame()
	{
		Application.Quit();
    }


    IEnumerator FadeToBlackTransition()
    {
        blackScreenUI.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(1);
    }
}
