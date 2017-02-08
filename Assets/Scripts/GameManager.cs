using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameObject PlayerOne;
    static public GameObject PlayerTwo;
    static public GameObject MainCamera;
    static public GameObject Map;

    static public Transform respawnPointOne;
    static public Transform respawnPointTwo;
    static Rigidbody2D rb2dOne;
    static Rigidbody2D rb2dTwo;

    static public bool isChangingMap;
    static public int stage;
    static public int map;

    void Awake()
    {
        PlayerOne = GameObject.Find("PlayerOne");
        PlayerTwo = GameObject.Find("PlayerTwo");
        MainCamera = GameObject.Find("Main Camera");
        rb2dOne = PlayerOne.GetComponent<Rigidbody2D>();
        rb2dTwo = PlayerOne.GetComponent<Rigidbody2D>();
        stage = 1;
        map = 1;
        isChangingMap = false;
        ImportMap();
    }
    void Update()
    {
        Debug.Log("map int = " + map);
        if (Input.GetKeyDown("r"))
        {
            RespawnOne();
        }
    }

    static void ImportMap()
    {
        Map = GameObject.Find("Map" + map);
        respawnPointOne = GameObject.Find("Map" + map + "/RespawnOne").transform;
    }
    static public void MapChange()
    {
        //test code start
        if (map == 3)
        { map = 0; }
        //test code end
        map += 1;
        ImportMap();
        RespawnOne();
        MoveCamera();
    }

    public static void RespawnOne()
    {
        Debug.Log("RespawnOne");
        PlayerOne.transform.position = new Vector2(respawnPointOne.position.x, respawnPointOne.position.y);
        rb2dOne.velocity = new Vector2(0f, 0f);
    }
    static void MoveCamera()
    {
        MainCamera.transform.position = new Vector3(Map.transform.position.x, Map.transform.position.y, MainCamera.transform.position.z);
    }
}
