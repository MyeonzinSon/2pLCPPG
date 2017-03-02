using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActiveOnLight : MonoBehaviour {

	Collider2D coll;

	List<GameObject> childrenObjects;

	// Use this for initialization
	void Start () {
		coll = GetComponent<Collider2D>();
		childrenObjects = new List<GameObject>();
		GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(x => childrenObjects.Add(x.gameObject));
	}

	// Update is called once per frame
	void Update () {
		Bounds region = coll.bounds;
		if (Physics2D.OverlapAreaAll (region.max, region.min, LayerMask.GetMask ("Light")).Any(k => k.isTrigger == true))
			SetActiveByLight();
		else
			SetInactiveByLight();
	}

	void SetActiveByLight()
	{
		coll.isTrigger = false;
		childrenObjects.ForEach(go => go.GetComponent<SpriteRenderer>().color = Color.white);
	}

	void SetInactiveByLight()
	{
		coll.isTrigger = true;
		childrenObjects.ForEach(go => go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f));
	}
}
