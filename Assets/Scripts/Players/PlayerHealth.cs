using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool isdead { get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private Slider healthSlider;

    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;
    private Movement playerMovement; // ��Ҷ֧ʤ�Ի����Ǻ����������͹��Ǣͧ����Ф�

    const string TOWN_TEXT = "scene+";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    private void Start()
    {
        isdead = false;
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        playerMovement = GetComponent<Movement>(); // ��Ҷ֧ʤ�Ի����Ǻ����������͹��Ǣͧ����Ф�
        currentHealth = maxHealth;

        if (healthSlider == null)
        {
            Debug.LogError("HealthSlider reference not set in the Inspector.");
            return;
        }

        UpdateHealthSlider();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var enemy = other.gameObject.GetComponent<EnemyAI>() as MonoBehaviour ??
                    other.gameObject.GetComponent<SkeletonAi>() as MonoBehaviour;

        if (enemy != null && canTakeDamage)
        {
            TakeDamage(1);
            knockback.GetKnockedBack(other.gameObject.transform, knockBackThrustAmount);
            StartCoroutine(flash.FlashRoutine());
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider();
        }
    }

    private void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;

        canTakeDamage = false;
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");
        }

        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isdead)
        {
            isdead = true;
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);

            // ��ش�������͹��Ǣͧ����Ф���������ʹ���
            if (playerMovement != null)
            {
                playerMovement.ChangeState(PlayerState.dead); // ����¹ʶҹТͧ����Ф��� 'dead'
            }
            else
            {
                Debug.LogError("Movement script is not assigned or found.");
            }

            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f); // Adjust the delay as needed
        SceneManager.LoadScene(TOWN_TEXT);
    }
}
