using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public static BrickManager instance;
    [SerializeField]
    private GameObject brick;

    GameObject[] allInstantBricks;
    private void Awake()
    {
        instance = this;
    }
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
                Instantbrick.GetComponent<Brick>().Set(index, i, isNormal: true);
                allInstantBricks[index] = Instantbrick;

                positionX += size;
            }
            positionY -= size / 2;
        }
    }

    /// <summary>
    /// It randomly chooses 5 bricks from the first 5 rows of the brick wall, and sets them to be
    /// hard bricks. Then it randomly chooses 3 bricks from another brick, and sets them
    /// to be undestroyable bricks, then chooses 9 brick which isn't unbreakable and sets them to be
    /// special brick contain item
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
            temp[i].GetComponent<Brick>().Set(-1, 0, isHardBrick: true);
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
            temp[i].GetComponent<Brick>().Set(-1, 0, isUndestroyable: true);
        }

        //Set special brick contain item

        //random 7 brick but not use unbreakable brick
        //then shuffle random index to 7 first index in new array represent choose them be special brick contain item
        for (int i = 0; i < 7; i++)
        {
            int indexToSwap = Random.Range(i, 70);
            //if temp[indexToSwap] is unbreakable brick, then continue
            if (temp[indexToSwap].GetComponent<Brick>().GetDurable() == -1)
            {
                i--;
                continue;
            }
            {
                GameObject swap = temp[i];
                temp[i] = temp[indexToSwap];
                temp[indexToSwap] = swap;
            }
        }
        //set special brick contain item
        for (int i = 0; i < 7; i++)
        {
            temp[i].GetComponent<Brick>().Set(-1, 0, isContainitem: true);
        }

        //Spawn 4 bomb brick by random and set index that can't be use in dictionary (can't be unbreakable box)
        //then set brick to Bombbrick
        Dictionary<int, bool> indexBombCanUse = new Dictionary<int, bool>();
        for (int i = 0; i < 4; i++)
        {
            int indexChoose = Random.Range(0, 70);
            if (indexBombCanUse.ContainsKey(indexChoose) || temp[indexChoose].GetComponent<Brick>().GetDurable() == -1)
            {
                i--;
                continue;
            }
            else
            {
                SetBombBrickCantUseIndex(indexChoose, indexBombCanUse);
                temp[indexChoose].GetComponent<Brick>().Set(-1, 0, isBombBrick: true);
            }
        }

        //Sort all brick by index
        for (int i = 0; i < 70; i++)
        {
            for (int j = i + 1; j < 70; j++)
            {
                if (temp[i].GetComponent<Brick>().GetIndex() > temp[j].GetComponent<Brick>().GetIndex())
                {
                    GameObject swap = temp[i];
                    temp[i] = temp[j];
                    temp[j] = swap;
                }
            }
        }
    }

    /// <summary>
    /// It takes an index and a dictionary of indices and sets the value of the dictionary at the index
    /// to true
    /// </summary>
    /// <param name="index">the index of the brick that is being destroyed</param>
    /// <param name="indexBombCanUse">a dictionary that stores the index on right(\) and on left(/) of the brick that can't be used
    /// to place a bomb</param>
    void SetBombBrickCantUseIndex(int index, Dictionary<int, bool> indexBombCanUse)
    {
        int tempindex = index;
        while (tempindex >= 0)
        {
            if (!indexBombCanUse.ContainsKey(tempindex))
            {
                indexBombCanUse.Add(tempindex, true);
            }
            if (tempindex % 10 == 0)
                break;
            tempindex -= 11;
        }
        tempindex = index;
        while (tempindex >= 0)
        {
            if (!indexBombCanUse.ContainsKey(tempindex))
            {
                indexBombCanUse.Add(tempindex, true);
            }
            if (tempindex % 10 == 9)
                break;
            tempindex -= 9;
        }
        tempindex = index;
        while (tempindex < 70)
        {
            if (!indexBombCanUse.ContainsKey(tempindex))
            {
                indexBombCanUse.Add(tempindex, true);
            }
            if (tempindex % 10 == 9)
                break;
            tempindex += 11;
        }
        tempindex = index;
        while (tempindex < 70)
        {
            if (!indexBombCanUse.ContainsKey(tempindex))
            {
                indexBombCanUse.Add(tempindex, true);
            }
            if (tempindex % 10 == 0)
                break;
            tempindex += 9;
        }
    }

    /// <summary>
    /// It destroys all the bricks in a diagonal line,(left or right) depending on 
    /// the direction
    /// </summary>
    /// <param name="index">the index of the brick that was hit by the ball</param>
    public void DestroyByBombBrick(int index)
    {
        int tempindex = index;
        bool direction = Random.Range(0, 2) == 0 ? true : false;

        if (direction)
        {
            while (true)
            {
                // hanedle index out of range
                if (tempindex % 10 == 0)
                    break;
                tempindex -= 11;
                if (tempindex < 0) break;

                //destroy brick if it not already destroyed
                if (allInstantBricks[tempindex] != null)
                {
                    allInstantBricks[tempindex].GetComponent<Brick>().DestroyBrick();
                }
            }
            tempindex = index;
            while (true)
            {
                // hanedle index out of range
                if (tempindex % 10 == 9)
                    break;
                tempindex += 11;
                if (tempindex >= 70) break;

                //destroy brick if it not already destroyed
                if (allInstantBricks[tempindex] != null)
                {
                    allInstantBricks[tempindex].GetComponent<Brick>().DestroyBrick();
                }
            }
        }
        else if (!direction)
        {
            while (true)
            {
                // hanedle index out of range
                if (tempindex % 10 == 9)
                    break;
                tempindex -= 9;
                if (tempindex < 0) break;

                //destroy brick if it not already destroyed
                if (allInstantBricks[tempindex] != null)
                {
                    allInstantBricks[tempindex].GetComponent<Brick>().DestroyBrick();
                }
            }
            tempindex = index;
            while (true)
            {
                // hanedle index out of range
                if (tempindex % 10 == 0)
                    break;
                tempindex += 9;
                if (tempindex >= 70) break;

                //destroy brick if it not already destroyed
                if (allInstantBricks[tempindex] != null)
                {
                    allInstantBricks[tempindex].GetComponent<Brick>().DestroyBrick();
                }
            }
        }
    }

    /// <summary>
    /// return true if all the bricks are destroyed else false
    /// </summary>
    public bool AllBlockWasDestroy()
    {
        for (int i = 0; i < 70; i++)
        {
            if (allInstantBricks[i] != null)
            {
                return false;
            }
        }
        return true;
    }
}
