using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardsController : MonoBehaviour
{

    public GameObject firstChunk;
    public GameObject secondChunk;
    public GameObject thirdChunk;

    public Transform cameraPosition;

    private float step = 172f;
    void Update()
    {

        int increment = (int) cameraPosition.position.z / 172;

        firstChunk.transform.position = new Vector3(0, 0, increment * 172);
        secondChunk.transform.position = new Vector3(0, 0, increment * 172 + 172);
        thirdChunk.transform.position = new Vector3(0, 0, increment * 172 + 172 +172);
        
    }
}
