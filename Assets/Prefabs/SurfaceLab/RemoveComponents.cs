using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using Leap.Unity.Interaction.PhysicsHands;
using Clayxels;
using System;
using UnityEngine.UI;
using Leap.Unity;

public class RemoveComponents : MonoBehaviour
{


    public Clayxels.ClayContainer surfacePrefab;


    // Start is called before the first frame update
    void Start()
    {

        Destroy(surfacePrefab.gameObject.GetComponent<SymmetricSurface>()); 
        
        foreach (SphereCollider sphere in surfacePrefab.GetComponentsInChildren<SphereCollider>())
        {

            sphere.radius = 0.5f;
            Destroy(sphere.gameObject.GetComponent<SymmetryLink>());
            Destroy(sphere.gameObject.GetComponent<InteractionBehaviour>());
            sphere.GetComponentInChildren<ClayObject>().gameObject.AddComponent<SphereCollider>().radius = 0.5f;
            
        }


    }

}
