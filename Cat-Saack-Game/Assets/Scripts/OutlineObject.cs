using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Switches the material on the Object to give it an outline.
public class OutlineObject : MonoBehaviour
{

    public Material originalMaterial;
    public Material outlineMaterial;

    public SpriteRenderer spriteRenderer;
    public Image image;

    public void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (image == null)
        {
            image = GetComponent<Image>();
        }
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
