using UnityEngine;
using System.Collections;

public class PlayerTwoController : MonoBehaviour
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
    bool isAbilityActive = false;
    int inputXDirection = 0;
    int inputYDirection = 0;
    int inputYCount = 0;

    public LayerMask layerMaskPlatform;
    public LayerMask layerMaskLadder;

    Rigidbody2D rb2d;
    float otherLadderX;

    Transform groundChecker;
    Transform ladderChecker;
    
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        groundChecker = transform.FindChild("GroundChecker");
        ladderChecker = transform.FindChild("LadderChecker");
    }
    void Update()
    {
        InputKeys();
        CollisionCheck();
        UpdateAbilityAvailableState();
    }
    void UpdateAbilityAvailableState()
    {

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
                    SetVelocity(rb2d.velocity.x, jumpSpeed * Sign(gravity));
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
        if (Input.GetKeyDown("d"))
        { inputXDirection += 1; }
        if (Input.GetKeyUp("d"))
        { inputXDirection -= 1; }
        if (Input.GetKeyDown("a"))
        { inputXDirection -= 1; }
        if (Input.GetKeyUp("a"))
        { inputXDirection += 1; }

        if (Input.GetKeyDown("w"))
        { inputYDirection += 1; inputYCount += 1; }
        if (Input.GetKeyUp("w"))
        { inputYDirection -= 1; inputYCount -= 1; }
        if (Input.GetKeyDown("s"))
        { inputYDirection -= 1; inputYCount += 1; }
        if (Input.GetKeyUp("s"))
        { inputYDirection += 1; inputYCount -= 1; }

        if (Input.GetKeyDown("z"))
        { inputJumping = true; }
        if (Input.GetKeyUp("z"))
        { inputJumping = false; }
        if (Input.GetKeyDown("x"))
        {
            CastAbility();
        }
    }
    void CollisionCheck()
    {
        Vector2 checkPlatform = groundChecker.position;
        Vector2 checkLadder = ladderChecker.position;
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

    void CastAbility()
    {

    }

    public void Die()
    {
        GameManager.RespawnTwo();
    }

    public void Initialize()
    {
        rb2d.velocity = new Vector2(0f, 0f);
        isAbilityActive = false;
        transform.rotation = Quaternion.Euler(0,0,0);
    }
}