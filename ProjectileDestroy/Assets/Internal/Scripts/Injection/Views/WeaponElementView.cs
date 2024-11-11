using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class WeaponElementView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Image weaponImage;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private Color availableColor;
    [SerializeField] private Color lockedColor;
    public Button button;
    public void SetAvailability(string weaponName, Sprite weaponSprite, bool isAvailable)
    {
        this.weaponName.text = weaponName;
        weaponImage.sprite = weaponSprite;

        if (isAvailable)
        {
            lockImage.SetActive(false);
            weaponImage.color = availableColor;
            button.interactable = true;
        }
        else
        {
            lockImage.SetActive(true);
            weaponImage.color = lockedColor;
            button.interactable = false;
        }
    }
    
    
    
}
