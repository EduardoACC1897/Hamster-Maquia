using System.Collections;
using UnityEngine;

public class AttackAbility : PlayerAbility
{
    #region References

    [Header("References")]

    [SerializeField] private Transform visualTransform;

    [SerializeField] private Transform attackPoint;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField]
    private Vector3 airAttackOffset = Vector3.zero;

    private WeaponManager weaponManager;

    #endregion

    #region State

    private bool canAttack = true;

    private bool airAttackUsed;

    #endregion

    #region Unity Messages

    protected override void Awake()
    {
        base.Awake();

        weaponManager = GetComponent<WeaponManager>();
    }

    #endregion

    #region Custom Update

    public override void OnCustomUpdate()
    {
        if (controller.IsGrounded)
        {
            airAttackUsed = false;
        }

        if (!canAttack)
            return;

        if (!input.AttackPressed)
            return;

        if (weaponManager.CurrentWeapon == null)
            return;

        if (!controller.IsGrounded &&
            weaponManager.CurrentWeapon.IsMelee &&
            weaponManager.CurrentMeleeWeapon.SingleAirHitPerJump &&
            airAttackUsed)
        {
            return;
        }

        StartCoroutine(AttackRoutine());
    }

    #endregion

    #region Attack

    private IEnumerator AttackRoutine()
    {
        canAttack = false;

        controller.IsAttacking = true;

        if (weaponManager.CurrentWeapon.IsMelee)
        {
            yield return PerformMeleeAttack();
        }
        else if (weaponManager.CurrentWeapon.IsRanged)
        {
            yield return PerformRangedAttack();
        }

        controller.IsAttacking = false;

        yield return new WaitForSeconds(GetAttackCooldown());

        canAttack = true;
    }

    private IEnumerator PerformMeleeAttack()
    {
        MeleeWeaponData weapon = weaponManager.CurrentMeleeWeapon;

        bool isAirAttack = !controller.IsGrounded;

        if (isAirAttack &&
            weapon.SingleAirHitPerJump)
        {
            airAttackUsed = true;
        }

        if (!isAirAttack)
        {
            HandleGroundLunge(weapon);
        }

        float timer = 0f;

        while (timer < weapon.HitboxActiveTime)
        {
            if (CheckMeleeHit(isAirAttack, weapon))
            {
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (timer < weapon.AttackDuration)
        {
            yield return new WaitForSeconds(
                weapon.AttackDuration - timer);
        }
    }

    private IEnumerator PerformRangedAttack()
    {
        RangedWeaponData weapon = weaponManager.CurrentRangedWeapon;

        bool isAirAttack = !controller.IsGrounded;

        if (isAirAttack &&
            weapon.FloatWhileAttacking)
        {
            controller.IsGravityPaused = true;
        }

        if (weapon.ProjectilePrefab != null)
        {
            GameObject projectileObject =
                Instantiate(
                    weapon.ProjectilePrefab,
                    attackPoint.position,
                    Quaternion.identity);

            Projectile projectile =
                projectileObject.GetComponent<Projectile>();

            if (projectile != null)
            {
                int dir = (int)Mathf.Sign(transform.localScale.x);

                projectile.SetDirection(Vector2.right * dir);
            }
        }

        weaponManager.ConsumeOneUse();

        yield return new WaitForSeconds(
            weapon.AttackDuration);

        if (isAirAttack &&
            weapon.FloatWhileAttacking)
        {
            controller.IsGravityPaused = false;
        }
    }

    #endregion

    #region Melee

    private void HandleGroundLunge(MeleeWeaponData weapon)
    {
        if (Mathf.Abs(input.MoveX) > 0.1f)
        {
            float direction = Mathf.Sign(input.MoveX);

            controller.TargetVelocity =
                new Vector2(
                    direction * weapon.LungeForce,
                    0f);
        }
        else
        {
            controller.HorizontalVelocity = 0f;
        }
    }

    private bool CheckMeleeHit(
        bool isAirAttack,
        MeleeWeaponData weapon)
    {
        Vector2 center;
        float radius;

        if (isAirAttack)
        {
            center =
                (Vector2)transform.position +
                (Vector2)airAttackOffset;

            radius = weapon.AirAttackRange;
        }
        else
        {
            center = attackPoint.position;
            radius = weapon.AttackRange;
        }

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                center,
                radius,
                enemyLayer);

        bool hitSomething = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(
                out IDamageable damageable))
            {
                damageable.TakeDamage();

                hitSomething = true;
            }
        }

        if (isAirAttack && hitSomething)
        {
            controller.ApplyImpulse(
                new Vector2(
                    controller.TargetVelocity.x,
                    weapon.AirBounceForce));
        }

        if (hitSomething)
        {
            weaponManager.ConsumeOneUse();
        }

        return hitSomething;
    }

    #endregion

    #region Helpers

    private float GetAttackCooldown()
    {
        if (weaponManager.CurrentWeapon.IsMelee)
        {
            return weaponManager
                .CurrentMeleeWeapon
                .AttackCooldown;
        }

        return weaponManager
            .CurrentRangedWeapon
            .AttackCooldown;
    }

    #endregion

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (weaponManager == null)
            weaponManager = GetComponent<WeaponManager>();

        if (weaponManager == null)
            return;

        if (weaponManager.CurrentMeleeWeapon == null)
            return;

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(
                attackPoint.position,
                weaponManager.CurrentMeleeWeapon.AttackRange);
        }

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position + airAttackOffset,
            weaponManager.CurrentMeleeWeapon.AirAttackRange);
    }

#endif
}