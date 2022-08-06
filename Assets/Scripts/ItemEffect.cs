using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    void Update()
    {
        Fall();
        Die();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if item collide with object
        if (other.gameObject.tag == "Paddle")
        {
            string name = gameObject.name.Split('(')[0];
            // item is fireball
            if (name == "FireballItem")
            {
                FireballEffect();
            }
            // item is magnet
            if (name == "MagnetItem")
            {
                MagnetEffect();
            }
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// When the FireballEffect function is called, it will call the SetPaddleFire function in the
    /// PaddleManager script.
    /// </summary>
    void FireballEffect()
    {
        PaddleManager.instance.SetPaddleFire();
    }

    /// <summary>
    /// When the magnet effect is triggered, the paddle will be set to magnet mode.
    /// </summary>
    void MagnetEffect()
    {
        PaddleManager.instance.SetPaddleMagnet();
    }

    /// <summary>
    /// If the item's y position is less than -5, destroy the item
    /// </summary>
    void Die()
    {
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// The function Fall() moves the object down 4 units per second
    /// </summary>
    void Fall()
    {
        transform.position += Vector3.down * Time.deltaTime * 4f;
    }
}
