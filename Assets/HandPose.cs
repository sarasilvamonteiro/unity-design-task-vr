using UnityEngine;
using System.Data;
using Leap;
using Leap.Unity;
using System;
using Leap.Unity.HandsModule;

public class HandPose : MonoBehaviour
{
    /// <summary>
    /// Read hand tracking data and visualize it. 
    /// </summary>

    public LeapProvider leapProvider;
    // Filename:
    public string data;
    public int subject;
    public int trial;
    public int holes;
    
    private string Lpath;
    private string Rpath;
    private DataSet LhandData;
    private DataSet RhandData;
    public GameObject LHand;
    public GameObject RHand;

    public int timeframe;       // First timeframe of movement.
    public float timestamp;       // Fisrt timestamps of movement.
    public int range;           // Range of timeframes of movement.
    public bool autoPlay;      // Use autoplay.
    private int t_0;            // Track first timeframe to allow runtime changes.
    private float ts_0 = 0;         // Track first timestamps to allow runtime changes.
    private int t;              // Timeframe runtime advancer.
    private float delay = 25;   // Play delay.
    int timer = 0;              // Play delay timer.
    private int nr_timeframes;
    private float max_timestamp;

    /*  public Transform palm;
      public Transform wrist;

      public Transform indexMetacarpal;
      public Transform middleMetacarpal;
      public Transform pinkyMetacarpal;
      public Transform ringMetacarpal;
      public Transform thumbMetacarpal;

      public Transform indexProximal;
      public Transform middleProximal;
      public Transform pinkyProximal;
      public Transform ringProximal;
      public Transform thumbProximal;

      public Transform indexIntermediate;
      public Transform middleIntermediate;
      public Transform pinkyIntermediate;
      public Transform ringIntermediate;
      public Transform thumbIntermediate;

      public Transform indexDistal;
      public Transform middleDistal;
      public Transform pinkyDistal;
      public Transform ringDistal;

      public Transform indexTip;
      public Transform middleTip;
      public Transform pinkyTip;
      public Transform ringTip;
      public Transform thumbTip;


  */


    void Start()
    {
        // Import L hand:
        Lpath = @"C:\Users\Sara\Desktop\InverseDesign\Data_" + data + @"\subject_" + subject.ToString() + @"\" + holes.ToString() + "_holes_1_flows_left_hand_" + trial.ToString();
        LhandData = new DataSet();
        LhandData.ReadXml(Lpath);
        // Import R hand:
        Rpath = @"C:\Users\Sara\Desktop\InverseDesign\Data_" + data + @"\subject_" + subject.ToString() + @"\" + holes.ToString() + "_holes_1_flows_right_hand_" + trial.ToString();
        RhandData = new DataSet();
        RhandData.ReadXml(Rpath);

        nr_timeframes = RhandData.Tables[0].Rows.Count - 1;
        max_timestamp = Convert.ToSingle(RhandData.Tables[0].Rows[nr_timeframes][3]);

        /*for (int t = timeframe; t < timeframe + range; t++)
        {
            UpdateHand(hand: LHand, handData: LhandData, timeframe: t);
            UpdateHand(hand: RHand, handData: RhandData, timeframe: t);
        }*/

    }




    void Update()
    {

        timeframe = Math.Max(0, timeframe);
        timeframe = Math.Min(timeframe, nr_timeframes);
        timestamp = Math.Max(0, timestamp);
        timestamp = Math.Min(timestamp, max_timestamp);



        if (timestamp != ts_0)
        {
            timeframe = GetTimeframe(RhandData, timestamp);
            ts_0 = timestamp;
        }



        if (timeframe != t_0)
        {
            t_0 = timeframe;
            t = timeframe;
        }

        if (timeframe <= t)
        {
            
            if (autoPlay)
            {
                timer += 1;

                if (timer > delay && t < timeframe + range)
                {
                    UpdateHand(hand: LHand, handData: LhandData, timeframe: t);
                    UpdateHand(hand: RHand, handData: RhandData, timeframe: t);
                    timer = 0;
                    t++;
                    t = Math.Max(0, t);
                    t = Math.Min(t, nr_timeframes);

                }
            }

            else
            {
                UpdateHand(hand: LHand, handData: LhandData, timeframe: timeframe);
                UpdateHand(hand: RHand, handData: RhandData, timeframe: timeframe);
            }

            
            
        }

    }




