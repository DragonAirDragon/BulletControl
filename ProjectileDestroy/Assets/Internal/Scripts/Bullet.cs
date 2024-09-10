using System;
using DinoFracture;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

public class Bullet : MonoBehaviour
{
    [Title("Main Gameplay Params (Основные геймплей параметры)")]
    [Space]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float maneuverability = 1f;
    [SerializeField] private int countRicochet = 3;
    [SerializeField] private float distanceRayForContact = 1f;
    public int bulletCost = 20;



    [Title("Cashed Variables (Кешированные переменные)")]
    [Space]
    [ShowInInspector] private float rotationX;
    [ShowInInspector] private float rotationY;
    [ShowInInspector] private Vector3 direction;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private bool bulletPaused = false;


    [Title("Ref Services (Ссылки на сервисы)")]
    [Space]
    IInputService inputService;
    LevelService levelService;


    [Title("Other Ref (Другие ссылки)")]
    [Space]
    // For Camera
    public Transform target;

    [Title("Actions")]
    public Action OnBulletDestroyed;



    [Inject]
    public void Construct(IInputService inputService, LevelService levelService)
    {
        this.inputService = inputService;
        this.levelService = levelService;
        this.levelService.OnPaused += () => { SetPauseBullet(true); };
        this.levelService.OnUnpaused += () => { SetPauseBullet(false); };
    }

    void Start()
    {
        rotationX = bulletTransform.eulerAngles.x;
        rotationY = bulletTransform.eulerAngles.y;
    }


    void Update()
    {
        if (bulletPaused) return;
        var inputDirection = Vector2.zero;
        if (inputService != null)
        {
            inputDirection = inputService.GetMovementDirection() * maneuverability;
        }

        // Update rotation based on mouse input
        rotationX -= inputDirection.x;
        rotationY += inputDirection.y;

        // Check for collision in front of the object
        if (Physics.Raycast(bulletTransform.position, bulletTransform.forward, out RaycastHit hit, distanceRayForContact))
        {
            // Проверка тега объекта
            if (hit.collider.CompareTag("DestroyTag"))
            {
                // Попытка получить компонент FractureGeometry
                FractureGeometry fractureGeometry = hit.collider.GetComponent<FractureGeometry>();
                if (fractureGeometry != null)
                {
                    // Разрушаем объект
                    Vector3 localPoint = hit.transform.InverseTransformPoint(hit.point);
                    fractureGeometry.Fracture(localPoint);
                }
                else
                {
                    // Если компонент FractureGeometry отсутствует, просто уничтожаем объект
                    Destroy(hit.collider.gameObject);
                }
            }

            if (hit.collider.CompareTag("Map"))
            {
                // Calculate reflection direction
                Vector3 reflectionDirection = Vector3.Reflect(bulletTransform.forward, hit.normal);
                // Convert reflection direction to rotation
                QuaternionToRotation(DirectionToRotation(reflectionDirection), out rotationX, out rotationY);
                countRicochet--;
            }
        }

        bulletTransform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        bulletTransform.position += bulletTransform.forward * (speed * Time.deltaTime);

        if (countRicochet <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    Quaternion DirectionToRotation(Vector3 direction)
    {
        return Quaternion.LookRotation(direction, Vector3.up);
    }

    private void QuaternionToRotation(Quaternion rotation, out float rotationX, out float rotationY)
    {
        Vector3 eulerAngles = rotation.eulerAngles;
        rotationY = eulerAngles.y;
        rotationX = eulerAngles.x;
    }

    public void OnDestroy()
    {
        OnBulletDestroyed.Invoke();
    }

    private void SetPauseBullet(bool isActive)
    {
        bulletPaused = isActive;
    }
}