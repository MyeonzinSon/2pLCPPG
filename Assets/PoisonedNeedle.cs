using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedNeedle : MonoBehaviour
{
    public float speed;
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
    }

}
