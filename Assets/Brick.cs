using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    int score;
    int durable;

    [SerializeField]
    Sprite[] sprite;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Set(int row, bool isNormal, bool isHardBrick, bool isUndestroyable)
    {
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
}
