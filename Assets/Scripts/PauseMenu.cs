using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pausePanel;
    GameObject player;
    // Update is called once per frame
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ResumeGame();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        pausePanel.SetActive(false);
        isPaused = false;
    }
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        isPaused = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void OpenSettings()
    {

    }

    public void QuitGame()
    {
        //Application.Quit();
    }
}
