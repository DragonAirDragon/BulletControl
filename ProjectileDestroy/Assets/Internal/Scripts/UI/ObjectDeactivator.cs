using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectDeactivator : MonoBehaviour
{
    // Ссылки на объекты, которые нужно деактивировать
    public GameObject[] objectsToDeactivate;

    // Вызывается при активации объекта
    void OnEnable()
    {
        SetObjectsActive(true);
    }

    // Вызывается при деактивации объекта
    void OnDisable()
    {
        SetObjectsActive(false);
    }

    // Метод для активации или деактивации объектов
    private void SetObjectsActive(bool isActive)
    {
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }
}

