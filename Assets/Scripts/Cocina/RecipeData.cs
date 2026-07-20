using UnityEngine;

[CreateAssetMenu(fileName = "NuevaReceta", menuName = "Hamster-Maquia/Receta")]
public class RecipeData : ScriptableObject
{
    [Header("Informacion General")]
    public string recipeName;
    public Sprite recipeIcon;

    [Header("Costos de Esencia")]
    public int redEssenceCost;
    public int greenEssenceCost;
    public int blueEssenceCost;

    [Header("Recompensa de Cocina")]
    public WeaponData weaponToUnlock;
}
