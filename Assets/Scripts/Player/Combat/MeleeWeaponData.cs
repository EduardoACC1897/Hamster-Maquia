using UnityEngine;

[CreateAssetMenu(
    fileName = "New Melee Weapon",
    menuName = "Game/Weapons/Melee Weapon")]
public class MeleeWeaponData : WeaponData
{
    [Header("General")]

    [SerializeField] private float attackDuration = 0.3f;

    [SerializeField] private float attackCooldown = 0.2f;

    [SerializeField] private float hitboxActiveTime = 0.2f;

    [Header("Ground Attack")]

    [SerializeField] private float attackRange = 0.4f;

    [SerializeField] private float lungeForce = 8f;

    [SerializeField] private float attackFriction = 0.9f;

    [Header("Air Attack")]

    [SerializeField] private float airAttackRange = 1.6f;

    [SerializeField] private float airBounceForce = 16f;

    [SerializeField] private bool singleAirHitPerJump = true;

    [Header("Visual")]

    [SerializeField] private int spinRotations = 1;

    [SerializeField]
    private Vector3 attackPunchScale =
        new(0.5f, -0.3f, 0f);

    public float AttackDuration => attackDuration;

    public float AttackCooldown => attackCooldown;

    public float HitboxActiveTime => hitboxActiveTime;

    public float AttackRange => attackRange;

    public float LungeForce => lungeForce;

    public float AttackFriction => attackFriction;

    public float AirAttackRange => airAttackRange;

    public float AirBounceForce => airBounceForce;

    public bool SingleAirHitPerJump => singleAirHitPerJump;

    public int SpinRotations => spinRotations;

    public Vector3 AttackPunchScale => attackPunchScale;
}