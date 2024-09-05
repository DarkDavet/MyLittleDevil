using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Bar: MonoBehaviour
{
    [SerializeField] private AspectManager aspect;
    public SpriteAtlas spriteAtlas;
    private Image uiImage;
    
    protected string spriteNameBlue = "HealthHeartBlue";
    protected string spriteNameRed = "HealthHeart";

    protected void Start()
    {
        uiImage = GetComponent<Image>();
        LoadSprite(spriteNameRed);
    }

    protected void LoadSprite(string spriteName)
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

    public void SpriteUpdate()
    {
        Debug.Log("SpriteUpdate is working");
        if (aspect.ShowToggle() == true && aspect.isActiveAndEnabled) LoadSprite(spriteNameRed);
        else if (aspect.isActiveAndEnabled == false)
        {
            LoadSprite(spriteNameRed);
        }
        else LoadSprite(spriteNameBlue);
    }
}
