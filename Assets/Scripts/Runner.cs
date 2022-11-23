using UnityEngine;

namespace DefaultNamespace
{
    public class Runner
    {
        public GameObject mesh;
        public RunnerController controller;
        public int index;

        public Runner(GameObject mesh, int index)
        {
            this.mesh = mesh;
            this.index = index;
            controller = mesh.GetComponent<RunnerController>();
        }
    }
}