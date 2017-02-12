using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerOne")
            other.gameObject.GetComponent<PlayerOneController>().Die();
        if (other.gameObject.tag == "PlayerTwo")
            other.gameObject.GetComponent<PlayerTwoController>().Die();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
