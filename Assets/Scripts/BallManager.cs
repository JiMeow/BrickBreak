using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;
    Rigidbody2D rb;
    GameObject paddle;
    bool alive = true;
    bool onFire = false;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        paddle = GameObject.Find("Paddle");
        //Set ball speed
        rb.velocity = new Vector2(Random.Range(-1f, 1f), 5);
    }

    void Update()
    {
        FireBallAfterDie();
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
        float startYPosiotion = -3.975f;
        //when ball hit bottom
        if (transform.position.y < -5)
        {
            alive = false;
            GameManager.instance.RemoveLives();

            //Set ball speed to 0
            rb.velocity = Vector2.zero;
            //set ball on paddle
            transform.position = new Vector3(paddle.transform.position.x, startYPosiotion, 0);
        }
    }


    /// <summary>
    /// If the ball is not alive, then the ball's position is set to the paddle's position
    /// </summary>
    void FollowPaddle()
    {
        if (!alive)
        {
            float startYPosiotion = -3.975f;
            transform.position = new Vector3(paddle.transform.position.x, startYPosiotion, 0);
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
            rb.velocity = new Vector2(Random.Range(-1f, 1f), 5);
        }
    }

    /// <summary>
    /// Start a coroutine called Fireball.
    /// </summary>
    public void BeFireball()
    {
        StartCoroutine(Fireball());
    }

    /// <summary>
    /// "Turn on trigger and wait 15 seconds, then turn off the trigger."
    /// on trigger mean ball on fire
    /// </summary>
    IEnumerator Fireball()
    {
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        onFire = true;
        circleCollider.isTrigger = true;
        yield return new WaitForSeconds(15f);
        onFire = false;
        circleCollider.isTrigger = false;
    }

    // If collition was enter add some error rotation to the ball
    // and if paddle collect fire item set ball to fire state
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Paddle")
        {
            if (GameObject.Find("Paddle").GetComponent<PaddleManager>().GetPaddleFire())
            {
                BeFireball();
                PaddleManager.instance.SetPaddleNotFire();
            }
        }
        rb.velocity = new Vector2(rb.velocity.x + Random.Range(-0.1f, 0.1f), rb.velocity.y);
    }

    // Fire ball trigger anythin except brick will bound
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Brick" && other.gameObject.tag != "Item")
        {
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.isTrigger = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag != "Brick" && other.gameObject.tag != "Item" && onFire)
        {
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
        }
    }

}
