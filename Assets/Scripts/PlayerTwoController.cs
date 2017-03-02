using UnityEngine;
using System.Collections;

public enum Item { Null, NoteSeven, BluePill, BallotPaper }
public class ItemSlot
{
    public Item item;
    public int numOfItem;
    public ItemSlot() : this(Item.Null, 0) { }
    public ItemSlot(Item item, int num)
    {
        this.item = item;
        this.numOfItem = num;
    }
    public bool IsBlank()
    {
        if (item == Item.Null) return true;
        else return false;
    }
    public void GetItem(Item newItem)
    {
        if (IsBlank())
        {
            item = newItem;
            numOfItem += 1;
        }
        else if (item == newItem)
        { numOfItem += 1; }
    }
    public Item UseItem()
    {
        Item output = item;
        if (numOfItem > 0)
        {
            numOfItem -= 1;
            if (numOfItem == 0)
            { item = Item.Null; }
        }
        return output;
    }
}
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

    bool inputJumping = false;
    int inputXDirection = 0;
    int lastXDirection = 0;
    int inputYDirection = 0;
    int inputYCount = 0;

    public LayerMask layerMaskPlatform;
    public LayerMask layerMaskLadder;
    public LayerMask layerMaskItemNoteSeven;
    public LayerMask layerMaskItemBallotPaper;
    public LayerMask layerMaskItemBluePill;

    Rigidbody2D rb2d;
    float otherLadderX;

    Transform groundChecker;
    Transform ladderChecker;

    ItemSlot itemSlot;
    public GameObject NoteSeven;
    float initMoveSpeed;
    float initJumpSpeed;
    public float bluePillMoveSpeed;
    public float bluePillJumpSpeed;
    public float bluePillTime;
    int bluePillCount;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        groundChecker = transform.FindChild("GroundChecker");
        ladderChecker = transform.FindChild("LadderChecker");
        initMoveSpeed = moveSpeed;
        initJumpSpeed = jumpSpeed;
    }
    void Start()
    { Initialize(); }
    void Update()
    {
        InputKeys();
        CollisionCheck();
        UpdateAbilityAvailableState();
    }
    void UpdateAbilityAvailableState()
    {

    }
    void CastAbility()
    {
        if (PickUpItem(layerMaskItemNoteSeven, Item.NoteSeven)) { }
        else if (PickUpItem(layerMaskItemBallotPaper, Item.BallotPaper)) { }
        else if (PickUpItem(layerMaskItemBluePill, Item.BluePill)) { }
        else if (!itemSlot.IsBlank())
        {
            switch (itemSlot.UseItem())
            {
                case Item.Null:
                    { break; }
                case Item.NoteSeven:
                    {
                        Vector2 newPosition = new Vector2(gameObject.transform.position.x + lastXDirection * 0.45f, gameObject.transform.position.y);
                        GameObject clone = Instantiate(NoteSeven, newPosition, gameObject.transform.rotation) as GameObject;
                        clone.GetComponent<Rigidbody2D>().velocity = new Vector2(lastXDirection * moveSpeed, jumpSpeed);
                        break;
                    }
                case Item.BluePill:
                    {
                        StartCoroutine(BluePill());
                        break;
                    }
                case Item.BallotPaper:
                    {
                        if (isOnLadder)
                        {
                            isOnLadder = false;
                            SetVelocity(inputXDirection * moveSpeed * 0.7f, jumpSpeed * 0.7f);
                        }
                        else
                        {
                            isOnPlatform = false;
                            SetVelocity(rb2d.velocity.x, jumpSpeed * Sign(gravity));
                        }
                        break;
                    }
            }
        }
    }
    bool PickUpItem(LayerMask layerMask, Item item)
    {
        Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, 0.1f, layerMask);
        if (array.Length > 0 && (itemSlot.item == item || itemSlot.numOfItem == 0))
        {
            array[0].gameObject.SetActive(false);
            itemSlot.GetItem(item);
            return true;
        }
        else { return false; }
    }
    IEnumerator BluePill()
    {
        bluePillCount += 1;
        GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.7f, 0.9f, 1);
        moveSpeed = bluePillMoveSpeed;
        jumpSpeed = bluePillJumpSpeed;
        yield return new WaitForSeconds(bluePillTime);
        bluePillCount -= 1;
        if (bluePillCount < 1)
        {
            bluePillCount = 0;
            GetComponent<SpriteRenderer>().color = Color.white;
            moveSpeed = initMoveSpeed;
            jumpSpeed = initJumpSpeed;
        }
    }
    void FixedUpdate()
    {

        if (isOnLadder)
        {
            SetVelocity(0f, inputYDirection * ladderSpeed);
            gameObject.transform.position = new Vector3(otherLadderX, gameObject.transform.position.y, gameObject.transform.position.z);
            if (inputJumping)
            {
                isOnLadder = false;
                SetVelocity(inputXDirection * moveSpeed * 0.7f, jumpSpeed * 0.7f);
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
                else
                {
                    SetVelocity(rb2d.velocity.x, 0);
                }
            }
            else
            {
                AddVelocity(0, -gravity);
                if (rb2d.velocity.x == 0f)
                { AddVelocity(inputXDirection * moveForce, 0f); }
                if (rb2d.velocity == new Vector2(0f, 0f))
                {
                    AddVelocity(inputXDirection * moveForce, 0f);
                    if (inputJumping)
                    { AddVelocity(0f, jumpSpeed); }
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

        if (rb2d.velocity.y != 0)
            GetComponent<Animator>().SetBool("isMovingVertical", true);
        else
            GetComponent<Animator>().SetBool("isMovingVertical", false);

        if (isOnLadder)
            GetComponent<Animator>().SetBool("isOnLadder", true);
        else
            GetComponent<Animator>().SetBool("isOnLadder", false);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        { otherLadderX = other.gameObject.transform.position.x; }
    }

    void InputKeys()
    {
        if (Input.GetKeyDown("d"))
        { inputXDirection += 1; lastXDirection = 1; }
        if (Input.GetKeyUp("d"))
        { inputXDirection -= 1; }
        if (Input.GetKeyDown("a"))
        { inputXDirection -= 1; lastXDirection = -1; }
        if (Input.GetKeyUp("a"))
        { inputXDirection += 1; }

        if (Input.GetKeyDown("w"))
        { inputYDirection += 1; inputYCount += 1; lastXDirection = 0; }
        if (Input.GetKeyUp("w"))
        { inputYDirection -= 1; inputYCount -= 1; }
        if (Input.GetKeyDown("s"))
        { inputYDirection -= 1; inputYCount += 1; lastXDirection = 0; }
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

        //test item {
        if (Input.GetKey("e"))
        {
            if (!itemSlot.IsBlank())
            {
                itemSlot.GetItem(itemSlot.item);
            }
        }
        if (Input.GetKeyDown("g"))
        {
            switch (itemSlot.item)
            {
                case Item.Null: itemSlot.item = Item.NoteSeven; break;
                case Item.NoteSeven: itemSlot.item = Item.BallotPaper; break;
                case Item.BallotPaper: itemSlot.item = Item.BluePill; break;
                case Item.BluePill: itemSlot.item = Item.NoteSeven; break;
            }
            itemSlot.numOfItem = 1;
            Debug.Log(itemSlot.item);
        }
        //test item }

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
        StopCoroutine(BluePill());
        moveSpeed = initMoveSpeed;
        jumpSpeed = initJumpSpeed;
        SetVelocity(0f, 0f);
        isOnLadder = false;
        inputJumping = false;
        inputXDirection = 0;
        inputYDirection = 0;
        inputYCount = 0;
        if (Input.GetKey("d"))
        { inputXDirection += 1; }
        if (Input.GetKey("a"))
        { inputXDirection -= 1; }
        if (Input.GetKey("w"))
        { inputYDirection += 1; inputYCount += 1; }
        if (Input.GetKey("s"))
        { inputYDirection -= 1; inputYCount += 1; }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        itemSlot = new ItemSlot();
        GetComponent<SpriteRenderer>().color = Color.white;
        lastXDirection = 0;
        bluePillCount = 0;
    }
}