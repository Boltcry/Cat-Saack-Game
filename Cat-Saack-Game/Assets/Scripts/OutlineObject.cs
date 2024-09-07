using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Switches the material on the Object to give it an outline.
public class OutlineObject : MonoBehaviour
{

    public Material originalMaterial;
    public Material outlineMaterial;

    SpriteRenderer spriteRenderer;
    Image image;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
    }

    // Shows or hides outline based on given bool.
    public void SetOutlineActive(bool aActive)
    {
        if(aActive)
        {
            if (image != null)
            {
                image.material = outlineMaterial;
            }
            if (spriteRenderer != null)
            {
                spriteRenderer.material = outlineMaterial;
            }
        }
        else if (!aActive)
        {
            if (image != null)
            {
                image.material = originalMaterial;
            }
            if (spriteRenderer != null)
            {
                spriteRenderer.material = originalMaterial;
            }
        }
    }
}
