using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
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


    [Title("Audio")]
    [SerializeField] private AudioClip shootAudio;
    [SerializeField] private float delayShoot;
    [SerializeField] private float shootPitch;
    [SerializeField] private float delayPitch;
    
    [SerializeField] private AudioClip backReloadAudio;
    [SerializeField] private float delayBackReload;
    [SerializeField] private float backReloadPitch;
    
    [SerializeField] private AudioClip forwardReloadAudio;
    [SerializeField] private float delayForwardReload;
    
    [SerializeField] private AudioClip[] casingAudioClips; // Изменено здесь
    
    [SerializeField] private AudioSource audioSourceShoot;
    [SerializeField] private AudioSource audioSourceReload;
    
    
   
    
    
    
    public async UniTask ShootAudioPlay()
    {
        if(delayShoot!=0f) await UniTask.Delay(TimeSpan.FromSeconds(delayShoot*timeSettings.timeDilationMultiply));
        audioSourceShoot.PlayOneShot(shootAudio);
        if(delayPitch!=0f) await UniTask.Delay(TimeSpan.FromSeconds(delayPitch*timeSettings.timeDilationMultiply));
        audioSourceShoot.pitch = shootPitch;
    }
    
    
    public async UniTask BackReloadAudioPlay()
    {
        if(backReloadAudio==null) return;
        
        if(delayBackReload!=0f) await UniTask.Delay(TimeSpan.FromSeconds(delayBackReload*timeSettings.timeDilationMultiply));
        audioSourceReload.pitch = backReloadPitch;
        audioSourceReload.PlayOneShot(backReloadAudio);
    }
    
    public async UniTask ForwardReloadAudioPlay()
    {
        if(forwardReloadAudio==null) return;
        
        if(delayForwardReload!=0f) await UniTask.Delay(TimeSpan.FromSeconds(delayForwardReload*timeSettings.timeDilationMultiply));
        audioSourceReload.pitch = 1f;
        
        audioSourceReload.PlayOneShot(forwardReloadAudio);
    }
    

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
        ShootAudioPlay().Forget();
        BackReloadAudioPlay().Forget();
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
            PlayRandomCasingAudio();
        }
        else
        {
            Debug.LogWarning("ParticleSystem is not assigned!");
        }
        ForwardReloadAudioPlay().Forget();
        foreach (var part in weaponParts)
        {
            EndAnimatePart(part);
        }
        muzzleFlash.SetActive(false);

    }

    private void PlayRandomCasingAudio()
    {
        if (casingAudioClips.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, casingAudioClips.Length);
            AudioClip randomClip = casingAudioClips[randomIndex];
            audioSourceReload.PlayOneShot(randomClip);
        }
        else
        {
            Debug.LogWarning("Нет аудиоклипов в массиве casingAudioClips!");
        }
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
