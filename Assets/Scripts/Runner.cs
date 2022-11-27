using UnityEngine;

namespace DefaultNamespace
{
    public class Runner
    {
        public GameObject mesh;
        public RunnerController controller;
        public int index;
        public MeshRenderer headRenderer;

        public Runner(GameObject mesh, int index)
        {
            this.mesh = mesh;
            this.index = index;
            controller = mesh.GetComponent<RunnerController>();
            headRenderer = mesh.transform.Find("head").GetComponent<MeshRenderer>();

        }
    }
}