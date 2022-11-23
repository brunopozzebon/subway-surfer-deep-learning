using UnityEngine;

public class CameraController : MonoBehaviour
{

    public RaceController raceController;
    public Vector3 cameraRotationOffset;
    public Vector3 cameraPositionOffset;
    void Update()
    {
        Vector3 firstPositionRunner =  raceController.getFirstPositionRunner();

        if (firstPositionRunner != default(Vector3))
        {
            transform.position = firstPositionRunner + cameraPositionOffset;
       
            transform.LookAt(firstPositionRunner);
            transform.rotation = Quaternion.Euler(cameraRotationOffset + transform.rotation.eulerAngles);
        }
      
    }
}
