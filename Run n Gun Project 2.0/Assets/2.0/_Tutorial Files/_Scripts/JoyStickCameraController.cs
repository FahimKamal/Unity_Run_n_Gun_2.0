
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class JoyStickCameraController : MonoBehaviour
{

    public FixedJoystick lookStick;

    public Vector2 delta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delta = lookStick.Direction;
        //aimCamera.GetCinemachineComponent<AxisState>().m

        //delta = lookStick.Direction;
        //cameraAngle += lookStick.Horizontal * cameraAngleSpeed;

        //Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngle, Vector3.up) * new Vector3(0, 3, 4);
    }
}
