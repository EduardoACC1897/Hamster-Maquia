using UnityEngine;
using UnityEngine.InputSystem;

public class CookingManager : MonoBehaviour
{
    public static CookingManager Instance { get; private set; }

    //para no solapsar la pausa con la cocina
    public bool isKitchenOpen => ventanaCocina != null && ventanaCocina.activeSelf;

    [Header("Referencias de UI")]
    [SerializeField] private GameObject ventanaCocina;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (ventanaCocina != null)
        {
            ventanaCocina.SetActive(false);
        }

    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            bool cocinaAbierta = ventanaCocina.activeSelf;
            //al estar en pausa, la tecla c no sirve
            if (!cocinaAbierta && Time.timeScale == 0f) return;

            AlternarVentanaCocina();
        }
    }

    public void AlternarVentanaCocina()
    {
        if(ventanaCocina == null) return;

        bool estaActiva = !ventanaCocina.activeSelf;
        ventanaCocina.SetActive(estaActiva);

        //pausar juego al activar cocina

        Time.timeScale = estaActiva ? 0f : 1f;

        Debug.Log(estaActiva ? "Ventana de cocina abierta. Pausa" : "Ventana de cocina cerrada. Renaudado");
    }

    public void CocinarReceta(RecipeData recipe)
    {
        if (recipe == null) return;
        
        PlayerDataManager data = PlayerDataManager.Instance;
        PlayerIngredientEssence playerEssence = FindAnyObjectByType<PlayerIngredientEssence>();
        PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (data == null)
        {
            Debug.LogError("No se encontro el playerDataManager");
            return;
        }

        if(playerEssence == null)
        {
            Debug.LogError("No se encontro el componente PlayerIngredientEssence");
            return;
        }

        if(recipe.tipoReceta == RecipeType.Curacion && playerHealth != null)
        {
            if(playerHealth.CurrentHealth >= playerHealth.MaxHealth)
            {
                Debug.Log("No puedes curarte, tu salud ya esta al maximo.");
                return;
            }
        }

        bool tieneSuficiente = playerEssence.HasEssence(IngredientEssenceType.Red, recipe.redEssenceCost) &&
                               playerEssence.HasEssence(IngredientEssenceType.Green, recipe.greenEssenceCost) &&
                               playerEssence.HasEssence(IngredientEssenceType.Yellow, recipe.blueEssenceCost);

        if (tieneSuficiente)
        {
            // Restar la esencia utilizada
            playerEssence.RemoveEssence(IngredientEssenceType.Red, recipe.redEssenceCost);
            playerEssence.RemoveEssence(IngredientEssenceType.Green, recipe.greenEssenceCost);
            playerEssence.RemoveEssence(IngredientEssenceType.Yellow, recipe.blueEssenceCost);
            // Desbloquear el arma asociada a la receta
            if (recipe.tipoReceta == RecipeType.Curacion)
            {
                if (playerHealth != null)
                {
                    //curar 1 punto
                    playerHealth.Heal(recipe.healingAmount);

                    //activa efecto visual
                    PlayerAnimation playerAnimation = playerHealth.GetComponent<PlayerAnimation>();

                    if (playerAnimation != null)
                    {
                        playerAnimation.PlayHealCompleteEffect();
                    }

                    Debug.Log($"Receta {recipe.recipeName} cocinada con exito. Has curado {recipe.healingAmount} puntos de vida.");
                }
            }

            else if(recipe.tipoReceta == RecipeType.Weapon)
            {
                WeaponManager weaponManager = FindAnyObjectByType<WeaponManager>();

                if(weaponManager != null && recipe.weaponToUnlock != null)
                {
                    weaponManager.EquipWeapon(recipe.weaponToUnlock);

                    Debug.Log($"Receta {recipe.recipeName} cocinada con exito. Has desbloqueado el arma {recipe.weaponToUnlock.WeaponName}.");
                }
                else
                {
                    Debug.LogError("No se pudo desbloquear el arma. Asegúrate de que WeaponManager y weaponToUnlock no sean nulos.");
                }
            }

        }
        else
        {
            Debug.Log("No tienes suficiente esencia para cocinar esta receta.");
        }
    }
    //no sirve esta wea, pero podria servir juasjuasj
    private void DesbloquearNuevaArma(WeaponData nuevaArma)
    {
        if (nuevaArma == null) return;

        WeaponManager weaponManager = FindAnyObjectByType<WeaponManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}
