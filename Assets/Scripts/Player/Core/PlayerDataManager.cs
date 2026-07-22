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

    public int RemainingLives =>
        playerData.RemainingLives;

    public int RedEssence =>
    playerData.RedEssence;

    public int BlueEssence =>
        playerData.BlueEssence;

    public int GreenEssence =>
        playerData.GreenEssence;

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

    public void SetLives(int lives)
    {
        playerData.RemainingLives = lives;
    }

    public void SetEssence(
    IngredientEssenceType type,
    int amount)
    {
        switch (type)
        {
            case IngredientEssenceType.Red:
                playerData.RedEssence = amount;
                break;

            case IngredientEssenceType.Blue:
                playerData.BlueEssence = amount;
                break;

            case IngredientEssenceType.Green:
                playerData.GreenEssence = amount;
                break;
        }
    }

    public void ResetRun()
    {
        playerData = new PlayerData();
    }

    #endregion
}