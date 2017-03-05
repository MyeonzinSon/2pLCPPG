using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{

    public GameObject Lightbulb;
    bool isLightbulbOn;
    public bool initStateOn;

    public Sprite inactiveSprite;
    public Sprite activeSprite;

    public GameObject soundClick;

    void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (initStateOn)
            TurnOnLight();
        else
            TurnOffLight();
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "PlayerOne") || (coll.gameObject.tag == "PlayerTwo"))
        {
            Instantiate<GameObject>(soundClick, transform.position, transform.rotation);
            if (isLightbulbOn)
            { TurnOffLight(); }
            else
            { TurnOnLight(); }
        }
    }

    void TurnOnLight()
    {
        Lightbulb.GetComponent<Lightbulb>().TurnOnLight();
        GetComponent<SpriteRenderer>().sprite = activeSprite;
        isLightbulbOn = true;
    }

    void TurnOffLight()
    {
        Lightbulb.GetComponent<Lightbulb>().TurnOffLight();
        GetComponent<SpriteRenderer>().sprite = inactiveSprite;
        isLightbulbOn = false;
    }
}
