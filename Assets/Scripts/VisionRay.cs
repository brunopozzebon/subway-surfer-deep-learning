using UnityEngine;

namespace DefaultNamespace
{
    public class VisionRay
    {
        public GameObject mesh;
        public RayController controller;

        public VisionRay(GameObject mesh)
        {
            this.mesh = mesh;
            this.controller = mesh.GetComponent<RayController>();
        }
    }
}