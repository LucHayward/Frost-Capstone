using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pausePanel;
    GameObject player;
    Player playerScript;
    // Update is called once per frame
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        ResumeGame();
        
    }

    public void ResumeGame()
    {
        playerScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausePanel.SetActive(false);
        isPaused = false;
    }
    public void PauseGame()
    {
        playerScript.enabled = false;
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
