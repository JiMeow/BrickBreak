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
        PaddleManager.instance.SetPaddleMagnet();
    }

    void Die()
    {
        if (transform.position.y < -5)
        {
            Destroy(gameObject);
        }
    }

    void Fall()
    {
        transform.position += Vector3.down * Time.deltaTime * 4f;
    }
}
