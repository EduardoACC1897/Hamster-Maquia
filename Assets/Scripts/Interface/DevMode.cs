using UnityEngine;

public class DevMode : MonoBehaviour
{
    [Header("Referencias a las Barras de la UI")]
    [SerializeField] private BarraEsenciaManager barraRoja;
    [SerializeField] private BarraEsenciaManager barraVerde;
    [SerializeField] private BarraEsenciaManager barraAzul;

    private int rojaActual = 0;
    private int verdeActual = 0;
    private int azulActual = 0;

    private PlayerIngredientEssence playerEssence()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            return playerObject.GetComponent<PlayerIngredientEssence>();

        }
        return null;
    }
    /// <summary>
    /// Modificas los valores de la esencia roja
    /// </summary>
    public void ModificarRoja(int cantidad)
    {
        rojaActual = Mathf.Clamp(rojaActual + cantidad, 0, 10);

        PlayerIngredientEssence essence = playerEssence();
        if (barraRoja != null)
        {
            if (cantidad > 0) essence.AddEssence(IngredientEssenceType.Red, cantidad);
            else essence.RemoveEssence(IngredientEssenceType.Red, Mathf.Abs(cantidad));
        }
        if(barraRoja != null) barraRoja.ActualizarCantidad(rojaActual);
        Debug.Log($"[Modo Dios] Esencia Roja: {rojaActual}/10");
    }

    /// <summary>
    /// Modifica la esencia verde
    /// </summary>
    public void ModificarVerde(int cantidad)
    {
        verdeActual = Mathf.Clamp(verdeActual + cantidad, 0, 10);

        PlayerIngredientEssence essence = playerEssence();
        if (barraVerde != null)
        {
            if (cantidad > 0) essence.AddEssence(IngredientEssenceType.Green, cantidad);
            else essence.RemoveEssence(IngredientEssenceType.Green, Mathf.Abs(cantidad));
        }
        Debug.Log($"[Modo Dios] Esencia Verde: {verdeActual}/10");
    }

    /// <summary>
    /// Modifica la esencia azul
    /// </summary>
    public void ModificarAzul(int cantidad)
    {
        azulActual = Mathf.Clamp(azulActual + cantidad, 0, 10);

        PlayerIngredientEssence essence = playerEssence();
        if (barraAzul != null)
        {
            if (cantidad > 0) essence.AddEssence(IngredientEssenceType.Yellow, cantidad);
            else essence.RemoveEssence(IngredientEssenceType.Yellow, Mathf.Abs(cantidad));
        }
        if(barraAzul != null) barraAzul.ActualizarCantidad(azulActual);
        Debug.Log($"[Modo Dios] Esencia Azul: {azulActual}/10");
    }
}
