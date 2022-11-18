using UnityEngine;

namespace DefaultNamespace
{
    public class Runner
    {
        public GameObject mesh;
        public bool isDead;
        public int index;
        public RunnerController controller;

        public Runner(GameObject mesh, bool isDead, int index)
        {
            this.mesh = mesh;
            this.isDead = isDead;
            this.index = index;
            this.controller = mesh.GetComponent<RunnerController>();
        }
    }
}