    private void UpdateHand(GameObject hand, DataSet handData, int timeframe)
    {
/*

        wrist = hand.GetComponentsInChildren<Transform>()[1];
        palm = hand.GetComponentsInChildren<Transform>()[2];
        

        indexMetacarpal = palm.GetComponentsInChildren<Transform>()[1];
        middleMetacarpal = palm.GetComponentsInChildren<Transform>()[6];
        pinkyMetacarpal = palm.GetComponentsInChildren<Transform>()[11];
        ringMetacarpal = palm.GetComponentsInChildren<Transform>()[16];
        thumbMetacarpal = palm.GetComponentsInChildren<Transform>()[21];

        indexProximal = indexMetacarpal.GetComponentsInChildren<Transform>()[1];
        middleProximal = middleMetacarpal.GetComponentsInChildren<Transform>()[1];
        pinkyProximal = pinkyMetacarpal.GetComponentsInChildren<Transform>()[1];
        ringProximal = ringMetacarpal.GetComponentsInChildren<Transform>()[1];
        thumbProximal = thumbMetacarpal.GetComponentsInChildren<Transform>()[1];

        indexIntermediate = indexProximal.GetComponentsInChildren<Transform>()[1];
        middleIntermediate = middleProximal.GetComponentsInChildren<Transform>()[1];
        pinkyIntermediate = pinkyProximal.GetComponentsInChildren<Transform>()[1];
        ringIntermediate = ringProximal.GetComponentsInChildren<Transform>()[1];
        thumbIntermediate = thumbProximal.GetComponentsInChildren<Transform>()[1];

        indexDistal = indexIntermediate.GetComponentsInChildren<Transform>()[1];
        middleDistal = middleIntermediate.GetComponentsInChildren<Transform>()[1];
        pinkyDistal = pinkyIntermediate.GetComponentsInChildren<Transform>()[1];
        ringDistal = ringIntermediate.GetComponentsInChildren<Transform>()[1];

        indexTip = indexDistal.GetComponentsInChildren<Transform>()[1];
        middleTip = middleDistal.GetComponentsInChildren<Transform>()[1];
        pinkyTip = pinkyDistal.GetComponentsInChildren<Transform>()[1];
        ringTip = ringDistal.GetComponentsInChildren<Transform>()[1];
        thumbTip = thumbIntermediate.GetComponentsInChildren<Transform>()[1];*/

        Transform wrist = hand.GetComponentsInChildren<Transform>()[1];
        Transform palm = hand.GetComponentsInChildren<Transform>()[2];


        Transform indexMetacarpal = palm.GetComponentsInChildren<Transform>()[1];
        Transform middleMetacarpal = palm.GetComponentsInChildren<Transform>()[6];
        Transform pinkyMetacarpal = palm.GetComponentsInChildren<Transform>()[11];
        Transform ringMetacarpal = palm.GetComponentsInChildren<Transform>()[16];
        Transform thumbMetacarpal = palm.GetComponentsInChildren<Transform>()[21];

        Transform indexProximal = indexMetacarpal.GetComponentsInChildren<Transform>()[1];
        Transform middleProximal = middleMetacarpal.GetComponentsInChildren<Transform>()[1];
        Transform pinkyProximal = pinkyMetacarpal.GetComponentsInChildren<Transform>()[1];
        Transform ringProximal = ringMetacarpal.GetComponentsInChildren<Transform>()[1];
        Transform thumbProximal = thumbMetacarpal.GetComponentsInChildren<Transform>()[1];

        Transform indexIntermediate = indexProximal.GetComponentsInChildren<Transform>()[1];
        Transform middleIntermediate = middleProximal.GetComponentsInChildren<Transform>()[1];
        Transform pinkyIntermediate = pinkyProximal.GetComponentsInChildren<Transform>()[1];
        Transform ringIntermediate = ringProximal.GetComponentsInChildren<Transform>()[1];
        Transform thumbIntermediate = thumbProximal.GetComponentsInChildren<Transform>()[1];

        Transform indexDistal = indexIntermediate.GetComponentsInChildren<Transform>()[1];
        Transform middleDistal = middleIntermediate.GetComponentsInChildren<Transform>()[1];
        Transform pinkyDistal = pinkyIntermediate.GetComponentsInChildren<Transform>()[1];
        Transform ringDistal = ringIntermediate.GetComponentsInChildren<Transform>()[1];

        Transform indexTip = indexDistal.GetComponentsInChildren<Transform>()[1];
        Transform middleTip = middleDistal.GetComponentsInChildren<Transform>()[1];
        Transform pinkyTip = pinkyDistal.GetComponentsInChildren<Transform>()[1];
        Transform ringTip = ringDistal.GetComponentsInChildren<Transform>()[1];
        Transform thumbTip = thumbIntermediate.GetComponentsInChildren<Transform>()[1];


        Vector3 wrist_offset = palm.position - wrist.position;
        Vector3 wrist_rot_offset = palm.eulerAngles - wrist.eulerAngles;

        wrist.position = JointPosition(handData, 1, timeframe) - wrist_offset;
        wrist.eulerAngles = JointPosition(handData, 2, timeframe) - wrist_rot_offset;

        palm.position = JointPosition(handData, 1, timeframe);
        palm.eulerAngles = JointPosition(handData, 2, timeframe);

        thumbMetacarpal.position = JointPosition(handData, 3, timeframe);
        indexMetacarpal.position = JointPosition(handData, 4, timeframe);
        middleMetacarpal.position = JointPosition(handData, 5, timeframe);
        ringMetacarpal.position = JointPosition(handData, 6, timeframe);
        pinkyMetacarpal.position = JointPosition(handData, 7, timeframe);

        thumbProximal.position = JointPosition(handData, 8, timeframe);
        indexProximal.position = JointPosition(handData, 9, timeframe);
        middleProximal.position = JointPosition(handData, 10, timeframe);
        ringProximal.position = JointPosition(handData, 11, timeframe);
        pinkyProximal.position = JointPosition(handData, 12, timeframe);

        thumbIntermediate.position = JointPosition(handData, 13, timeframe);
        indexIntermediate.position = JointPosition(handData, 14, timeframe);
        middleIntermediate.position = JointPosition(handData, 15, timeframe);
        ringIntermediate.position = JointPosition(handData, 16, timeframe);
        pinkyIntermediate.position = JointPosition(handData, 17, timeframe);

        indexDistal.position = JointPosition(handData, 19, timeframe);
        middleDistal.position = JointPosition(handData, 20, timeframe);
        ringDistal.position = JointPosition(handData, 21, timeframe);
        pinkyDistal.position = JointPosition(handData, 22, timeframe);

        thumbTip.position = JointPosition(handData, 23, timeframe);
        indexTip.position = JointPosition(handData, 24, timeframe);
        middleTip.position = JointPosition(handData, 25, timeframe);
        ringTip.position = JointPosition(handData, 26, timeframe);
        pinkyTip.position = JointPosition(handData, 27, timeframe);

        /*        indexMetacarpal.eulerAngles = JointPosition(handData, 2, timeframe);
                middleMetacarpal.eulerAngles = JointPosition(handData, 2, timeframe);
                ringMetacarpal.eulerAngles = JointPosition(handData, 2, timeframe);
                pinkyMetacarpal.eulerAngles = JointPosition(handData, 2, timeframe);*/

        /*   indexDistal.eulerAngles = JointPosition(handData, 29, timeframe);
           middleDistal.eulerAngles = JointPosition(handData, 30, timeframe);
           ringDistal.eulerAngles = JointPosition(handData, 31, timeframe);
           pinkyDistal.eulerAngles = JointPosition(handData, 32, timeframe);*/
        /*
                thumbTip.eulerAngles = JointPosition(handData, 28, timeframe);
                indexTip.eulerAngles = JointPosition(handData, 29, timeframe);
                middleTip.eulerAngles = JointPosition(handData, 30, timeframe);
                ringTip.eulerAngles = JointPosition(handData, 31, timeframe);
                pinkyTip.eulerAngles = JointPosition(handData, 32, timeframe);*/


    }



