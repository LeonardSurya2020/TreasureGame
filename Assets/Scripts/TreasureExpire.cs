using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureExpire : MonoBehaviour
{
    public float invincibleDuration = 2f;   // Durasi invincibility
    public float blinkInterval = 0.1f;      // Interval berkedip

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        float elapsedTime = 0f;
        yield return new WaitForSeconds(10f);

        while (elapsedTime < invincibleDuration)
        {
            // Membalikkan visibilitas sprite
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // Menunggu interval berkedip
            yield return new WaitForSeconds(blinkInterval);

            // Menambah waktu yang berlalu
            elapsedTime += blinkInterval;
        }
        Destroy(gameObject);
    }
}
