using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// - Kutsu TakeDamage(amount)-funktiota aiheuttaaksesi vahinkoa.
/// </summary>
public class CharacterHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    [Tooltip("Current health. Set in inspector for testing; clamped to [0,maxHealth] on Awake.")]
    public int currentHealth = -1;

    [Header("Events")]
    public UnityEvent<int> onDamaged; // int = damagen määrä
    public UnityEvent onDeath;

    bool _isDead = false;

    public bool IsDead => _isDead;

    void Awake()
    {
        if (maxHealth <= 0) maxHealth = 1;
        if (currentHealth < 0) currentHealth = maxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        _isDead = (currentHealth == 0);
    }

    /// <summary>
    /// Hahmo ottaa vahinkoa, jos hahmo on elossa.
    /// </summary>
    /// <param name="amount">Damage amount (positive integer)</param>
    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;
        if (_isDead) return;

        int prev = currentHealth;
        currentHealth = Mathf.Max(0, currentHealth - amount);

        int applied = prev - currentHealth;
        onDamaged?.Invoke(applied);

        Debug.LogFormat("{0} took {1} damage (HP {2}/{3})", gameObject.name, applied, currentHealth, maxHealth);

        if (currentHealth == 0)
            Die();
    }

    void Die()
    {
        if (_isDead) return;
        _isDead = true;
        Debug.LogFormat("{0} died.", gameObject.name);
        onDeath?.Invoke();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        _isDead = false;
    }
}
