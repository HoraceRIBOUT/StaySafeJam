using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public bool inGame = false;
    private bool isPaused = false;
    [SerializeField] private GameObject pauseCanvas;

    void Update()
    {
        if (Input.GetKeyDown("p") && inGame)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (isPaused)
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
        }
        else
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0.0001f;
            isPaused = true;
        }
    }
}
