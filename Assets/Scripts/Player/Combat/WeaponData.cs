using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    [Header("General")]

    [SerializeField] private string weaponName;

    [SerializeField] private Sprite icon;

    [SerializeField] private string groundAttackAnimation = "Attack";

    [SerializeField] private string airAttackAnimation = "AirAttack";

    [Header("Uses")]

    [SerializeField] private bool infiniteUses = true;

    [SerializeField] private int maxUses = 0;

    public string WeaponName => weaponName;

    public Sprite Icon => icon;

    public string GroundAttackAnimation => groundAttackAnimation;

    public string AirAttackAnimation => airAttackAnimation;

    public bool InfiniteUses => infiniteUses;

    public int MaxUses => maxUses;

    public bool IsMelee => this is MeleeWeaponData;

    public bool IsRanged => this is RangedWeaponData;
}