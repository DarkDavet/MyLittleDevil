using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

enum RendererType
{
    SpriteRenderer,
    UIImage
}
public class SpriteLoader : MonoBehaviour
{
    [SerializeField] private RendererType type;

    public SpriteAtlas spriteAtlas;
    public string spriteName;
    private SpriteRenderer spriteRenderer;
    private Image uiImage;

    private void Start()
    {
        switch (type)
        {
            case RendererType.SpriteRenderer: spriteRenderer = GetComponent<SpriteRenderer>();
                                              LoadSpriteToRenderer();
                break;
            case RendererType.UIImage: uiImage = GetComponent<Image>();
                                       LoadSpriteToImage();
                break;
        }
    }

    private void LoadSpriteToRenderer()
    {
        if (spriteAtlas != null)
        {
            Sprite sprite = spriteAtlas.GetSprite(spriteName);
            if (sprite != null)
            {
                spriteRenderer.sprite = sprite;
            }
            else
            {
                Debug.LogError("Sprite not found in atlas: " + spriteName);
            }
        }
        else
        {
            Debug.LogError("Sprite Atlas is not assigned.");
        }
    }

    private void LoadSpriteToImage()
    {
        if (spriteAtlas != null)
        {
            Sprite sprite = spriteAtlas.GetSprite(spriteName);
            if (sprite != null)
            {
                uiImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("Sprite not found in atlas: " + spriteName);
            }
        }
        else
        {
            Debug.LogError("Sprite Atlas is not assigned.");
        }
    }
}