    private int GetTimeframe(DataSet handData, float timestamp)
    {

        DataTable timestamps = handData.Tables[0];
        
        // Loop through each row to find the row where the column value is 200
        int foundTimeframe = 0;
        
        foreach (DataRow row in timestamps.Rows)
        {

            if (Math.Round(timestamp, 1) <= Math.Round(Convert.ToSingle(row[3]), 1) + 0.1f && Math.Round(timestamp, 1) >= Math.Round(Convert.ToSingle(row[3]), 1) - 0.1f)
            {
                Debug.Log("Timeframe found.");
                break;
            }
            foundTimeframe ++;
        }
        return foundTimeframe;
    }


    private Vector3 JointPosition(DataSet handData, int joint, int timeframe)
    {

        DataTable jointPose = handData.Tables[joint];
        Debug.Log("Column: " + jointPose.TableName);
        DataRow rowData = jointPose.Rows[timeframe];

        object xvalue = rowData[jointPose.Columns[0].ColumnName];
        object yvalue = rowData[jointPose.Columns[1].ColumnName];
        object zvalue = rowData[jointPose.Columns[2].ColumnName];
        float x = Convert.ToSingle(xvalue);
        float y = Convert.ToSingle(yvalue);
        float z = Convert.ToSingle(zvalue);

        return new Vector3(x, y, z);
    }


}
