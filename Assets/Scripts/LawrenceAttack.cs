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
    
    [Header("Attack Stats - Muokattavissa Inspectorissa")]
    [Tooltip("Curbstomp: Perusiskun damage (15)")]
    public int curbstompDamage = 15;
    
    [Tooltip("Ragebait: Bleed damage per kierros (5)")]
    public int ragebaitDamage = 5;
    
    [Tooltip("Ragebait: Kuinka monta kierrosta bleed kestää (2)")]
    public int ragebaitDuration = 2;
    
    [Tooltip("Charge Attack: Heal prosenttina max HP (50%)")]
    public float chargeAttackHealPercent = 0.5f;
    
    [Tooltip("Disguise: Accuracy vähennys prosentteina (25%)")]
    public float disguiseAccuracyReduction = 0.25f;
    
    [Tooltip("Disguise: Kuinka monta kierrosta kestää (2)")]
    public int disguiseDuration = 2;
    
    [Header("Cooldowns (kierroksia) - Alkuarvot")]
    public int curbstompCooldown = 0;
    public int disguiseCooldown = 2;  // Ei voi käyttää heti
    public int ragebaitCooldown = 3;  // Ei voi käyttää heti
    public int chargeAttackCooldown = 2;  // Ei voi käyttää heti
    
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
    /// Tarkista onko hyökkäys käytettävissä (ei cooldownia)
    /// </summary>
    public bool CanUseAttack(string attackName)
    {
        switch (attackName)
        {
            case "Curbstomp": return curbstompCooldown <= 0;
            case "Disguise": return disguiseCooldown <= 0;
            case "Ragebait": return ragebaitCooldown <= 0;
            case "ChargeAttack": return chargeAttackCooldown <= 0;
            default: return false;
        }
    }

    /// <summary>
    /// Päivitä cooldownit (kutsutaan BattleManagerista jokaisen kierroksen lopussa)
    /// </summary>
    public void UpdateCooldowns()
    {
        if (curbstompCooldown > 0) curbstompCooldown--;
        if (disguiseCooldown > 0) disguiseCooldown--;
        if (ragebaitCooldown > 0) ragebaitCooldown--;
        if (chargeAttackCooldown > 0) chargeAttackCooldown--;
        
        Debug.Log($"Cooldowns: Curbstomp={curbstompCooldown}, Disguise={disguiseCooldown}, Ragebait={ragebaitCooldown}, Charge={chargeAttackCooldown}");
    }

    /// <summary>
    /// Nollaa kaikki cooldownit (taistelun alussa)
    /// </summary>
    public void ResetCooldowns()
    {
        curbstompCooldown = 0;
        disguiseCooldown = 0;
        ragebaitCooldown = 0;
        chargeAttackCooldown = 0;
        Debug.Log("All cooldowns reset!");
    }

    /// <summary>
    /// Disguise: Vihollisen osumistodennäköisyys -25% 2 kierroksen ajaksi.
    /// </summary>
    public void UseDisguise()
    {
        if (enemyHealth == null) return;
        if (disguiseCooldown > 0)
        {
            Debug.LogWarning($"Disguise on cooldown! ({disguiseCooldown} kierrosta jäljellä)");
            return;
        }
        
        // Add accuracy debuff to enemy - kesto alkaa seuraavasta vihollisen vuorosta
        statusEffects.AddEffect(new StatusEffect("AccuracyDown", disguiseDuration, -disguiseAccuracyReduction));
        disguiseCooldown = 2; // Aseta cooldown käytön jälkeen
        
        Debug.Log($"Lawrence uses Disguise! Enemy's accuracy reduced by {disguiseAccuracyReduction*100}% for {disguiseDuration} rounds. (Cooldown: {disguiseCooldown})");
    }

    /// <summary>
    /// Curbstomp: Lawrencen perusisku.
    /// </summary>
    public void UseCurbstomp()
    {
        if (enemyHealth == null) return;
        if (curbstompCooldown > 0)
        {
            Debug.LogWarning($"Curbstomp on cooldown! ({curbstompCooldown} kierrosta jäljellä)");
            return;
        }

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
        
        curbstompCooldown = 0; // Ei cooldownia
    }

    /// <summary>
    /// Ragebait: Lawrence mumisee vastustajalle ja vastustaja ottaa bleed damagea.
    /// </summary>
    public void UseRagebait()
    {
        if (enemyHealth == null) return;
        if (ragebaitCooldown > 0)
        {
            Debug.LogWarning($"Ragebait on cooldown! ({ragebaitCooldown} kierrosta jäljellä)");
            return;
        }

        // Apply bleed status effect
        statusEffects.AddEffect(new StatusEffect("Bleed", ragebaitDuration, ragebaitDamage, effectType: StatusEffectType.DoT));
        ragebaitCooldown = 3; // Aseta cooldown käytön jälkeen
        
        Debug.Log($"Lawrence uses Ragebait! Enemy will take {ragebaitDamage} bleed damage for {ragebaitDuration} rounds. (Cooldown: {ragebaitCooldown})");
    }

    /// <summary>
    /// Charge Attack: Lawrence palauttaa 50% HP ja latautuu jokaisen kierroksen lopussa.
    /// </summary>
    public void UseChargeAttack()
    {
        if (ownHealth == null) return;
        if (chargeAttackCooldown > 0)
        {
            Debug.LogWarning($"Charge Attack on cooldown! ({chargeAttackCooldown} kierrosta jäljellä)");
            return;
        }

        chargeCount++;
        
        // Calculate healing amount based on charge count
        int baseHeal = (int)(ownHealth.maxHealth * chargeAttackHealPercent);
        int chargedHeal = (int)(baseHeal * (1.0f + chargeCount * 0.2f)); // Each charge adds 20%
        
        ownHealth.currentHealth = Mathf.Min(ownHealth.currentHealth + chargedHeal, ownHealth.maxHealth);
        
        Debug.Log($"Lawrence uses Charge Attack! Recovered {chargedHeal} HP (Charge level: {chargeCount}/{maxChargeCount})");
        
        chargeAttackCooldown = 2; // Aseta cooldown käytön jälkeen
        
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
    /// Hae bleed damage (kutsutaan BattleManagerista)
    /// </summary>
    public int GetBleedDamage()
    {
        StatusEffect bleedEffect = statusEffects.GetEffect("Bleed");
        return bleedEffect != null ? (int)bleedEffect.value : 0;
    }

    /// <summary>
    /// Get current charge count for UI display.
    /// </summary>
    public int GetChargeCount() => chargeCount;
    public int GetMaxChargeCount() => maxChargeCount;
    
    /// <summary>
    /// Get cooldown info for UI
    /// </summary>
    public int GetCooldown(string attackName)
    {
        switch (attackName)
        {
            case "Curbstomp": return curbstompCooldown;
            case "Disguise": return disguiseCooldown;
            case "Ragebait": return ragebaitCooldown;
            case "ChargeAttack": return chargeAttackCooldown;
            default: return 0;
        }
    }
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
    private bool appliedThisRound = false;

    // Effect expires when currentRound > durationRounds (eli effect kestää oikeasti duration kierrosta)
    public bool IsExpired => currentRound > durationRounds;

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
        appliedThisRound = false;
    }

    public bool ShouldApplyDamage()
    {
        // Apply damage once per round, starting from the round after the effect is applied
        if (!appliedThisRound && currentRound <= durationRounds && currentRound > 0)
        {
            appliedThisRound = true;
            return true;
        }
        return false;
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
