using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public WeaponData EquippedWeapon;

    public int RemainingWeaponUses;

    public int RemainingLives = -1;

    public int RedEssence = 0;

    public int BlueEssence = 0;

    public int GreenEssence = 0;
}