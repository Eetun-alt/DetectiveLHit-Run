using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Lawrencen hyökkäyssysteemi pokémon-tyylisellä taistelumekaniikalla.
/// </summary>
public class LawrenceAttack : MonoBehaviour
{
    [Header("Character References")]
    public CharacterHealth ownHealth;
    public CharacterHealth enemyHealth;
    
    [Header("Attack Stats")]
    public int curbstompDamage = 15;
    public int ragebaitDamage = 5; // bleed damage per round
    public int ragebaitDuration = 2;
    public float chargeAttackHealPercent = 0.5f; // 50% HP recovery
    
    // Status effects tracking
    private StatusEffectManager statusEffects = new StatusEffectManager();
    
    // Charge attack state
    private int chargeCount = 0;
    private const int maxChargeCount = 3;

    void Start()
    {
        if (ownHealth == null)
            ownHealth = GetComponent<CharacterHealth>();
    }

    void Update()
    {
        // Update status effects each frame
        statusEffects.Update();
    }

    /// <summary>
    /// Disguise: Opponent's hit chance is reduced by 25% for 2 rounds.
    /// </summary>
    public void UseDisguise()
    {
        if (enemyHealth == null) return;
        
        // Add accuracy debuff to enemy
        statusEffects.AddEffect(new StatusEffect("AccuracyDown", 2, -0.25f));
        Debug.Log($"Lawrence uses Disguise! Enemy's accuracy reduced by 25% for 2 rounds.");
    }

    /// <summary>
    /// Curbstomp: Lawrence's basic attack.
    /// </summary>
    public void UseCurbstomp()
    {
        if (enemyHealth == null) return;

        // Apply damage with potential accuracy check
        float accuracy = 1.0f - GetEnemyAccuracyDebuff();
        float hitRoll = Random.value;

        if (hitRoll <= accuracy)
        {
            enemyHealth.TakeDamage(curbstompDamage);
            Debug.Log($"Lawrence uses Curbstomp! Hits for {curbstompDamage} damage.");
        }
        else
        {
            Debug.Log("Lawrence uses Curbstomp, but it missed!");
        }
    }

    /// <summary>
    /// Ragebait: Lawrence taunts the opponent, who takes bleed damage for the next couple of turns.
    /// </summary>
    public void UseRagebait()
    {
        if (enemyHealth == null) return;

        // Apply bleed status effect
        statusEffects.AddEffect(new StatusEffect("Bleed", ragebaitDuration, ragebaitDamage, effectType: StatusEffectType.DoT));
        Debug.Log($"Lawrence uses Ragebait! Enemy will take {ragebaitDamage} bleed damage for {ragebaitDuration} rounds.");
    }

    /// <summary>
    /// Charge attack, Young T: Lawrence recovers 50% HP. Charges up a little each round.
    /// </summary>
    public void UseChargeAttack()
    {
        if (ownHealth == null) return;

        chargeCount++;
        
        // Calculate healing amount based on charge count
        int baseHeal = (int)(ownHealth.maxHealth * chargeAttackHealPercent);
        int chargedHeal = (int)(baseHeal * (1.0f + chargeCount * 0.2f)); // Each charge adds 20%
        
        ownHealth.currentHealth = Mathf.Min(ownHealth.currentHealth + chargedHeal, ownHealth.maxHealth);
        
        Debug.Log($"Lawrence uses Charge Attack! Recovered {chargedHeal} HP (Charge level: {chargeCount}/{maxChargeCount})");
        
        // Reset charge after use if maxed
        if (chargeCount >= maxChargeCount)
            ResetChargeAttack();
    }

    /// <summary>
    /// Reset the charge attack counter.
    /// </summary>
    public void ResetChargeAttack()
    {
        chargeCount = 0;
    }

    /// <summary>
    /// Get current accuracy debuff from enemy status effects.
    /// </summary>
    private float GetEnemyAccuracyDebuff()
    {
        StatusEffect accuracyEffect = statusEffects.GetEffect("AccuracyDown");
        return accuracyEffect != null ? -accuracyEffect.value : 0.0f;
    }

    /// <summary>
    /// Get current charge count for UI display.
    /// </summary>
    public int GetChargeCount() => chargeCount;
    public int GetMaxChargeCount() => maxChargeCount;
}

/// <summary>
/// Tracks status effects and debuffs during battle.
/// </summary>
public class StatusEffectManager
{
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    public void AddEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
    }

    public StatusEffect GetEffect(string name)
    {
        return activeEffects.Find(e => e.name == name);
    }

    public void Update()
    {
        // Process damage-over-time effects and update durations
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect effect = activeEffects[i];
            effect.Update();

            // Apply damage if it's a DoT effect
            if (effect.effectType == StatusEffectType.DoT && effect.ShouldApplyDamage())
            {
                // Damage will be applied by the battle system
                Debug.Log($"Status effect '{effect.name}' applies {effect.value} damage.");
            }

            // Remove expired effects
            if (effect.IsExpired)
            {
                activeEffects.RemoveAt(i);
                Debug.Log($"Status effect '{effect.name}' expired.");
            }
        }
    }

    public void Clear()
    {
        activeEffects.Clear();
    }
}

/// <summary>
/// Represents a single status effect (buff/debuff/DoT).
/// </summary>
public class StatusEffect
{
    public string name;
    public int durationRounds;
    public float value; // For debuffs like accuracy reduction, or damage amounts
    public StatusEffectType effectType;
    private int currentRound = 0;

    public bool IsExpired => currentRound >= durationRounds;

    public StatusEffect(string name, int duration, float value = 0f, StatusEffectType effectType = StatusEffectType.Debuff)
    {
        this.name = name;
        this.durationRounds = duration;
        this.value = value;
        this.effectType = effectType;
    }

    public void Update()
    {
        currentRound++;
    }

    public bool ShouldApplyDamage()
    {
        return currentRound % 1 == 0; // Apply damage every round for DoT
    }
}

/// <summary>
/// Types of status effects.
/// </summary>
public enum StatusEffectType
{
    Buff,      // Positive effect
    Debuff,    // Negative effect (accuracy, defense, etc)
    DoT        // Damage over time (bleed, poison, etc)
}
