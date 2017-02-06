using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    static Dictionary<KeyValuePair<int, int>, Vector2> respawn = new Dictionary<KeyValuePair<int, int>, Vector2>();
    static Dictionary<KeyValuePair<int, int>, Vector2> camera = new Dictionary<KeyValuePair<int, int>, Vector2>();

    static Data()
    {
        Add(1, 1, 0f, 0f, 0f, 0f);
        Add(1, 2, -3f+40f, 5f, 40f, 0f);
    }

    static private void Add(int x, int y, float a, float b, float c, float d)
    {
        respawn.Add(new KeyValuePair<int, int>(x, y), new Vector2(a, b));
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
