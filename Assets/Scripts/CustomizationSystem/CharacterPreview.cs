using System.Collections.Generic;
using UnityEngine;

public class CharacterPreview : MonoBehaviour
{
    [SerializeField] private SpriteRenderer clothingRenderer;
    private Sprite defaultSprite;

    private void Start() => defaultSprite = clothingRenderer.sprite;

    public void ApplyPreview(CustomizationItem item)
    {
        clothingRenderer.sprite = item.visualSprite;
    }

    public void ResetPreview() => clothingRenderer.sprite = defaultSprite;
}