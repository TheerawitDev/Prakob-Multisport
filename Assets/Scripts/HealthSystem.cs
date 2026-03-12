using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public float invincibilityDuration = 2f;
    private int currentHealth;

    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent OnDamageTaken;
    public UnityEvent OnDeath;

    private bool isDead = false;
    private bool isInvincible = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damageAmount, bool bypassInvincibility = false)
    {
        if (isDead) return;
        if (isInvincible && !bypassInvincibility) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamageTaken?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (invincibilityDuration > 0 && !bypassInvincibility)
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    public void Heal(int healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}