using System.Collections;
using UnityEngine;

public class DropDownAbility : PlayerAbility
{
    #region Settings

    [Header("Drop Down")]

    [SerializeField] private float ignoreCollisionTime = 0.4f;

    [SerializeField] private float downwardImpulse = 2f;

    #endregion

    #region Custom Update

    public override void OnCustomUpdate()
    {
        if (!controller.IsGrounded)
            return;

        if (input.MoveY > -0.5f)
            return;

        if (!input.JumpPressed)
            return;

        Collider2D platform = GetCurrentPlatform();

        if (platform == null)
            return;

        StartCoroutine(DropRoutine(platform));
    }

    #endregion

    #region Private Methods

    private Collider2D GetCurrentPlatform()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            controller.GroundCheck.position,
            controller.GroundCheckRadius,
            controller.GroundLayer
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<PlatformEffector2D>() != null)
            {
                return hit;
            }
        }

        return null;
    }

    private IEnumerator DropRoutine(Collider2D platform)
    {
        controller.IsDropping = true;

        Physics2D.IgnoreCollision(
            controller.Collider,
            platform,
            true
        );

        controller.ApplyImpulse(new Vector2(
            controller.HorizontalVelocity,
            -downwardImpulse
        ));

        yield return new WaitForSeconds(ignoreCollisionTime);

        if (platform != null)
        {
            Physics2D.IgnoreCollision(
                controller.Collider,
                platform,
                false
            );
        }

        controller.IsDropping = false;
    }

    #endregion
}