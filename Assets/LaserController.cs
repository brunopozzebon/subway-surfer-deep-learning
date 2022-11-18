using UnityEngine;

public class LaserController : MonoBehaviour
{
    private const float MOVEMENT_INCREMENT = 0.005f;
    private void Update()
    {
        transform.position+= new Vector3(MOVEMENT_INCREMENT, 0, 0);
    }
}
