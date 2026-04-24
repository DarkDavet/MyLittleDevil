using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WalletSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    public void Setup(Sprite icon, int count)
    {
        iconImage.sprite = icon;
        countText.text = count.ToString();
    }
}
