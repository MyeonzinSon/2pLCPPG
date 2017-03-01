﻿using System.Collections;
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

    static public int stage;
    static public int map;

    static public GameManager instance;

    void Awake()
    {
        instance = this;
        PlayerOne = GameObject.Find("PlayerOne");
        PlayerTwo = GameObject.Find("PlayerTwo");
        MainCamera = GameObject.Find("Main Camera");
        stage = 1;
        map = 1;
        ImportMap();
    }
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            RespawnOne();
            RespawnTwo();
        }
    }

    static void ImportMap()
    {
        Map = GameObject.Find("Map" + map);
        respawnPointOne = GameObject.Find("Map" + map + "/RespawnOne").transform;
        respawnPointTwo = GameObject.Find("Map" + map + "/RespawnTwo").transform;
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
        RespawnTwo();
        MoveCamera();
    }
    public static void RestartMap()
    {
        instance.StartCoroutine(Restart());
    }
    static IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);
        RespawnOne();
        RespawnTwo();
    }
    public static void RespawnOne()
    {
        PlayerOne.SetActive(true);
        PlayerOne.transform.position = new Vector2(respawnPointOne.position.x, respawnPointOne.position.y);
        PlayerOne.GetComponent<PlayerOneController>().Initialize();
    }
    public static void RespawnTwo()
    {
        PlayerTwo.SetActive(true);
        PlayerTwo.transform.position = new Vector2(respawnPointTwo.position.x, respawnPointTwo.position.y);
        PlayerTwo.GetComponent<PlayerTwoController>().Initialize();
    }
    static void MoveCamera()
    {
        MainCamera.transform.position = new Vector3(Map.transform.position.x, Map.transform.position.y, MainCamera.transform.position.z);
    }
}
