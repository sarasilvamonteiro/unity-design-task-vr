using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction.PhysicsHands;
using System.Linq;

public class Setup : MonoBehaviour
{

    //[System.NonSerialized]
    public bool start = false;
    [System.NonSerialized]
    public bool setupDone = false;
    [System.NonSerialized]
    public bool running = false;
    [System.NonSerialized]
    private StartButton startButton;
    [System.NonSerialized]
    public Slider holesSlider;
    [System.NonSerialized]
    public Slider flowsSlider;
    private GameObject enabledHolesSlider;
    private GameObject enabledFlowsSlider;


    public bool VR = false;
    public bool useHands = false;
    public bool physics = false;

    [NamedArrayAttribute(new string[] { "Empty Trial", "Match Surface Trial", "Symmetric Trial", "No Fluid Trial", "No Surface Trial"})]
    public bool[] trialTypes;

    // Trial types
    [System.NonSerialized]
    public bool emptyTrial;
    [System.NonSerialized]
    public bool matchSurfaceTrial;
    [System.NonSerialized]
    public bool symmetricTrial;
    [System.NonSerialized]
    public bool noFluidTrial;
    [System.NonSerialized]
    public bool noSurfaceTrial;
    [System.NonSerialized]
    public bool flowsOn;
    [System.NonSerialized]
    public bool holesOn;
    public int holes;
    public int flows;
    public int totalParticles;

    public GameObject Room0;
    public GameObject Room1;
    public GameObject Room2;
    public GameObject Room3;
    public GameObject Room4;
    [System.NonSerialized]
    public GameObject activeRoom;
    Dictionary<int, GameObject> Rooms = new Dictionary<int, GameObject>();

    //[System.NonSerialized]
    public Clayxels.ClayContainer clayContainer;
    //[System.NonSerialized]
    public Clayxels.ClayContainer groundContainer;
    [System.NonSerialized]
    public GameObject emitterContainer;
    [System.NonSerialized]
    public GameObject liquidContainer;
    [System.NonSerialized]
    public Dictionary<int, int> SphereIDs;
    [System.NonSerialized]
    public int sphereNumber;
    private Material transparentMaterial;



    // Start is called before the first frame update
    void Start()
    {

        emptyTrial = trialTypes[0];
        matchSurfaceTrial = trialTypes[1];
        symmetricTrial = trialTypes[2];
        noFluidTrial = trialTypes[3];
        noSurfaceTrial = trialTypes[4];

        startButton = FindObjectOfType<StartButton>();
        holesSlider = FindObjectsOfType<Slider>()[1];
        flowsSlider = FindObjectsOfType<Slider>()[0];
        enabledHolesSlider = holesSlider.gameObject.GetComponentInParent<Transform>().gameObject.GetComponentInParent<Transform>().gameObject;
        enabledFlowsSlider = flowsSlider.gameObject.GetComponentInParent<Transform>().gameObject.GetComponentInParent<Transform>().gameObject;

        // Add rooms to dictionary
        Rooms.Add(0, Room0);
        Rooms.Add(1, Room1);
        Rooms.Add(2, Room2);
        Rooms.Add(3, Room3);
        Rooms.Add(4, Room4);

        transparentMaterial = new Material(Shader.Find("Diffuse"));
    }

