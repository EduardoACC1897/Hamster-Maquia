using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CornerCorrectionAbility : PlayerAbility
{
    #region Settings

    [Header("Detection")]

    [SerializeField]
    private float topRayDistance = 0.5f;

    [SerializeField]
    private float edgeInset = 0.15f;

    [Header("Correction")]

    [SerializeField]
    private float cornerCorrectionDistance = 0.1f;

    #endregion

    #region References

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
        if (controller.VerticalVelocity <= 0f)
            return;

        Bounds bounds = boxCollider.bounds;

        Vector2 topLeft = new(bounds.min.x, bounds.max.y);
        Vector2 topRight = new(bounds.max.x, bounds.max.y);

        Vector2 innerLeft = new(bounds.min.x + edgeInset, bounds.max.y);
        Vector2 innerRight = new(bounds.max.x - edgeInset, bounds.max.y);

        bool hitOuterLeft = Physics2D.Raycast(
            topLeft,
            Vector2.up,
            topRayDistance,
            controller.GroundLayer
        );

        bool hitInnerLeft = Physics2D.Raycast(
            innerLeft,
            Vector2.up,
            topRayDistance,
            controller.GroundLayer
        );

        bool hitInnerRight = Physics2D.Raycast(
            innerRight,
            Vector2.up,
            topRayDistance,
            controller.GroundLayer
        );

        bool hitOuterRight = Physics2D.Raycast(
            topRight,
            Vector2.up,
            topRayDistance,
            controller.GroundLayer
        );

        if (hitInnerLeft || hitInnerRight)
            return;

        if (hitOuterLeft && !hitOuterRight)
        {
            controller.Rigidbody2D.position +=
                Vector2.right * cornerCorrectionDistance;
        }
        else if (hitOuterRight && !hitOuterLeft)
        {
            controller.Rigidbody2D.position +=
                Vector2.left * cornerCorrectionDistance;
        }
    }

    #endregion

#if UNITY_EDITOR

    #region Debug

    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
            return;

        Bounds bounds = boxCollider.bounds;

        Vector2 topLeft = new(bounds.min.x, bounds.max.y);
        Vector2 topRight = new(bounds.max.x, bounds.max.y);

        Vector2 innerLeft = new(bounds.min.x + edgeInset, bounds.max.y);
        Vector2 innerRight = new(bounds.max.x - edgeInset, bounds.max.y);

        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            topLeft,
            topLeft + Vector2.up * topRayDistance
        );

        Gizmos.DrawLine(
            topRight,
            topRight + Vector2.up * topRayDistance
        );

        Gizmos.color = Color.green;

        Gizmos.DrawLine(
            innerLeft,
            innerLeft + Vector2.up * topRayDistance
        );

        Gizmos.DrawLine(
            innerRight,
            innerRight + Vector2.up * topRayDistance
        );
    }

    #endregion

#endif
}