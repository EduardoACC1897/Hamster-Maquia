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
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private readonly List<PlayerAbility> abilities = new();

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
    }

    private void Update()
    {
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
    }

    private void ApplyGravity()
    {
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