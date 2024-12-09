using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Material flashMat;
    private Material originalMat;
    private Color originalColor;

    [Header("Default flash settings")]
    [SerializeField]
    float defaultDuration = 0.1f;
    Color defaultFlashColor = Color.white;

    public void Flash(float aDuration, Color aColor)
    {
        originalMat = spriteRenderer.material;
        originalColor = spriteRenderer.color;
        spriteRenderer.material = flashMat;
        spriteRenderer.color = aColor;
        StartCoroutine(FlashCoroutine(aDuration));
    }

    public void Flash()
    {
        Flash(defaultDuration, defaultFlashColor);
    }

    IEnumerator FlashCoroutine(float aDuration)
    {
        yield return new WaitForSeconds(aDuration);
        spriteRenderer.material = originalMat;
        spriteRenderer.color = originalColor;
    }
}
