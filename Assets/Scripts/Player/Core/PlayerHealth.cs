using System.Collections;
using UnityEngine;
using DG.Tweening;
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

    [Header("References")]

    [SerializeField]
    private Transform visual;

    #endregion

    #region Animations

    [Header("Death Animation")]

    [SerializeField]
    private float deathJumpHeight = 1.5f;

    [SerializeField]
    private float deathJumpDuration = 0.3f;

    [SerializeField]
    private float deathPauseDuration = 0.6f;

    [SerializeField]
    private float deathFallDistance = 8f;

    [SerializeField]
    private float deathFallDuration = 0.9f;

    [Header("Invulnerability Flash")]

    [SerializeField]
    private float flashInterval = 0.1f;

    #endregion

    #region State

    private int currentHealth;

    private int currentLives;

    private bool isInvulnerable;

    private bool isDead;

    private Coroutine invulnerabilityCoroutine;

    private PlayerController controller;

    private PlayerRespawn respawn;

    private int playerLayer;
    private int enemyLayer;

    private Sequence deathSequence;

    private SpriteRenderer visualRenderer;

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

        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");

        if (visual != null)
        {
            visualRenderer =
                visual.GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentLives = startingLives;
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

    private void ApplyKnockback(
        Vector2 damageSource)
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
            StopCoroutine(
                invulnerabilityCoroutine);
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

        StartCoroutine(
            FlashRoutine());

        yield return new WaitForSeconds(
            invulnerabilityTime);

        Physics2D.IgnoreLayerCollision(
            playerLayer,
            enemyLayer,
            false);

        isInvulnerable = false;

        if (visualRenderer != null)
        {
            visualRenderer.enabled = true;
        }

        invulnerabilityCoroutine = null;
    }

    private IEnumerator FlashRoutine()
    {
        if (visualRenderer == null)
            yield break;

        while (isInvulnerable)
        {
            visualRenderer.enabled = false;

            yield return new WaitForSeconds(
                flashInterval);

            visualRenderer.enabled = true;

            yield return new WaitForSeconds(
                flashInterval);
        }

        visualRenderer.enabled = true;
    }

    private IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(
            hurtDuration);

        controller.IsHurt = false;
    }

    private IEnumerator PlayDeathAnimation()
    {
        if (visual == null)
            yield break;

        Vector3 startPosition = visual.localPosition;

        deathSequence = DOTween.Sequence();

        deathSequence
            // Subida del jugador
            .Append(
                visual.DOLocalMoveY(
                    startPosition.y + deathJumpHeight,
                    deathJumpDuration)
                .SetEase(Ease.OutQuad)
            )

            // Mantenerse en el aire
            .AppendInterval(
                deathPauseDuration)

            // Caída del jugador
            .Append(
                visual.DOLocalMoveY(
                    startPosition.y - deathFallDistance,
                    deathFallDuration)
                .SetEase(Ease.InQuad)
            );


        yield return deathSequence.WaitForCompletion();


        // Restauramos la posición del sprite
        if(currentLives > 0)
        {
            visual.localPosition = startPosition;
        }       
    }

    private IEnumerator DeathRoutine()
    {
        isDead = true;
        controller.IsDead = true;

        currentLives--;

        // 1. Detener cualquier movimiento
        controller.ApplyImpulse(Vector2.zero);
        controller.Rigidbody2D.linearVelocity = Vector2.zero;

        // 2. Desactivar la física
        controller.Rigidbody2D.simulated = false;

        // 3. Ignorar colisiones con enemigos
        Physics2D.IgnoreLayerCollision(
            playerLayer,
            enemyLayer,
            true);

        controller.Collider.enabled = false;

        // 4. Animación de muerte
        yield return PlayDeathAnimation();

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

        // Sin vidas restantes
        SceneManager.LoadScene("GameOver");
    }

    #endregion
}