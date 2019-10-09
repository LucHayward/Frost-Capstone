using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the interaction with the main menu
/// </summary>
public class MainMenu : MonoBehaviour
{
	public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		// TODO: Async so that it is less jarring
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
