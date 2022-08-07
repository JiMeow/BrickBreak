using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;
    [SerializeField]
    Sprite[] sprite;
    Rigidbody2D rb;
    GameObject paddle;
    bool alive = true;
    public float timeToOnFireByItem = 15f;
    bool onFire = false;
    public float timeToOnMagnetByItem = 10f;
    bool onMagnet = false;
    Coroutine onFireCoroutine;
    Coroutine onMagnetCoroutine;
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
        // want to fire ball from paddle when ball stick with paddle
        FireballThatStickwithPaddle();
        // if dead, ball will stick with paddle
        FollowPaddle();
        // if the ball dead, reset the ball
        IsDie();
        CheckSpeed();
    }

    /// <summary>
    /// If the player is alive and the player's velocity magnitude is not equal to 6, then set the player's velocity magnitude
    /// to 6
    /// </summary>
    void CheckSpeed()
    {
        if (alive && (rb.velocity.magnitude < 6f || rb.velocity.magnitude > 6f))
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
    /// random velocity and a random angle
    /// </summary>
    void FireballThatStickwithPaddle()
    {
        if (!alive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            alive = true;
            rb.velocity = new Vector2(Random.Range(-1f, 1f), 5);
        }
    }

    // If ball collide with paddle taht collect fire item set ball to fire state or if paddle collect magnet item set ball to magnet state
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Paddle")
        {
            PaddleManager paddle = GameObject.Find("Paddle").GetComponent<PaddleManager>();
            if (paddle.GetPaddleFire())
            {
                BeFireball();
                //reset paddle fire to false
                PaddleManager.instance.SetPaddleNotFire();
            }
            if (paddle.GetPaddleMagnet())
            {
                BeMagnet();
                //reset paddle magnet to false
                PaddleManager.instance.SetPaddleNotMagnet();
            }

            // Add change some velocity when paddle hit ball depend on where ball hit paddle
            rb.velocity = rb.velocity + new Vector2((transform.position.x - paddle.gameObject.transform.position.x) * 4, 0);

            // if on magnet state and paddle hit ball, then magnet on
            if (onMagnet)
                MagnetOn();

        }
        //Add error value every time ball collide something make ball not stuck for long
        rb.velocity = new Vector2(rb.velocity.x + Random.Range(-0.1f, 0.1f), rb.velocity.y);
    }

    // Fire ball set trigger on when go out from paddle or border
    private void OnCollisionExit2D(Collision2D other)
    {
        if ((other.gameObject.tag == "Paddle" || other.gameObject.tag == "Border") && onFire)
        {
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
        }
    }

    // Fire ball trigger anything except brick and item will set trigger of for bound from it
    // and if paddle collect fire item set ball to fire state or if paddle collect magnet item set ball to magnet state
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Paddle" || other.gameObject.tag == "Border")
        {
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.isTrigger = false;
        }
        // if onfire ball hit paddle that has magnet, then magnet on
        // if onfire ball hit paddle that has fire, then fire on again
        if (other.gameObject.tag == "Paddle")
        {
            PaddleManager paddle = GameObject.Find("Paddle").GetComponent<PaddleManager>();
            if (paddle.GetPaddleFire())
            {
                BeFireball();
                PaddleManager.instance.SetPaddleNotFire();
            }
            if (paddle.GetPaddleMagnet())
            {
                BeMagnet();
                PaddleManager.instance.SetPaddleNotMagnet();
            }

            // Add change some velovity when paddle hit ball depend on where ball hit paddle
            rb.velocity = rb.velocity + new Vector2((transform.position.x - paddle.gameObject.transform.position.x) * 4, 0);

            if (onMagnet)
                MagnetOn();
        }
    }

    // Fire ball set trigger on when go out from paddle or border
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Paddle" || other.gameObject.tag == "Border")
        {
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;
        }
    }


    /// <summary>
    /// Stop before coroutine if it not null then start a coroutine called Fireball.
    /// </summary>
    public void BeFireball()
    {
        if (onFireCoroutine != null)
            StopCoroutine(onFireCoroutine);
        onFireCoroutine = StartCoroutine(Fireball());
    }

    /// <summary>
    /// "Turn on trigger change ball sprite and wait 15 seconds, then turn off the trigger."
    /// on trigger mean ball on fire
    /// </summary>
    IEnumerator Fireball()
    {
        //set sprite to fireball
        GetComponent<SpriteRenderer>().sprite = sprite[1];
        //set trigger on
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        //set ball fire state to true
        onFire = true;
        //wait 15 seconds
        yield return new WaitForSeconds(timeToOnFireByItem);
        //set sprite to normal
        GetComponent<SpriteRenderer>().sprite = sprite[0];
        //set trigger off
        circleCollider.isTrigger = false;
        //set ball fire state to false
        onFire = false;
    }


    /// <summary>
    /// Stop before coroutine if it not null then start a coroutine called Magnetball.
    /// </summary>
    public void BeMagnet()
    {
        if (onMagnetCoroutine != null)
            StopCoroutine(onMagnetCoroutine);
        onMagnetCoroutine = StartCoroutine(Magnetball());
    }

    /// <summary>
    /// "Set onMagnet variable to true and wait 15 seconds, then set the onMagnet variable to false."
    /// </summary>

    IEnumerator Magnetball()
    {
        //set ball magnet state to true
        onMagnet = true;
        //wait 15 seconds
        yield return new WaitForSeconds(timeToOnMagnetByItem);
        //set ball magnet state to false
        onMagnet = false;
    }

    /// <summary>
    /// The MagnetOn function sets the ball's velocity to zero, and sets the ball's position to the
    /// paddle's position (like when ball died but not decrease live).
    /// </summary>
    void MagnetOn()
    {
        float startYPosiotion = -3.975f;
        alive = false;
        //Set ball speed to 0
        rb.velocity = Vector2.zero;
        //set ball on paddle
        transform.position = new Vector3(paddle.transform.position.x, startYPosiotion, 0);
    }
}
