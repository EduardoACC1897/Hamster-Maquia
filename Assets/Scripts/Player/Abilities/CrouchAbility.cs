using UnityEngine;

public class CrouchAbility : PlayerAbility
{
    #region Settings

    [Header("Standing Collider")]

    [SerializeField] private Vector2 standingOffset = new(0f, 0f);

    [SerializeField] private Vector2 standingSize = new(0.5f, 2f);

    [Header("Crouching Collider")]

    [SerializeField] private Vector2 crouchingOffset = new(0f, -0.5f);

    [SerializeField] private Vector2 crouchingSize = new(0.5f, 1f);

    [Header("Ceiling Check")]

    [SerializeField] private float ceilingCheckDistance = 0.1f;

    #endregion

    #region State

    private BoxCollider2D boxCollider;

    #endregion

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();

        boxCollider = GetComponent<BoxCollider2D>();
    }

    #endregion

    #region Custom Update

    public override void OnCustomFixedUpdate()
    {
        if (controller.IsDropping)
        {
            ExitCrouch();
            return;
        }

        bool wantsToCrouch =
            controller.IsGrounded &&
            input.MoveY < -0.5f;

        if (wantsToCrouch)
        {
            EnterCrouch();
            return;
        }

        if (controller.IsCrouching)
        {
            TryStandUp();
        }
    }

    #endregion

    #region Private Methods

    private void EnterCrouch()
    {
        if (controller.IsCrouching)
            return;

        controller.IsCrouching = true;

        boxCollider.offset = crouchingOffset;
        boxCollider.size = crouchingSize;
    }

    private void TryStandUp()
    {
        if (HasCeiling())
            return;

        ExitCrouch();
    }

    private bool HasCeiling()
    {
        Bounds bounds = boxCollider.bounds;

        Vector2 left =
            new(bounds.min.x + 0.05f, bounds.max.y);

        Vector2 right =
            new(bounds.max.x - 0.05f, bounds.max.y);

        bool leftHit = Physics2D.Raycast(
            left,
            Vector2.up,
            ceilingCheckDistance,
            controller.GroundLayer
        );

        bool rightHit = Physics2D.Raycast(
            right,
            Vector2.up,
            ceilingCheckDistance,
            controller.GroundLayer
        );

        return leftHit || rightHit;
    }

    private void ExitCrouch()
    {
        if (!controller.IsCrouching)
            return;

        controller.IsCrouching = false;

        boxCollider.offset = standingOffset;
        boxCollider.size = standingSize;
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
            return;

        Bounds bounds = boxCollider.bounds;

        Vector3 left =
            new(bounds.min.x + 0.05f, bounds.max.y);

        Vector3 right =
            new(bounds.max.x - 0.05f, bounds.max.y);

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(
            left,
            left + Vector3.up * ceilingCheckDistance
        );

        Gizmos.DrawLine(
            right,
            right + Vector3.up * ceilingCheckDistance
        );
    }

#endif
}