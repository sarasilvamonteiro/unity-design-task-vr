using UnityEngine;
using Clayxels;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Leap.Unity.Interaction;
using Leap.Unity;
using Leap.Unity.Interaction.PhysicsHands;

public class SurfaceLab : MonoBehaviour
{
    private bool VR;
    public bool useHands = false;
    public bool physics = false;
    public int holes;
    [System.NonSerialized]
    public Clayxels.ClayContainer clayContainer;
    [System.NonSerialized]
    public int sphereNumber;
    [System.NonSerialized]
    public Dictionary<int, int> SphereIDs;
    private List<com.zibra.liquid.Manipulators.ZibraLiquidCollider> ColliderList;

    private float xMax;
    private float zMax;


    // Start is called before the first frame update
    void Awake()
    {
        ColliderList = new List<com.zibra.liquid.Manipulators.ZibraLiquidCollider>();
        SphereIDs = new Dictionary<int, int>();
        // Create surface. //
        Surface(rows: 9);
        OptimalSurface(rows: 9, holes: holes);

    }


    private void Surface(int rows)
    {

        sphereNumber = (int)(Mathf.Pow(rows, 2) + Mathf.Pow(rows - 1, 2));
        float sphereScale = 0.25f;
        // We chose a sphere ID independent to Unity's gameObject ID. 
        // It starts arbitrarily at 2000, and has a 25 int increment. //
        int sphereID = 2000; // 

        // EDIT: Initial sphere position (we want the surface centered). //
        float x = -sphereScale * (rows - 1); // -(sphereScale * Rows) / 2;
        float y = 0f;
        float z = -sphereScale * (rows - 1); // -(sphereScale * Rows) / 2;

        xMax = Math.Abs(x);
        zMax = Math.Abs(z);

        // Create ClayContainer. //
        clayContainer = new GameObject().AddComponent<ClayContainer>();
        clayContainer.setInteractive(true);
        clayContainer.name = "Surface";
        // empty script so far, just to name the object
        clayContainer.gameObject.AddComponent<Surface>();

        // EDIT: I swapped x with z so that all axes are positive. 
        // We want the container to be centered at x.
        clayContainer.transform.position = new Vector3(0, 0, 0);
        clayContainer.transform.localScale = Vector3.one;
        clayContainer.setMaxSolidsPerVoxel(2048);


        int current_row = 0;

        // Create one sphere (n) for each loop iteration. //
        for (int n = 0, r = 0; n < sphereNumber; n++, r++)
        {

            /*!\ Start next row.
                 if r == rows, it means we have created as many spheres as Rows
                 so we want to restart the row counter (r = 0). */
            if (n < rows * rows && r == rows)
            {
                r = 0;
                x = -sphereScale * (rows - 1);
                z += 2 * sphereScale;
                current_row += 1;
    
            }
            // if n == rows, we need to start creating the connection spheres (inside main grid).
            if (n == rows * rows)
            {
                r = 0;
                x = -sphereScale * (rows - 1) + sphereScale;
                z = -sphereScale * (rows - 1) + sphereScale;
                current_row = 0;
            }
            /*!\ This loop accounts for when the connection spheres rows reach its final limit (r == Rows-1).
                 Connection spheres have a grid of (Rows-1) by (Rows-1) and they sit inside the main Rows by Rows
                 main grid. */
            if (n > rows * rows && r == rows - 1)
            {
                r = 0;
                x = -sphereScale * (rows - 1) + sphereScale;
                z += 2 * sphereScale;
                current_row += 1;
            }

            // Add colliders to the sphere. //
            GameObject newCollider = new GameObject();
            newCollider.name = "collider_" + n.ToString();
            newCollider.transform.parent = clayContainer.transform;
            newCollider.transform.localScale = 2 * sphereScale * Vector3.one;
            newCollider.transform.localPosition = new Vector3(x, y, z);
            newCollider.AddComponent<SphereCollider>();
            newCollider.AddComponent<com.zibra.liquid.SDFObjects.AnalyticSDF>().ChosenSDFType = com.zibra.liquid.SDFObjects.AnalyticSDF.SDFType.Sphere;
            ColliderList.Add(newCollider.AddComponent<com.zibra.liquid.Manipulators.ZibraLiquidCollider>());

            // add on collision detection script
            //newCollider.AddComponent<OnCollision>();

            if (useHands)
            {
                newCollider.AddComponent<Rigidbody>().useGravity = false;
                newCollider.GetComponent<Rigidbody>().mass = 1f;
                newCollider.AddComponent<SurfaceInteractionPhysics>();

                if (!physics)
                {
                    newCollider.AddComponent<InteractionBehaviour>().manager = FindObjectOfType<InteractionManager>();
                    // We don't want the software to recognize a certain finger configuration as grasping, so that it behaves more naturaly. 
                    newCollider.GetComponent<InteractionBehaviour>().moveObjectWhenGrasped = false;
                    newCollider.GetComponent<SphereCollider>().radius = 0.9f;
                }
            }
            if (!useHands)
            {
                if (physics)
                {
                    newCollider.AddComponent<Rigidbody>().useGravity = false;
                    newCollider.GetComponent<Rigidbody>().mass = 100;
                    newCollider.AddComponent<SurfaceInteractionPhysics>();
                    /*!\ PhysicsCursor() applies vector increments to the spheres in the direction of the cursor movement. 
                         Unity's built in physics simulator is responsible for making all the sphere's movement dynamics 
                         under those vector increments. */
                newCollider.AddComponent<PhysicsCursor>();

                }
                if (!physics)
                {
                    // MoveCursor() is responsible for programming all the sphere's movement under cursor control.
                    newCollider.AddComponent<MoveCursor>().enabled = true;
                }

            }
            SphereIDs.Add(newCollider.gameObject.GetInstanceID(), sphereID);
            sphereID += 25;

            // Add new clay object. //
            ClayObject newSphere = clayContainer.addClayObject();
            // sphere primitive type = 1; see claySDF.compute solidType for other shapes.
            newSphere.setPrimitiveType(1);
            newSphere.name = "clay_" + n.ToString();
            newSphere.transform.parent = newCollider.transform;
            newSphere.transform.localPosition = Vector3.zero;
            newSphere.transform.localScale = 0.5f * Vector3.one;
            newSphere.blend = 0.55f;
            newSphere.color = new Color(0.35f, 0.90f, 1, 1);

            x += 2 * sphereScale;

        }

        // save initial positions //

    }


