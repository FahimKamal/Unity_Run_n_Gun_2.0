using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickController : MonoBehaviour
{
    private PlayerController playerController;
    public FixedJoystick movementStick;
    public FixedButton jumpButton;
    public FixedButton rollButton;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        playerController.movementInput.x = movementStick.Horizontal;
        if(movementStick.Vertical > 0.6)
        {
            TouchJump();
        }
        if (movementStick.Vertical < -0.6)
        {
            TouchRoll();
        }
    }

    public void TouchJump()
    {
        playerController.touchJump = true;
    }

    public void TouchRoll()
    {
        playerController.touchRoll = true;
    }

    public void TouchShoot()
    {
        playerController.ShootGun();
    }
}
