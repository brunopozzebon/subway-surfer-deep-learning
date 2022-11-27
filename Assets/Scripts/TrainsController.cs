using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrainsController : MonoBehaviour
{

    public GameObject firstBoardsChunk;
    public GameObject secondBoardsChunk;
    public GameObject thirdBoardsChunk;
    
    public Transform firstTrainPosition;
    public Transform secondTrainPosition;
    public Transform thirdTrainPosition;

    public GameObject train;

    public Transform cameraPosition;

    private List<GameObject> trains;

    private const int boardsStep = 172;
    private const int trainStep = 155;
    private const float initialTrainOffset = 100f;

    private const int trainsQuantity = 20;

    private void Start()
    {
        createTrains();
    }

    private void createTrains()
    {
        trains = new List<GameObject>();

        for (int i = 0; i < trainsQuantity; i++)
        {
            float x = getXTrainPosition(i==0);
            float z = (trainStep * i) + initialTrainOffset;
            
            GameObject newTrain = Instantiate(train, new Vector3(x, 0, z), Quaternion.identity);
            newTrain.SetActive(true);
            newTrain.transform.parent = transform;

            trains.Add(newTrain);
        }
    }
    
    public void recreateTrains()
    {
        for (int i = 0; i < trainsQuantity; i++)
        {
            float x = getXTrainPosition(i==0);
            float z = (trainStep * i) + initialTrainOffset;
            trains[i].transform.position = new Vector3(x, 0, z);
        }
    }
    
    private float getXTrainPosition(bool isFirstTrain)
    
    {

        if (isFirstTrain)
        {
            return secondTrainPosition.position.x;
        }
        int randomNumber = Random.Range(0,3);

        switch (randomNumber)
        {
            case 0:
                return firstTrainPosition.position.x;
            case 1:
                return secondTrainPosition.position.x;
            default: 
                return thirdTrainPosition.position.x;
        }
        
    }

    void Update()
    {

        int increment = (int) cameraPosition.position.z / boardsStep;

        firstBoardsChunk.transform.position = new Vector3(0, 0, increment * boardsStep);
        secondBoardsChunk.transform.position = new Vector3(0, 0, increment * boardsStep + boardsStep);
        thirdBoardsChunk.transform.position = new Vector3(0, 0, increment * boardsStep + boardsStep +boardsStep);
        
    }

    
}
