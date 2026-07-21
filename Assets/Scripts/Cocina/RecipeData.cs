using UnityEngine;

public enum RecipeType
{
    Weapon,
    Curacion
}

[CreateAssetMenu(fileName = "NuevaReceta", menuName = "Hamster-Maquia/Receta")]
public class RecipeData : ScriptableObject
{
    [Header("Informacion General")]
    public string recipeName;
    public Sprite recipeIcon;
    public RecipeType tipoReceta;

    [Header("Costos de Esencia")]
    public int redEssenceCost;
    public int greenEssenceCost;
    public int blueEssenceCost;

    [Header("Recompensa de Arma")]
    public WeaponData weaponToUnlock;

    [Header("Recompensa de Curacion")]
    public int healingAmount = 1; //recupera un corazon
}
