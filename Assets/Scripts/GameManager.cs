using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerOne;
    public GameObject PlayerTwo;

    int stage = 1;
    int map = 1;
    
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            RespawnOne();
        }
    }

    void RespawnOne()
    {
        Vector2 respawn = Data.OutputRespawn(stage, map);
        PlayerOne.gameObject.transform.position = respawn;
    }
}
