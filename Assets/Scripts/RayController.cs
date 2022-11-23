using System;
using UnityEngine;

public class RayController : MonoBehaviour
{
    
    private static Material normalRay, collidingRay;
    [NonSerialized]
    public float hitDistance;
    private Vector3 rayDirection;

    private void Start()
    {
        normalRay = Resources.Load("normalRay", typeof(Material)) as Material;
        collidingRay = Resources.Load("collidingRay", typeof(Material)) as Material;
        GetComponent<MeshRenderer>().material = normalRay;
        rayDirection = transform.Find("end").transform.position - transform.Find("start").transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Train")
        {
            GetComponent<MeshRenderer>().material = collidingRay;

            Ray ray = new Ray(transform.Find("start").transform.position, rayDirection);
            float distance;
            if(other.bounds.IntersectRay(ray, out distance))
            {
                //Vector3 point = ray.origin + ray.direction * distance;
                hitDistance = distance;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Train")
        {
            hitDistance = float.MaxValue;
            GetComponent<MeshRenderer>().material = normalRay;
        }
      
    }
}
