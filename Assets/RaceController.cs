using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class RaceController : MonoBehaviour
{
    public GameObject meshRunner;
    public TextMeshProUGUI text;
    public Transform laser;

    private const int RUNNER_QUANTITY = 1;
    private const float DISTANCE_BETWEEN_RUNNERS = 10;
    private static Material commomMaterial, victoryMaterial;

    private int generation = 1;
    private int killds;

    private static List<Runner> runners = new List<Runner>(RUNNER_QUANTITY);

    private const float MIN_HEAD_Y_POSITION = 0.10f;
    public Runner firstRunner;

    void Start()
    {
        commomMaterial = Resources.Load("common", typeof(Material)) as Material;
        victoryMaterial = Resources.Load("victory", typeof(Material)) as Material;

        Vector3 position = new Vector3(0, 0, 0);
        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
            GameObject newRunnerMesh = Instantiate(meshRunner, position, Quaternion.identity);
            newRunnerMesh.SetActive(true);

            Runner newRunner = new Runner(newRunnerMesh, false, i);
            runners.Add(newRunner);
            position.z += DISTANCE_BETWEEN_RUNNERS;
        }
    }

    private void Update()
    {
        for (int i = 0; i < runners.Count; i++)
        {
            Runner r = runners[i];
            GameObject runnerMesh = r.mesh;
            if (runnerMesh == null)
            {
                continue;
            }
            RunnerController rController = r.controller;

            if (rController.wrist.position.y < MIN_HEAD_Y_POSITION ||
                laser.position.x > rController.wrist.position.x)
            {
                Destroy(runnerMesh);
                r.isDead = true;
                killds++;
                
                if (killds >= RUNNER_QUANTITY)
                {
                    killds = 0;
                    text.text = (++generation).ToString();
                    laser.position = new Vector3(-10, 0.35f, 0);
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
            }
            else
            {
                if (r.isDead)
                    continue;
                
                RunnerController comparableController = r.controller;
                
                GameObject firstRunnerMesh = firstRunner.mesh;
                RunnerController firstRunnerController = firstRunner.controller;
                if (comparableController.getRunnerPosition() > firstRunnerController.getRunnerPosition())
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
        }
    }

    private void recreateRunners()
    {
        Vector3 position = new Vector3(0, 0, 0);

        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
            GameObject newRunnerMesh = Instantiate(meshRunner, position, Quaternion.identity);
            newRunnerMesh.SetActive(true);
            
            Runner newRunner = new Runner(newRunnerMesh, false, i);
            
            RunnerController victoryController = firstRunner.controller;
            NeuralNetwork victoryNetwork = victoryController.brain.DeepCopy();

            RunnerController newRunnerController = newRunner.controller;
            newRunnerController.brain = victoryNetwork.mutate(0.00f);
            
            runners[i] = newRunner;
            position.z += DISTANCE_BETWEEN_RUNNERS;
        }

        firstRunner = null;
    }

    private void findNewFirstRunner()
    {

        float maxValue = float.NegativeInfinity;
        for (int i = 0; i < RUNNER_QUANTITY; i++)
        {
            Runner runner = runners[i];
            if (!runner.isDead)
            {
                RunnerController controller = runner.controller;
                float characterPosition = controller.wrist.position.y;
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
        RunnerController controller = firstRunner.mesh.GetComponent<RunnerController>();
        float characterPosition = controller.wrist.position.y;
        Debug.Log(characterPosition);
    }
}