    // Update is called once per frame
    void Update()
    {

        if (!running)
        {
            emptyTrial = trialTypes[0];
            matchSurfaceTrial = trialTypes[1];
            symmetricTrial = trialTypes[2];
            noFluidTrial = trialTypes[3];

            // Set holes/flows status On/Off
            if (emptyTrial || matchSurfaceTrial || noFluidTrial)
            {
                holesOn = false;
                flowsOn = false;
            }
            if (noFluidTrial && !emptyTrial && !matchSurfaceTrial)
            {
                holesOn = true;
            }
            if (!noFluidTrial && !emptyTrial && !matchSurfaceTrial)
            {
                flowsOn = true;
                holesOn = true;
            }

            // Enable/Disable holes/flows sliders
            if (flowsOn)
            {
                flowsSlider.value = flows;
                holesSlider.value = holes;
                if (!enabledFlowsSlider.activeSelf)
                {
                    enabledFlowsSlider.SetActive(true);
                }
                if (!enabledHolesSlider.activeSelf)
                {
                    enabledHolesSlider.SetActive(true);
                }
            }
            if (holesOn)
            {
                holesSlider.value = holes;
                if (!enabledHolesSlider.activeSelf)
                {
                    enabledHolesSlider.SetActive(true);
                }
            }
            if (!flowsOn)
            {   if(enabledFlowsSlider.activeSelf)
                {
                    enabledFlowsSlider.SetActive(false);
                }
            }
            if (!holesOn)
            {
                if (enabledHolesSlider.activeSelf)
                {
                    enabledHolesSlider.SetActive(false);
                }
            }

        }


        // Check if StartButton is pressed. OR manual start
        if (startButton.start || start)
        {
            Debug.Log("Setup");
            RoomSetup();
            setupDone = true;
            // GetData turns start to false, if not enabled we need to turn it to false here
            if (gameObject.GetComponent<GetData>().enabled == false)
            {
                DisableButton();
                StartSimulation();
            }
        }

        // Make fluid emitter stop if total created particles is above our totalParticles limit. 
        // if GetData is enabled, fluid will stop after max number of rows is achieved. //
        if (running && !emptyTrial && !matchSurfaceTrial && gameObject.GetComponent<GetData>().enabled == false)
        {
            if (emitterContainer.GetComponentInChildren<com.zibra.liquid.Manipulators.ZibraLiquidEmitter>().CreatedParticlesTotal >= totalParticles)
            {
                emitterContainer.GetComponentInChildren<com.zibra.liquid.Manipulators.ZibraLiquidEmitter>().enabled = false;
                running = false;
                Debug.Log("Stop simulation");
            }
        }


    }


    void RoomSetup()
    {
        

        if (emptyTrial)
        {
            activeRoom = Room0;
        }
        else if (matchSurfaceTrial)
        {
            activeRoom = Room4;
        }
        else
        {
            if (flowsOn)
            {
                flows = flowsSlider.GetComponent<StartSlider>().number;
                holes = holesSlider.GetComponent<StartSlider>().number;
                activeRoom = Rooms[holes];
                emitterContainer = activeRoom.GetComponentInChildren<com.zibra.liquid.Manipulators.ZibraLiquidEmitter>().gameObject;
                liquidContainer = activeRoom.GetComponentInChildren<com.zibra.liquid.Solver.ZibraLiquid>().gameObject;
            }
            if (holesOn)
            {
                holes = holesSlider.GetComponent<StartSlider>().number;
                activeRoom = Rooms[holes];
                groundContainer = activeRoom.GetComponentsInChildren<Clayxels.ClayContainer>()[1];
            }
            
        }

        // Define the containers that we will get data from in GetData.cs
        if (!flowsOn)
        {
            //emitterContainer.SetActive(false);
            // We need ground container for fluid score.   
        }

        clayContainer = activeRoom.GetComponentsInChildren<Clayxels.ClayContainer>()[0];
       
        if (symmetricTrial)
        {
            clayContainer.gameObject.AddComponent<SymmetricSurface>();
        }
        if (noSurfaceTrial)
        {
            clayContainer.customMaterial = transparentMaterial;
        }


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

        sphereNumber = SphereIDs.Count;

    }



    public void DisableButton()
    {

        // Disable colliders
        startButton.GetComponent<BoxCollider>().enabled = false;
        foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }
        // Disable visuals
        foreach (Canvas canvas in gameObject.GetComponentsInChildren<Canvas>())
        {
            canvas.enabled = false;
        }
        if (flowsOn)
        {
            flows = flowsSlider.GetComponent<StartSlider>().number;
            holes = holesSlider.GetComponent<StartSlider>().number;
        }
        if (holesOn)
        {
            holes = holesSlider.GetComponent<StartSlider>().number;
        }
    }

    public void StartSimulation()
    {
        Debug.Log("Start simulation");
        startButton.start = false;
        start = false;
        setupDone = false;
        activeRoom.SetActive(true);
        running = true;
    }
}
