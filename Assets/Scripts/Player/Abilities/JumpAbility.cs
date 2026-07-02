using UnityEngine;

public class JumpAbility : PlayerAbility
{
    #region Settings

    [Header("Jump")]

    [SerializeField] private float jumpForce = 16f;

    [Header("Game Feel")]

    [SerializeField] private float jumpBufferTime = 0.2f;

    [SerializeField] private float coyoteTime = 0.2f;

    [SerializeField]
    [Range(0f, 1f)]
    private float jumpCutMultiplier = 0.5f;

    #endregion

    #region State

    private float jumpBufferCounter;

    private float coyoteCounter;

    private bool shouldCutJump;

    #endregion

    #region Custom Update

    public override void OnCustomUpdate()
    {
        UpdateJumpBuffer();
        UpdateCoyoteTime();
        UpdateVariableJump();
    }

    public override void OnCustomFixedUpdate()
    {
        if (jumpBufferCounter > 0f &&
            coyoteCounter > 0f)
        {
            PerformJump();
        }

        if (shouldCutJump)
        {
            CutJump();
        }
    }

    #endregion

    #region Private Methods

    private void UpdateJumpBuffer()
    {
        if (input.JumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void UpdateCoyoteTime()
    {
        if (controller.IsGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
    }

    private void UpdateVariableJump()
    {
        if (input.JumpReleased)
        {
            shouldCutJump = true;
        }
    }

    private void PerformJump()
    {
        jumpBufferCounter = 0f;
        coyoteCounter = 0f;
        shouldCutJump = false;

        controller.ApplyImpulse(new Vector2(
            controller.HorizontalVelocity,
            jumpForce
        ));
    }

    private void CutJump()
    {
        if (controller.VerticalVelocity > 0f)
        {
            controller.VerticalVelocity *= jumpCutMultiplier;
        }

        shouldCutJump = false;
    }

    #endregion
}