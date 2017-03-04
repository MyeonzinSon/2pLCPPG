using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

	Image image;
	Text text;

	Image keyImage;

	public Sprite trans;
	public Sprite bluepill;
	public Sprite note7;
	public Sprite paper;

	public Sprite key;

	// Use this for initialization
	void Start () {
		image = GameObject.Find("ItemImage").GetComponent<Image>();
		text = GameObject.Find("ItemText").GetComponent<Text>();
		keyImage = GameObject.Find("KeyImage").GetComponent<Image>();

		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Initialize()
	{
		image.sprite = trans;
		text.text = " ";
		keyImage.sprite = trans;
	}

	public void UpdateKeySlotState(bool isGet)
	{
		if (isGet)
			keyImage.sprite = key;
		else
			keyImage.sprite = trans;
	}

	public void UpdateItemSlotState(Item item, int numOfItem)
	{
		switch (item)
		{
			case Item.BluePill:
				image.sprite = bluepill;
				break;
			case Item.NoteSeven:
				image.sprite = note7;
				break;
			case Item.BallotPaper:
				image.sprite = paper;
				break;
			case Item.Null:
				image.sprite = trans;
				break;
		}
		if (item != Item.Null)
			text.text = "X " + numOfItem;
		else
			text.text = " ";
	}
}
