using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSeven : MonoBehaviour
{
    Rigidbody2D rb2d;
    Collider2D[] nearByColliders;

    public float explosionRadius;
    public float explosionForceX;
    public float explosionForceY;
    public LayerMask layerMasks;

    public float gravity;
    public float collideForce;
    bool isOnPlatform = false;

    void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(2f);
        Explode();
    }
    void Explode()
    {
        nearByColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, explosionRadius, layerMasks);
        foreach (var collider in nearByColliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
            }
            else if (collider.gameObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity += new Vector2(Sign(rb.position.x - gameObject.transform.position.x) * explosionForceX, Sign(rb.position.y - gameObject.transform.position.y) * explosionForceY);
            }
        }
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rb2d.velocity += new Vector2(0f, -gravity);
        if (isOnPlatform)
        {
            rb2d.velocity += new Vector2(-1 * Sign(rb2d.velocity.x) * collideForce, 0f);
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
