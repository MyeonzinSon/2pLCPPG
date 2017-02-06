using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    GameObject PlayerOne;
    GameObject PlayerTwo;

    bool enterOne;
    bool enterTwo = true;

    void Start()
    {
        PlayerOne = GameObject.Find("PlayerOne");
        PlayerTwo = GameObject.Find("PlayerTwo");
    }
    void Update ()
    {
		if (enterOne && enterTwo)
        {
            Debug.Log("Hello!");
            GameManager.MapChange();
        }
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        if (other.gameObject == PlayerOne)
        { enterOne = true; Debug.Log("bool true"); }
        if (other.gameObject == PlayerTwo)
        { enterTwo = true; }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == PlayerOne)
        { enterOne = false; }
        if (other.gameObject == PlayerTwo)
        { enterTwo = false; }
    }
}
