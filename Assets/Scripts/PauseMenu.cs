using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pausePanel;
    GameObject[] players;
    Player[] playerScripts;
    // Update is called once per frame
    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        playerScripts = new Player[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            GameObject go = players[i];
            playerScripts[i] = go.GetComponent<Player>();
        }
        ResumeGame();
        
    }

    public void ResumeGame()
    {
        foreach(Player playerScript in playerScripts)
        {
            playerScript.enabled = true;

        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausePanel.SetActive(false);
        isPaused = false;
    }
    public void PauseGame()
    {
        foreach (Player playerScript in playerScripts)
        {
            playerScript.enabled = false;

        }
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
        SceneManager.LoadScene(0);
    }
}
