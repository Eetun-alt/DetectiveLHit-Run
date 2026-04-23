using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Hallitsee taistelun kulkua ja kutsuu hyökkäyksiä.
/// </summary>
public class BattleManager : MonoBehaviour
{
    [Header("Battle Participants")]
    public LawrenceAttack playerAttack;  // Lawrence
    public CharacterHealth playerHealth; // Lawrence Health
    public CharacterHealth enemyHealth;  // Vihollinen Health

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
        
        // TODO: Lisää vihollisen AI-logiikka tähän
        // Esimerkiksi: Random hyökkäys
        // int randomAttack = Random.Range(0, 3);
        // if (randomAttack == 0)
        //     enemyAttack.UseBasicAttack();
        // else if (randomAttack == 1)
        //     enemyAttack.UseDefend();
        // etc.
        
        // Väliaikaisesti: Pelaajan vuoro alkaa uudelleen 2 sekunnin päästä
        Invoke(nameof(StartPlayerTurn), 2f);
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
    }
}
