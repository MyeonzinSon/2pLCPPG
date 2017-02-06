using UnityEngine;
using System.Collections;

public class PlayerOneController : MonoBehaviour
{
    
    public float moveForce;
    public float collideForce;
    public float gravity;
    public float moveSpeed;
    public float ladderSpeed;
    public float jumpSpeed;

    private bool isOnPlatform = false;
    private bool isWithLadder = false;
    private bool isOnLadder = false;
    private bool isFacingRight = false;

    bool inputJumping = false;
    int inputXDirection = 0;
    int inputYDirection = 0;
    int inputYCount = 0;

    Vector2 checkPlatform;
    Vector2 checkLadder;
    public LayerMask layerMaskPlatform;
    public LayerMask layerMaskLadder;

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
        { inputJumping = true; }
        if (Input.GetKeyUp("z"))
        { inputJumping = false; }

        checkPlatform = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.45f);
        checkLadder = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        isOnPlatform = Physics2D.OverlapCircle(checkPlatform, 0.25f, layerMaskPlatform);
        isWithLadder = Physics2D.OverlapCircle(checkLadder, 0.1f, layerMaskLadder);

        //replace
        if (Input.GetKeyDown("r"))
        { transform.position = new Vector3(0f, 0f, transform.position.z); }
    }
    void FixedUpdate()
    {
        if (isOnLadder)
        {
            rb2d.velocity = new Vector2(0f, inputYDirection * ladderSpeed);
            if (inputJumping)
            {
                isOnLadder = false;
                rb2d.velocity = new Vector2(inputXDirection * moveSpeed * 0.5f, jumpSpeed);
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
                if (inputJumping)
                {
                    isOnPlatform = false;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
                }

                if (Mathf.Abs(rb2d.velocity.x) <= moveSpeed)
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
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            otherLadderX = other.gameObject.transform.position.x;
        }
    }
}