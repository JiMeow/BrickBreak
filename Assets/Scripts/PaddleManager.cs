using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleManager : MonoBehaviour
{
    void Update()
    {
        //Game not over
        if (Time.timeScale != 0)
        {
            SetPaddlePos();
        }
    }

    /// <summary>
    /// The function takes the mouse positionX and converts it to a value between -8.35 and 8.35
    /// </summary>
    void SetPaddlePos()
    {
        //track mouse position
        Vector3 mousePosition = Input.mousePosition;
        float paddlenewPositionX = mousePosition.x / Screen.width * 16 - 8;
        if (paddlenewPositionX >= 8.35)
        {
            transform.position = new Vector3(8.35f, transform.position.y, transform.position.z);
        }
        else if (paddlenewPositionX <= -8.35)
        {
            transform.position = new Vector3(-8.35f, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(paddlenewPositionX, transform.position.y, transform.position.z);
        }
    }
}
