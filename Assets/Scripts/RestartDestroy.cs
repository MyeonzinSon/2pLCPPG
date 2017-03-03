﻿using System.Collections;
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
        StopAllCoroutines();
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
        StartCoroutine(PseudoPseudoDestroy(delay));
    }
    IEnumerator PseudoPseudoDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);
        PseudoDestroy();
        yield return null;
    }
}
