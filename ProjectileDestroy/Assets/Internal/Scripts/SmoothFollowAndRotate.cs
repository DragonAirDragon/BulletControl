using System;
using UnityEngine;
using VContainer;

public class SmoothFollowAndRotate : MonoBehaviour
{
    public Transform target;
    public float positionSmoothSpeed = 0.125f;
    public float rotationSmoothSpeed = 0.05f;
    public Vector3 positionOffset;
    [Inject]
    public void Construct(BulletFactory bulletFactory)
    {
        bulletFactory.OnBulletCreated += GetBulletTarget;
    }
    public void GetBulletTarget(Bullet bullet)
    {
        transform.position = bullet.target.transform.position;
        transform.rotation = bullet.target.transform.rotation;
        target = bullet.target;
    }


    private void LateUpdate()
    {
        if (target != null)
        {
            // Плавное движение
            Vector3 desiredPosition = target.position + positionOffset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * positionSmoothSpeed);
            transform.position = smoothedPosition;
            // Плавное вращение
            Quaternion desiredRotation = target.rotation;
            Quaternion smoothedRotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothSpeed);
            transform.rotation = smoothedRotation;
        }

    }



}
