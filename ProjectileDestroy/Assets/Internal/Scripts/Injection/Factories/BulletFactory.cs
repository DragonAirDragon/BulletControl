using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BulletFactory
{
    private readonly IObjectResolver container;
    private readonly Dictionary<BulletСaliber, Bullet> bulletPrefabs;

    // Событие, вызываемое при создании пули
    public event Action<Bullet> OnBulletCreated;
    public event Action OnBulletDestroyed;
    
    // Текущая пуля
    private Bullet currentBullet = null;


    public BulletFactory(IObjectResolver container, Dictionary<BulletСaliber, Bullet> bulletPrefabs)
    {
        this.container = container;
        this.bulletPrefabs = bulletPrefabs;
    }

    public Bullet Create(BulletСaliber bulletСaliber,Vector3 position,Quaternion rotation)
    {
        if (!bulletPrefabs.TryGetValue(bulletСaliber, out Bullet prefab))
        {
            Debug.LogError($"Bullet prefab for type {bulletСaliber} not found!");
            return null;
        }

        currentBullet = container.Instantiate(prefab);
        currentBullet.transform.position = position;
        currentBullet.transform.rotation = rotation;
        // Вызываем событие создания пули
        OnBulletCreated?.Invoke(currentBullet);
        currentBullet.OnBulletDestroyed += Destroy;
        return currentBullet;
    }

    private void Destroy()
    {
        OnBulletDestroyed.Invoke();
    }

    
    
    
}