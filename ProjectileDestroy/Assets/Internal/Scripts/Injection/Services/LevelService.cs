using System;
using System.Collections.Generic;
using DinoFracture;
using Unity.VisualScripting;
using UnityEngine;
using VContainer.Unity;


public class LevelService
{
    // Numbers objects
    private int currentRequiredObject;
    private int maxRequiredObject;

    private int currentOptionalObject;
    private int maxOptionalObject;

    private int currentTriggerObject;
    private int maxTriggerObject;

    // Actions
    public event Action OnRequiredObjectsDestroyed;
    public event Action OnOptionalObjectsDestroyed;
    public event Action OnTriggerObjectsDestroyed;

    public event Action OnPaused;
    public event Action OnUnpaused;

    // Refs destroyable objects
    private LevelObjects levelObjects;

    public Bullet bullet;


    public (int currentOptionalObject, int maxOptionalObject) GetOptionalCount()
    {
        return (currentOptionalObject, maxOptionalObject);
    }
    
    
    

    public LevelService(LevelObjects levelObjects)
    {
        this.levelObjects = levelObjects;

        InitializeServiceAndSubscribeToEvents();
    }


    private void InitializeServiceAndSubscribeToEvents()
    {
        maxRequiredObject = levelObjects.requiredObjects.Count;
        maxOptionalObject = levelObjects.optionalObjects.Count;
        maxTriggerObject = levelObjects.triggerObjects.Count;
        foreach (var requiredObject in levelObjects.requiredObjects)
        {
            requiredObject.OnFractureCompleted.AddListener(AddNewDestroyedRequiredObject);
        }
        foreach (var optionalObject in levelObjects.optionalObjects)
        {
            optionalObject.OnFractureCompleted.AddListener(AddNewDestroyedOptionalObject);
        }
        foreach (var triggerObject in levelObjects.triggerObjects)
        {
            triggerObject.OnFractureCompleted.AddListener(AddNewDestroyedTriggerObject);
        }
    }

    private void AddNewDestroyedRequiredObject(OnFractureEventArgs args)
    {
        currentRequiredObject++;
        CheckRequiredObjects();
    }
    private void AddNewDestroyedOptionalObject(OnFractureEventArgs args)
    {
        currentOptionalObject++;
        CheckOptionalObjects();
    }
    private void AddNewDestroyedTriggerObject(OnFractureEventArgs args)
    {
        currentTriggerObject++;
        CheckTriggerObjects();
    }

    private void CheckRequiredObjects()
    {
        if (currentRequiredObject >= maxRequiredObject)
        {
            OnRequiredObjectsDestroyed?.Invoke();
        }
    }

    private void CheckOptionalObjects()
    {
        if (currentOptionalObject >= maxOptionalObject)
        {
            OnOptionalObjectsDestroyed?.Invoke();
        }
    }

    private void CheckTriggerObjects()
    {
        if (currentTriggerObject >= maxTriggerObject)
        {
            OnTriggerObjectsDestroyed?.Invoke();
        }
    }

    public void Pause()
    {
        OnPaused?.Invoke();
    }

    public void Unpause()
    {
        OnUnpaused?.Invoke();
    }


    public (int currentOptional, int maxOptional) GetOptionalObjectCounts()
    {
        return (currentOptional: currentOptionalObject, maxOptional: maxOptionalObject);
    }





}

[Serializable]
public class LevelObjects
{
    public List<PreFracturedGeometry> requiredObjects;
    public List<PreFracturedGeometry> optionalObjects;
    public List<PreFracturedGeometry> triggerObjects;
}