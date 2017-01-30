using UnityEngine;
using System.Collections;

public class PlayerOneController : MonoBehaviour
{
    
    [System.NonSerialized] public float moveForce = 100f;
    [System.NonSerialized] public float collideForce = 25f;
    [System.NonSerialized] public float gravity = 9.80f;
    private float maxSpeed = 5;
    public bool isOnPlatform = false;
    private bool isFacingRight = false;
    int inputDirection = 0;
    Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyUp("right") || Input.GetKeyUp("left"))
        { inputDirection = 0; }
        if (Input.GetKey("right"))
        { inputDirection = 1; }
        if (Input.GetKey("left"))
        { inputDirection = -1; }

        Debug.Log(isOnPlatform);
        Debug.Log(inputDirection);
        if (Input.GetKeyDown("r"))
        { transform.position = new Vector3(0f, 0f, transform.position.z); }
    }
    void FixedUpdate()
    {
        if (isOnPlatform)
        {
            if (Input.GetKeyDown("up"))
            {
                isOnPlatform = false;
                rb2d.AddForce(new Vector2(0, gravity * 0.5f), ForceMode2D.Impulse);
            }
        }
        else
        {
            rb2d.AddForce(new Vector2(0, -gravity));
        }

        if (Mathf.Abs(rb2d.velocity.x) <= maxSpeed)
        { rb2d.AddForce(new Vector2(inputDirection * moveForce, 0)); }

        rb2d.AddForce(new Vector2(-1 * rb2d.velocity.x * collideForce, 0));
        if (Mathf.Abs(rb2d.velocity.x) <= 0.01f)
        { rb2d.velocity = new Vector2(0, rb2d.velocity.y); }

        if (Input.GetKeyDown("q"))
        { rb2d.AddForce(new Vector2(1000f, 0)); }
    }
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            isOnPlatform = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
            isOnPlatform = false;
    }
}