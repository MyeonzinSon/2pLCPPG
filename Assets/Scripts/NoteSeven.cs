using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSeven : MonoBehaviour
{
    Collider2D[] nearByColliders;

    public float explosionRadius;
    public float explosionForceX;
    public float explosionForceY;
    public float explosionTime;
    public LayerMask layerMasks;

    public GameObject explodeObjectRed;
    public GameObject explodeObjectYellow;

    void Start()
    {
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(explosionTime);
        Explode();
    }
    void Explode()
    {
        Instantiate(explodeObjectRed, transform.position, Quaternion.identity);
        Instantiate(explodeObjectYellow, transform.position, Quaternion.identity);

        nearByColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, explosionRadius, layerMasks);
        foreach (var collider in nearByColliders)
        {
            if (collider.gameObject.tag == "Enemy")
            {
            }
            else if (collider.gameObject.tag == "PillarWood")
            {
                collider.gameObject.SetActive(false);
            }
            else if (collider.gameObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity += new Vector2(Sign(rb.position.x - gameObject.transform.position.x) * explosionForceX, Sign(rb.position.y - gameObject.transform.position.y) * explosionForceY);
            }
        }
        Destroy(gameObject);
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
