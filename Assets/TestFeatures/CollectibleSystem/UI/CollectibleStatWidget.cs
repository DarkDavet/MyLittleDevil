using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleStatWidget : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI addedText;

    public void Setup(Sprite icon, int collected, int total, int added = 0)
    {
        iconImage.sprite = icon;
        countText.text = $"{collected}/{total}";

        if (addedText  != null)
        {
            if (added > 0)
            {
                addedText.gameObject.SetActive(true);
                addedText.text = $"+{added}";
            }
            else
            {
                addedText.gameObject.SetActive(false);
            }
        }   
    }
}
