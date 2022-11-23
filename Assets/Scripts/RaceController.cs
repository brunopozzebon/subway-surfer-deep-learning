using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public GameObject meshRunner;
    public TextMeshProUGUI text;

    private const int RUNNER_QUANTITY = 25;
    private static Material commomMaterial, victoryMaterial;

    private int generation = 1;
    private int killds = 0;

    private static List<Runner> runners = new List<Runner>(RUNNER_QUANTITY);

    public Runner firstRunner;
    private bool firstPass = true;
    private float maxMutationFactor = 0.3f;
    private float minMaxMutationFactor = 0.08f;
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

        firstRunner = runners[0];
        firstRunner.mesh.transform.Find("head").GetComponent<MeshRenderer>().material = victoryMaterial;
    }

    void Update()
    {
        for (int i = 0; i < runners.Count; i++)
        {
            Runner r = runners[i];
            GameObject runnerMesh = r.mesh;
            RunnerController runnerController = r.controller;

            if (!runnerController.isIsDestroyed)
            {
                if (runnerController.itHits)
                {
                    runnerController.isIsDestroyed = true;
                    Destroy(runnerMesh);
                    killds++;

                    if (killds >= RUNNER_QUANTITY)
                    {
                        killds = 0;
                        text.text = (++generation).ToString();
                         recreateRunners();
                         firstPass = true;
                        break;
                    }

                    if (r.index == firstRunner.index)
                    {
                        findNewFirstRunner();
                    }
                }
                else if (firstRunner.index != i && !runnerController.itHits)
                {
                    GameObject firstRunnerMesh = firstRunner.mesh;

                    if (runnerMesh.transform.position.z 
                        > firstRunnerMesh.transform.position.z)
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

                if (generation==2 && firstPass)
                {
                    List<NeuralNetwork> n = new List<NeuralNetwork>();
                    for (int j = 0; j < RUNNER_QUANTITY; j++)
                    {
                        n.Add(runners[0].controller.brain);
                        
                    }
                    string json = JsonConvert.SerializeObject(n);
                    File.WriteAllText(@"D:\runners.json", json);
                    firstPass = false;
                }

                runnerController.setIfRaysAreVisible(i == firstRunner.index);
                List<float> visionRay = runnerController.getRaysPerception();

                List<float> output = NeuralNetwork.feedForward(
                    visionRay, runnerController.brain
                );

                r.controller.onGoLeft(output[0] > 0.5);
                r.controller.onGoRight(output[1] > 0.5);
               
            }
        }
    }

    private void recreateRunners()
    {
        RunnerController victoryController = firstRunner.controller;
        NeuralNetwork victoryNetwork = victoryController.brain;
        runners = new List<Runner>();
        
        
        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
            float mutationAmount = 0;

            if (i != RUNNER_QUANTITY - 1)
            {
                mutationAmount =  MathUtils.remap(i, 0, RUNNER_QUANTITY, 0, maxMutationFactor);
            }
           
            GameObject newRunnerMesh = Instantiate(meshRunner, new Vector3(0, 0, 0), Quaternion.identity);
            newRunnerMesh.SetActive(true);

            Runner newRunner = new Runner(newRunnerMesh, i);

            NeuralNetwork newVictoryNetwork = victoryNetwork.DeepCopy().mutate(mutationAmount);
            newRunner.controller.brain = newVictoryNetwork;

            runners.Add(newRunner);
        }

        if (maxMutationFactor - 0.05f > minMaxMutationFactor)
        {
            maxMutationFactor -= 0.05f;
        }
        


        firstRunner = runners[0];
        firstRunner.mesh.transform.Find("head").GetComponent<MeshRenderer>().material = victoryMaterial;
    }

    private void findNewFirstRunner()
    {
        float maxValue = float.NegativeInfinity;

        for (int i = 1; i < RUNNER_QUANTITY; i++)
        {
            Runner runner = runners[i];
            if (!runner.controller.itHits)
            {
                if (firstRunner == null)
                {
                    firstRunner = runner;
                    maxValue = runner.mesh.transform.position.z;
                }
                else
                {
                    float runnerPosition = runner.mesh.transform.position.z;
                    if (runnerPosition > maxValue)
                    {
                        firstRunner = runner;
                        maxValue = runnerPosition;
                    }
                }
            }
        }
    }

    public Vector3 getFirstPositionRunner()
    {
        if (firstRunner.mesh != null)
        {
            return firstRunner.mesh.transform.position;
        }

        return default(Vector3);
    }
}