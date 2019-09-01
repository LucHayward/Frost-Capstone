﻿using System;
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

    public void Setup()
    {
        playerMovement = instanceOfPlayer.GetComponent<PlayerMovement>();
        cameraController = camera.GetComponent<CameraController>();
        playerScript = instanceOfPlayer.GetComponent<Player>();
        playerAttack = instanceOfPlayer.GetComponent<PlayerAttack>();
        playerAttack.cam = camera;
        playerMovement.cameraTransform = camera.transform;
        if(instanceOfPlayer.transform.GetChild(3) != null)
        {
            cameraController.orbitTarget = instanceOfPlayer.transform.GetChild(3);
        }

		healthResetValue = playerScript.health;
    }

    public void RoundReset()
    {
        ResetHealth();
    }

    private void ResetHealth()
    {
        playerScript.health = healthResetValue;
    }
}
