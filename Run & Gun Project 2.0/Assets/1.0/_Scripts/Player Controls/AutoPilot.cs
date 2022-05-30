using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPilot : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("trigger working");
        if (other.CompareTag("Obstacle"))
        {
            if(other.gameObject.GetComponent<AutoPilotTrigger>().AutopilotCommand == pilot.jump)
            {
                transform.parent.gameObject.GetComponent<PlayerController>().Jump();
            }
            else if (other.gameObject.GetComponent<AutoPilotTrigger>().AutopilotCommand == pilot.roll)
            {
                Debug.Log("Auto Pilot roll.");
                transform.parent.gameObject.GetComponent<PlayerController>().Roll();
            }
        }
    }
}
