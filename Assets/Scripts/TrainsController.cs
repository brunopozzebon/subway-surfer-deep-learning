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
    public RaceController raceController;

    public GameObject train;

    public Transform cameraPosition;

    private List<GameObject> trains = new List<GameObject>();

    private const int boardsStep = 172;
    private const int trainStep = 155;
    private const float initialTrainOffset = 100f;

    private const int trainsQuantity = 4;

    private int trainOffsetIndex = 0;

    private int trainOffsetMargin = 100;
    private float totalLineSize = 0;

    private void Start()
    {
        createTrains();
    }

    private void createTrains()
    {
        
        int lastXIndex = getXTrainIndex(true, -1);
        float z = 0;
        for (int i = 0; i < trainsQuantity; i++)
        {
            if (i != 0)
                lastXIndex = getXTrainIndex(false, lastXIndex);

            float x = getXTrainPosition(lastXIndex);
            z = (trainStep * i) + initialTrainOffset;

            GameObject newTrain = Instantiate(train, new Vector3(x, 0, z), Quaternion.identity);
            newTrain.SetActive(true);
            newTrain.transform.parent = transform;
            trains.Add(newTrain);
        }
        totalLineSize = z + trainStep - initialTrainOffset;
    }

    public void recreateTrains()
    {
        trainOffsetIndex = 0;
        int lastXIndex = getXTrainIndex(true, -1);
        for (int i = 0; i < trainsQuantity; i++)
        {
            if (i != 0)
                lastXIndex = getXTrainIndex(false, lastXIndex);

            float x = getXTrainPosition(lastXIndex);
            float z = (trainStep * i) + initialTrainOffset;
            trains[i].transform.position = new Vector3(x, 0, z);
        }
    }

    void Update()
    {
        int increment = (int)cameraPosition.position.z / boardsStep;

        firstBoardsChunk.transform.position = new Vector3(0, 0, increment * boardsStep);
        secondBoardsChunk.transform.position = new Vector3(0, 0, increment * boardsStep + boardsStep);
        thirdBoardsChunk.transform.position =new Vector3(0, 0, increment * boardsStep + boardsStep + boardsStep);

        Vector3 firstPlacePosition = raceController.getFirstPositionRunner();

        if (firstPlacePosition != null &&
            firstPlacePosition.z > trainOffsetMargin + trains[trainOffsetIndex].transform.position.z)
        {
            int index = getXTrainIndex(false, trainOffsetIndex > 0 ? trainOffsetIndex -1 :
                trains.Count -1);
            float newIndex = getXTrainPosition(index);
            
            trains[trainOffsetIndex].transform.position = new Vector3(newIndex, 0, 
                
                trains[trainOffsetIndex].transform.position.z + totalLineSize);
            trainOffsetIndex = (trainOffsetIndex + 1) % trains.Count;
        }
    }
    
    private int getXTrainIndex(bool isFirstTrain, int lastIndex)

    {
        if (isFirstTrain)
            return 1;

        int randomIndex = Random.Range(0, 3);

        while (true)
        {
            if (randomIndex != lastIndex)
                break;
            randomIndex = Random.Range(0, 3);
        }

        return randomIndex;
    }

    private float getXTrainPosition(int index)
    {
        switch (index)
        {
            case 0:
                return firstTrainPosition.position.x;
            case 1:
                return secondTrainPosition.position.x;
            default:
                return thirdTrainPosition.position.x;
        }
    }
}