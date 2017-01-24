using UnityEngine;
using System.Collections;

public class PlayerOneController : MonoBehaviour
{
    
    [System.NonSerialized] public float moveForce = 100f;
    [System.NonSerialized] public float collideForce = 50f;
    [System.NonSerialized] public float gravity = 9.80f;
    private float maxSpeed = 5;
    public bool isOnPlatform = false;
    private bool isFacingRight = false;
    Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        { transform.position = new Vector3(0f, 0f, transform.position.z); }
    }
    void FixedUpdate()
    {
        int inputDirection = 0;

        if (Input.GetKey("right"))
        { inputDirection = 1; }
        else if (Input.GetKey("left"))
        { inputDirection = -1; }
        else { inputDirection = 0; }

        if (Mathf.Abs(rb2d.velocity.x) <= maxSpeed)
        { rb2d.AddForce(new Vector2(inputDirection * moveForce, 0)); }

        if (inputDirection == 0 && Mathf.Abs(rb2d.velocity.x) <= 0.1f)
        { rb2d.velocity = new Vector2(0, rb2d.velocity.y); }

        if (Input.GetKeyDown("up"))
        {
            rb2d.AddForce(new Vector2(0, gravity*0.5f), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown("q"))
        { rb2d.AddForce(new Vector2(1000f, 0)); }

        if (!isOnPlatform)
        {
            rb2d.AddForce(new Vector2(0, -gravity));
        }
    }
}