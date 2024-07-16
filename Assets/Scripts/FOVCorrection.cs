using UnityEngine;
using UnityEngine.XR;
using Leap.Unity;

public class FOVCorrection : MonoBehaviour
{
    private LeapXRServiceProvider xrServiceProvider;

    // Start is called before the first frame update
    void Update()
    {
        gameObject.transform.position = new Vector3(2.6f, -8.8f, 0);
    }

}
