using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class RunnerController : MonoBehaviour
{
    [NonSerialized] public NeuralNetwork brain;
    [NonSerialized] public List<VisionRay> rays = new List<VisionRay>();
    [NonSerialized] public bool itHits, isDestroyed;

    public GameObject rayObject;
    public float zVelocity;
    public float sideVelocity;

    private int raysQuantity = 13;
    private bool goRight, goLeft;
    private float RUNNER_ANGLE_VIEW = 260f;

    [NonSerialized] public Animator animator;

    private Vector3 rightCollision = new Vector3(2f, 1.87f, 0.51f);
    private Vector3 leftCollision = new Vector3(-2f, 1.87f, 0.51f);

    void Start()
    {
        itHits = false;

        float angleIncrement = (RUNNER_ANGLE_VIEW / raysQuantity);

        for (int i = 0; i < raysQuantity; i++)
        {
            GameObject newRay = Instantiate(rayObject, new Vector3(0, 4, 0), Quaternion.Euler(
                90, (i * angleIncrement) - (130f - (angleIncrement / 2)), 0));
            newRay.SetActive(true);
            newRay.transform.parent = transform;

            Vector3 transformIncrement = newRay.transform.rotation
                                         * new Vector3(0, 30, 0);
            newRay.transform.position += transformIncrement;

            VisionRay visionRay = new VisionRay(newRay);
            rays.Add(visionRay);
        }

        animator = transform.Find("runnerMesh").gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (itHits)
        {
            return;
        }

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

        float leftOffset = goLeft ? -sideVelocity : 0;
        float rightOffset = goRight ? sideVelocity : 0;

        float xOffSet = leftOffset + rightOffset;

        transform.position = new Vector3(
            transform.position.x + xOffSet, transform.position.y, transform.position.z + zVelocity
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Train") && !itHits)
        {
            itHits = true;
            Vector3 collisionVector = collision.contacts[0].point - transform.position;

            if (approximately(collisionVector, rightCollision, 0.2f) ||
                approximately(collisionVector, leftCollision, 0.2f))
            {
                animator.SetBool("fallingFromWall", true);
            }
            else
            {
                animator.SetBool("wallCrash", true);
            }
        }
    }

    public void onGoRight(bool isGoingToRight)
    {
        goRight = isGoingToRight;
    }

    public void onGoLeft(bool isGoingToLeft)
    {
        goLeft = isGoingToLeft;
    }

    public List<float> getRaysPerception()
    {
        List<float> perception = new List<float>(rays.Count);

        for (int i = 0; i < rays.Count; i++)
            perception.Add(rays[i].controller.hitDistance);

        return perception;
    }

    public void setIfRaysAreVisible(bool isVisible)
    {
        for (int i = 0; i < rays.Count; i++)
            if (!itHits)
                rays[i].renderer.enabled = isVisible;
    }

    public void update(bool ifTheFirstRunner)
    {
        setIfRaysAreVisible(ifTheFirstRunner);
        List<float> visionRay = getRaysPerception();

        List<float> output = NeuralNetwork.feedForward(
            visionRay, brain
        );

        bool goToLeft = output[0] > 0.5;
        bool goToRight = output[1] > 0.5;

        //bool goToLeft = Input.GetKey(KeyCode.A);
        //bool goToRight = Input.GetKey(KeyCode.D);

        onGoLeft(goToLeft);
        onGoRight(goToRight);

        animator.SetBool("goRight", !goToLeft && goToRight);
        animator.SetBool("goLeft", goToLeft && !goToRight);
        animator.SetBool("goForward", (goToRight && goToLeft) ||
                                      (!goToRight && !goToLeft));
    }

    public bool approximately(Vector3 me, Vector3 other, float allowedDifference)
    {
        var dx = me.x - other.x;
        if (Mathf.Abs(dx) > allowedDifference)
            return false;

        var dy = me.y - other.y;
        if (Mathf.Abs(dy) > allowedDifference)
            return false;

        var dz = me.z - other.z;

        return Mathf.Abs(dz) <= allowedDifference;
    }
}