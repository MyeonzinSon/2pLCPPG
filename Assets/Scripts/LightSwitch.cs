using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour {

	public GameObject Lightbulb;
	bool isLightbulbOn;
	public bool initStateOn;

	public Sprite inactiveSprite;
	public Sprite activeSprite;

	// Use this for initialization
	void Start () {
		if (initStateOn)
			TurnOnLight();
		else
			TurnOffLight();
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		Debug.Log("--");
		if ((coll.gameObject.tag == "PlayerOne") || (coll.gameObject.tag == "PlayerTwo"))
		{
			if (isLightbulbOn)
			{
				TurnOffLight();
			}
			else
			{
				TurnOnLight();
			}
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
