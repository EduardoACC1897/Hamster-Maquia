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

    private int blueEssence;

    private int greenEssence;

    //Agregados Por Diego
    public static event Action<IngredientEssenceType, int> OnEssenceChanged; //envia el tipo de esencia y la cantidad actual

    #endregion

    #region Public Properties

    public int RedEssence => redEssence;

    public int BlueEssence => blueEssence;

    public int GreenEssence => greenEssence;

    public int MaxEssence => maxEssence;

    public int TotalEssence =>
        redEssence +
        blueEssence +
        greenEssence;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        redEssence =
            PlayerDataManager.Instance.RedEssence;

        blueEssence =
            PlayerDataManager.Instance.BlueEssence;

        greenEssence =
            PlayerDataManager.Instance.GreenEssence;

        //Agregado Por Diego
        OnEssenceChanged?.Invoke(IngredientEssenceType.Red, redEssence);
        OnEssenceChanged?.Invoke(IngredientEssenceType.Blue, blueEssence);
        OnEssenceChanged?.Invoke(IngredientEssenceType.Green, greenEssence); 
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

        AudioManager.Instance.PlaySFX(AudioManager.Instance.sonidoRecogerEsencia);

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

    public int EssenceLevel
    {
        get
        {
            int total = TotalEssence;

            if (total <= 10)
                return 0;

            if (total <= 20)
                return 1;

            return 2;
        }
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

            case IngredientEssenceType.Blue:
                return blueEssence;

            case IngredientEssenceType.Green:
                return greenEssence;
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

            case IngredientEssenceType.Blue:
                blueEssence = value;
                break;

            case IngredientEssenceType.Green:
                greenEssence = value;
                break;
        }

        PlayerDataManager.Instance.SetEssence(
            type,
            value);

        Debug.Log(
        $"Essence -> " +
        $"Red: {redEssence} | " +
        $"Blue: {blueEssence} | " +
        $"Green: {greenEssence}");

        //Agregado Por Diego
        OnEssenceChanged?.Invoke(type, value);
    }

    #endregion
}