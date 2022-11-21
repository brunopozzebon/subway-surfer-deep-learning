using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour
{
    
    private static Material normalRay, collidingRay;
    public bool itHits;

    private void Start()
    {
        normalRay = Resources.Load("normalRay", typeof(Material)) as Material;
        collidingRay = Resources.Load("collidingRay", typeof(Material)) as Material;
        GetComponent<MeshRenderer>().material = normalRay;
        itHits = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Train")
        {
            itHits = true;
            GetComponent<MeshRenderer>().material = collidingRay;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Train")
        {
            itHits = false;
            GetComponent<MeshRenderer>().material = normalRay;
        }
      
    }
}
