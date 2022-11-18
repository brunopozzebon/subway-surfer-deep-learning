
using DefaultNamespace;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RaceController raceController;
    void Update()
    {
       Runner firstRunner =  raceController.firstRunner;

        if (firstRunner!=null)
        {
//            Vector3 firstRunnerPosition = firstRunner.runner.transform.Find("head").gameObject.transform.position;
            
        //    transform.position = firstRunnerPosition + new Vector3(10,10,10);
         //   transform.LookAt(firstRunnerPosition);
        }
        
    }
}
