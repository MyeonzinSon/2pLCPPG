using UnityEngine;
using System.Collections;
using System.Linq;

public class GuardController : MonoBehaviour
{
    public float moveForce;
    public float collideForce;
    public float gravity;
    public float moveSpeed;
    
    private bool isOnPlatform = false;
    // 0이면 로프의 아래 끝, 1이면 로프의 위 끝
    
    int inputXDirection = 0;
    int lastXDirection = 0;
    int inputYDirection = 0;
    int inputYCount = 0;

    public LayerMask layerMaskPlatform;

    Rigidbody2D rb2d;

    Transform groundChecker;
    public Transform leftBoundMarker;
    public Transform rightBoundMarker;

    float initMoveSpeed;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        groundChecker = transform.FindChild("GroundChecker");
        initMoveSpeed = moveSpeed;
        Initialize();
    }

    void Start()
    {
        Initialize(); 
        inputXDirection -= 1; lastXDirection = -1;
    }

    void Update()
    {
        CollisionCheck();
        if (transform.position.x < rightBoundMarker.position.x)
        {
            inputXDirection += 1;
            inputXDirection += 1; lastXDirection = 1;
        }
        
        if (transform.position.x > leftBoundMarker.position.x)
        {
            inputXDirection -= 1;
            inputXDirection -= 1; lastXDirection = -1;
        }
    }

    void FixedUpdate()
    {

        UpdateVelocityInFixedUpdate();

        // sprite direction
        if (rb2d.velocity.x > collideForce / 2)
        {
            Quaternion origin = transform.rotation;
            transform.rotation = Quaternion.Euler(origin.x, 0, origin.z);
        }
        else if (rb2d.velocity.x < -collideForce / 2)
        {
            Quaternion origin = transform.rotation;
            transform.rotation = Quaternion.Euler(origin.x, 180, origin.z);
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
    }

    void UpdateVelocityInFixedUpdate() 
    {
        if (isOnPlatform)
        {
            SetVelocity(rb2d.velocity.x, 0);
        }
        else
        {
            AddVelocity(0, -gravity);
            if (rb2d.velocity.x == 0f)
            { AddVelocity(inputXDirection * moveForce, 0f); }
            if (rb2d.velocity == new Vector2(0f, 0f))
            {
                AddVelocity(inputXDirection * moveForce, 0f);
            }
        }

        if (Mathf.Abs(rb2d.velocity.x) <= moveSpeed)
        { AddVelocity(inputXDirection * moveForce, 0); }
        else if (inputXDirection * Sign(rb2d.velocity.x) < 0)
        { AddVelocity(inputXDirection * (moveForce + collideForce), 0); }

        if (Mathf.Abs(rb2d.velocity.x) >= collideForce)
        { AddVelocity(-1 * Sign(rb2d.velocity.x) * collideForce, 0); }
        else
        { SetVelocity(0, rb2d.velocity.y); }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    void CollisionCheck()
    {
        Vector2 checkPlatform = groundChecker.position;
        isOnPlatform = (Physics2D.BoxCastAll(checkPlatform, new Vector2(0.89f, 0.15f), 0, new Vector2(0, 0), 0, layerMaskPlatform).Any(x => x.collider.isTrigger == false));
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
    }
    public void Initialize()
    {
        moveSpeed = initMoveSpeed;
        SetVelocity(0f, 0f);
        inputXDirection = 0;
        inputYDirection = 0;
        inputYCount = 0;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<SpriteRenderer>().color = Color.white;
        lastXDirection = 0;
    }
}
