using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private GameObject hero;

    public void StartGame()
    {
        hero.SetActive(true);
        if (buttonSound) { buttonSound.Play(); }
        menuCanvas.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        if (buttonSound) { buttonSound.Play(); }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartCredits()
    {
        if (buttonSound) { buttonSound.Play(); }
        creditsCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        if (buttonSound) { buttonSound.Play(); }
        Application.Quit();
    }

    public void GoToMenu()
    {
        if (buttonSound) { buttonSound.Play(); }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Back()
    {
        creditsCanvas.SetActive(false);
    }
}
