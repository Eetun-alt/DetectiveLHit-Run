using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Päivittää buttonien cooldown tekstit ja estää painamisen cooldownin aikana.
/// </summary>
public class BattleUI : MonoBehaviour
{
    [Header("References")]
    public LawrenceAttack lawrenceAttack;
    public BattleManager battleManager;

    [Header("Attack Buttons")]
    public Button curbstompButton;
    public Button disguiseButton;
    public Button ragebaitButton;
    public Button chargeAttackButton;

    [Header("Button Texts (TMP)")]
    public TextMeshProUGUI curbstompText;
    public TextMeshProUGUI disguiseText;
    public TextMeshProUGUI ragebaitText;
    public TextMeshProUGUI chargeAttackText;

    void Start()
    {
        // Yritä hakea referenssit automaattisesti
        if (lawrenceAttack == null)
        {
            LawrenceAttack[] attacks = FindObjectsOfType<LawrenceAttack>();
            if (attacks.Length > 0)
                lawrenceAttack = attacks[0];
        }

        if (battleManager == null)
        {
            BattleManager[] managers = FindObjectsOfType<BattleManager>();
            if (managers.Length > 0)
                battleManager = managers[0];
        }
    }

    void Update()
    {
        if (lawrenceAttack == null) return;

        UpdateButtonState(curbstompButton, curbstompText, "Curbstomp", "CURBSTOMP");
        UpdateButtonState(disguiseButton, disguiseText, "Disguise", "DISGUISE");
        UpdateButtonState(ragebaitButton, ragebaitText, "Ragebait", "RAGEBAIT");
        UpdateButtonState(chargeAttackButton, chargeAttackText, "ChargeAttack", "CHARGE");
    }

    private void UpdateButtonState(Button button, TextMeshProUGUI textMesh, string attackName, string baseName)
    {
        if (button == null) return;

        int cooldown = lawrenceAttack.GetCooldown(attackName);
        bool canUse = lawrenceAttack.CanUseAttack(attackName);

        // Estä tai salli nappi
        button.interactable = canUse;

        // Päivitä teksti
        if (textMesh != null)
        {
            if (cooldown > 0)
            {
                textMesh.text = $"{baseName}\n({cooldown})";
            }
            else
            {
                textMesh.text = baseName;
            }
        }
    }
}