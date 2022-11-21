using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public GameObject meshRunner;
    public TextMeshProUGUI text;

    private const int RUNNER_QUANTITY = 1;
    private static Material commomMaterial, victoryMaterial;

    private int generation = 1;
    private int killds;

    private static List<Runner> runners = new List<Runner>(RUNNER_QUANTITY);

    public Runner firstRunner;
    
    void Start()
    {
        commomMaterial = Resources.Load("common", typeof(Material)) as Material;
        victoryMaterial = Resources.Load("victory", typeof(Material)) as Material;

        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
            GameObject newRunnerMesh = Instantiate(meshRunner, new Vector3(0, 0, 0), Quaternion.identity);
            newRunnerMesh.SetActive(true);

            Runner newRunner = new Runner(newRunnerMesh, i);
            runners.Add(newRunner);
        }

    }

    void FixedUpdate()
    {
        
        for (int i = 0; i < runners.Count; i++)
        {
            Runner r = runners[i];
            GameObject runnerMesh = r.mesh;

            if (r.controller.itHits)
            {
                Destroy(runnerMesh);
                killds++;
                
                if (killds >= RUNNER_QUANTITY)
                {
                    killds = 0;
                    text.text = (++generation).ToString();
                    saveResult();
                    recreateRunners();
                    break;
                }
                
                if (r.index == firstRunner.index) {
                    findNewFirstRunner();
                }

            }else if (firstRunner == null)
            {
                firstRunner = runners[0];
                runnerMesh.transform.Find("head").GetComponent<MeshRenderer>().material =
                    victoryMaterial;
            }
            else if (firstRunner.index != i && !r.controller.itHits) {
                GameObject firstRunnerMesh = firstRunner.mesh;
                
                if (r.mesh.transform.position.x > firstRunner.mesh.transform.position.x)
                {
                    firstRunnerMesh.transform.Find("head").GetComponent<MeshRenderer>().material =
                        commomMaterial;
                    firstRunner = r;
                    
                    runnerMesh.transform.Find("head").GetComponent<MeshRenderer>().material = victoryMaterial;
                }
                else
                {
                    runnerMesh.transform.Find("head").GetComponent<MeshRenderer>().material = commomMaterial;
                }
            }

            int[] visionRay = r.controller.getRaysPerception();
            
        }
    }

    private void recreateRunners()
    {
        
        RunnerController victoryController = firstRunner.controller;
        NeuralNetwork victoryNetwork = victoryController.brain;

        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
            GameObject newRunnerMesh = Instantiate(meshRunner, new Vector3(0, 0, 0), Quaternion.identity);
            newRunnerMesh.SetActive(true);
            
            Runner newRunner = new Runner(newRunnerMesh, i);
            
            victoryNetwork = victoryController.brain.DeepCopy();

            RunnerController newRunnerController = newRunner.controller;
            newRunnerController.brain = victoryNetwork.mutate(0.05f);

            runners[i] = newRunner;
        }

        firstRunner = null;
    }

    private void findNewFirstRunner()
    {
        float maxValue = float.NegativeInfinity;
        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
            Runner runner = runners[i];
            if (!runner.controller.itHits)
            {
                float characterPosition = firstRunner.mesh.transform.position.z;
                if (characterPosition > maxValue)
                {
                    firstRunner = runner;
                    maxValue = characterPosition;
                }
            }
           
        }
    }

    private void saveResult()
    {
        float characterPosition = firstRunner.mesh.transform.position.z;
        Debug.Log(characterPosition);
    }

    public Vector3 getFirstPositionRunner()
    {
        return firstRunner.mesh.transform.position;
    }
}