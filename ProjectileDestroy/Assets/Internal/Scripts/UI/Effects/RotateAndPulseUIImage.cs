using UnityEngine;
using DG.Tweening;

public class RotateAndPulseUIImage : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationDuration = 2f; // Длительность вращения по оси Z

    [Header("Pulsing Scale Settings")]
    public float pulseDuration = 1f; // Длительность пульсации
    public float scaleFactor = 0.1f; // Максимальное изменение масштаба

    private Vector3 initialScale;
    private Tween rotationTween;
    private Tween scaleTween;

    private void OnEnable()
    {
        initialScale = transform.localScale;
        StartAnimation();
        //Debug.Log("Анимация круговая началась");
    }

    private void OnDisable()
    {
        StopAnimation();
        //Debug.Log("Анимация круговая окончилась");
    }

    public void StartAnimation()
    {
        // Останавливаем текущие твины, если они еще активны
        StopAnimation();

        // Запускаем анимацию вращения и пульсации
        StartRotationAndPulsingScale();
    }

    public void StopAnimation()
    {
        // Останавливаем анимацию вращения и пульсации
        if (rotationTween != null && rotationTween.IsActive())
        {
            rotationTween.Kill();
        }
        if (scaleTween != null && scaleTween.IsActive())
        {
            scaleTween.Kill();
        }
        transform.localScale = initialScale;
        transform.rotation = Quaternion.identity;
    }

    void StartRotationAndPulsingScale()
    {
        rotationTween = transform.DORotate(new Vector3(0f, 0f, 360f), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        scaleTween = transform.DOScale(initialScale * (1 + scaleFactor), pulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}