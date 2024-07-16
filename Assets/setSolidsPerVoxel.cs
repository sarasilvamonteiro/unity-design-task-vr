using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setSolidsPerVoxel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Clayxels.ClayContainer>().setMaxSolidsPerVoxel(2048);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
