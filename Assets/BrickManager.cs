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

    void Update()
    {

    }

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
                GameObject Instantbrick = Instantiate(brick, new Vector2(positionX, positionY), Quaternion.identity);
                Instantbrick.GetComponent<Brick>().Set(i, true, false, false);
                allInstantBricks[i * 10 + j] = Instantbrick;

                positionX += size;
            }
            positionY -= size / 2;
        }
    }

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
            temp[i].GetComponent<Brick>().Set(0, false, true, false);
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
            temp[i].GetComponent<Brick>().Set(0, false, false, true);
        }

    }
}
