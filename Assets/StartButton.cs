using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			SceneManager.LoadScene("Stage1");	
		}

		if (Input.GetKeyDown("1"))
		{
			SceneManager.LoadScene("Stage1");
		}
		if (Input.GetKeyDown("2"))
		{
			SceneManager.LoadScene("Stage2");
		}
	}
	
	void OnMouseDown()
	{
		SceneManager.LoadScene("Stage1");
	}
}
