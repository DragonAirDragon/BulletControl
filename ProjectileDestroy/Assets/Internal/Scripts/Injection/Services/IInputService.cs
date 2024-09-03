using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputService
{
    Vector2 GetMovementDirection();
    Vector2 GetRotateFirstPersonCamera();
    bool GetAim();
    bool GetShoot();
    bool GetPause();
}
