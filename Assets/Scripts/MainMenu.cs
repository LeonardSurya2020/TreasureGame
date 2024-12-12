using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject GameOverMenu;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void PlayGame()
    {
        StartCoroutine(SceneInterval(1));
        
    }

    public void MainMenuScreen()
    {

        StartCoroutine(SceneInterval(0));
        //SceneManager.LoadSceneAsync(0);
    }

    public void About()
    {

        StartCoroutine(SceneInterval(2));
        //SceneManager.LoadSceneAsync(2);
    }

    public void Pause()
    {
        audioManager.PlaySFX(audioManager.confirm);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        audioManager.PlaySFX(audioManager.confirm);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        GameOverMenu.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void GameOverScreen()
    {
        GameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void MainMenuMenu()
    {

        SceneManager.LoadSceneAsync(0);
    }

    public IEnumerator SceneInterval(int sceneIndex)
    {
        
        audioManager.PlaySFX(audioManager.confirm);
        yield return new WaitForSeconds(0.3f);
        
        SceneManager.LoadSceneAsync(sceneIndex);

    }
}
