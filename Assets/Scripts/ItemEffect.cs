using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    void Update()
    {
        Die();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Paddle")
        {
            string name = gameObject.name.Split('(')[0];
            if (name == "FireballItem")
            {
                FireballEffect();
            }
            if (name == "MagnetItem")
            {
                MagnetEffect();
            }
            Destroy(gameObject);
        }
    }

    void FireballEffect()
    {
        PaddleManager.instance.SetPaddleFire();
    }

    void MagnetEffect()
    {

    }

    void Die()
    {
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }
}