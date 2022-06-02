using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum pilot { jump, roll, turnLeft, turnRight };

public class AutoPilotTrigger : MonoBehaviour
{
    public pilot AutopilotCommand;
}