    private void OptimalSurface(int rows, int holes)
    {


        Rigidbody[] surface = clayContainer.GetComponentsInChildren<Rigidbody>(); // has length of number of spheres in material
        for (int s = 0; s < sphereNumber; s++)
        {
            GameObject sphere = surface[s].gameObject;
            Vector3 spherePosition = sphere.transform.localPosition;

            if (holes == 1)
            {


                if ((Math.Abs(spherePosition.x) == xMax && spherePosition.z != -zMax) || spherePosition.z == zMax)
                {
                    sphere.transform.localPosition = new Vector3(spherePosition.x, xMax/2, spherePosition.z);
                }

                if (spherePosition.z == -zMax)
                {
                    sphere.transform.localPosition += new Vector3(0, -(xMax / 2 - Math.Abs(spherePosition.x)), 0);
                }

                if (Math.Abs(spherePosition.z) != zMax && Math.Abs(spherePosition.x) != xMax)
                {
                    sphere.transform.localPosition += new Vector3(0, -(zMax / rows - spherePosition.z/2), 0);
                    sphere.transform.localPosition += new Vector3(0, (Math.Abs(spherePosition.x)), 0);

                    
                    if (sphere.transform.localPosition.y > xMax / 2)
                    {
                        sphere.transform.localPosition += new Vector3(0, xMax/2 - sphere.transform.localPosition.y, 0); 
                    }
        
                }



            }



        }

    }









    private void SphereY(int rows)
    {

        float sphereScale = 0.25f;
        // EDIT: Initial sphere position (we want the surface centered). //
        float x = -sphereScale * (rows - 1); // -(sphereScale * Rows) / 2;
        float y = 0.5f;
        float z = -sphereScale * (rows - 1); // -(sphereScale * Rows) / 2;
        int current_row = 0;

        // Create one sphere (n) for each loop iteration. //
        for (int n = 0, r = 0; n < sphereNumber; n++, r++)
        {
            /*!\ EDIT: Change y to create each Level's surface */
            if (holes == 1)
            {
                /*!\ n < rows * rows means we are still in the main rows by rows grid.
                     Here, the first and last spheres of each row must be higher. */



                if (n < rows * rows)
                {
                    // outer frames up
                    if (r == 0 || r == rows - 1 || n >= (rows * rows) - rows)
                    {
                        y = 1f;
                    }
                    else if (r > 0 && n < (int)(rows / 2))
                    {
                        y -= 0.5f;
                    }
                    else if (r > (int)(rows / 2) && n < rows)
                    {
                        y += 0.5f;
                    }
                    // center front down
                    if (n == (int)(rows / 2))
                    {
                        y = -1f;
                    }
                    if (r > 0 && r < (int)(rows / 2) && n >= rows)
                    {
                        y -= 0.5f - 0.5f * current_row / rows;
                        Debug.Log("current row " + current_row);


                    }

                    if (r > (int)(rows / 2) && r < rows && n >= rows)
                    {
                        y += 0.25f;// - 0.5f * (1 - current_row/rows);
                        Debug.Log("current row " + current_row);
                        Debug.Log(y);
                    }

                }

                // start connection spheres
                if (n == rows * rows)
                {
                    y = 0;
                }


            }



        }
    }
}
