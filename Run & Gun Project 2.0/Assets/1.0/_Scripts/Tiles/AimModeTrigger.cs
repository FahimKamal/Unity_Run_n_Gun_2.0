using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimModeTrigger : MonoBehaviour
{    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().ActiveAimMode();
        }
    }
}
