using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleDispenser : MonoBehaviour
{
    public GameObject Needle;
    public float needleSpeed;
    public float waitTime;

    public void Initialize()
    {
        StopCoroutine(ShootNeedle());
    }

    void Start()
    {
        StartCoroutine(ShootNeedle());
    }
    IEnumerator ShootNeedle()
    {
        while (true)
        {
            Quaternion origin = transform.rotation;
            float speed = needleSpeed * Sign(origin.y - 0.5f) * -1;
            GameObject clone = Instantiate(Needle, transform.localPosition, origin) as GameObject;
            clone.GetComponent<PoisonedNeedle>().speed = speed;
            yield return new WaitForSeconds(waitTime);
        }
    }

    int Sign(float f)
    {
        if (f > 0)
        { return 1; }
        else if (f < 0)
        { return -1; }
        else { return 0; }
    }
}
