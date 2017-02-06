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
    bool inputAbility = false;
    int inputXDirection = 0;
    int inputYDirection = 0;
    int inputYCount = 0;

    public LayerMask layerMaskPlatform;
    public LayerMask layerMaskLadder;

    Rigidbody2D rb2d;
    float otherLadderX;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        InputKeys();
        CollisionCheck();
    }
    void FixedUpdate()
    {
        if (isOnLadder)
        {
            SetVelocity(0f, inputYDirection * ladderSpeed);
            if (inputJumping)
            {
                isOnLadder = false;
                SetVelocity(inputXDirection * moveSpeed * 0.5f, jumpSpeed);
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
                    SetVelocity(0f, 0f);
                    gameObject.transform.position = new Vector2(otherLadderX, gameObject.transform.position.y);
                }
            }
            if (isOnPlatform)
            {
                if (inputJumping)
                {
                    isOnPlatform = false;
                    SetVelocity(rb2d.velocity.x, jumpSpeed);
                }

                if (Mathf.Abs(rb2d.velocity.x) <= moveSpeed)
                { AddVelocity(inputXDirection * moveForce, 0); }
                else if (inputXDirection * Sign(rb2d.velocity.x) < 0)
                { AddVelocity(inputXDirection * (moveForce + collideForce), 0); }

                AddVelocity(-1 * Sign(rb2d.velocity.x) * collideForce, 0);
                if (Mathf.Abs(rb2d.velocity.x) <= collideForce/2)
                { SetVelocity(0, rb2d.velocity.y); }
            }
            else
            { AddVelocity(0, -gravity); }
        }
        //test force
        if (Input.GetKeyDown("q"))
        { AddVelocity(30f, 0); }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        { otherLadderX = other.gameObject.transform.position.x; }
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
        { inputJumping = true; }
        if (Input.GetKeyUp("z"))
        { inputJumping = false; }
        if (Input.GetKeyDown("x"))
        { inputAbility = true; }
        if (Input.GetKeyUp("x"))
        { inputAbility = false; }
    }
    void CollisionCheck()
    {
        Vector2 checkPlatform = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.45f);
        Vector2 checkLadder = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        isOnPlatform = Physics2D.OverlapCircle(checkPlatform, 0.25f, layerMaskPlatform);
        isWithLadder = Physics2D.OverlapCircle(checkLadder, 0.1f, layerMaskLadder);
    }

    void SetVelocity(float x, float y)
    { rb2d.velocity = new Vector2(x, y); }
    void AddVelocity(float x, float y)
    { rb2d.velocity = rb2d.velocity + new Vector2(x, y); }
    int Sign(float f)
    {
        if (f == 0)
        { return 0; }
        else if (f > 0)
        { return 1; }
        else { return -1; }
    }
}