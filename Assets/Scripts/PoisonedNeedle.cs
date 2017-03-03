using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedNeedle : MonoBehaviour
{
    public float speed;
    public void Initialize()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        StartCoroutine(Suicide());
    }
    IEnumerator Suicide()
    {
        yield return new WaitForSeconds(36 / speed);
        Destroy(gameObject);
    }
    void FixedUpdate()
    {
        Vector3 position = transform.position;
        transform.position = new Vector3(position.x + speed*Time.deltaTime, position.y, position.z);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerOne")
            other.gameObject.GetComponent<PlayerOneController>().Die();
        if (other.gameObject.tag == "PlayerTwo")
            other.gameObject.GetComponent<PlayerTwoController>().Die();
        Destroy(gameObject);
    }

}
