using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    public static bool GameIsPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                PauseOff();
            } 
            else
            {
                SetPause();
            }
        }
    }

    private void Awake()
    {
        pausePanel.SetActive(false);
    }

    public void SetPause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    public void PauseOff()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }
}
