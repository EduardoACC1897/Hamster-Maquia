using UnityEngine;

public class MoveAbility : PlayerAbility
{
    #region Settings

    [Header("Ground Movement")]

    [SerializeField]
    private float maxGroundSpeed = 6f;

    [SerializeField]
    private float runSpeedBonus = 2f;

    [SerializeField]
    private float groundAcceleration = 500f;

    [SerializeField]
    private float groundDeceleration = 600f;

    [Header("Air Movement")]

    [SerializeField]
    private float airAcceleration = 400f;

    [SerializeField]
    private float airDeceleration = 350f;

    #endregion

    #region State

    private bool keepRunSpeedInAir;

    #endregion

    #region References

    private WeaponManager weaponManager;

    #endregion

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();

        weaponManager = GetComponent<WeaponManager>();
    }

    #endregion

    #region Custom Update Methods

    public override void OnCustomFixedUpdate()
    {
        if (controller.IsAttacking)
        {
            bool blockMovement = controller.IsGrounded;

            if (!blockMovement &&
                weaponManager.CurrentWeapon != null &&
                weaponManager.CurrentWeapon.IsRanged)
            {
                blockMovement =
                    weaponManager.CurrentRangedWeapon.LockMovementInAir;
            }

            if (blockMovement)
            {
                controller.HorizontalVelocity = 0f;
                return;
            }
        }

        if (controller.IsHurt)
        {
            return;
        }

        if (controller.IsHealing)
        {
            controller.HorizontalVelocity = 0f;
            return;
        }

        if (controller.IsDead)
        {
            controller.HorizontalVelocity = 0f;
            return;
        }

        if (controller.IsCrouching)
        {
            controller.HorizontalVelocity = 0f;
            return;
        }

        // Al tocar el suelo decidimos si la siguiente vez
        // saltaremos corriendo o caminando.
        if (controller.IsGrounded)
        {
            keepRunSpeedInAir = input.RunHeld;
        }

        float speed =
            maxGroundSpeed *
            controller.MovementSpeedMultiplier;

        if (controller.IsGrounded)
        {
            if (input.RunHeld)
            {
                speed += runSpeedBonus;
            }
        }
        else
        {
            if (keepRunSpeedInAir)
            {
                speed += runSpeedBonus;
            }
        }

        float currentSpeed = controller.HorizontalVelocity;
        float targetSpeed = input.MoveX * speed;

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
            acceleration * Time.fixedDeltaTime);

        controller.HorizontalVelocity = currentSpeed;
    }

    #endregion
}