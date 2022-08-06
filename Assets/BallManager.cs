using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    bool alive = true;
    Rigidbody2D rb;
    GameObject paddle;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        paddle = GameObject.Find("Paddle");
        //Set ball speed
        rb.velocity = new Vector2(Random.Range(-1f, 1f), -5);
    }

    void Update()
    {
        FollowPaddle();
        IsDie();
        CheckSpeed();
    }

    /// <summary>
    /// If the player is alive and the player's velocity magnitude is less than 6, then set the player's velocity magnitude
    /// to 6
    /// </summary>
    void CheckSpeed()
    {
        if (alive && rb.velocity.magnitude < 6f)
        {
            rb.velocity = rb.velocity.normalized * 6f;
        }
    }

    /// <summary>
    /// If the ball's y position is less than -5, the ball is dead
    /// </summary>
    void IsDie()
    {
        //when ball hit bottom
        if (transform.position.y < -5)
        {
            alive = false;


            //Set ball speed to 0
            rb.velocity = Vector2.zero;
            //set ball on paddle
            transform.position = new Vector3(paddle.transform.position.x, -3.975f, 0);
        }
    }


    /// <summary>
    /// If the ball is not alive, then the ball's position is set to the paddle's position
    /// </summary>
    void FollowPaddle()
    {
        if (!alive)
        {
            transform.position = new Vector3(paddle.transform.position.x, -3.975f, 0);
        }
    }

    /// <summary>
    /// If the player presses the spacebar or left mouse button, the ball is set to alive and is given a
    /// random velocity
    /// </summary>
    void FireBallAfterDie()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            alive = true;
            rb.velocity = new Vector2(Random.Range(-1f, 1f), -5);
        }
    }

    // If collition was enter add some error rotation to the ball
    private void OnCollisionEnter2D(Collision2D other)
    {
        rb.velocity = new Vector2(rb.velocity.x + Random.Range(-0.1f, 0.1f), rb.velocity.y);
    }
}
