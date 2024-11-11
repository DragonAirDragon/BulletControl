using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopInputService : IInputService
{
    readonly InputSettings inputSettings;

    public DesktopInputService(InputSettings inputSettings)
    {
        this.inputSettings = inputSettings;
    }

    public Vector2 GetMovementDirection()
    {
        Vector2 inputVector = Vector2.zero;
        // inputVector.x = Input.GetAxis("Mouse Y");
        // inputVector.y = Input.GetAxis("Mouse X");
        inputVector.x = Input.GetAxis("Vertical");
        inputVector.y = Input.GetAxis("Horizontal");
        inputVector *=  inputSettings.defaultBulletManeuverability;
        return inputVector;
    }

    public Vector2 GetRotateFirstPersonCamera()
    {
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Mouse Y");
        inputVector.y = Input.GetAxis("Mouse X");
        inputVector = inputVector * inputSettings.sensitivityPlayerCamera;
        return inputVector;
    }

    public bool GetAim()
    {
        return Input.GetKeyDown(KeyCode.Mouse1);
    }

    public bool GetShoot()
    {
        return Input.GetKeyDown(KeyCode.Mouse0);
    }

    public bool GetPause()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }
}
