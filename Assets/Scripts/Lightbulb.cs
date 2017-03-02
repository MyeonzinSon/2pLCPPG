using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbulb : MonoBehaviour {

	public GameObject lightObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnOnLight()
	{
		lightObject.SetActive(true);
	}

	public void TurnOffLight()
	{
		lightObject.SetActive(false);
	}
}
