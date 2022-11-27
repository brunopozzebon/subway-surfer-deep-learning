using System.Collections;
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
    public TrainsController trainsController;

    private const int RUNNER_QUANTITY = 20;

    private int generation = 1;
    private int killds;

    private static List<Runner> runners = new List<Runner>(RUNNER_QUANTITY);

    public Runner firstRunner;
    private bool firstPass = true;
    private float maxMutationFactor = 0.3f;
    private float minMaxMutationFactor = 0.08f;

    void Start()
    {
        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
            GameObject newRunnerMesh = Instantiate(meshRunner, new Vector3(0, 0, 0), Quaternion.identity);
            newRunnerMesh.SetActive(true);
            Runner newRunner = new Runner(newRunnerMesh, i);
            runners.Add(newRunner);
        }

        firstRunner = runners[0];
        firstRunner.mesh.transform.Find("head").GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        for (int i = 0; i < runners.Count; i++)
        {
            Runner r = runners[i];
            GameObject runnerMesh = r.mesh;
            RunnerController runnerController = r.controller;

            if (runnerController.isDestroyed)
            {
                continue;
            }

            if (runnerController.itHits)
            {
                runnerController.isDestroyed = true;
                StartCoroutine(destroyMesh(runnerMesh));
                killds++;

                if (killds >= RUNNER_QUANTITY)
                {
                    killds = 0;
                    text.text = (++generation).ToString();
                    recreateRunners();
                    trainsController.recreateTrains();
                    firstPass = true;
                    break;
                }

                if (r.index == firstRunner.index)
                {
                    findNewFirstRunner();
                }
            }
            else
            {
                runnerController.update(i == firstRunner.index);
                
                if (firstRunner.index != i)
                {
                    GameObject firstRunnerMesh = firstRunner.mesh;

                    if (runnerMesh.transform.position.z
                        > firstRunnerMesh.transform.position.z)
                    {
                        firstRunner.headRenderer.enabled = false;
                        firstRunner = r;
                        r.headRenderer.enabled = true;
                    }
                    else
                    {
                        r.headRenderer.enabled = false;
                    }
                }
            }
        }
    }

    private void saveBrain()
    {
        NeuralNetwork brain = firstRunner.controller.brain;
        string json = JsonConvert.SerializeObject(brain);
        File.WriteAllText(@"D:\runners.json", json);
        firstPass = false;
    }

    private void recreateRunners()
    {
        RunnerController victoryController = firstRunner.controller;
        NeuralNetwork victoryNetwork = victoryController.brain;
        runners = new List<Runner>();

        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
           // float mutationAmount = 0;
           float mutationAmount = 0.2f;
            //if (i < RUNNER_QUANTITY - 1)
            //{
            //    mutationAmount = MathUtils.remap(i, 0, RUNNER_QUANTITY, 0, maxMutationFactor);
            //}

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
        firstRunner.headRenderer.enabled = true;
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
                else if (runner.mesh.transform.position.z > maxValue)
                {
                    firstRunner = runner;
                    maxValue = runner.mesh.transform.position.z;
                }
            }
        }
    }

    public Vector3 getFirstPositionRunner()
    {
        return firstRunner.mesh.transform.position;
    }

    IEnumerator destroyMesh(GameObject runner)
    {
        yield return new WaitForSeconds(2f);
        Destroy(runner);
    }
}