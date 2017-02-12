using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    GameObject PlayerOne;
    GameObject PlayerTwo;

    bool enterOne;
    bool enterTwo;

    void Awake()
    {
        PlayerOne = GameObject.Find("PlayerOne");
        PlayerTwo = GameObject.Find("PlayerTwo");
        enterOne = false;
        enterTwo = false;
    }
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerOne")
        { enterOne = true; }
        if (other.gameObject.tag == "PlayerTwo")
        { enterTwo = true; }

        if (enterOne && enterTwo)
        { GameManager.MapChange(); }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerOne")
        { enterOne = false; }
        if (other.gameObject.tag == "PlayerTwo")
        { enterTwo = false; }
    }
}
