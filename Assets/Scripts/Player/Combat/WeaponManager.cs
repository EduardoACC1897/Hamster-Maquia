using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region References

    [Header("Default Weapon")]

    [SerializeField] private WeaponData defaultWeapon;

    #endregion

    #region State

    private WeaponData currentWeapon;

    private int remainingUses;

    #endregion

    #region Public Properties

    public WeaponData CurrentWeapon => currentWeapon;

    public MeleeWeaponData CurrentMeleeWeapon =>
        currentWeapon as MeleeWeaponData;

    public RangedWeaponData CurrentRangedWeapon =>
        currentWeapon as RangedWeaponData;

    public int RemainingUses => remainingUses;

    public bool HasInfiniteUses =>
        currentWeapon != null &&
        currentWeapon.InfiniteUses;

    public bool HasUsesRemaining =>
        HasInfiniteUses || remainingUses > 0;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        EquipWeapon(defaultWeapon);
    }

    #endregion

    #region Public Methods

    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null)
            return;

        currentWeapon = weapon;

        if (weapon.InfiniteUses)
        {
            remainingUses = -1;
        }
        else
        {
            remainingUses = weapon.MaxUses;
        }
    }

    public void ConsumeOneUse()
    {
        if (currentWeapon == null)
            return;

        if (currentWeapon.InfiniteUses)
            return;

        remainingUses--;

        if (remainingUses <= 0)
        {
            EquipDefaultWeapon();
        }
    }

    public void EquipDefaultWeapon()
    {
        EquipWeapon(defaultWeapon);
    }

    #endregion
}