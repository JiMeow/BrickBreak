using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField]
    private GameObject brick;

    GameObject[] allInstantBricks;

    void Start()
    {
        allInstantBricks = new GameObject[70];
        Spawn();
        SetSpecialBrick();
    }

    /// <summary>
    /// "Spawns bricks in a grid pattern, with the top left brick being at (-9, 3.5) and the bottom
    /// right brick being at (9, -3.5)."
    /// 
    /// The first for loop is for the rows, and the second for loop is for the columns
    /// </summary>
    void Spawn()
    {
        // size is width of brick
        float size = 0.64f * 1.5f;

        // start position of brick (top left)
        float positionX = -9 * size;
        float positionY = 3.5f;

        // spawn brick
        for (int i = 0; i < 7; i++)
        {
            positionX = -4.5f * size;
            for (int j = 0; j < 10; j++)
            {
                int index = i * 10 + j;

                GameObject Instantbrick = Instantiate(brick, new Vector2(positionX, positionY), Quaternion.identity);
                Instantbrick.GetComponent<Brick>().Set(index, i, true, false, false);
                allInstantBricks[index] = Instantbrick;

                positionX += size;
            }
            positionY -= size / 2;
        }
    }

    /// <summary>
    /// It randomly chooses 5 bricks from the first 5 rows of the brick wall, and sets them to be
    /// hard bricks. Then it randomly chooses 3 bricks from another brick, and sets them
    /// to be undestroyable bricks
    /// </summary>
    void SetSpecialBrick()
    {
        //Set hard brick
        GameObject[] temp = allInstantBricks;

        //random on last 5 row mean hard brick on index less than 50 
        //then shuffle random that index to first 5 index in new array represent choose them be hard brick
        for (int i = 0; i < 5; i++)
        {
            int indexToSwap = Random.Range(0, 50);
            GameObject swap = temp[i];
            temp[i] = temp[indexToSwap];
            temp[indexToSwap] = swap;
        }
        //set hard brick
        for (int i = 0; i < 5; i++)
        {
            temp[i].GetComponent<Brick>().Set(-1, 0, false, true, false);
        }

        //Set undestroyable brick

        //random on next 3 index by swap with index from 5 to 70
        //then shuffle random that index to next 3 index in new array represent choose them be undestroyable brick
        for (int i = 5; i < 8; i++)
        {
            int indexToSwap = Random.Range(5, 70);
            GameObject swap = temp[i];
            temp[i] = temp[indexToSwap];
            temp[indexToSwap] = swap;
        }
        //set undestroyable brick
        for (int i = 5; i < 8; i++)
        {
            temp[i].GetComponent<Brick>().Set(-1, 0, false, false, true);
        }

    }
}
