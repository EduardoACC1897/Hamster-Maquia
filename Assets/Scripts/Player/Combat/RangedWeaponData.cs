using UnityEngine;

[CreateAssetMenu(
    fileName = "New Ranged Weapon",
    menuName = "Game/Weapons/Ranged Weapon")]
public class RangedWeaponData : WeaponData
{
    [Header("Attack")]

    [SerializeField] private float attackDuration = 0.2f;

    [SerializeField] private float attackCooldown = 0.15f;

    [Header("Projectile")]

    [SerializeField] private GameObject projectilePrefab;

    [Header("Air Attack")]

    [SerializeField] private bool floatWhileAttacking = true;

    [SerializeField]
    private bool lockMovementInAir = true;

    public float AttackDuration => attackDuration;

    public float AttackCooldown => attackCooldown;

    public GameObject ProjectilePrefab => projectilePrefab;

    public bool FloatWhileAttacking => floatWhileAttacking;

    public bool LockMovementInAir => lockMovementInAir;
}