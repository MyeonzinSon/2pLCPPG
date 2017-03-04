using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeapOfBox : MonoBehaviour {

	CircleCollider2D coll;
	SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		coll = GetComponent<CircleCollider2D>();
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		Bounds region = coll.bounds;
		if ((Physics2D.OverlapAreaAll (region.max, region.min, LayerMask.GetMask ("PlayerOne")).Length == 0) &&	
			(Physics2D.OverlapAreaAll (region.max, region.min, LayerMask.GetMask ("PlayerTwo")).Length == 0))
			sr.color = Color.white;
		else
			sr.color = new Color(1,1,1,0.5f);
	}
}
