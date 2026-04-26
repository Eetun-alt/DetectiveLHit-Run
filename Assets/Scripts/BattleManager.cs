using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// Hallitsee taistelun kulkua ja kutsuu hyökkäyksiä.
/// </summary>
public class BattleManager : MonoBehaviour
{
    [Header("Battle Participants")]
    public LawrenceAttack playerAttack;  // Lawrence
    public CharacterHealth playerHealth; // Lawrence Health
    public CharacterHealth enemyHealth;  // Vihollinen Health

    [Header("Scene Transition")]
    [Tooltip("Nimi scenestä johon siirrytään taistelun jälkeen")]
    public string nextSceneName = "Kartta";

    [Header("State")]
    public bool isPlayerTurn = true;
    public UnityEvent onPlayerTurnStart;
    public UnityEvent onPlayerTurnEnd;
    public UnityEvent onBattleEnd;

    void Start()
    {
        // Pyyhi vanhat efektit kun taistelu alkaa
        if (playerAttack != null)
            playerAttack.ResetChargeAttack();
    }

    void Update()
    {
        // Tarkista onko taistelu ohi
        if (playerHealth != null && playerHealth.IsDead)
        {
            Debug.Log("Lawrence died!");
            EndBattle();
        }
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            Debug.Log("Enemy died!");
            EndBattle();
        }
    }

    /// <summary>
    /// Kutsutaan kun pelaaja painaa Curbstomp-nappia
    /// </summary>
    public void PlayerAction_Curbstomp()
    {
        Debug.Log("BattleManager: Curbstomp clicked.");
        if (!isPlayerTurn)
        {
            Debug.LogWarning("BattleManager: Curbstomp ignored because it is not the player's turn.");
            return;
        }
        if (playerAttack == null)
        {
            Debug.LogError("BattleManager: Curbstomp failed because playerAttack is not assigned.");
            return;
        }
        
        playerAttack.UseCurbstomp();
        EndPlayerTurn();
    }

    /// <summary>
    /// Kutsutaan kun pelaaja painaa Disguise-nappia
    /// </summary>
    public void PlayerAction_Disguise()
    {
        Debug.Log("BattleManager: Disguise clicked.");
        if (!isPlayerTurn)
        {
            Debug.LogWarning("BattleManager: Disguise ignored because it is not the player's turn.");
            return;
        }
        if (playerAttack == null)
        {
            Debug.LogError("BattleManager: Disguise failed because playerAttack is not assigned.");
            return;
        }
        
        playerAttack.UseDisguise();
        EndPlayerTurn();
    }

    /// <summary>
    /// Kutsutaan kun pelaaja painaa Ragebait-nappia
    /// </summary>
    public void PlayerAction_Ragebait()
    {
        Debug.Log("BattleManager: Ragebait clicked.");
        if (!isPlayerTurn)
        {
            Debug.LogWarning("BattleManager: Ragebait ignored because it is not the player's turn.");
            return;
        }
        if (playerAttack == null)
        {
            Debug.LogError("BattleManager: Ragebait failed because playerAttack is not assigned.");
            return;
        }
        
        playerAttack.UseRagebait();
        EndPlayerTurn();
    }

    /// <summary>
    /// Kutsutaan kun pelaaja painaa Charge Attack-nappia
    /// </summary>
    public void PlayerAction_ChargeAttack()
    {
        Debug.Log("BattleManager: Charge Attack clicked.");
        if (!isPlayerTurn)
        {
            Debug.LogWarning("BattleManager: Charge Attack ignored because it is not the player's turn.");
            return;
        }
        if (playerAttack == null)
        {
            Debug.LogError("BattleManager: Charge Attack failed because playerAttack is not assigned.");
            return;
        }
        
        playerAttack.UseChargeAttack();
        EndPlayerTurn();
    }

    private void EndPlayerTurn()
    {
        isPlayerTurn = false;
        onPlayerTurnEnd?.Invoke();
        Debug.Log("Player turn ended.");
        
        // Async esimerkki: vihollisen vuoro alkaa 1 sekunnin päästä
        Invoke(nameof(StartEnemyTurn), 1f);
    }

    private void StartEnemyTurn()
    {
        if (enemyHealth == null || enemyHealth.IsDead) return;
        
        Debug.Log("Enemy's turn!");
        
        // Vihollisen AI-logiikka - tasapainotettu pelaajan kanssa
        ExecuteEnemyAI();
        
        // Pelaajan vuoro alkaa 1.5 sekunnin päästä
        Invoke(nameof(StartPlayerTurn), 1.5f);
    }

    /// <summary>
    /// Vihollisen tekoäly - tasapainotettu vaikeustaso
    /// </summary>
    private void ExecuteEnemyAI()
    {
        if (playerHealth == null || enemyHealth == null) return;
        
        int playerHP = playerHealth.currentHealth;
        int playerMaxHP = playerHealth.maxHealth;
        int enemyHP = enemyHealth.currentHealth;
        int enemyMaxHP = enemyHealth.maxHealth;
        
        // Laske pelaajan HP prosenttina
        float playerHPPercent = (float)playerHP / playerMaxHP;
        float enemyHPPercent = (float)enemyHP / enemyMaxHP;
        
        int attackChoice;
        
        // Strategia: Jos pelaaja on low HP, käytä voimakasta hyökkäystä
        if (playerHPPercent < 0.25f)
        {
            // 70% todennäköisyys voimakkaaseen hyökkäykseen
            attackChoice = Random.value < 0.7f ? 0 : Random.Range(1, 4);
        }
        // Strategia: Jos vihollinen on low HP, parannus tai puolustus
        else if (enemyHPPercent < 0.3f)
        {
            // 50% todennäköisyys parannukseen tai puolustukseen
            attackChoice = Random.value < 0.5f ? 3 : Random.Range(0, 3);
        }
        // Normaali tilanne: Satunnainen valinta
        else
        {
            attackChoice = Random.Range(0, 4);
        }
        
        // Suorita valittu hyökkäys
        switch (attackChoice)
        {
            case 0: // Basic attack - 12-18 damage (hieman heikompi kuin pelaajan 15)
                int basicDamage = Random.Range(12, 19);
                playerHealth.TakeDamage(basicDamage);
                Debug.Log($"Enemy uses Basic Attack! Deals {basicDamage} damage.");
                break;
                
            case 1: // Heavy attack - 20-25 damage (harvinainen)
                int heavyDamage = Random.Range(20, 26);
                playerHealth.TakeDamage(heavyDamage);
                Debug.Log($"Enemy uses Heavy Attack! Deals {heavyDamage} damage.");
                break;
                
            case 2: // Quick attack - 8-12 damage + mahdollisuus toiseen iskuun
                int quickDamage = Random.Range(8, 13);
                playerHealth.TakeDamage(quickDamage);
                Debug.Log($"Enemy uses Quick Attack! Deals {quickDamage} damage.");
                
                // 30% todennäköisyys toiselle iskulle
                if (Random.value < 0.3f)
                {
                    int bonusDamage = Random.Range(5, 9);
                    playerHealth.TakeDamage(bonusDamage);
                    Debug.Log($"Enemy follows up with a second hit! +{bonusDamage} bonus damage.");
                }
                break;
                
            case 3: // Heal - parantaa itseään
                int healAmount = (int)(enemyMaxHP * 0.2f); // 20% max HP
                enemyHealth.currentHealth = Mathf.Min(enemyHP + healAmount, enemyMaxHP);
                Debug.Log($"Enemy uses Heal! Recovered {healAmount} HP. (Now at {enemyHealth.currentHealth}/{enemyMaxHP})");
                break;
        }
    }

    private void StartPlayerTurn()
    {
        if (playerHealth == null || playerHealth.IsDead) return;
        
        isPlayerTurn = true;
        onPlayerTurnStart?.Invoke();
        Debug.Log("Player's turn!");
    }

    private void EndBattle()
    {
        isPlayerTurn = false;
        onBattleEnd?.Invoke();
        Debug.Log("Battle ended.");
        
        // Siirry seuraavaan sceneen
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"Loading scene: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
