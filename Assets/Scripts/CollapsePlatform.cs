using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CollapsePlatform : MonoBehaviour {

	public float delay = 0.5f;
	float delta = 0.15f;
	int speed = 10;
	List<GameObject> childrenObjects;

	bool isShaking;

	// Use this for initialization
	void Start () {
		childrenObjects = new List<GameObject>();
		GetComponentsInChildren<SpriteRenderer>().ToList().ForEach(x => childrenObjects.Add(x.gameObject));
	
		isShaking = false;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.GetComponent<PlayerOneController>().isAbilityActive) return;
		if (isShaking) return;
		StartCoroutine(Shake());
		Destroy(this.gameObject, 1);
	}

	IEnumerator Shake()
	{
		isShaking = true;
		for (int i = 0; i < speed; i++)
		{
			foreach (var block in childrenObjects)
			{
				int num = Random.Range(0,3);
				if (num != 0)
					StartCoroutine(ShakeEachBlock(block));
				yield return new WaitForSeconds(delay/(float)speed / 2.0f);
			}
		}
		yield return null;
	}

	IEnumerator ShakeEachBlock(GameObject block)
	{
		block.transform.position -= Vector3.up * delta;
		yield return new WaitForSeconds(delay/(float)speed / 2.0f);
		block.transform.position += Vector3.up * delta;
	}
}
