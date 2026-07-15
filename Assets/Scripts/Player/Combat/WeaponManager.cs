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
        LoadSavedWeapon();
    }

    #endregion

    #region Public Methods

    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null)
            return;

        EquipWeapon(
            weapon,
            weapon.InfiniteUses
                ? -1
                : weapon.MaxUses);

        if (weapon == defaultWeapon)
            return;

        SaveCurrentWeapon();
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
            RemoveCurrentWeapon();
        }
        else
        {
            SaveCurrentWeapon();
        }
    }

    private void RemoveCurrentWeapon()
    {
        EquipWeapon(defaultWeapon);

        PlayerDataManager.Instance?.ClearWeapon();
    }

    private void EquipWeapon(
        WeaponData weapon,
        int uses)
    {
        if (weapon == null)
            return;

        currentWeapon = weapon;
        remainingUses = uses;
    }

    private void SaveCurrentWeapon()
    {
        if (PlayerDataManager.Instance == null)
            return;

        PlayerDataManager.Instance.SetWeapon(
            currentWeapon,
            remainingUses);
    }

    private void LoadSavedWeapon()
    {
        if (PlayerDataManager.Instance == null)
        {
            EquipWeapon(defaultWeapon);
            return;
        }

        WeaponData weapon =
            PlayerDataManager.Instance.EquippedWeapon;

        if (weapon == null)
        {
            EquipWeapon(defaultWeapon);
            return;
        }

        EquipWeapon(
            weapon,
            PlayerDataManager.Instance.RemainingWeaponUses);
    }

    #endregion
}