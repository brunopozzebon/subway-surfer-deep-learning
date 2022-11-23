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
    [NonSerialized]
    public bool itHits, isIsDestroyed;
    
    public GameObject rayObject;
    public int raysQuantity;
    public float zVelocity;
    public float sideVelocity;
    
    private bool goRight, goLeft;
    private float RUNNER_ANGLE_VIEW = 220f;
    
    void Start()
    {
        itHits = false;

        float angleIncrement = (RUNNER_ANGLE_VIEW / raysQuantity) ;

        for (int i = 0; i < raysQuantity; i++)
        {
            GameObject newRay = Instantiate(rayObject, new Vector3(0, 4, 0), Quaternion.Euler(
                90, (i * angleIncrement) - (110f - (angleIncrement/2)),0));
            newRay.SetActive(true);
           
            newRay.transform.parent = transform;

            Vector3 transformIncrement = newRay.transform.rotation
                                         * new Vector3(0,30,0);
                newRay.transform.position += transformIncrement;
            
            VisionRay visionRay = new VisionRay(newRay);
            rays.Add(visionRay);
        }
    }
    
    void Update()
    {
        if (!itHits)
        {
            if (brain == null)
            {
                brain = new NeuralNetwork(
                    new int[]
                    {
                        raysQuantity,
                        6,
                        2
                    }
                );
            }
            float leftOffset = goRight ? sideVelocity : 0;
            float rightOffset = goLeft ? -sideVelocity : 0;
            
            float xOffSet = leftOffset + rightOffset;
            
            transform.position = new Vector3(
                transform.position.x + xOffSet, transform.position.y,transform.position.z
                                                                     + zVelocity
                );
        }
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

    public List<float> getRaysPerception()
    {
        List<float> perception = new List<float>(rays.Count);

        for (int i = 0; i < rays.Count; i++)
        {
            perception.Add(rays[i].controller.hitDistance);
        }

        return perception;
    }

    public void setIfRaysAreVisible(bool isVisible)
    {
        
        for (int i = 0; i < rays.Count; i++)
        {
            if (!itHits)
            {
                rays[i].mesh.GetComponent<MeshRenderer>().enabled = isVisible;
            }
           
        }
    }
}