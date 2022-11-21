using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public RaceController raceController;
    public Vector3 cameraRotationOffset;
    public Vector3 cameraPositionOffset;
    void Update()
    {
        Vector3 firstPositionRunner =  raceController.getFirstPositionRunner();
        this.transform.position = firstPositionRunner + cameraPositionOffset;
       
        this.transform.LookAt(firstPositionRunner);
        this.transform.rotation = Quaternion.Euler(cameraRotationOffset + this.transform.rotation.eulerAngles);
    }
}
