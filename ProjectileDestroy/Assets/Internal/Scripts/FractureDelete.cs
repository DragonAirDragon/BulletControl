using DinoFracture;
using UnityEngine;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(PreFracturedGeometry))]
public class FractureDelete : MonoBehaviour
{
    PreFracturedGeometry geometry;
    private GameObject fractures;

    void Start()
    {
        geometry = GetComponent<PreFracturedGeometry>();
        geometry.OnFractureCompleted.AddListener(DestroyFractures);
    }

    private void DestroyFractures(OnFractureEventArgs args)
    {
        // Получаем сгенерированные части объекта
        fractures = geometry.GeneratedPieces;
        
        // Запускаем удаление через определенное время
        if (fractures != null)
        {
            DestroyAfterDelay(5f).Forget(); // 5 секунд ожидания
        }
    }

    private async UniTask DestroyAfterDelay(float delay)
    {
        await UniTask.Delay(System.TimeSpan.FromSeconds(delay)); // Задержка
        if (fractures != null)
        {
            Destroy(fractures); // Уничтожение объекта
        }
    }
}