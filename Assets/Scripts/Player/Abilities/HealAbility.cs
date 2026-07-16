using UnityEngine;

public class HealAbility : PlayerAbility
{
    #region Settings

    [Header("Healing")]

    [SerializeField]
    private float healTime = 3f;

    [SerializeField]
    private int healAmount = 1;

    [SerializeField]
    private int greenEssenceCost = 3;

    #endregion

    #region References

    private PlayerHealth playerHealth;

    private PlayerIngredientEssence playerEssence;

    private PlayerAnimation playerAnimation;

    #endregion

    #region State

    private float healTimer;

    #endregion

    #region Public Properties

    public float HealProgress =>
        Mathf.Clamp01(
            healTimer / healTime);

    #endregion

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();

        playerHealth =
            GetComponent<PlayerHealth>();

        playerEssence =
            GetComponent<PlayerIngredientEssence>();

        playerAnimation =
            GetComponent<PlayerAnimation>();
    }

    #endregion

    #region Custom Update

    public override void OnCustomUpdate()
    {
        if (!CanHeal())
        {
            CancelHealing();
            return;
        }

        if (!input.HealHeld)
        {
            CancelHealing();
            return;
        }

        controller.IsHealing = true;

        healTimer += Time.deltaTime;

        playerAnimation.UpdateHealingEffect(
            HealProgress);

        if (healTimer >= healTime)
        {
            CompleteHealing();
        }
    }

    #endregion

    #region Private Methods

    private bool CanHeal()
    {
        if (!controller.IsGrounded)
            return false;

        if (controller.IsDead)
            return false;

        if (controller.IsHurt)
            return false;

        if (controller.IsAttacking)
            return false;

        if (controller.IsCrouching)
            return false;

        if (playerHealth.CurrentHealth >=
            playerHealth.MaxHealth)
            return false;

        if (!playerEssence.HasEssence(
            IngredientEssenceType.Green,
            greenEssenceCost))
            return false;

        return true;
    }

    private void CompleteHealing()
    {
        playerEssence.RemoveEssence(
            IngredientEssenceType.Green,
            greenEssenceCost);

        playerHealth.Heal(
            healAmount);

        playerAnimation.PlayHealCompleteEffect();

        CancelHealing(false);
    }

    private void CancelHealing(
    bool stopAnimation = true)
    {
        if (!controller.IsHealing &&
            healTimer <= 0f)
        {
            return;
        }

        controller.IsHealing = false;

        if (stopAnimation)
        {
            playerAnimation.StopHealingEffect();
        }

        healTimer = 0f;
    }

    #endregion
}