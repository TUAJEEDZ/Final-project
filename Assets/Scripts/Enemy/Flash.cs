using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreDefaultmaTime = .2f;

    private Material defaultMat;
    private SpriteRenderer spriteRenderer;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }

    public IEnumerator FlashRoutine()
    {
        if (spriteRenderer != null && whiteFlashMat != null)
        {
            spriteRenderer.material = whiteFlashMat;
            yield return new WaitForSeconds(restoreDefaultmaTime);

            // ตรวจสอบว่า spriteRenderer ยังคงอยู่ก่อนที่จะเปลี่ยน material กลับ
            if (spriteRenderer != null)
            {
                spriteRenderer.material = defaultMat;
            }
        }

        if (enemyHealth != null)
        {
            enemyHealth.DetectDeath(); // เรียกใช้ DetectDeath
        }
    }

}