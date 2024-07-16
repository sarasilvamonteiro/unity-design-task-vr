using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextButton : MonoBehaviour
{

    public GameObject surface1;
    public GameObject surface2;
    public GameObject surface3;

    private int maxHoles;




    // Start is called before the first frame update
    void Start()
    {
        maxHoles = FindObjectOfType<Setup>().holes;
        surface1.gameObject.SetActive(true);
        surface2.gameObject.SetActive(false);
        surface3.gameObject.SetActive(false);
        //FindObjectOfType<Setup>().clayContainer = surface1.GetComponent<Clayxels.ClayContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // When touched, next surface appears
    private void OnTriggerEnter(Collider other)
    {
        if (surface1.activeSelf && maxHoles >= 2)
        {
            surface1.SetActive(false);
            surface2.SetActive(true);
        }
        else if (surface2.activeSelf && maxHoles >= 3)
        {
            surface2.SetActive(false);
            surface3.SetActive(true);
        }
        else if (surface3.activeSelf)
        {
            surface3.SetActive(false);
            surface1.SetActive(true);
        }
    }
}
