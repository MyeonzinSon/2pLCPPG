using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    GameObject PlayerOne;
    GameObject PlayerTwo;

    bool enterOne;
    bool enterTwo;

    public bool opened = true;
    bool initialOpened;
    public bool isLastDoorIn1stStage;
    public bool isLastDoorIn2ndStage;

    ItemUI itemUI;

    void Awake()
    {
        PlayerOne = GameObject.Find("PlayerOne");
        PlayerTwo = GameObject.Find("PlayerTwo");
        enterOne = false;
        enterTwo = false;
        initialOpened = opened;
        UpdateColor();
        itemUI = FindObjectOfType<ItemUI>();
    }
    public void Initialize()
    {
        opened = initialOpened;
        UpdateColor();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerOne")
        { enterOne = true; }
        if (other.gameObject.tag == "PlayerTwo")
        { enterTwo = true; }

        if (opened && enterOne && enterTwo)
        {
            itemUI.UpdateKeySlotState(false);

            if (isLastDoorIn1stStage)
                SceneManager.LoadScene("Stage2");
            if (isLastDoorIn2ndStage)
                SceneManager.LoadScene("Ending");
            GameManager.MapChange();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerOne")
        { enterOne = false; }
        if (other.gameObject.tag == "PlayerTwo")
        { enterTwo = false; }
    }
    public void StateSwitch()
    {
        opened = !opened;
        UpdateColor();
    }
    public void StateOpened()
    {
        itemUI.UpdateKeySlotState(true);
        opened = true;
        UpdateColor();
    }
    public void StateClosed()
    {
        opened = false;
        itemUI.UpdateKeySlotState(false);
        UpdateColor();
    }
    void UpdateColor()
    {
        if (!opened)
        { gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 1f); }
        else
        { gameObject.GetComponent<SpriteRenderer>().color = Color.white; }
    }
}
