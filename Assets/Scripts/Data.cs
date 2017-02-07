using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    static Dictionary<KeyValuePair<int, int>, Vector2> respawn = new Dictionary<KeyValuePair<int, int>, Vector2>();
    static Dictionary<KeyValuePair<int, int>, Vector2> camera = new Dictionary<KeyValuePair<int, int>, Vector2>();

    static Data()
    {
        /*
        x = stage number
        y = map number
        a = x coordinate of RespawnOne which is relative to camera x coordinate
        b = y coordinate of RespawnOne which is relative to camera y coordinate
        c = absolute x coordinate of camera
        d = absolute y coordinate of camera
        */
        Add(1, 1, -10f, -6f, 0f, 0f);
        Add(1, 2, -13f, -4f, 40f, 0f);
    }

    static private void Add(int x, int y, float a, float b, float c, float d)
    {
        respawn.Add(new KeyValuePair<int, int>(x, y), new Vector2(a + c, b + d));
        camera.Add(new KeyValuePair<int, int>(x, y), new Vector2(c, d));
    }

    static public Vector2 OutputRespawn(int stage, int map)
    {
        var key = new KeyValuePair<int, int>(stage, map);
        return respawn[key];
    }
    static public Vector2 OutputCamera(int stage, int map)
    {
        var key = new KeyValuePair<int, int>(stage, map);
        return camera[key];
    }
}
