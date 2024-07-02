using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public evelator_controll elevatorController; // Riferimento allo script del controllore dell'ascensore

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            elevatorController.OnFloorButtonPressed("Button floor 1");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            elevatorController.OnFloorButtonPressed("Button floor 2");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            elevatorController.OnFloorButtonPressed("Button floor 3");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            elevatorController.OnFloorButtonPressed("Button floor 4");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            elevatorController.OnFloorButtonPressed("Button floor 5");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            elevatorController.OnFloorButtonPressed("Button floor 6");
        }
    }
}
