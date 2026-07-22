using System.IO;
using UnityEngine;

public class DevMode : MonoBehaviour
{
   
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
        PlayerIngredientEssence essence = playerEssence();
   
        if (essence != null)
        {
            if (cantidad > 0) essence.AddEssence(IngredientEssenceType.Red, cantidad);
            else essence.RemoveEssence(IngredientEssenceType.Red, Mathf.Abs(cantidad));

            Debug.Log($"[Modo Dios] Esencia Roja: {essence.RedEssence}/10");
        }
        else
        {
            Debug.LogWarning("[Modo Dios] No se encontró el componente PlayerIngredientEssence en el objeto del jugador.");
        }
        
        
    }

    /// <summary>
    /// Modifica la esencia azul
    /// </summary>
    public void ModificarAzul(int cantidad)
    {
        
        PlayerIngredientEssence essence = playerEssence();
        if (essence != null)
        {
            if (cantidad > 0) essence.AddEssence(IngredientEssenceType.Blue, cantidad);
            else essence.RemoveEssence(IngredientEssenceType.Blue, Mathf.Abs(cantidad));

            Debug.Log($"[Modo Dios] Esencia Azul: {essence.BlueEssence}/10");
        }
        else
        {
            Debug.LogWarning("[Modo Dios] No se encontró el componente PlayerIngredientEssence en el objeto del jugador.");
        }
        
    }

    /// <summary>
    /// Modifica la esencia verde
    /// </summary>
    public void ModificarVerde(int cantidad)
    {

        PlayerIngredientEssence essence = playerEssence();
        if (essence != null)
        {
            if (cantidad > 0) essence.AddEssence(IngredientEssenceType.Green, cantidad);
            else essence.RemoveEssence(IngredientEssenceType.Green, Mathf.Abs(cantidad));

            Debug.Log($"[Modo Dios] Esencia Verde: {essence.GreenEssence}/10");
        }
        else
        {
            Debug.LogWarning("[Modo Dios] No se encontró el componente PlayerIngredientEssence en el objeto del jugador.");
        }

    }
}
