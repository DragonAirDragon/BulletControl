using DG.Tweening;
using UnityEngine;
public class RightHandAnimator : MonoBehaviour
{
    private bool isAim;
    private Tween idleAnimationTween;
    private Sequence aimSequence;
    private Sequence recoilSequence;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private WeaponAnimator currentWeaponAnimator;
    public Transform firePoint;
    public WeaponAnimationSettings weaponAnimationSettings;
    public TimeSettings timeSettings;

    private void Start()
    {
        currentWeaponAnimator = Instantiate(weaponAnimationSettings.weaponPrefab, rightHandTransform);
        currentWeaponAnimator.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        currentWeaponAnimator.timeSettings = timeSettings;
        firePoint = currentWeaponAnimator.pointOfFire;
        StartCycleIdle();
    }

    public void Aim()
    {
        if (isAim)
        {
            ReturnToIdle();
        }
        else
        {
            StartAimAnimation();
        }
        isAim = !isAim;
    }



    private void StartCycleIdle()
    {
        StopAllAnimations();
        transform.localPosition = weaponAnimationSettings.idlePosition;
        transform.localRotation = Quaternion.Euler(weaponAnimationSettings.idleRotation);
        // Создаем бесконечную анимацию движения вверх и вниз
        idleAnimationTween = transform.DOLocalMoveY(transform.localPosition.y + weaponAnimationSettings.yDifferent, weaponAnimationSettings.timeIdleAnimation / 2)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void StartAimAnimation()
    {
        StopAllAnimations();
        aimSequence = DOTween.Sequence();
        // Анимация позиции
        aimSequence.Append(transform.DOLocalMove(weaponAnimationSettings.aimPosition, weaponAnimationSettings.aimTransitionTime)
            .SetEase(Ease.OutQuad));
        // Анимация вращения
        aimSequence.Join(transform.DOLocalRotate(weaponAnimationSettings.aimRotation, weaponAnimationSettings.aimTransitionTime)
            .SetEase(Ease.OutQuad));
    }

    private void ReturnToIdle()
    {
        StopAllAnimations();
        aimSequence = DOTween.Sequence();
        // Анимация позиции
        aimSequence.Append(transform.DOLocalMove(weaponAnimationSettings.idlePosition, weaponAnimationSettings.aimTransitionTime)
            .SetEase(Ease.OutQuad));
        // Анимация вращения
        aimSequence.Join(transform.DOLocalRotate(weaponAnimationSettings.idleRotation, weaponAnimationSettings.aimTransitionTime)
            .SetEase(Ease.OutQuad));
        aimSequence.OnComplete(StartCycleIdle);
    }

    public void StartShootAnim()
    {
        currentWeaponAnimator.StartShootAnimation();
        StopAllAnimations();
        recoilSequence = DOTween.Sequence();
        // Сохраняем текущую позицию и вращение
        Vector3 startPos;
        Vector3 startRot;
        if (isAim)
        {
            startPos = weaponAnimationSettings.aimPosition;
            startRot = weaponAnimationSettings.aimRotation;
        }
        else
        {
            startPos = weaponAnimationSettings.idlePosition;
            startRot = weaponAnimationSettings.idleRotation;
        }
        // Анимация отдачи
        recoilSequence.Append(transform.DOLocalMove(startPos + weaponAnimationSettings.recoilOffset, weaponAnimationSettings.recoilDuration * timeSettings.timeDilationMultiply));
        recoilSequence.Join(transform.DOLocalRotate(startRot + weaponAnimationSettings.recoilRotation, weaponAnimationSettings.recoilDuration * timeSettings.timeDilationMultiply));
        recoilSequence.SetEase(Ease.OutQuint);
    }

    public void EndShootAnim()
    {
        currentWeaponAnimator.EndShootAnimation();
        recoilSequence = DOTween.Sequence();
        Vector3 startPos;
        Vector3 startRot;
        if (isAim)
        {
            startPos = weaponAnimationSettings.aimPosition;
            startRot = weaponAnimationSettings.aimRotation;
        }
        else
        {
            startPos = weaponAnimationSettings.idlePosition;
            startRot = weaponAnimationSettings.idleRotation;
        }
        // Возвращение в исходное положение
        recoilSequence.Append(transform.DOLocalMove(startPos, weaponAnimationSettings.returnDuration));
        recoilSequence.Join(transform.DOLocalRotate(startRot, weaponAnimationSettings.returnDuration));

        // Возобновляем предыдущую анимацию после отдачи
        recoilSequence.OnComplete(() =>
        {
            if (isAim)
                StartAimAnimation();
            else
                StartCycleIdle();
        });
    }

    public void ShootAnim()
    {
        currentWeaponAnimator.PlayShootAnimation();
        StopAllAnimations();
        recoilSequence = DOTween.Sequence();
        // Сохраняем текущую позицию и вращение
        Vector3 startPos;
        Vector3 startRot;
        if (isAim)
        {
            startPos = weaponAnimationSettings.aimPosition;
            startRot = weaponAnimationSettings.aimRotation;
        }
        else
        {
            startPos = weaponAnimationSettings.idlePosition;
            startRot = weaponAnimationSettings.idleRotation;
        }


        // Анимация отдачи
        recoilSequence.Append(transform.DOLocalMove(startPos + weaponAnimationSettings.recoilOffset, weaponAnimationSettings.recoilDuration));
        recoilSequence.Join(transform.DOLocalRotate(startRot + weaponAnimationSettings.recoilRotation, weaponAnimationSettings.recoilDuration));

        // Возвращение в исходное положение
        recoilSequence.Append(transform.DOLocalMove(startPos, weaponAnimationSettings.returnDuration));
        recoilSequence.Join(transform.DOLocalRotate(startRot, weaponAnimationSettings.returnDuration));

        // Возобновляем предыдущую анимацию после отдачи
        recoilSequence.OnComplete(() =>
        {
            if (isAim)
                StartAimAnimation();
            else
                StartCycleIdle();
        });
    }



    private void StopAllAnimations()
    {
        if (idleAnimationTween != null && idleAnimationTween.IsActive())
        {
            idleAnimationTween.Kill();
        }
        if (aimSequence != null && aimSequence.IsActive())
        {
            aimSequence.Kill();
        }
        if (recoilSequence != null && recoilSequence.IsActive())
        {
            recoilSequence.Kill();
        }
    }

    private void OnDestroy()
    {
        StopAllAnimations();
    }
}