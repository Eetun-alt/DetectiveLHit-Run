using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TaisteluAlku : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform enemy;
    [Tooltip("Where the player should end up on the battlefield.")]
    public Transform playerTarget;
    [Tooltip("Where the enemy should end up on the battlefield.")]
    public Transform enemyTarget;

    [Header("Slide Settings")]
    [Tooltip("How long the slide-in lasts (seconds)")]
    public float slideDuration = 0.8f;
    [Tooltip("Easing curve (use EaseInOut for smooth motion)")]
    public AnimationCurve ease = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    [Header("Offsets")]
    [Tooltip("Player start offset relative to playerTarget (local units). Default slides from left.")]
    public Vector3 playerOffset = new Vector3(-10f, 0f, 0f);
    [Tooltip("Enemy start offset relative to enemyTarget (local units). Default slides from right.")]
    public Vector3 enemyOffset = new Vector3(10f, 0f, 0f);

    [Header("Behaviour")]
    public bool startOnAwake = false;
    public UnityEvent onBattleStarted; // Tapahtuu kun slide-in on valmis

    void Awake()
    {
        // Hahmot spawnaa kameran ulkopuolelle jos ne on asetettu editorissa tähän scriptiin.
        if (player != null)
        {
            Vector3 start = (playerTarget != null) ? playerTarget.position + playerOffset : player.position + playerOffset;
            player.position = start;
        }

        if (enemy != null)
        {
            Vector3 start = (enemyTarget != null) ? enemyTarget.position + enemyOffset : enemy.position + enemyOffset;
            enemy.position = start;
        }
    }

    void Start()
    {
        if (startOnAwake)
            StartBattle();
    }


    public void StartBattle()
    {
        StopAllCoroutines();
        StartCoroutine(DoSlideIn());
    }

    IEnumerator DoSlideIn()
    {
        float t = 0f;

        Vector3 pFrom = (player != null) ? player.position : Vector3.zero;
        Vector3 eFrom = (enemy != null) ? enemy.position : Vector3.zero;

        Vector3 pTo = (playerTarget != null) ? playerTarget.position : pFrom - playerOffset;
        Vector3 eTo = (enemyTarget != null) ? enemyTarget.position : eFrom - enemyOffset;

        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / slideDuration);
            float eased = ease.Evaluate(normalized);

            if (player != null)
                player.position = Vector3.LerpUnclamped(pFrom, pTo, eased);
            if (enemy != null)
                enemy.position = Vector3.LerpUnclamped(eFrom, eTo, eased);

            yield return null;
        }

        // Ensure final positions
        if (player != null)
            player.position = pTo;
        if (enemy != null)
            enemy.position = eTo;

        // Callback
        onBattleStarted?.Invoke();
        Debug.Log("BattleStart: slide-in complete.");
    }
}
