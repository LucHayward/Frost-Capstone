using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the Pause menu interaction as well as pausing the necessary game elements
/// </summary>
public class PauseMenu : MonoBehaviour
{
	public bool isPaused;
	public GameObject pausePanel;

	GameObject[] players;
	Player[] playerScripts;

	/// <summary>
	/// Setup knowledge of the players in order to pause them when needed.
	/// </summary>
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

	/// <summary>
	/// Enables all players and resets the cursor to a locked state
	/// </summary>
	public void ResumeGame()
	{
		foreach (Player playerScript in playerScripts)
		{
			playerScript.enabled = true;

		}
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		pausePanel.SetActive(false);
		isPaused = false;
        Time.timeScale = 1;
	}

	/// <summary>
	/// Disables all players and frees the cursor for use in the menu
	/// </summary>
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

	public void QuitGame()
	{
		SceneManager.LoadScene(0);
	}
}
