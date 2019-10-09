using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the loading of scenes and the passing through of the player scores user PlayerPrefs
/// </summary>
public class GameOver : MonoBehaviour
{
	private Text finalScoreText;
	private Text p1ScoreText;
	private Text p2ScoreText;
	private int finalScore = 0;
	void Start()
	{
		GameObject ScoreUI = GameObject.Find("Score");
		finalScoreText = ScoreUI.GetComponentsInChildren<Text>()[0];
		p1ScoreText = ScoreUI.GetComponentsInChildren<Text>()[1];
		p2ScoreText = ScoreUI.GetComponentsInChildren<Text>()[2];

		int P1score, P2score;
		P1score = PlayerPrefs.GetInt("P1score");
		P2score = PlayerPrefs.GetInt("P2score");


		finalScore = P1score + P2score;
		finalScoreText.text = "Final Score: " + finalScore;
		p1ScoreText.text = "Player 1 Score: " + P1score;
		p2ScoreText.text = "Player 2 Score: " + P2score;

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void MenuReturn()
	{
		SceneManager.LoadScene(0);
	}
}
