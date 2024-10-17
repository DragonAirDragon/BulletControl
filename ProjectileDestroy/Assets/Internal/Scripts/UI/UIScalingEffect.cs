using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;

public class UIScalingEffect : MonoBehaviour
{
    public UIParticle uiParticle;
    public float baseResolutionWidth = 1920f; // Базовая ширина экрана.
    public float baseResolutionHeight = 1080f; // Базовая высота экрана.
    public float baseScale = 40f; // Базовое значение scale для базового разрешения.
    private Vector2 previousResolution;
    private void Start()
    {
        previousResolution = new Vector2(Screen.width, Screen.height);
        UpdateScale();
    }

    private void Update()
    {
        // Обновление масштаба, если разрешение экрана изменилось.
        if (Screen.width != previousResolution.x || Screen.height != previousResolution.y)
        {
            previousResolution = new Vector2(Screen.width, Screen.height);
            UpdateScale();
        }
    }

    private void UpdateScale()
    {
        // Получаем текущее разрешение экрана.
        float currentWidth = Screen.width;
        float currentHeight = Screen.height;

        // Вычисляем коэффициент изменения по сравнению с базовым разрешением.
        float widthRatio = currentWidth / baseResolutionWidth;
        float heightRatio = currentHeight / baseResolutionHeight;

        // Используем среднее арифметическое соотношение для адаптации масштаба.
        float scaleRatio = (widthRatio + heightRatio) / 2.0f;

        // Устанавливаем новый scale для эффекта.
        if (uiParticle != null)
        {
            uiParticle.scale = baseScale * scaleRatio;
        }
    }
}