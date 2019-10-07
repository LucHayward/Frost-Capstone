using System;
using UnityEngine;

[Serializable]
public class PlayerManager
{
	[HideInInspector] public GameObject instanceOfPlayer;
    private PlayerMovement playerMovement;
    private CameraController cameraController;
    private Player playerScript;
    private PlayerAttack playerAttack;
	private int healthResetValue;

	public Transform spawnPoint;

	public Camera camera;
    [HideInInspector] public int playerNumber;

    public void Setup(int playerNum)
    {
		playerNumber = playerNum;
        playerMovement = instanceOfPlayer.GetComponent<PlayerMovement>();
        cameraController = camera.GetComponent<CameraController>();
        playerScript = instanceOfPlayer.GetComponent<Player>();
        playerAttack = instanceOfPlayer.GetComponent<PlayerAttack>();
        playerScript.SetPlayerNum(playerNum);
        cameraController.setPlayerNum(playerNum);
		playerAttack.setPlayerNum(playerNum);
		playerMovement.setPlayerNum(playerNum);
        playerAttack.cam = camera;
        playerMovement.cameraTransform = camera.transform;
        if(instanceOfPlayer.transform.GetChild(3) != null)
        {
            cameraController.orbitTarget = instanceOfPlayer.transform.GetChild(3);
        }
        instanceOfPlayer.name = "P" + (playerNum+1).ToString() + "_" + instanceOfPlayer.name;
		healthResetValue = playerScript.currrentHealth;
    }

    public void RoundReset()
    {
        ResetHealth();
    }

    private void ResetHealth()
    {
        playerScript.currrentHealth = healthResetValue;
    }

    public void StopMovment()
    {
        playerScript.enabled = false;
    }

    public void ResumeMovement()
    {
        playerScript.enabled = true;
    }
}
