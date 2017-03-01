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

    bool isReturningFromAbility = false;
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
    void Start()
    { Initialize(); }
    void Update()
    {
        InputKeys();
        CollisionCheck();
        UpdateAbilityAvailableState();
    }
    void FixedUpdate()
    {
        // 유체이탈 상태에서 본체 스캔용
        hit = Physics2D.Raycast(transform.position + new Vector3(0, Sign(gravity) * 0.6f, 0), Vector2.up * Sign(gravity));

        if (isOnLadder)
        {
            SetVelocity(0f, inputYDirection * ladderSpeed);
            gameObject.transform.position = new Vector3(otherLadderX, gameObject.transform.position.y, gameObject.transform.position.z);
            if (inputJumping)
            {
                isOnLadder = false;
                SetVelocity(inputXDirection * moveSpeed*0.7f, Sign(gravity) * jumpSpeed * 0.7f);
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
                }
            }
            if (isOnPlatform)
            {
                if (inputJumping)
                {
                    isOnPlatform = false;
                    SetVelocity(rb2d.velocity.x, jumpSpeed * Sign(gravity));
                }
                if (Mathf.Abs(rb2d.velocity.y) <= (jumpSpeed - Mathf.Abs(gravity))/2)
                {
                    SetVelocity(rb2d.velocity.x, 0f);
                }
            }
            else
            {
                AddVelocity(0, -gravity);
                if (rb2d.velocity.x == 0f && !isReturningFromAbility)
                { AddVelocity(inputXDirection * moveForce, 0f); }
            }
            if (!isReturningFromAbility)
            {
                if (Mathf.Abs(rb2d.velocity.x) <= moveSpeed)
                { AddVelocity(inputXDirection * moveForce, 0); }
                else if (inputXDirection * Sign(rb2d.velocity.x) < 0)
                { AddVelocity(inputXDirection * (moveForce + collideForce), 0); }

            }

            if (Mathf.Abs(rb2d.velocity.x) >= collideForce)
            { AddVelocity(-1 * Sign(rb2d.velocity.x) * collideForce, 0); }
            else
            { SetVelocity(0, rb2d.velocity.y); }
        }

        // sprite direction
        if ((!isAbilityActive && rb2d.velocity.x > collideForce/2) || (isAbilityActive && rb2d.velocity.x < -collideForce/2))
        {
            Quaternion origin = transform.rotation;
            transform.rotation = Quaternion.Euler(origin.eulerAngles.x, 180, origin.eulerAngles.z);
        }
        else if ((!isAbilityActive && rb2d.velocity.x < -collideForce/2) || (isAbilityActive && rb2d.velocity.x > collideForce))
        {
            Quaternion origin = transform.rotation;
            transform.rotation = Quaternion.Euler(origin.eulerAngles.x, 0, origin.eulerAngles.z);
        }

        // Animator
        if ((rb2d.velocity.x != 0) && isOnPlatform)
            GetComponent<Animator>().SetBool("isWalking", true);
        else
            GetComponent<Animator>().SetBool("isWalking", false);

        if (isOnPlatform)
            GetComponent<Animator>().SetBool("isGrounded", true);
        else
            GetComponent<Animator>().SetBool("isGrounded", false);

        if (rb2d.velocity.y != 0)
            GetComponent<Animator>().SetBool("isMovingVertical", true);
        else
            GetComponent<Animator>().SetBool("isMovingVertical", false);

        if (isOnLadder)
            GetComponent<Animator>().SetBool("isOnLadder", true);
        else
            GetComponent<Animator>().SetBool("isOnLadder", false);

        if (isAbilityActive)
            GetComponent<Animator>().SetBool("isAbilityActive", true);
        else
            GetComponent<Animator>().SetBool("isAbilityActive", false);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        { otherLadderX = other.gameObject.transform.position.x; }

        if (isReturningFromAbility && existDummyObject != null && other.gameObject == existDummyObject)
        {
            isAbilityActive = false;
            isReturningFromAbility = false;
            // GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.4f);
            Destroy(existDummyObject);
        }
    }

    void UpdateAbilityAvailableState()
    {
        if (existDummyObject != null)
        {
            if (isReturningFromAbility)
            { existDummyObject.GetComponent<SpriteRenderer>().color = Color.white; }
            else if (CanReturnToBody() && isOnPlatform && !isOnLadder)
            { existDummyObject.GetComponent<SpriteRenderer>().color = Color.green; }
            else
            { existDummyObject.GetComponent<SpriteRenderer>().color = Color.gray; }
        }
    }
    bool CanReturnToBody()
    {
        if (existDummyObject == null) return false;
        if (hit.collider == null) return false;
        return hit.collider.gameObject == existDummyObject;
    }
    void CastAbility()
    {
        if (!isOnPlatform) return;
        if (isOnLadder) return;

        if (!isAbilityActive)
        {
            isAbilityActive = true;
            existDummyObject = Instantiate(dummyObject, transform.position, transform.rotation) as GameObject;
            // GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.4f);
            Vector3 origin = groundChecker.localPosition;
            groundChecker.localPosition = new Vector3(origin.x, -1 * origin.y, origin.z);
            // Quaternion origin = transform.rotation;
            // transform.rotation = Quaternion.Euler(180, origin.eulerAngles.y, origin.eulerAngles.z);
        }
        else
        {
            if (!CanReturnToBody()) return;
            isReturningFromAbility = true;
            SetVelocity(0f, 0f);
            Vector3 origin = groundChecker.localPosition;
            groundChecker.localPosition = new Vector3(origin.x, -1 * origin.y, origin.z);
            gameObject.transform.position = new Vector3(existDummyObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            transform.rotation = existDummyObject.transform.rotation;
        }
        gravity *= -1;
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

        if (Input.GetKeyDown("n"))
        { inputJumping = true; }
        if (Input.GetKeyUp("n"))
        { inputJumping = false; }
        if (Input.GetKeyDown("m"))
        {
            CastAbility();
        }
    }
    void CollisionCheck()
    {
        Vector2 checkPlatform = groundChecker.position;
        Vector2 checkLadder = ladderChecker.position;
        isOnPlatform = Physics2D.OverlapCircle(checkPlatform, 0.25f, layerMaskPlatform);
        isWithLadder = Physics2D.OverlapCircle(checkLadder, 0.25f, layerMaskLadder);
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

    public void Die()
    {
        gameObject.SetActive(false);
        GameManager.RestartMap();
    }
    public void Initialize()
    {
        SetVelocity(0f, 0f);
        gravity = Mathf.Abs(gravity);
        isAbilityActive = false;
        isReturningFromAbility = false;
        isOnLadder = false;
        inputXDirection = 0;
        if (Input.GetKey("right"))
        { inputXDirection += 1; }
        if (Input.GetKey("left"))
        { inputXDirection -= 1; }
        inputYDirection = 0;
        inputYCount = 0;
        if(Input.GetKey("up"))
        { inputYDirection += 1; inputYCount += 1; }
        if (Input.GetKey("down"))
        { inputYDirection -= 1; inputYCount += 1; }
        GetComponent<SpriteRenderer>().color = Color.white;
        Vector3 origin = groundChecker.localPosition;
        groundChecker.localPosition = new Vector3(origin.x, -1 * Mathf.Abs(origin.y), origin.z);
        transform.rotation = Quaternion.Euler(0, 180, 0);
        Destroy(existDummyObject);
    }
}