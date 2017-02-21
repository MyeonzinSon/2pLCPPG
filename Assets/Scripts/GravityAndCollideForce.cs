using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAndCollideForce : MonoBehaviour
{
    Rigidbody2D rb2d;
    public float gravity;
    public float collideForce;
    bool isOnPlatform = false;

    void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb2d.velocity += new Vector2(0f, -gravity);
        if (isOnPlatform)
        {
            rb2d.velocity += new Vector2(-1 * Sign(rb2d.velocity.x) * collideForce, 0f);
            if (Mathf.Abs(rb2d.velocity.x) >= collideForce)
            { rb2d.velocity += new Vector2(-1 * Sign(rb2d.velocity.x) * collideForce, 0); }
            else
            { rb2d.velocity = new Vector2(0, rb2d.velocity.y); }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        { isOnPlatform = true; }
    }

    int Sign(float f)
    {
        if (f == 0)
        { return 0; }
        else if (f > 0)
        { return 1; }
        else { return -1; }
    }
}
