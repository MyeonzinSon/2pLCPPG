using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityButton : MonoBehaviour {

	public GameObject wire;
	bool isElectricityOn;

	public Sprite inactiveSprite;
	public Sprite activeSprite;

	// Use this for initialization
	void Start () 
	{
		TurnOnElectricity();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if ((coll.gameObject.tag == "PlayerOne") || (coll.gameObject.tag == "PlayerTwo"))
		{
			if (isElectricityOn)
			{
				TurnOffElectricity();
			}
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if ((coll.gameObject.tag == "PlayerOne") || (coll.gameObject.tag == "PlayerTwo"))
		{
			TurnOnElectricity();
		}
	}
	
	void TurnOnElectricity()
	{
		wire.GetComponent<EletricityWire>().TurnOnElectricity();
		GetComponent<SpriteRenderer>().sprite = activeSprite;
		isElectricityOn = true;
	}

	void TurnOffElectricity()
	{
		wire.GetComponent<EletricityWire>().TurnOffElectricity();		
		GetComponent<SpriteRenderer>().sprite = inactiveSprite;
		isElectricityOn = false;
	}
}
