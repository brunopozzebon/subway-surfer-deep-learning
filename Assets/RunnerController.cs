using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    public NeuralNetwork brain;

    [NonSerialized]
    public Transform leftFoot;
    [NonSerialized]
    public Transform rightFoot;
    [NonSerialized]
    public Transform wrist;

    private GameObject leftLegJoint;
    private GameObject rightLegJoint;
    private GameObject leftKneeJoint;
    private GameObject rightKneeJoint;

    private const float ANGLE_INCREMENT = 0.2f;

    void Start()
    {
        wrist = gameObject.transform.Find("head").gameObject.transform;
        rightFoot = gameObject.transform.Find("rightLeg/downRight/rightFoot").gameObject.transform;
        leftFoot = gameObject.transform.Find("leftLeg/downLeft/leftFoot").gameObject.transform;
        
        rightLegJoint = gameObject.transform.Find("rightLeg").gameObject;
        leftLegJoint = gameObject.transform.Find("leftLeg").gameObject;
        leftKneeJoint = gameObject.transform.Find("leftLeg/downLeft").gameObject;
        rightKneeJoint = gameObject.transform.Find("rightLeg/downRight").gameObject;
        
        brain = new NeuralNetwork(
            new int[]
            {
                6,
                6,
                4
            }
        );
    }

    public float getRunnerPosition()
    {
        return (rightFoot.position.x + leftFoot.position.x) / 2;
    }

    void Update()
    {

        List<float> inputs = new List<float>()
        {
            leftFoot.position.x,
            leftFoot.position.y,
            rightFoot.position.x,
            rightFoot.position.y,
            wrist.position.x,
            wrist.position.y,
        };
        List<float> outputs = NeuralNetwork.feedForward(
            inputs, brain
        );

        float leftLegAngle = outputs[0];
        float rightLegAngle = outputs[1];
        float leftKneeAngle = outputs[2];
        float rightKneeAngle = outputs[3];

        
        
        //LeftLeg
        float leftLegIncrement = leftLegAngle > 0 ? ANGLE_INCREMENT : -ANGLE_INCREMENT;
       
        leftLegJoint.transform.localRotation = Quaternion.Euler(
            leftLegJoint.transform.localRotation.eulerAngles + new Vector3(0, 0, leftLegIncrement)
        );
        
        //RightLeg
        float rightLegIncrement = rightLegAngle > 0 ? ANGLE_INCREMENT : -ANGLE_INCREMENT;
        
        rightLegJoint.transform.localRotation = Quaternion.Euler(
            rightLegJoint.transform.localRotation.eulerAngles + new Vector3(0, 0, rightLegIncrement));

        //LeftKnee
        float leftKneeIncrement = leftKneeAngle > 0 ? ANGLE_INCREMENT : -ANGLE_INCREMENT;
      
        leftKneeJoint.transform.localRotation = Quaternion.Euler(
            leftKneeJoint.transform.localRotation.eulerAngles +new Vector3(0, 0, leftKneeIncrement));

        //RightKnee
        float rightKneeIncrement = rightKneeAngle > 0 ? ANGLE_INCREMENT : -ANGLE_INCREMENT;
        
     
        rightKneeJoint.transform.localRotation = Quaternion.Euler(
            rightKneeJoint.transform.localRotation.eulerAngles+ new Vector3(0, 0,rightKneeIncrement));
        
    }
    
    
}