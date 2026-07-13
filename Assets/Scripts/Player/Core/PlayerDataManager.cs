using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    #region Singleton

    public static PlayerDataManager Instance { get; private set; }

    #endregion

    #region Data

    private PlayerData playerData = new();

    #endregion

    #region Public Properties

    public WeaponData EquippedWeapon =>
        playerData.EquippedWeapon;

    public int RemainingWeaponUses =>
        playerData.RemainingWeaponUses;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (Instance != null &&
            Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Public Methods

    public void SetWeapon(
        WeaponData weapon,
        int remainingUses)
    {
        playerData.EquippedWeapon = weapon;
        playerData.RemainingWeaponUses = remainingUses;
    }

    public void ClearWeapon()
    {
        playerData.EquippedWeapon = null;
        playerData.RemainingWeaponUses = 0;
    }

    public void ResetRun()
    {
        playerData = new PlayerData();
    }

    #endregion
}