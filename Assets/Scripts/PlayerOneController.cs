using UnityEngine;
using System.Collections;
using System.Linq;

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
    public float ropeSpeed;

    private bool isOnPlatform = false;
    private bool isWithLadder = false;
    private bool isOnLadder = false;
    private bool isWithRope = false;
    private bool isOnRope = false;
    // 0이면 로프의 아래 끝, 1이면 로프의 위 끝
    private float ropePosition = 0;
    private Rope rope = null;
    bool isReturningFromAbility = false;
    bool inputJumping = false;
    public bool isAbilityActive = false;
    int inputXDirection = 0;
    int inputYDirection = 0;
    int inputYCount = 0;

    public LayerMask layerMaskPlatform;
    public LayerMask layerMaskLadder;
    public LayerMask layerMaskRope;

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
        Initialize();
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

        UpdateVelocityInFixedUpdate();

        // sprite direction
        if (rb2d.velocity.x > collideForce/2) 
        {
            Quaternion origin = transform.rotation;
            transform.rotation = Quaternion.Euler(origin.eulerAngles.x, 180, origin.eulerAngles.z);
        }
        else if (rb2d.velocity.x < -collideForce/2)
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

        if (isOnLadder || isOnRope)
            GetComponent<Animator>().SetBool("isOnLadder", true);
        else
            GetComponent<Animator>().SetBool("isOnLadder", false);

        if (isAbilityActive)
            GetComponent<Animator>().SetBool("isAbilityActive", true);
        else
            GetComponent<Animator>().SetBool("isAbilityActive", false);
    }

    void UpdateVelocityInFixedUpdate() 
    {
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

            return;
        }

        if (isOnRope)
        {
            ropePosition = ropePosition + inputYDirection * ropeSpeed;
            ropePosition = Mathf.Clamp(ropePosition, 0, 1);
            Vector2 playerXY = rope.GetPosFromRatio(ropePosition);
            gameObject.transform.position = new Vector3(playerXY.x, playerXY.y, transform.position.z);

            if (inputJumping)
            {
                isOnRope = false;
                SetVelocity(inputXDirection * moveSpeed * 0.7f, Sign(gravity) * jumpSpeed * 0.7f);
            } 

            return;
        }

        if (isWithLadder)
        {
            if (inputYCount > 0 && !isAbilityActive)
            {
                isOnLadder = true;
                SetVelocity(0f, 0f);
            }
        }

        if (isWithRope)
        {
            if (inputYCount > 0 && !isAbilityActive)
            {
                isOnRope = true;
                ropePosition = rope.GetRatioFromPos(transform.position);
                Debug.Log("Calculated ropePosition is " + ropePosition);
                rope.StartMove(transform.position);
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
            else
            {
                SetVelocity(rb2d.velocity.x, 0);
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        { otherLadderX = other.gameObject.transform.position.x; }

        if (isReturningFromAbility && existDummyObject != null && other.gameObject == existDummyObject)
        {
            isAbilityActive = false;
            isReturningFromAbility = false;
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
            SetVelocity(0f, 0f);
            isAbilityActive = true;
            existDummyObject = Instantiate(dummyObject, transform.position, transform.rotation) as GameObject;
            Vector3 origin = groundChecker.localPosition;
            groundChecker.localPosition = new Vector3(origin.x, -1 * origin.y, origin.z);
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
        Vector2 checkRope = ladderChecker.position;
        isOnPlatform = (Physics2D.BoxCastAll(checkPlatform, new Vector2(0.89f, 0.12f), 0, new Vector2(0,0),0, layerMaskPlatform).Any(x => x.collider.isTrigger == false));
        isWithLadder = Physics2D.OverlapCircle(checkLadder, 0.25f, layerMaskLadder);
        Collider2D ropeCollider = Physics2D.OverlapCircle(checkRope, 0.25f, layerMaskRope);
        isWithRope = ropeCollider != null;
        if (ropeCollider != null) {
            rope = ropeCollider.transform.parent.GetComponent<Rope>();
            Debug.Assert(rope != null);
        }
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
        inputJumping = false;
        inputXDirection = 0;
        inputYDirection = 0;
        inputYCount = 0;
        if (Input.GetKey("right"))
        { inputXDirection += 1; }
        if (Input.GetKey("left"))
        { inputXDirection -= 1; }
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
