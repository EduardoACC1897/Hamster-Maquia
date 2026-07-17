using UnityEngine;
using System;

public class PlayerIngredientEssence : MonoBehaviour
{
    #region Settings
        
    [Header("Essence")]

    [SerializeField]
    private int maxEssence = 10;

    #endregion

    #region State

    private int redEssence;

    private int greenEssence;

    private int yellowEssence;

    //Agregados Por Diego
    public static event Action<IngredientEssenceType, int> OnEssenceChanged; //envia el tipo de esencia y la cantidad actual

    #endregion

    #region Public Properties

    public int RedEssence => redEssence;

    public int GreenEssence => greenEssence;

    public int YellowEssence => yellowEssence;

    public int MaxEssence => maxEssence;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        redEssence =
            PlayerDataManager.Instance.RedEssence;

        greenEssence =
            PlayerDataManager.Instance.GreenEssence;

        yellowEssence =
            PlayerDataManager.Instance.YellowEssence;

        //Agregado Por Diego
        OnEssenceChanged?.Invoke(IngredientEssenceType.Red, redEssence);
        OnEssenceChanged?.Invoke(IngredientEssenceType.Green, greenEssence);
        OnEssenceChanged?.Invoke(IngredientEssenceType.Yellow, yellowEssence);
    }

    #endregion

    #region Public Methods

    public bool AddEssence(
        IngredientEssenceType type,
        int amount)
    {
        int currentValue =
            GetEssence(type);

        if (currentValue >= maxEssence)
            return false;

        SetEssence(
            type,
            Mathf.Min(
                currentValue + amount,
                maxEssence));

        return true;
    }

    public bool RemoveEssence(
        IngredientEssenceType type,
        int amount)
    {
        int currentValue =
            GetEssence(type);

        if (currentValue < amount)
            return false;

        SetEssence(
            type,
            currentValue - amount);

        return true;
    }

    public bool HasEssence(
        IngredientEssenceType type,
        int amount)
    {
        return GetEssence(type) >= amount;
    }

    #endregion

    #region Private Methods

    private int GetEssence(
        IngredientEssenceType type)
    {
        switch (type)
        {
            case IngredientEssenceType.Red:
                return redEssence;

            case IngredientEssenceType.Green:
                return greenEssence;

            case IngredientEssenceType.Yellow:
                return yellowEssence;
        }

        return 0;
    }

    private void SetEssence(
        IngredientEssenceType type,
        int value)
    {
        switch (type)
        {
            case IngredientEssenceType.Red:
                redEssence = value;
                break;

            case IngredientEssenceType.Green:
                greenEssence = value;
                break;

            case IngredientEssenceType.Yellow:
                yellowEssence = value;
                break;
        }

        PlayerDataManager.Instance.SetEssence(
            type,
            value);

        Debug.Log(
        $"Essence -> " +
        $"Red: {redEssence} | " +
        $"Green: {greenEssence} | " +
        $"Yellow: {yellowEssence}");

        //Agregado Por Diego
        OnEssenceChanged?.Invoke(type, value);
    }

    #endregion
}