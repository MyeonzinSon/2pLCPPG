using UnityEngine;
using System.Collections;
using System.Linq;
using Rewired;

public enum Item { Null, NoteSeven, BluePill, BallotPaper }
public class ItemSlot
{
    ItemUI itemUI;

    public Item item;
    public int numOfItem;
    public ItemSlot() : this(Item.Null, 0) { }
    public ItemSlot(Item item, int num)
    {
        itemUI = MonoBehaviour.FindObjectOfType<ItemUI>();

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

        itemUI.UpdateItemSlotState(newItem, numOfItem);
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

        itemUI.UpdateItemSlotState(item, numOfItem);

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
    public float ropeSpeed;

    private bool isOnPlatform = false;
    private bool isWithLadder = false;
    private bool isOnLadder = false;
    private bool isWithRope = false;
    private bool isOnRope = false;
    // 0이면 로프의 아래 끝, 1이면 로프의 위 끝
    private float ropePosition = 0;
    private Rope rope = null;

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
    public LayerMask layerMaskRope;

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
    Player player;

    ItemUI itemUI;

    void Awake()
    {
        itemUI = FindObjectOfType<ItemUI>();
        rb2d = GetComponent<Rigidbody2D>();
        groundChecker = transform.FindChild("GroundChecker");
        ladderChecker = transform.FindChild("LadderChecker");
        initMoveSpeed = moveSpeed;
        initJumpSpeed = jumpSpeed;
        Initialize();
        player = ReInput.players.GetPlayer(1);
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
                        Debug.Log("asdf");
                        if (isOnLadder)
                        {
                            isOnLadder = false;
                            SetVelocity(inputXDirection * moveSpeed * 0.7f, jumpSpeed * 0.7f);
                        }
                        else
                        {
                            Debug.Log("qwer");
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

        if (rb2d.velocity.y != 0)
            GetComponent<Animator>().SetBool("isMovingVertical", true);
        else
            GetComponent<Animator>().SetBool("isMovingVertical", false);

        if (isOnLadder || isOnRope)
            GetComponent<Animator>().SetBool("isOnLadder", true);
        else
            GetComponent<Animator>().SetBool("isOnLadder", false);
    }

    void UpdateVelocityInFixedUpdate() {
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
            if (inputYCount > 0)
            {
                isOnLadder = true;
                SetVelocity(0f, 0f);
                gameObject.transform.position = new Vector2(otherLadderX, gameObject.transform.position.y);
            }
        }

        if (isWithRope)
        {
            if (inputYCount > 0)
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ladder")
        { otherLadderX = other.gameObject.transform.position.x; }

        if (other.gameObject.tag == "Key")
        {
            other.gameObject.GetComponentInParent<Door>().StateOpened();
            other.gameObject.SetActive(false);
        }
    }

    void InputKeys()
    {
        if (player.GetButtonDown("Horizontal"))
        { inputXDirection += 1; lastXDirection = 1; }
        if (player.GetButtonUp("Horizontal"))
        { inputXDirection -= 1; }
        if (player.GetNegativeButtonDown("Horizontal"))
        { inputXDirection -= 1; lastXDirection = -1; }
        if (player.GetNegativeButtonUp("Horizontal"))
        { inputXDirection += 1; }

        if (player.GetButtonDown("Vertical"))
        { inputYDirection += 1; inputYCount += 1; lastXDirection = 0; }
        if (player.GetButtonUp("Vertical"))
        { inputYDirection -= 1; inputYCount -= 1; }
        if (player.GetNegativeButtonDown("Vertical"))
        { inputYDirection -= 1; inputYCount += 1; lastXDirection = 0; }
        if (player.GetNegativeButtonUp("Vertical"))
        { inputYDirection += 1; inputYCount -= 1; }

        if (player.GetButtonDown("Jump"))
        { inputJumping = true; }
        if (player.GetButtonUp("Jump"))
        { inputJumping = false; }
        if (player.GetButtonDown("Action"))
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
        Vector2 checkRope = ladderChecker.position;
        isOnPlatform = (Physics2D.BoxCastAll(checkPlatform, new Vector2(0.89f, 0.15f), 0, new Vector2(0, 0), 0, layerMaskPlatform).Any(x => x.collider.isTrigger == false));
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
        StopCoroutine(BluePill());
        moveSpeed = initMoveSpeed;
        jumpSpeed = initJumpSpeed;
        SetVelocity(0f, 0f);
        isOnLadder = false;
        isOnRope = false;
        rope = null;
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
