using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColorChange : MonoBehaviour
{
    public List <Color> damageColors;
    public List <GameObject> objectsForColorChange;
    public EnemyHealth health;

    private Color originalColor; 
    private List<Renderer> objectRenderers = new List<Renderer>();

    void Start()
    {
        if (objectsForColorChange != null)
        {
            foreach (var obj in objectsForColorChange)
            {
                if (obj != null)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        objectRenderers.Add(renderer);
                        originalColor = obj.GetComponent<Renderer>().material.color;
                    }
                    else
                    {
                        Debug.LogError("No Renderer found on " + obj.name);
                    }
                }
                else
                {
                    Debug.LogError("One of the objects in objectsForColorChange is null!");
                }
            }
        }
        else
        {
            Debug.LogError("objectsForColorChange is not assigned!");
        }

        if (health == null)
        {
            Debug.LogError("Health reference is not assigned!");
        }

        if (damageColors == null || damageColors.Count < 2)
        {
            Debug.LogError("Damage colors are not properly assigned!");
        }
    }

    public void ChangeDamageColor()
    {
        if (health == null || objectRenderers.Count == 0 || damageColors == null || damageColors.Count < 2)
        {
            return;
        }
        Color newColor = originalColor; // Default color if no match
        switch (health.currentHealth)
        {
            case 2: newColor = damageColors[0]; break;
            case 1: newColor = damageColors[1]; break;
            case 3: newColor = originalColor; break;
        }

        foreach (var renderer in objectRenderers)
        {
            renderer.material.color = newColor;
        }
        //Invoke("ResetColor", 0.5f);
    }

   /* void ResetColor()
    {
        objectRenderer.material.color = originalColor;
    }*/
}
