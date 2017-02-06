using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerOne;
    public GameObject PlayerTwo;

    static public int stage;
    static public int map;
    
    void Start()
    {
        stage = 1;
        map = 1;
    }
    void Update()
    {
        Debug.Log("map int = "+map);
        if (Input.GetKeyDown("r"))
        {
            RespawnOne();
        }
    }

    static public void MapChange()
    {
        map += 1;
    }

    void RespawnOne()
    {
        Vector2 respawn = Data.OutputRespawn(stage, map);
        PlayerOne.gameObject.transform.position = respawn;
    }
}
