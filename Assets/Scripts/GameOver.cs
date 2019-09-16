using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update

    private Text finalScoreText;
    private int finalScore = 0;
    void Start()
    {
        GameObject finalScoreUI = GameObject.Find("FinalScore");
        finalScoreText = finalScoreUI.GetComponentInChildren<Text>();
        finalScore = PlayerPrefs.GetInt("score");
        finalScoreText.text = "Final Score:" + finalScore;

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
