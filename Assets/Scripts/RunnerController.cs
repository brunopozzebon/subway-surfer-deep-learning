using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    [NonSerialized]
    public NeuralNetwork brain;
    [NonSerialized]
    public List<VisionRay> rays = new List<VisionRay>();

    public GameObject rayObject;

    public int raysQuantity;

    public float zVelocity;
    public bool itHits;
    private bool goRight, goLeft;
    public float sideVelocity;

    private float RUNNER_ANGLE_VIEW = 160f;
    
    void Start()
    {

        float angleIncrement = (RUNNER_ANGLE_VIEW / raysQuantity) ;

        for (int i = 0; i < raysQuantity; i++)
        {
            GameObject newRay = Instantiate(rayObject, new Vector3(0, 8, 0), Quaternion.Euler(
                90, (i * angleIncrement) - (90f - angleIncrement),0));
            newRay.SetActive(true);
           
            newRay.transform.parent = transform;

            Vector3 transformIncrement = newRay.transform.rotation
                                         * new Vector3(0,10,0);
                newRay.transform.position += transformIncrement;
            
            VisionRay visionRay = new VisionRay(newRay);
            rays.Add(visionRay);
        }

        brain = new NeuralNetwork(
            new int[]
            {
                9,
                9,
                12
            }
        );
    }
    
    void Update()
    {
        if (!itHits)
        {
            float leftOffset = goRight ? sideVelocity : 0;
            float rightOffset = goLeft ? -sideVelocity : 0;
            
            float xOffSet = leftOffset + rightOffset;
            
            transform.position = new Vector3(
                transform.position.x + xOffSet, transform.position.y,transform.position.z + zVelocity);
        }
        // goLeft = Input.GetKey(KeyCode.A);
       // goRight = Input.GetKey(KeyCode.D);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Train")
        {
            this.itHits = true;
        }
    }

    public void onGoRight(bool isGoingToRight)
    {
        goRight = isGoingToRight;
    }

    public void onGoLeft(bool isGoingToLeft)
    {
        goLeft  = isGoingToLeft;
    }

    public int[] getRaysPerception()
    {
        int[] perception = new int[rays.Count];

        for (int i = 0; i < rays.Count; i++)
        {
            perception[i] = rays[i].controller.itHits? 0 :1;
        }

        return perception;
    }
}