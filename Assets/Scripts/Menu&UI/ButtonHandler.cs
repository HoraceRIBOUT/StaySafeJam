using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private AudioSource buttonSound;

    public void StartGame()
    {
        if (buttonSound) { buttonSound.Play(); }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Back()
    {
        creditsCanvas.SetActive(false);
    }
}
