using DG.Tweening;
using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Sequence = DG.Tweening.Sequence;

public class ColorUIImageAnimation : MonoBehaviour
{
   public bool buyedAd;
   public TextMeshProUGUI text;
   public Color buyedColor;
   
   public BetterImage buyMoney;
   public BetterImage buyAdFree;
   public float transitionDuration = 2f; // Время перехода между цветами
   public float stayDuration = 1f; // Время задержки на одном цвете
   private Tween colorTween;
   private bool isAnimating = false;
   public Color[] colors = new Color[3];
   private int currentColorIndex = 0;

   void OnEnable()
   {
      StartAnimation();
   }

   void OnDisable()
   {
      StopAnimation();
   }
   void Start()
   {
      LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
   }
   
   void OnDestroy()
   {
      LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
   }
   public void StartAnimation()
   {
      if (!isAnimating)
      {
         isAnimating = true;
         AnimateColor();
      }
   }

   public void StopAnimation()
   {
      isAnimating = false;
      if (colorTween != null)
      {
         colorTween.Kill();
         colorTween = null;
      }
   }

   void AnimateColor()
   {
      if (!isAnimating)
         return;

      
      currentColorIndex = (currentColorIndex + 1) % colors.Length;


      Sequence colorSequence = DOTween.Sequence();

      if (!buyedAd)
      {
         // Добавляем анимации изменения цвета для обоих изображений
         colorSequence.Append(buyMoney.DOColor(colors[currentColorIndex], transitionDuration).SetEase(Ease.Linear))
            .Join(buyAdFree.DOColor(colors[currentColorIndex], transitionDuration).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
               // После завершения перехода ждем заданный промежуток времени
               DOVirtual.DelayedCall(stayDuration, () =>
               {
                  // Затем запускаем следующий цикл анимации
                  AnimateColor();
               });
            });
      }
      else
      {
         // Добавляем анимации изменения цвета для обоих изображений
         colorSequence.Append(buyMoney.DOColor(colors[currentColorIndex], transitionDuration).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
               // После завершения перехода ждем заданный промежуток времени
               DOVirtual.DelayedCall(stayDuration, () =>
               {
                  // Затем запускаем следующий цикл анимации
                  AnimateColor();
               });
            });
      }
      // Сохраняем последовательность, чтобы можно было остановить анимацию при необходимости
      colorTween = colorSequence;
   }

   public void SetBuyingAd()
   {
      StopAnimation();
      buyedAd = true;
      UpdateText();
      buyAdFree.color = buyedColor;
      StartAnimation();
   }

   void OnLocaleChanged(UnityEngine.Localization.Locale locale)
   {
      UpdateText();
   }
   
   public void UpdateText()
   {
      if(!buyedAd) return;
      text.text = LocalizationSettings.StringDatabase.GetLocalizedString("Donat", "donatePurchased");;
  
   }
   
}
