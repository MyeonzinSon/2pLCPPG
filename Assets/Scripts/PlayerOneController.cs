using UnityEngine;
using System.Collections;

public class PlayerOneController : MonoBehaviour
{
    public GameObject dummyObject;
    GameObject existDummyObject;

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
    RaycastHit2D hit;
    
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
        if (CanReturnToBody())
            existDummyObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        else
        {   
            if (existDummyObject != null)    
                existDummyObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    bool CanReturnToBody()
    {
        if (existDummyObject == null) return false;
        if (hit.collider == null) return false;
        return hit.collider.gameObject == existDummyObject;
    }

    void FixedUpdate()
    {
        // 유체이탈 상태에서 본체 스캔용
        hit = Physics2D.Raycast(transform.position + new Vector3(0,Sign(gravity)*0.6f, 0), Vector2.up*Sign(gravity));
        
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

        if (!isAbilityActive && existDummyObject != null && other.gameObject == existDummyObject)
        {
            Destroy(existDummyObject);
        }
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
        if (!isOnPlatform) return;
        if (isOnLadder) return;
        
        if (!isAbilityActive)
        {
            isAbilityActive = true;
            existDummyObject = Instantiate(dummyObject, transform.position, Quaternion.identity) as GameObject;
            GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.4f);
        }
        else
        {
            if (!CanReturnToBody()) return;
            isAbilityActive = false;
            GetComponent<SpriteRenderer>().color += new Color(0,0,0,0.4f);
        }

        transform.Rotate(new Vector3(180,0,0));
        gravity *= -1;
    }

    public void Die()
    {
        GameManager.RespawnOne();
    }

    public void Initialize()
    {
        rb2d.velocity = new Vector2(0f, 0f);
        gravity = Mathf.Abs(gravity);
        isAbilityActive = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        transform.rotation = Quaternion.Euler(0,0,0);
        Destroy(existDummyObject);
    }
}