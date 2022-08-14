using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SterogramTimer : MonoBehaviour {
    private Material m;

    private void Start()
    {
        StartCoroutine(OneFrame());
    }

    private IEnumerator OneFrame()
    {
        yield return null;
        m = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update () {
        if (m != null)
            m.SetFloat("time", Time.time);
	}
}
