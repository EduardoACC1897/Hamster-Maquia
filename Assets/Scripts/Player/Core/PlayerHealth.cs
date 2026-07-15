using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    #region Settings

    [Header("Health")]

    [SerializeField]
    private int maxHealth = 3;

    [SerializeField]
    private int startingLives = 3;

    [Header("Damage")]

    [SerializeField]
    private Vector2 knockbackForce =
        new(8f, 10f);

    [SerializeField]
    private float hurtDuration = 0.25f;

    [SerializeField]
    private float invulnerabilityTime = 3f;

    #endregion

    #region State

    private int currentHealth;

    private int currentLives;

    private bool isInvulnerable;

    private bool isDead;

    private Coroutine invulnerabilityCoroutine;

    private PlayerController controller;

    private PlayerRespawn respawn;

    private PlayerAnimation playerAnimation;

    private int playerLayer;

    private int enemyLayer;

    #endregion

    #region Public Properties

    public int CurrentHealth => currentHealth;

    public int MaxHealth => maxHealth;

    public int CurrentLives => currentLives;

    public bool IsDead => isDead;

    public bool IsInvulnerable => isInvulnerable;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        respawn = GetComponent<PlayerRespawn>();
        playerAnimation = GetComponent<PlayerAnimation>();

        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");

        currentHealth = maxHealth;

        if (PlayerDataManager.Instance.RemainingLives == -1)
        {
            currentLives = startingLives;

            PlayerDataManager.Instance.SetLives(
                currentLives);
        }
        else
        {
            currentLives =
                PlayerDataManager.Instance.RemainingLives;
        }
    }

    #endregion

    #region Public Methods

    public void TakeDamage(
        int damage,
        Vector2 damageSource)
    {
        if (isDead)
            return;

        if (isInvulnerable)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        controller.IsHurt = true;

        ApplyKnockback(damageSource);

        StartCoroutine(HurtRoutine());

        StartInvulnerability();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(
            currentHealth + amount,
            maxHealth);
    }

    #endregion

    #region Private Methods

    private void ApplyKnockback(Vector2 damageSource)
    {
        float direction = Mathf.Sign(
            transform.position.x -
            damageSource.x);

        controller.ApplyImpulse(
            new Vector2(
                direction * knockbackForce.x,
                knockbackForce.y));
    }

    private void Die()
    {
        StartCoroutine(DeathRoutine());
    }

    private void RestoreHealth()
    {
        currentHealth = maxHealth;
    }

    private void StartInvulnerability()
    {
        if (invulnerabilityCoroutine != null)
        {
            StopCoroutine(invulnerabilityCoroutine);
        }

        invulnerabilityCoroutine =
            StartCoroutine(
                InvulnerabilityRoutine());
    }

    #endregion

    #region Coroutines

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;

        Physics2D.IgnoreLayerCollision(
            playerLayer,
            enemyLayer,
            true);

        playerAnimation.StartFlash(
            invulnerabilityTime);

        yield return new WaitForSeconds(
            invulnerabilityTime);

        Physics2D.IgnoreLayerCollision(
            playerLayer,
            enemyLayer,
            false);

        isInvulnerable = false;

        playerAnimation.StopFlash();

        invulnerabilityCoroutine = null;
    }

    private IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(
            hurtDuration);

        controller.IsHurt = false;
    }

    private IEnumerator DeathRoutine()
    {
        isDead = true;
        controller.IsDead = true;

        currentLives--;

        PlayerDataManager.Instance.SetLives(
            currentLives);

        controller.ApplyImpulse(Vector2.zero);
        controller.Rigidbody2D.linearVelocity =
            Vector2.zero;

        controller.Rigidbody2D.simulated = false;

        Physics2D.IgnoreLayerCollision(
            playerLayer,
            enemyLayer,
            true);

        controller.Collider.enabled = false;

        yield return playerAnimation.PlayDeathAnimation(
            currentLives > 0);

        if (currentLives > 0)
        {
            RestoreHealth();

            controller.Collider.enabled = true;

            respawn.Respawn();

            controller.Rigidbody2D.simulated = true;

            StartInvulnerability();

            controller.IsDead = false;
            isDead = false;

            yield break;
        }

        PlayerDataManager.Instance.ResetRun();

        SceneManager.LoadScene("GameOver");
    }

    #endregion
}