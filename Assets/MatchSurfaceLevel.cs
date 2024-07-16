using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSurfaceLevel : MonoBehaviour
{

    private int holes;
    public GameObject surface1;
    public GameObject surface2;
    public GameObject surface3;
    [System.NonSerialized]
    public GameObject targetSurface;

    // Start is called before the first frame update
    void Start()
    {

        holes = FindObjectOfType<Setup>().holes;       
        surface1.gameObject.SetActive(false);
        surface2.gameObject.SetActive(false);
        surface3.gameObject.SetActive(false);


        if (holes == 1)
        {
            surface1.gameObject.SetActive(true);
            targetSurface = surface1;
        }
        if (holes == 2)
        {
            surface2.gameObject.SetActive(true);
            targetSurface = surface2;
        }
        if (holes == 3)
        {
            surface3.gameObject.SetActive(true);
            targetSurface = surface3;
        }
        
        
        
    }

}
