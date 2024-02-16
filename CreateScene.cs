using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

public class CreateScene : MonoBehaviour
{
    //public variables to test script functions, and lists to store the tree and stone game objects
    public int sizeOfForest;
    public int stonesRequired;

    public GameObject[] trees;
    public GameObject[] stones;

    //to generate random numbers
    Random random = new Random();

    // Start is called before the first frame update
    void Start()
    {
        InitializeVariables();
        CreateGround();
        CreateRandomForest();
        CreatePyramid();
    }

    //function that initializes the public variables
    void InitializeVariables()
    {
        sizeOfForest = 15;
        stonesRequired = 55;
        trees = new GameObject[sizeOfForest];
        stones = new GameObject[stonesRequired];
    }

    //function to create the plane that acts as the ground
    void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);

        Renderer planeRenderer = ground.GetComponent<Renderer>();
        Color planeColor = new Color(0.94f, 0.40f, 0.53f, 1);
        planeRenderer.material.SetColor("_Color", planeColor);

        ground.name = "Ground";
        ground.transform.position -= Vector3.back * -4.0f;
        
    }

    //function to create a forest of cylinders with random colors, positions and local scales
    void CreateRandomForest()
    {
        //empty parent object
        GameObject forest = new GameObject("Forest");

        int randomNum;
        double changeLength, changeWidth, changeHeight, changePosX, changePosZ;
        Renderer treeRenderer;
        Color treeColor;
        Vector3 scaleChange, positionChange;

        //create a tree equal to the number set in the public variable sizeOfForest
        for (int i = 0; i < sizeOfForest; i++)
        {
            trees[i] = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trees[i].transform.position += Vector3.up * 1.0f;
            trees[i].name = "Tree" + (i + 1);
            trees[i].transform.SetParent(forest.transform);

            treeRenderer = trees[i].GetComponent<Renderer>();

            randomNum = random.Next(1, 4);

            //randomly selecting a color
            if(randomNum == 1)
            {
                treeColor = new Color(0.06f, 0.88f, 0.15f, 1.0f);
            }

            else if(randomNum == 2)
            {
                treeColor = new Color(0.02f, 0.35f, 0.06f, 1.0f);
            }

            else
            {
                treeColor = new Color(0.35f, 0.24f, 0.02f, 1.0f);
            }

            treeRenderer.material.SetColor("_Color", treeColor);

            //randomly changing the tree's scale
            changeLength = random.NextDouble();
            changeWidth = random.NextDouble();
            changeHeight = random.NextDouble();

            scaleChange = new Vector3((float)changeLength, (float)changeHeight, (float)changeWidth);

            trees[i].transform.localScale -= scaleChange;

            //randomly changing the position of the tree
            trees[i].transform.position = Vector3.up - new Vector3(0, (float)changeHeight, 4.0f);

            changePosX = random.Next(1, 5) + changeLength;
            changePosZ = random.Next(1, 5) + changeWidth;

            positionChange = new Vector3((float)changePosX, 0, (float)changePosZ);

            trees[i].transform.position -= positionChange;
        }
    }

    //function to create a pyramid of cubes
    void CreatePyramid()
    {
        //empty parent game object
        GameObject pyramid = new GameObject("Pyramid");
        Renderer stoneRenderer;
        Color stoneColor;

        int counter = stonesRequired;
        int levels = 0;
        int stonesCounter = 0;

        //determining how many levels the pyramid will have
        while(counter > 0)
        {
            counter = counter - ((levels + 1) * (levels + 1));
            levels = levels + 1;
        }

        int levelCounter = 1;
        float offset = 0.55f;

        //loop to create the levels of cubes for the pyramid
        for(int i = levels; i > 0; i--)
        {
            //creating parent gameobjects to separate the cubes based on their level
            GameObject level = new GameObject("Level" + i);
            level.transform.SetParent(pyramid.transform);

            level.transform.position += Vector3.up * ((i - 1) * 1.1f);

            Vector3 moveStone = new Vector3(offset * -(levelCounter - 1), 0, offset * - (levelCounter - 1));

            //nested for loop to create the cubes
            for(int j = 0; j < levelCounter; j++)
            {   
                //check to see if we are out of cubes
                if (stonesCounter >= stonesRequired)
                {
                    break;
                }

                for(int k = 0; k < levelCounter; k++)
                {
                    //check to see if we are out of cubes
                    if (stonesCounter >= stonesRequired)
                    {
                        break;
                    }

                    stones[stonesCounter] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    stones[stonesCounter].name = "Stone" + (stonesCounter + 1);
                    stones[stonesCounter].transform.SetParent(level.transform);
                    stones[stonesCounter].transform.position += Vector3.up * ((i - 1) * 1.1f);

                    stoneRenderer = stones[stonesCounter].GetComponent<Renderer>();

                    //set the cube's color based on its level
                    if(levelCounter == 1)
                    {
                        stoneColor = new Color(1.0f, 0, 0, 1.0f);
                    }

                    else if (levelCounter == 2)
                    {
                        stoneColor = new Color(1.0f, 0, 0.651f, 1.0f);
                    }

                    else if (levelCounter == 3)
                    {
                        stoneColor = new Color(1.0f, 0.451f, 0.576f, 1.0f);
                    }

                    else if (levelCounter == 4)
                    {
                        stoneColor = new Color(1.0f, 0.773f, 0.380f, 1.0f);
                    }

                    else
                    {
                        stoneColor = new Color(1.0f, 0.918f, 0, 1.0f);
                    }

                    stoneRenderer.material.SetColor("_Color", stoneColor);

                    stones[stonesCounter].transform.position += moveStone;
                    moveStone = moveStone + new Vector3(1.1f, 0, 0);

                    stonesCounter = stonesCounter + 1;
                }

                moveStone = moveStone + new Vector3((offset * -levelCounter) * 2, 0, 1.1f);
            }

            levelCounter = levelCounter + 1;
        }

        //raise the pyramid up to the ground
        pyramid.transform.position += new Vector3(0, 0.5f, 0);
    }
}