using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    GameObject PlayerOne;
    GameObject PlayerTwo;

    bool enterOne;
    //temporarily set true for convenience
    bool enterTwo = true;

    void Awake()
    {
        PlayerOne = GameObject.Find("PlayerOne");
        PlayerTwo = GameObject.Find("PlayerTwo");
    }
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == PlayerOne)
        { enterOne = true; }
        if (other.gameObject == PlayerTwo)
        { enterTwo = true; }

        if (enterOne && enterTwo)
        { GameManager.MapChange(); }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == PlayerOne)
        { enterOne = false; }
        if (other.gameObject == PlayerTwo)
        { enterTwo = false; }
    }
}
