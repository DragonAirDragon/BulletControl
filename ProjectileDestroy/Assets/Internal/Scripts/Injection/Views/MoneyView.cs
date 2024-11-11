using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

public class MoneyView : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI moneyText;
   
   private LocalAndCloudDataService _localAndCloudDataService;
   
   [Inject]
   public void Construct(LocalAndCloudDataService localAndCloudDataService)
   {
      _localAndCloudDataService = localAndCloudDataService;
      _localAndCloudDataService.OnMoneyChanged += SetMoney;
   }
   
   private void OnEnable()
   {
      SetMoney(_localAndCloudDataService.GetCurrentMoney());
   }
   
   public void SetMoney(int money)
   {
      moneyText.text = money.ToString()+"$";
   }
   
   private void OnDestroy()
   {
      if (_localAndCloudDataService != null)
      {
         _localAndCloudDataService.OnMoneyChanged -= SetMoney; // Unsubscribe to prevent memory leaks
      }
   }
}
