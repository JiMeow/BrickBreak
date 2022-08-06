using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brick : MonoBehaviour
{
    int score;
    int durable;
    int index;

    [SerializeField]
    Sprite[] sprite;

    /// <summary>
    /// This function is used to set the brick's index, sprite, durability, and score
    /// </summary>
    /// <param name="index">The index of the brick</param>
    /// <param name="row">the row the brick is in</param>
    /// <param name="isNormal">If true, the brick will be a normal brick.</param>
    /// <param name="isHardBrick">If true, the brick will be a hard brick.</param>
    /// <param name="isUndestroyable">If true, the brick will be undestroyable.</param>
    public void Set(int index, int row, bool isNormal, bool isHardBrick, bool isUndestroyable)
    {
        if (index != -1)
            this.index = index;

        if (isNormal)
        {
            durable = 1;
            if (row < 2)
            {
                GetComponent<SpriteRenderer>().sprite = sprite[2];
                score = 50;
            }
            else if (row < 5)
            {
                GetComponent<SpriteRenderer>().sprite = sprite[1];
                score = 20;
            }
            else if (row < 7)
            {
                GetComponent<SpriteRenderer>().sprite = sprite[0];
                score = 10;
            }
        }
        if (isHardBrick)
        {
            durable = 3;
            GetComponent<SpriteRenderer>().sprite = sprite[3];
            score = 100;
        }
        if (isUndestroyable)
        {
            GetComponent<SpriteRenderer>().sprite = sprite[4];
            durable = -1;
            score = 0;
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
                    GameManager.instance.AddScore(score);
                    Destroy(gameObject);
                }
            }
        }
    }

    public int GetIndex()
    {
        return index;
    }

    public int GetDurable()
    {
        return durable;
    }

}
