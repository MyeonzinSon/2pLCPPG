using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartDestroy : MonoBehaviour
{
    Vector3 originPosition;

    void Start()
    {
        originPosition = transform.position;
    }

    public void Initialize()
    {
        transform.position = originPosition;
        if (gameObject.GetComponent<CollapsePlatform>() != null)
        { gameObject.GetComponent<CollapsePlatform>().Initialize(); }
    }

    public void PseudoDestroy()
    {
        gameObject.SetActive(false);
    }

    public void PseudoDestroy(float delay)
    {
        Invoke("PseudoDestroy", delay);
    }
}
