using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Leap.Unity.Interaction.PhysicsHands;


/*!\ This script runs in the VR format.
     For that reason, the environment is not generated automatically and needs to be setup previously via prefabs.
     If one wants to generate a different prefab, code is available for doing so (in the furute :( ).
     PREFABS NEEDED: surface, ground, fluid. 
*/
public class Create : MonoBehaviour
{

    public bool start = false;
    [System.NonSerialized]
    public bool running = false;
    // The surface has always the same type of behavior for hand input.
    // If useHands is true, and physics is true, only the hands' behavior changes.
    // If useHands is false (meaning cursor is true), and physics is true, only the surface's behavior changes. //
    public bool useHands = false;
    public bool physics = false;
    public bool emptyTrial = false;
    public bool VR = false;

    public Slider HolesSlider;
    public Slider FlowsSlider;
    public int holes = 1;
    public int flows = 1;
    public int totalParticles = 1;

    // This is not autimatized yet. Still need to finish that. :( //
    public Clayxels.ClayContainer clayContainer;
    public Clayxels.ClayContainer groundContainer;
    [System.NonSerialized]
    public GameObject emitterContainer;
    [System.NonSerialized]
    public GameObject liquidContainer;
    [System.NonSerialized]
    public List<GameObject> GroundDetectors;
    private List<com.zibra.liquid.Manipulators.ZibraLiquidCollider> ColliderList;
    [System.NonSerialized]
    public int sphereNumber;
    [System.NonSerialized]
    public Dictionary<int, int> SphereIDs;



    private void Start()
    {

        // upload prefabs!!!!
        emitterContainer = FindObjectOfType<com.zibra.liquid.Manipulators.ZibraLiquidEmitter>().gameObject;
        liquidContainer = FindObjectOfType<com.zibra.liquid.Solver.ZibraLiquid>().gameObject;
        groundContainer.gameObject.SetActive(false);
        liquidContainer.SetActive(false);
        emitterContainer.SetActive(false);
        clayContainer.gameObject.SetActive(false);

        // We chose a sphere ID independent to Unity's gameObject ID. 
        // It starts arbitrarily at 2000, and has a 25 int increment. //
        SphereIDs = new Dictionary<int, int>();
        int sphereID = 2000;

        foreach (Rigidbody sphere in clayContainer.GetComponentsInChildren<Rigidbody>())
        {
            SphereIDs.Add(sphere.gameObject.GetInstanceID(), sphereID);
            sphereID += 25; // 50 to make it the same number as data collected Jul. 23 
        }

        // Set type of leapManager and leapProvider for handtracking data. //
        if (useHands && gameObject.GetComponent<GetData>().enabled)
        {
            if (!physics)
            {
                //gameObject.GetComponent<GetData>().leapManager = FindObjectOfType<InteractionManager>();
                //gameObject.GetComponent<GetData>().leapProvider = FindObjectOfType<LeapServiceProvider>();
            }
            if (physics)
            {
                gameObject.GetComponent<GetData>().leapManager = null;
                gameObject.GetComponent<GetData>().leapProvider = FindObjectOfType<PhysicsProvider>();
            }
        }


    }
    private void Update()
    {

        Debug.Log(start);
        // These values are setup manually before trial starts
        HolesSlider.value = holes;
        FlowsSlider.value = flows;

        // Check if StartButton is pressed. //
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePoint = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePoint);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.transform == gameObject.transform) // Start Button pressed. //
                {
                    start = true;
                }
            }
        }

        // Manual game start. We didn't add StartButton to VR yet. //
        if (start == true)
        {
            // GetData.cs makes start = false. 
            // If it is disabled, we need to set start = false in this script. //
            //
            if (gameObject.GetComponent<GetData>().enabled == false)
            {
                start = false;
                // StartButton is not working. This only disables it's display. //
                DisableButton();
                StartSimulation();
            }

        }

        // Make fluid emitter stop if total created particles is above our totalParticles limit. 
        // if GetData is enabled, fluid will stop after max number of rows is achieved. //
        if (running && !emptyTrial && gameObject.GetComponent<GetData>().enabled == false)
        {
            if (emitterContainer.GetComponentInChildren<com.zibra.liquid.Manipulators.ZibraLiquidEmitter>().CreatedParticlesTotal >= totalParticles)
            {
                emitterContainer.GetComponentInChildren<com.zibra.liquid.Manipulators.ZibraLiquidEmitter>().enabled = false;
                running = false;
                Debug.Log("stop sim");
            }
        }

    }


    public void DisableButton()
    {

        gameObject.GetComponent<BoxCollider>().enabled = false;

        foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        foreach (Canvas canvas in gameObject.GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = false;
        }

        if (emptyTrial == false)
        {
            holes = HolesSlider.GetComponent<StartSlider>().number;
            flows = FlowsSlider.GetComponent<StartSlider>().number;
            
            gameObject.GetComponentInChildren<Canvas>().enabled = true;
        }
    }

    public void StartSimulation()
    {

        if (!VR)
        {
            ColliderList = new List<com.zibra.liquid.Manipulators.ZibraLiquidCollider>();
            GroundDetectors = new List<GameObject>();
            SphereIDs = new Dictionary<int, int>();

            // Create surface. //
            //Surface(Rows: 9);

            if (emptyTrial == false)
            {
                // Create environment. //
                //Ground(HolesNumber: holes);
                //Liquid(FlowsNumber: flows, LiquidColliders: ColliderList, VolumePerFrame: 0.01f);
            }
        }

        /////////////////////////////////////////////
        /// VR ///
        // FIX: Find a way to load prefabs into the scene, or just set it up as it is.
        // Maybe just find a way for the ground container prefab to be the correct one given the nr of holes..
        if (!emptyTrial)
        {
            groundContainer.gameObject.SetActive(true);
            emitterContainer.SetActive(true);
            liquidContainer.SetActive(true);
        }
        clayContainer.gameObject.SetActive(true);

        /////////////////////////////////////////////

        running = true;
       

    }
}

