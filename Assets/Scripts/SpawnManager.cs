using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public GameObject player;
	public GameObject[] spawnables;
	public Material[] materials;
	public int minX, maxX, minZ, maxZ;
	public float randomMaterialChance = 0.5f; //Set abnormally high to ensure markers can see effects 

	private void Start()
	{
		for (int i = 0; i < 12; i++)
		{
			SpawnNew();
		}
	}

	public void SpawnNew()
	{
		// Choose a random gameobject and (possibly) replace it's texture with a random texture provided it is not a brazier
		GameObject spawn = Instantiate(spawnables[Random.Range(0, spawnables.Length)]);

		if (!spawn.name.StartsWith("Brazier") && Random.value > randomMaterialChance)
		{
			spawn.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
		}

		// Spawn in some random position wihtin the bounds of the world, store a rotation about the y axis in Vec3.y
		Vector3 spawnPos = new Vector3(Random.Range(minX, maxX), Random.Range(0, 360), Random.Range(minZ, maxZ));
		spawn.transform.Translate(spawnPos.x, 0, spawnPos.z);
		spawn.transform.Rotate(Vector3.up, spawnPos.y);
	}


}
