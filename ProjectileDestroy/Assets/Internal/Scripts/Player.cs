using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

public class Player : MonoBehaviour
{
    [Title("Setting FPS Camera")]
    [Space]
    [SerializeField] private float verticalRotationLimit = 90f;
    [SerializeField] private float rotationX;
    [SerializeField] private float rotationY;
    [Title("Cashed Variable")]
    [Space]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform firePoint;
    bool playerPhase = true;
    bool playerActive = true;
    bool pausePlayer = false;

    
    private WeaponSettings weaponSettings;
    
    [Title("Ref and services")]
    [Space]
    [SerializeField] private RightHandAnimator rightHandAnimator;
    private LocalAndCloudDataService localAndCloudDataService;
    [ShowInInspector] private TimeSettings timeSettings;

    private BulletFactory bulletFactory;
    private IInputService inputService;
    private LevelService levelService;
    private PostProcessingEffectService postProcessingEffectService;
    private GameSessionView gameSessionView;
    

    private int numberOfBulletsUsed = 0;
    [Inject]
    public void Construct(BulletFactory bulletFactory, IInputService inputService, TimeSettings timeSettings, PostProcessingEffectService postProcessingEffectService, LevelService levelService,  GameSessionView gameSessionView,LocalAndCloudDataService localAndCloudDataService)
    {
        this.bulletFactory = bulletFactory;
        this.inputService = inputService;
        
        this.timeSettings = timeSettings;
        this.levelService = levelService;
        this.postProcessingEffectService = postProcessingEffectService;
        this.gameSessionView = gameSessionView;
        this.localAndCloudDataService = localAndCloudDataService;
        weaponSettings = this.localAndCloudDataService.GetCurrentWeaponSettings();
        rightHandAnimator.weaponAnimationSettings = weaponSettings.weaponAnimationSettings;
        rightHandAnimator.timeSettings = this.timeSettings;
       
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotationX = cameraTransform.eulerAngles.x;
        rotationY = cameraTransform.eulerAngles.y;
        bulletFactory.OnBulletDestroyed +=
            () =>
            {
                ControlActivityPlayerAndCamera(true).Forget();
                // Change Used Bullets
                // Show bullets used
                gameSessionView.SetGameStagePlayer();
                gameSessionView.UpdateBulletsUI(numberOfBulletsUsed);
                
            };

        levelService.OnRequiredObjectsDestroyed +=
            () =>
            {
                SetAllActivityPlayer(false);
            };

        levelService.OnPaused +=
            () =>
            {
                SetPausePlayer(true);
            };
        levelService.OnUnpaused +=
            () =>
            {
                SetPausePlayer(false);
            };
    }


    private void Shoot(BulletСaliber bulletCaliber)
    {
        bulletFactory.Create(bulletCaliber, rightHandAnimator.firePoint.position, RayCastAndCalculateAngle());
        numberOfBulletsUsed++;
    }

    private Quaternion RayCastAndCalculateAngle()
    {
        // Получаем главную камеру, которая управляется Cinemachine
        Camera mainCamera = Camera.main;

        // Определяем середину экрана
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // Создаем луч от камеры через точку на экране
        Ray ray = mainCamera.ScreenPointToRay(screenCenter);

        // Переменная для хранения информации о попадании
        RaycastHit hit;

        // Проверяем попадание луча
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Вычисляем направление от объекта к точке попадания
            Vector3 direction = (hit.point - rightHandAnimator.firePoint.position).normalized;
            //Debug.Log("попадание");
            // Поворачиваем объект в сторону точки попадания
            return Quaternion.LookRotation(direction);
        }
        else
        {
            //Debug.Log("непопадание");
            // Если луч не попадает, вычисляем точку на расстоянии maxDistance по направлению луча
            Vector3 endPoint = ray.origin + ray.direction * 10f;
            Vector3 direction = (endPoint - rightHandAnimator.firePoint.position).normalized;
            return Quaternion.LookRotation(direction);
        }
    }


    private void Update()
    {
        if (!playerActive) return;
        if (pausePlayer) return;
        if (!playerPhase) return;
        RotatePlayer();
        if (inputService.GetAim())
        {
            rightHandAnimator.Aim();
        }
        if (inputService.GetShoot())
        {
            Shoot(weaponSettings.bulletСaliber);
            
            ControlActivityPlayerAndCamera(false).Forget();
        }
    }

    private async UniTaskVoid ControlActivityPlayerAndCamera(bool activity)
    {

        if (!activity)
        {
            SetActivePlayer(false);
            rightHandAnimator.StartShootAnim();
            await UniTask.Delay(TimeSpan.FromSeconds(rightHandAnimator.weaponAnimationSettings.recoilDuration * timeSettings.timeDilationMultiply));
            bulletFactory.GetCurrentBullet().SetSpeed();
            
            if (cameraTransform != null) cameraTransform.gameObject.SetActive(activity);
            postProcessingEffectService.SetSpeedEffect(true);
        }
        else
        {
            SetActivePlayer(true);
            if (cameraTransform != null) cameraTransform.gameObject.SetActive(activity);
            postProcessingEffectService.SetSpeedEffect(false);
            rightHandAnimator.EndShootAnim();
        }
        
    }

    private void SetActivePlayer(bool activityPlayer)
    {
        playerPhase = activityPlayer;
    }
    private void SetAllActivityPlayer(bool activityPlayer)
    {
        playerActive = activityPlayer;
    }

    private void SetPausePlayer(bool value)
    {
        pausePlayer = value;
    }

    private void RotatePlayer()
    {
        var inputDirection = Vector2.zero;
        if (inputService != null)
        {
            inputDirection = inputService.GetRotateFirstPersonCamera();
        }
        rotationX = Mathf.Clamp(rotationX - inputDirection.x, -verticalRotationLimit, verticalRotationLimit);
        rotationY += inputDirection.y;
        cameraTransform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}
