using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EletricityWire : MonoBehaviour {

	public GameObject electricObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnOnElectricity()
	{
		electricObject.SetActive(true);
	}

	public void TurnOffElectricity()
	{
		electricObject.SetActive(false);
	}
}
