using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update

    private Text finalScoreText;
    private Text p1ScoreText;
    private Text p2ScoreText;
    private int finalScore = 0;
    void Start()
    {
        //TODO update end scene correctly
        GameObject finalScoreUI = GameObject.Find("FinalScore");
        finalScoreText = finalScoreUI.GetComponentsInChildren<Text>()[0];
        p1ScoreText = finalScoreUI.GetComponentsInChildren<Text>()[1];
        p2ScoreText = finalScoreUI.GetComponentsInChildren<Text>()[2];


        int P1score, P2score;
        P1score = PlayerPrefs.GetInt("P1score");
        P2score = PlayerPrefs.GetInt("P2score");


        finalScore =  P1score + P2score;
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
