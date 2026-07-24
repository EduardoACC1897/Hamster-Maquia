using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    #region References

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private readonly List<PlayerAbility> abilities = new();

    private PlayerInputHandler input;

    #endregion

    #region Settings

    [Header("Gravity")]
    [SerializeField] private float gravityScale = 4f;
    [SerializeField] private float fallGravityMultiplier = 1.5f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float jumpApexThreshold = 2f;
    [SerializeField] private float apexGravityMultiplier = 0.5f;

    #endregion

    #region State

    public bool IsGrounded { get; private set; }

    public Vector2 TargetVelocity { get; set; }

    public bool IsCrouching { get; set; }

    public bool IsDropping { get; set; }

    public bool IsAttacking { get; set; }

    public bool IsHurt { get; set; }

    public bool IsHealing { get; set; }

    public bool IsDead { get; set; }

    public bool IsGravityPaused { get; set; }

    public float MovementSpeedMultiplier { get; private set; } = 1f;

    public int FacingDirection { get; private set; } = 1;

    #endregion

    #region Public Properties

    public Rigidbody2D Rigidbody2D => rb;

    public BoxCollider2D Collider => boxCollider;

    public Transform GroundCheck => groundCheck;

    public LayerMask GroundLayer => groundLayer;

    public float GroundCheckRadius => groundCheckRadius;

    public float GravityScale => gravityScale;

    public float HorizontalVelocity
    {
        get => TargetVelocity.x;
        set => TargetVelocity = new Vector2(value, TargetVelocity.y);
    }

    public float VerticalVelocity
    {
        get => TargetVelocity.y;
        set => TargetVelocity = new Vector2(TargetVelocity.x, value);
    }

    #endregion

    #region Unity Messages

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        abilities.AddRange(GetComponents<PlayerAbility>());

        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        UpdateFacingDirection(input.MoveX);

        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].OnCustomUpdate();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();

        TargetVelocity = rb.linearVelocity;

        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].OnCustomFixedUpdate();
        }

        ApplyGravity();

        rb.linearVelocity = TargetVelocity;
    }

    #endregion

    #region Private Methods

    private void CheckGround()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        if (IsGrounded)
        {
            IsGravityPaused = false;
        }
    }

    private void ApplyGravity()
    {
        if (IsGravityPaused)
        {
            rb.gravityScale = 0f;
            VerticalVelocity = 0f;
            return;
        }

        if (VerticalVelocity > jumpApexThreshold)
        {
            rb.gravityScale = gravityScale;
        }
        else if (VerticalVelocity >= 0f)
        {
            rb.gravityScale = gravityScale * apexGravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
        }

        VerticalVelocity = Mathf.Max(
            VerticalVelocity,
            -maxFallSpeed
        );
    }

    public void ApplyImpulse(Vector2 velocity)
    {
        TargetVelocity = velocity;
        rb.linearVelocity = velocity;
    }

    public void SetMovementSpeedMultiplier(
    float multiplier)
    {
        MovementSpeedMultiplier =
            Mathf.Max(0f, multiplier);
    }

    public void ResetMovementSpeedMultiplier()
    {
        MovementSpeedMultiplier = 1f;
    }

    private void UpdateFacingDirection(float moveInput)
    {
        if (IsAttacking &&
            IsGrounded)
        {
            return;
        }

        if (IsDead)
        {
            return;
        }

        int newDirection = FacingDirection;

        if (moveInput > 0.01f)
        {
            newDirection = 1;
        }
        else if (moveInput < -0.01f)
        {
            newDirection = -1;
        }

        if (newDirection == FacingDirection)
            return;

        FacingDirection = newDirection;

        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * FacingDirection;

        transform.localScale = scale;
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }

#endif
}