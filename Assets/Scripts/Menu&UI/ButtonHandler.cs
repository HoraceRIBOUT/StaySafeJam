using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private AudioSource Button;

    // Start is called before the first frame update
    public void StartGame()
    {
        //Button.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartGame()
    {
        //Button.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartCredits()
    {
        //Button.Play();
        creditsCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        //Button.Play();
        Application.Quit();
    }

    public void Back()
    {
        creditsCanvas.SetActive(false);
    }
}
