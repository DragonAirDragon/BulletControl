using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponAnimator : MonoBehaviour
{
    public Transform pointOfFire;


    [System.Serializable]
    public class WeaponPartAnimation
    {
        public enum AnimationType
        {
            Move,
            Rotate,
            RotateCycle,
            MoveAndRotateInTurn
        }
        public string partName;
        public Transform partTransform;
        public AnimationType animationType;


        public Vector3 originalPosition;
        public Vector3 originalAngles;

        [ShowIf("animationType", AnimationType.Move)]
        public Vector3 moveOffset;

        [ShowIf("animationType", AnimationType.Rotate)]
        public Vector3 rotateAngle;

        [ShowIf("animationType", AnimationType.MoveAndRotateInTurn)]
        public Vector3 moveAndRotateOffset;
        [ShowIf("animationType", AnimationType.MoveAndRotateInTurn)]
        public Vector3 moveAndRotateAngle;

        public float animationDuration = 0.1f;
        public float returnDuration = 0.1f;
        public Ease easeType = Ease.OutQuad;
        public bool returnToOriginal = true;
        public Sequence partSequence;
    }


    [SerializeField] private List<WeaponPartAnimation> weaponParts = new List<WeaponPartAnimation>();

    [SerializeField] private ParticleSystem particleFire;

    [SerializeField] private GameObject muzzleFlash;

    public TimeSettings timeSettings;




    public void PlayShootAnimation()
    {
        foreach (var part in weaponParts)
        {
            AnimatePart(part);
        }
        particleFire.Play();

    }


    public void StartShootAnimation()
    {
        muzzleFlash.SetActive(true);
        foreach (var part in weaponParts)
        {
            FirstAnimatePart(part);
        }
    }
    public void EndShootAnimation()
    {
        if (particleFire != null)
        {
            particleFire.Play();
        }
        else
        {
            Debug.LogWarning("ParticleSystem is not assigned!");
        }
        foreach (var part in weaponParts)
        {
            EndAnimatePart(part);
        }
        muzzleFlash.SetActive(false);

    }


    private void AnimatePart(WeaponPartAnimation part)
    {
        if (part.partSequence.IsActive())
        {
            part.partSequence.Kill();
        }

        part.partSequence = DOTween.Sequence();

        switch (part.animationType)
        {
            case WeaponPartAnimation.AnimationType.Move:
                part.partSequence.Append(part.partTransform.DOLocalMove(part.moveOffset, part.animationDuration).SetEase(part.easeType));
                if (part.returnToOriginal)
                {
                    part.partSequence.Append(part.partTransform.DOLocalMove(part.originalPosition, part.returnDuration).SetEase(part.easeType));
                }
                break;

            case WeaponPartAnimation.AnimationType.Rotate:
                part.partSequence.Append(part.partTransform.DOLocalRotate(part.rotateAngle, part.animationDuration).SetEase(part.easeType));
                if (part.returnToOriginal)
                {
                    part.partSequence.Append(part.partTransform.DOLocalRotate(part.originalAngles, part.returnDuration).SetEase(part.easeType));
                }
                break;
            case WeaponPartAnimation.AnimationType.RotateCycle:
                part.partSequence.Append(part.partTransform.DOLocalRotate(part.rotateAngle, part.animationDuration).SetEase(part.easeType)).OnComplete(() =>
                   {
                       part.partTransform.localRotation = Quaternion.identity;
                   });
                break;
            case WeaponPartAnimation.AnimationType.MoveAndRotateInTurn:
                part.partSequence.Append(part.partTransform.DOLocalRotate(part.moveAndRotateAngle, part.animationDuration).SetEase(part.easeType));
                part.partSequence.Append(part.partTransform.DOLocalMove(part.moveAndRotateOffset, part.animationDuration).SetEase(part.easeType));
                if (part.returnToOriginal)
                {
                    part.partSequence.Append(part.partTransform.DOLocalMove(part.originalPosition, part.returnDuration).SetEase(part.easeType));
                    part.partSequence.Append(part.partTransform.DOLocalRotate(part.originalAngles, part.returnDuration).SetEase(part.easeType));
                }
                break;
        }
    }

    private void FirstAnimatePart(WeaponPartAnimation part)
    {
        if (part.partSequence.IsActive())
        {
            part.partSequence.Kill();
        }
        part.partSequence = DOTween.Sequence();
        switch (part.animationType)
        {
            case WeaponPartAnimation.AnimationType.Move:
                part.partSequence.Append(part.partTransform.DOLocalMove(part.moveOffset, part.animationDuration * timeSettings.timeDilationMultiply).SetEase(part.easeType));
                break;

            case WeaponPartAnimation.AnimationType.Rotate:
                part.partSequence.Append(part.partTransform.DOLocalRotate(part.rotateAngle, part.animationDuration * timeSettings.timeDilationMultiply).SetEase(part.easeType));
                break;
            case WeaponPartAnimation.AnimationType.MoveAndRotateInTurn:
                part.partSequence.Append(part.partTransform.DOLocalRotate(part.moveAndRotateAngle, part.animationDuration * timeSettings.timeDilationMultiply).SetEase(part.easeType));
                part.partSequence.Append(part.partTransform.DOLocalMove(part.moveAndRotateOffset, part.animationDuration * timeSettings.timeDilationMultiply).SetEase(part.easeType));
                break;
        }

    }
    private void EndAnimatePart(WeaponPartAnimation part)
    {
        if (part.partSequence.IsActive())
        {
            part.partSequence.Kill();
        }
        part.partSequence = DOTween.Sequence();
        switch (part.animationType)
        {
            case WeaponPartAnimation.AnimationType.Move:
                if (part.returnToOriginal)
                {
                    part.partSequence.Append(part.partTransform.DOLocalMove(part.originalPosition, part.returnDuration).SetEase(part.easeType));
                }
                break;

            case WeaponPartAnimation.AnimationType.Rotate:
                if (part.returnToOriginal)
                {
                    part.partSequence.Append(part.partTransform.DOLocalRotate(part.originalAngles, part.returnDuration).SetEase(part.easeType));
                }
                break;
            case WeaponPartAnimation.AnimationType.RotateCycle:
                part.partSequence.Append(part.partTransform.DOLocalRotate(part.rotateAngle, part.animationDuration).SetEase(part.easeType)).OnComplete(() =>
                   {
                       part.partTransform.localRotation = Quaternion.identity;
                   });
                break;
            case WeaponPartAnimation.AnimationType.MoveAndRotateInTurn:
                if (part.returnToOriginal)
                {
                    part.partSequence.Append(part.partTransform.DOLocalMove(part.originalPosition, part.returnDuration).SetEase(part.easeType));
                    part.partSequence.Append(part.partTransform.DOLocalRotate(part.originalAngles, part.returnDuration).SetEase(part.easeType));
                }
                break;
        }
    }


    private void OnDestroy()
    {
        foreach (var animation in weaponParts)
        {
            animation.partSequence.Kill();
        }
    }
}
