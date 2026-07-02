using UnityEngine;

public class MoveAbility : PlayerAbility
{
    #region Settings

    [Header("Ground Movement")]

    [SerializeField] private float maxGroundSpeed = 8f;

    [SerializeField] private float groundAcceleration = 500f;

    [SerializeField] private float groundDeceleration = 600f;

    [Header("Air Movement")]

    [SerializeField] private float airAcceleration = 400f;

    [SerializeField] private float airDeceleration = 350f;

    #endregion

    #region Custom Update Methods

    public override void OnCustomFixedUpdate()
    {
        float currentSpeed = controller.HorizontalVelocity;
        float targetSpeed = input.MoveX * maxGroundSpeed;

        float acceleration;

        if (Mathf.Abs(input.MoveX) > 0.01f)
        {
            acceleration = controller.IsGrounded
                ? groundAcceleration
                : airAcceleration;
        }
        else
        {
            acceleration = controller.IsGrounded
                ? groundDeceleration
                : airDeceleration;

            targetSpeed = 0f;
        }

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            targetSpeed,
            acceleration * Time.fixedDeltaTime
        );

        controller.HorizontalVelocity = currentSpeed;
    }

    #endregion
}