using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brick : MonoBehaviour
{
    int score;
    int durable;
    int index;
    bool containItem;
    bool isBomb;
    public int hardBrickBreakDurable = 3;
    public int brickBlueScore = 10;
    public int brickGreenScore = 20;
    public int brickYellowScore = 50;

    [SerializeField]
    Sprite[] sprite;

    [SerializeField]
    GameObject[] itemGameObject;

    /// <summary>
    /// This function is used to set the brick's index, sprite, durability, and score
    /// </summary>
    /// <param name="index">The index of the brick</param>
    /// <param name="row">the row the brick is in</param>
    /// <param name="isNormal">If true, the brick will be a normal brick.</param>
    /// <param name="isHardBrick">If true, the brick will be a hard brick.</param>
    /// <param name="isUndestroyable">If true, the brick will be undestroyable.</param>
    /// <param name="isContainitem">If true, the brick will be a special brick contain item.</param>
    /// <param name="isBombBrick">If true, the brick will be a bomb.</param>
    public void Set(int index, int row, bool isNormal = false, bool isHardBrick = false, bool isUndestroyable = false, bool isContainitem = false, bool isBombBrick = false)
    {
        if (index != -1)
            this.index = index;

        if (isNormal)
        {
            durable = 1;
            if (row < 2)
            {
                GetComponent<SpriteRenderer>().sprite = sprite[2];
                score = brickYellowScore;
            }
            else if (row < 5)
            {
                GetComponent<SpriteRenderer>().sprite = sprite[1];
                score = brickGreenScore;
            }
            else if (row < 7)
            {
                GetComponent<SpriteRenderer>().sprite = sprite[0];
                score = brickBlueScore;
            }
        }
        if (isHardBrick)
        {
            durable = hardBrickBreakDurable;
            GetComponent<SpriteRenderer>().sprite = sprite[3];
            score = 100;
        }
        if (isUndestroyable)
        {
            GetComponent<SpriteRenderer>().sprite = sprite[4];
            durable = -1;
            score = 0;
        }
        if (isContainitem)
        {
            containItem = true;
        }
        if (isBombBrick)
        {
            GetComponent<SpriteRenderer>().sprite = sprite[5];
            score = 100;
            isBomb = true;
        }
    }

    //when ball hit brick decrease durability if durability is 0 destroy brick
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (durable > 0)
            {
                durable--;
                if (durable == 0)
                {
                    DestroyBrick();
                }
            }
        }
    }

    // destroy every brick in that fire ball was triggered
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            DestroyBrick();
        }
    }

    /// <summary>
    /// This function is used to destroy the game object if the brick contains an item spawn item,
    /// if brick is a bomb then activate bomb
    /// </summary>
    public void DestroyBrick()
    {
        GameManager.instance.AddScore(score);
        if (containItem)
        {
            SpawnItem();
        }
        if (isBomb)
        {
            BrickManager.instance.DestroyByBombBrick(index);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// It creates a random number between 0 and 2, then spawns a game object from the itemGameObject
    /// array at the position of the brick that was destroy.
    /// </summary>
    void SpawnItem()
    {
        int itemNumber = Random.Range(0, 2);
        Vector3 itemPosition = transform.position;
        itemPosition.z = itemPosition.z - 1f;
        GameObject item = Instantiate(itemGameObject[itemNumber], itemPosition, Quaternion.identity);
    }

    public int GetIndex()
    {
        return index;
    }

    public int GetDurable()
    {
        return durable;
    }

    public bool GetIsBomb()
    {
        return isBomb;
    }

}
