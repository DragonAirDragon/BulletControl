using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;


public enum ObjectActionType
{
    Move,
    Rotate,
    Scale
}
public class TriggeredObjectController : MonoBehaviour
{
    [Title("Тип действия")]
    [EnumToggleButtons]
    [SerializeField]
    private ObjectActionType objectActivationType;

    [SerializeField]
    private float time;
    [SerializeField]
    private Ease easeType;


    [ShowIf("objectActivationType", Value = ObjectActionType.Move)]
    [SerializeField]
    private Vector3 vectorMove;

    [ShowIf("objectActivationType", Value = ObjectActionType.Rotate)]
    [SerializeField]
    private Vector3 vectorRotation;

    [ShowIf("objectActivationType", Value = ObjectActionType.Scale)]
    [SerializeField]
    private Vector3 vectorScale;
    private LevelService levelService;


    [Button("Activate Trigger")]
    private void ActivateTrigger()
    {
        Activate();
    }

    [Inject]
    public void Construct(LevelService levelService)
    {
        this.levelService = levelService;
        levelService.OnTriggerObjectsDestroyed += Activate;
    }

    public void Activate()
    {
        switch (objectActivationType)
        {
            case ObjectActionType.Move:
                transform.DOMove(transform.position + vectorMove, time).SetEase(easeType);
                break;
            case ObjectActionType.Rotate:
                transform.DORotate(vectorRotation, time).SetEase(easeType);
                break;
            case ObjectActionType.Scale:
                transform.DOScale(vectorScale, time).SetEase(easeType);
                break;
        }
    }
}
