using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleManager : MonoBehaviour
{
    public static PaddleManager instance;
    bool paddleFire = false;
    bool paddleMagnet = false;
    private void Awake()
    {
        instance = this;
    }
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
        else if (paddlenewPositionX > -8.35 && paddlenewPositionX < 8.35)
        {
            transform.position = new Vector3(paddlenewPositionX, transform.position.y, transform.position.z);
        }

    }

    /// <summary>
    /// set paddle to fire state
    /// </summary>
    public void SetPaddleFire()
    {
        paddleFire = true;
    }

    public void SetPaddleNotFire()
    {
        paddleFire = false;
    }

    public bool GetPaddleFire()
    {
        return paddleFire;
    }

    public void SetPaddleMagnet()
    {
        paddleMagnet = true;
    }
    public void SetPaddleNotMagnet()
    {
        paddleMagnet = false;
    }
    public bool GetPaddleMagnet()
    {
        return paddleMagnet;
    }
}