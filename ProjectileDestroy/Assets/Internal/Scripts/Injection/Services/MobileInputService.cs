using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputService : IInputService
{
    readonly InputSettings inputSettings;

    public MobileInputService(InputSettings inputSettings)
    {
        this.inputSettings = inputSettings;
    }

    public Vector2 GetMovementDirection()
    {
        Vector2 inputVector = Vector2.zero;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            inputVector.y = Input.touches[0].deltaPosition.x;
            inputVector.x = Input.touches[0].deltaPosition.y;
        }
        inputVector = inputVector * inputSettings.defaultBulletManeuverability;
        return inputVector;
    }

    public Vector2 GetRotateFirstPersonCamera()
    {
        Vector2 inputVector = Vector2.zero;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            inputVector.y = Input.touches[0].deltaPosition.x;
            inputVector.x = Input.touches[0].deltaPosition.y;
        }
        inputVector = inputVector * inputSettings.sensitivityPlayerCamera;
        return inputVector;
    }

    public bool GetAim()
    {
        return false;
    }

    public bool GetShoot()
    {
        return false;
    }

    public bool GetPause()
    {
        return false;
    }
}
