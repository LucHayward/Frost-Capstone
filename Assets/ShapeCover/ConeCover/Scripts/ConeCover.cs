//This script is a product of Twisted Webbe and its use is determined by the agreement with the Unity Asset Store.

using UnityEngine;
using System.Collections.Generic;


public class ConeCover : MonoBehaviour
{
    //mainObjectHeight is the height of the Cone, with the point facing up.
    //objectsToPlaceRadius is the size of the object if it was a sphere. A 1x1x1 Square has a radius of 0.5.
    //The coneWidthAdjustment changes the width of the cone. A 1 is standard, while the closer a number gets to 0 it gets narrower. Higher numbers spread it out.
    public float mainObjectHeight, objectsToPlaceRadius, coneWidthAdjustment;

    //This is a list of the objects you want placed. The number should correspond to the number of objects in the objectsToPlaceSpawnChance list. Object 0 will have a spawn chance of float 0.
    //Example: The order of the array is the same as the order for objectsToPlaceRadiusPrefabs, so objectsToPlaceRadiusPrefabs[6] will spawn with the chance for objectsToPlaceSpawnChance[6].
    public GameObject[] objectsToPlaceRadiusPrefabs;

    //This is the percent chance that you want your object to spawn. Anything 100 or higher will always spawn. 
    //The spawn chances are successive, so if the first object in the list fails to spawn it will try for the next object.
    //If all spawn checks fail, nothing will be spawned.
    public float[] objectsToPlaceSpawnChance;
    
    //Each Row of objects are placed in Rings.
    private float ringRadius, ringCircumference;

    //This determines where to spawn objects, from which row to which row.
    public float fillFromPercentage, fillToPercentage;

    //A quick list to see all objects spawned, if you need it for some reason.
    public List<GameObject> clonedObjectsList = new List<GameObject>();

    //Some private variables for looping.
    private int clonedObjectCount, numberOfOuterLoops, numberOfObjectsForThisLoop;

    //The gameobject being spawned for this loop.
    private GameObject thisClonedObject;

    public void Start()
    {
        CoverThatCone();
    }

    public void CoverThatCone()
    {
        //How many rows are necessary to cover the surface of the object, taking into account the object sizes
        int numberOfRows = Mathf.CeilToInt(mainObjectHeight / (objectsToPlaceRadius*2));

        //Determines the first and last rows to run the spawning loops through.
        int startRow = Mathf.FloorToInt(numberOfRows * fillFromPercentage * 0.01f);
        int endRow = Mathf.CeilToInt(numberOfRows * fillToPercentage * 0.01f);

        //MATH! It equally balances the space between rows. Keep in mind that this and the later code to space between objects does not necessarily create an exact distance diagonally between objects.
        float spaceBetweenRows = objectsToPlaceRadius*2 / numberOfRows;

        //Starting loop for objects
        for (int rowNumber = startRow; rowNumber <= endRow; rowNumber++)
        {
            //This math finds the height of each row as determined by loop and row spacing.
            float yPosition = mainObjectHeight - (rowNumber * objectsToPlaceRadius*2);

            //Creates the row and positions it, and sets the parent.
            GameObject thisRow = new GameObject();
            thisRow.transform.position = new Vector3(transform.position.x, transform.position.y + yPosition, transform.position.z);
            thisRow.transform.parent = transform;
            thisRow.name = "Row" + rowNumber.ToString();

            //This determines the radius of the circle that is the "ring" for this height.
            float radiusFromLoopHeight = (rowNumber * objectsToPlaceRadius* coneWidthAdjustment);

            //The top row should always have a single object to top it off. This may not space as nicely with the neighboring rows. Change the fillFromPercentage to remove this row.
            if (rowNumber == 0)
            {
                numberOfObjectsForThisLoop = 1;
            }
            else
            {
                //Getting the number of objects for this loop.
                numberOfObjectsForThisLoop = Mathf.FloorToInt((2 * Mathf.PI * radiusFromLoopHeight) / (objectsToPlaceRadius * 2));

                if(numberOfObjectsForThisLoop == 1 && rowNumber ==1)
                {
                    numberOfObjectsForThisLoop = 2;
                }
            }

            //Similar to the row spacing code above, this determines the space between objects in this row.
            float spaceBetweenObjects = (2 * Mathf.PI * radiusFromLoopHeight) / numberOfObjectsForThisLoop;

            //Starting loop for objects around the row/ring.
            for (int ringObjectNumber = 1; ringObjectNumber <= numberOfObjectsForThisLoop; ringObjectNumber++)
            {
                //Setting these variables beforehand so that they can still be used at the end of the for loop.
                float xPosition = 0;
                float zPosition = 0;
                float thisAngle = 0;

                //The first object (at the poles of the cone) is set this way as to avoid NaN errors.
                if (rowNumber == 0)
                {

                    thisAngle = 0;

                    xPosition = 0;
                    yPosition = mainObjectHeight;
                    zPosition = 0;
                }else
                {
                    //More math to determine the exact position of this object around the ring/row.
                    thisAngle = ((ringObjectNumber) * spaceBetweenObjects) / radiusFromLoopHeight;
                    xPosition = radiusFromLoopHeight * Mathf.Sin(thisAngle);
                    zPosition = radiusFromLoopHeight * Mathf.Cos(thisAngle);
                }

                //This preps the while loop so that it can go through all objects in the spawn list.
                bool spawnLoopFinished = false;
                int spawnLoop = 0;
                //Double safety net to make sure the loop ends at the right place.
        while (!spawnLoopFinished && spawnLoop <= objectsToPlaceRadiusPrefabs.Length-1){
                    //This is the objectsToPlaceSpawnChance variable. Any float between 0 and 100 will work, above that it just acts as 100% all the time.
                    if (Random.Range(0, 100) <= objectsToPlaceSpawnChance[spawnLoop] && objectsToPlaceSpawnChance[spawnLoop] != 0)
                     {
                    //Finally! Let's spawn that object!
                    GameObject thisClonedObject = Instantiate(objectsToPlaceRadiusPrefabs[spawnLoop], new Vector3(transform.position.x + xPosition, (yPosition + transform.position.y), transform.position.z + zPosition), Quaternion.identity, thisRow.transform);

                    //Same as immediately above!
                    thisClonedObject.name = transform.name + " Object" + (clonedObjectsList.Count + 1).ToString();
                    clonedObjectsList.Add(thisClonedObject);
              
                        spawnLoopFinished = true;
                    }
                    else
                    {
                        spawnLoop++;
                    }
        }
            }
        }

    }

}