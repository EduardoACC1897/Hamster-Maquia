using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public WeaponData EquippedWeapon;

    public int RemainingWeaponUses;

    public int RemainingLives = -1;

    public int RedEssence = 0;

    public int GreenEssence = 5;

    public int YellowEssence = 0;
}