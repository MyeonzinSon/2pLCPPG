using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    static public GameObject PlayerOne;
    static public GameObject PlayerTwo;
    static public GameObject MainCamera;
    static public GameObject Map;

    static public Transform respawnPointOne;
    static public Transform respawnPointTwo;
    static RestartSetActive[] arrayOfSetActive;
    static RestartDestroy[] arrayOfDestroy;
    static LightSwitch[] arrayOfLightSwitch;

    static List<GameObject> flexibleObjects;
    static List<GameObject> changableObjects;

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

    void Start()
    {
        flexibleObjects = new List<GameObject>();
        FindObjectsOfType<RestartDestroy>().ToList().ForEach(x => flexibleObjects.Add(x.gameObject));
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            RestartMap();
        }
    }

    static void ImportMap()
    {
        Map = GameObject.Find("Map" + map);
        respawnPointOne = GameObject.Find("Map" + map + "/RespawnOne").transform;
        respawnPointTwo = GameObject.Find("Map" + map + "/RespawnTwo").transform;
        arrayOfSetActive = Map.GetComponentsInChildren<RestartSetActive>();
        arrayOfLightSwitch = Map.GetComponentsInChildren<LightSwitch>();
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
        SetActiveOnRestart();
        DestroyOnRestart();
        InitializeOnRestart();
    }
    static void InitializeOnRestart()
    {
        Map.GetComponentInChildren<Door>().Initialize();
        foreach (var obj in arrayOfLightSwitch)
        {
            obj.GetComponent<LightSwitch>().Initialize();
        }
    }
    public static void SetActiveOnRestart()
    {
        foreach (var obj in arrayOfSetActive)
        {
            obj.gameObject.SetActive(true);
        }
    }
    public static void DestroyOnRestart()
    {
        foreach (var obj in flexibleObjects)
        {
            obj.SetActive(true);
            obj.GetComponent<RestartDestroy>().Initialize();
        }
    }
    public static void RespawnOne()
    {
        PlayerOne.transform.position = new Vector2(respawnPointOne.position.x, respawnPointOne.position.y);
        PlayerOne.GetComponent<PlayerOneController>().Initialize();
        PlayerOne.SetActive(true);
    }
    public static void RespawnTwo()
    {
        PlayerTwo.transform.position = new Vector2(respawnPointTwo.position.x, respawnPointTwo.position.y);
        PlayerTwo.GetComponent<PlayerTwoController>().Initialize();
        PlayerTwo.SetActive(true);
    }
    static void MoveCamera()
    {
        MainCamera.transform.position = new Vector3(Map.transform.position.x, Map.transform.position.y, MainCamera.transform.position.z);
    }
}
