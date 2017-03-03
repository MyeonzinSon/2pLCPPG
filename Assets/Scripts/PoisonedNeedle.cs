using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedNeedle : MonoBehaviour
{
    public float speed;
    float theta;
    
    public void Initialize()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        
        StartCoroutine(Suicide());
    }
    IEnumerator Suicide()
    {
        yield return new WaitForSeconds(36 / speed);
        Destroy(gameObject);
    }
    void FixedUpdate()
    {
        theta = transform.rotation.eulerAngles.z*Mathf.PI/180;
        Vector3 position = transform.position;
        transform.position = new Vector3(position.x + speed*Mathf.Cos(theta)*Time.deltaTime, position.y + speed*Mathf.Sin(theta)*Time.deltaTime, position.z);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerOne")
            other.gameObject.GetComponent<PlayerOneController>().Die();
        if (other.gameObject.tag == "PlayerTwo")
            other.gameObject.GetComponent<PlayerTwoController>().Die();
        Destroy(gameObject);
    }
    int Sign(float f)
    {
        if (f == 0)
        { return 0; }
        else if (f > 0)
        { return 1; }
        else { return -1; }
    }
}
