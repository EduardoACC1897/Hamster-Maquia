using UnityEngine;

public abstract class WeaponData : ScriptableObject
{
    [Header("General")]

    [SerializeField] private string weaponName;

    [SerializeField] private Sprite icon;

    [Header("Uses")]

    [SerializeField] private bool infiniteUses = true;

    [SerializeField] private int maxUses = 0;

    [Header("Animation")]

    [SerializeField] private WeaponAnimationSet animations;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;
    public string WeaponName => weaponName;

    public Sprite Icon => icon;

    public bool InfiniteUses => infiniteUses;

    public int MaxUses => maxUses;

    public bool IsMelee => this is MeleeWeaponData;

    public bool IsRanged => this is RangedWeaponData;

    public WeaponAnimationSet Animations => animations;

    //Agregador por Diego
    public AudioClip AttackSound => attackSound;
}