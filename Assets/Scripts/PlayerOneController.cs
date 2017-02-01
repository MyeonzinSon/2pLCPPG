using UnityEngine;
using System.Collections;

public class PlayerOneController : MonoBehaviour
{
    
    [System.NonSerialized] public float moveForce = 200f;
    [System.NonSerialized] public float collideForce = 20f;
    [System.NonSerialized] public float gravity = 19.6f;
    private float xSpeed = 10f;
    private float ySpeed = 5f;
    public bool isOnPlatform = false;
    public bool isOnLadder = false;
    public bool isWithLadder = false;
    private bool isFacingRight = false;
    public bool isJumping = false;
    int inputXDirection = 0;
    int inputYDirection = 0;
    int inputYCount = 0;

    Rigidbody2D rb2d;
    public float otherLadderX;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown("right"))
        { inputXDirection += 1; }
        if (Input.GetKeyUp("right"))
        { inputXDirection -= 1; }
        if (Input.GetKeyDown("left"))
        { inputXDirection -= 1; }
        if (Input.GetKeyUp("left"))
        { inputXDirection += 1; }

        if (Input.GetKeyDown("up"))
        {
            inputYDirection += 1;
            inputYCount += 1;
        }
        if (Input.GetKeyUp("up"))
        {
            inputYDirection -= 1;
            inputYCount -= 1;
        }
        if (Input.GetKeyDown("down"))
        {
            inputYDirection -= 1;
            inputYCount += 1;
        }
        if (Input.GetKeyUp("down"))
        {
            inputYDirection += 1;
            inputYCount -= 1;
        }

        if (Input.GetKeyDown("z"))
        { isJumping = true; }
        if (Input.GetKeyUp("z"))
        { isJumping = false; }
        //replace
        Debug.Log(inputXDirection);
        Debug.Log(inputYDirection);
        if (Input.GetKeyDown("r"))
        { transform.position = new Vector3(0f, 0f, transform.position.z); }
    }
    void FixedUpdate()
    {
        if (isOnLadder)
        {
            rb2d.velocity = new Vector2(0f, inputYDirection * ySpeed);
            if (isJumping)
            {
                isOnLadder = false;
                rb2d.velocity = new Vector2(inputXDirection * xSpeed * 0.5f, 0f);
                rb2d.AddForce(new Vector2(0, gravity * 0.5f), ForceMode2D.Impulse);
            }
            if (isOnPlatform || !isWithLadder)
            { isOnLadder = false; }
        }
        else
        {
            if (isWithLadder)
            {
                if (inputYCount > 0)
                {
                    isOnLadder = true;
                    rb2d.velocity = new Vector2(0f, 0f);
                    gameObject.transform.position = new Vector3(otherLadderX, gameObject.transform.position.y, gameObject.transform.position.z);
                }
            }
            if (isOnPlatform)
            {
                if (isJumping)
                {
                    isOnPlatform = false;
                    rb2d.AddForce(new Vector2(0, gravity * 0.5f), ForceMode2D.Impulse);
                }

                if (Mathf.Abs(rb2d.velocity.x) <= xSpeed)
                { rb2d.AddForce(new Vector2(inputXDirection * moveForce, 0)); }

                rb2d.AddForce(new Vector2(-1 * rb2d.velocity.x * collideForce, 0));
                if (Mathf.Abs(rb2d.velocity.x) <= 0.01f)
                { rb2d.velocity = new Vector2(0, rb2d.velocity.y); }
            }
            else
            {
                rb2d.AddForce(new Vector2(0, -gravity));
            }
        }
        if (Input.GetKeyDown("q"))
        { rb2d.AddForce(new Vector2(1000f, 0)); }
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            isOnPlatform = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
            isOnPlatform = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            isWithLadder = true;
            otherLadderX = other.gameObject.transform.position.x;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
            isWithLadder = false;
    }

}