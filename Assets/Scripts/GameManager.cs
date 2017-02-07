using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameObject PlayerOne;
    static public GameObject PlayerTwo;
    static public GameObject MainCamera;
    static public GameObject Map;

    static Rigidbody2D rb2dOne;
    static Rigidbody2D rb2dTwo;

    static public bool isChangingMap;
    static public int stage;
    static public int map;

    void Start()
    {
        PlayerOne = GameObject.Find("PlayerOne");
        PlayerTwo = GameObject.Find("PlayerTwo");
        MainCamera = GameObject.Find("Main Camera");
        rb2dOne = PlayerOne.GetComponent<Rigidbody2D>();
        rb2dTwo = PlayerOne.GetComponent<Rigidbody2D>();
        stage = 1;
        map = 1;
        isChangingMap = false;
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
    }
    static public void MapChange()
    {
        //test code start
        if (map == 2)
        { map = 0; }
        //test code end
        map += 1;
        ImportMap();
        RespawnOne();
        MoveCamera();
        isChangingMap = false;
    }

    static void RespawnOne()
    {
        Debug.Log("RespawnOne");
        Vector2 respawn = Data.OutputRespawn(stage, map);
        PlayerOne.transform.position = respawn;
        rb2dOne.velocity = new Vector2(0f, 0f);
    }
    static void MoveCamera()
    {
        Vector2 camera = Data.OutputCamera(stage, map);
        MainCamera.transform.position = new Vector3(camera.x, camera.y, MainCamera.transform.position.z);
    }
}
