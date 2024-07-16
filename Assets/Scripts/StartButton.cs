using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton: MonoBehaviour
{

    [System.NonSerialized]
    public bool start = false;

    private void OnTriggerEnter(Collider other)
    {
        start = true;
    }
}
