using UnityEngine;

namespace DefaultNamespace
{
    public class VisionRay
    {
        public RayController controller;
        public MeshRenderer renderer;
        
        public VisionRay(GameObject mesh)
        {
            this.controller = mesh.GetComponent<RayController>();
            renderer = mesh.GetComponent<MeshRenderer>();
        }
    }
}