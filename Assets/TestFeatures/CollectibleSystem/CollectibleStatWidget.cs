using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleStatWidget : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    public void Setup(Sprite icon, int collected, int total)
    {
        iconImage.sprite = icon;
        countText.text = $"{collected}/{total}";
    }
}
