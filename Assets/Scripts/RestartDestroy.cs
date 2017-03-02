using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartDestroy : MonoBehaviour {

    Vector3 originPosition;

    void Start()
    {
        originPosition = transform.position;
    }

    public void Initialize()
    {
        transform.position = originPosition;
    }

    public void PseudoDestroy()
    {
        gameObject.SetActive(false);
    }
}
