using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // Подключение библиотеки DOTween

public class MoveYUIImage : MonoBehaviour
{
    public RectTransform uiImage; // Ссылка на RectTransform UI Image
    public float moveDistance = 50f; // Расстояние, на которое будет двигаться изображение
    public float duration = 1f; // Время, за которое будет проходить движение

    void OnEnable()
    {
        // Активируем анимацию при включении объекта
        StartLoopMovement();
    }

    void OnDisable()
    {
        // Останавливаем анимацию при выключении объекта
        StopLoopMovement();
    }

    void StartLoopMovement()
    {
        // Начинаем движение вверх и вниз в бесконечном цикле
        LoopMovement();
    }

    void StopLoopMovement()
    {
        // Останавливаем все активные анимации для uiImage
        uiImage.DOKill();
    }

    void LoopMovement()
    {
        // Двигаем изображение вверх на заданное расстояние
        uiImage.DOAnchorPosY(moveDistance, duration)
            .SetEase(Ease.InOutSine) // Устанавливаем плавность движения
            .OnComplete(() =>
            {
                // После завершения движения вверх двигаем вниз
                uiImage.DOAnchorPosY(-moveDistance, duration)
                    .SetEase(Ease.InOutSine)
                    .OnComplete(LoopMovement); // Повторяем цикл движения
            });
    }
}