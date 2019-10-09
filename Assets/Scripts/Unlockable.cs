using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : MonoBehaviour
{
    public GameObject gate;
    public bool isUnlockable;
    public float scoreNeeded;
    private float currentScore;
    private GameManager gameManager;
    private Vector2 scoreVector;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isUnlockable)
        {
            scoreVector = gameManager.GetScore();
            currentScore = scoreVector[0] + scoreVector[1];
            
            if (currentScore >= scoreNeeded)
            {
                Destroy(gameObject);
            }
        }
    }
}
