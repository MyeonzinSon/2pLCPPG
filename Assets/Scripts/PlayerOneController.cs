using UnityEngine;
using System.Collections;

public class PlayerOneController : MonoBehaviour
{
    
    public float moveForce = 200f;
    public float collideForce = 20f;
    public float gravity = 19.6f;
    private float xSpeed = 10f;
    private float ySpeed = 5f;
    public bool isOnPlatform = false;
    public bool isOnLadder = false;
    public bool isWithLadder = false;
    private bool isFacingRight = false;

    private bool inputJump = false;
    private bool inputAbility;
    int inputXDirection = 0;
    int inputYDirection = 0;
    int inputYCount = 0;

    Rigidbody2D rb2d;
    public LayerMask layerMaskPlatform;
    public LayerMask layerMaskLadder;
    Vector2 checkPlatform;
    Vector2 checkLadder;

    bool collisionExit;
    float otherLadderX;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        InputKeys();
        CheckCollision();
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
            if (inputJump)
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
                if (inputJump)
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
    public void OnCollisionExit(Collision2D other)
    {
        if (other.gameObject.layer == layerMaskPlatform)
        {
            isOnPlatform = false;
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == layerMaskLadder)
        {
            isWithLadder = true;
            otherLadderX = other.gameObject.transform.position.x;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == layerMaskLadder)
        { isWithLadder = false; }
    }

    void InputKeys()
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
        { inputYDirection += 1; inputYCount += 1; }
        if (Input.GetKeyUp("up"))
        { inputYDirection -= 1; inputYCount -= 1; }
        if (Input.GetKeyDown("down"))
        { inputYDirection -= 1; inputYCount += 1; }
        if (Input.GetKeyUp("down"))
        { inputYDirection += 1; inputYCount -= 1; }

        if (Input.GetKeyDown("z"))
        { inputJump = true; }
        if (Input.GetKeyUp("z"))
        { inputJump = false; }

        if (Input.GetKeyDown("x"))
        { inputAbility = true; }
        if (Input.GetKeyUp("x"))
        { inputAbility = false; }
    }
    void CheckCollision()
    {
        checkPlatform = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f);
        checkLadder = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        if (Physics2D.OverlapCircle(checkPlatform, 0.25f, layerMaskPlatform))
        {
            if (!isOnPlatform)
            {
                isOnPlatform = true;
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }
        }
        else if (!Physics2D.OverlapCircle(checkPlatform, 0.25f, layerMaskPlatform))
        { isOnPlatform = false; }
    }
}