using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookingSlotiUI : MonoBehaviour
{
    [Header("Datos de la Receta")]
    public RecipeData recipeData;

    [Header("Componentes de la Receta")]
    public Image imgIconoComida;
    public TextMeshProUGUI txtCostoRojo;
    public TextMeshProUGUI txtCostoVerde;
    public TextMeshProUGUI txtCostoAzul;
    public Button btnCocinar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if(recipeData != null)
        {
            ConfigurarSlot();
        }
        else
        {
            Debug.LogWarning($"Error, inicio sin los datos {gameObject.name}");
        }
    }

    public void InicializarSlot(RecipeData nuevaReceta)
    {
        recipeData = nuevaReceta;
        ConfigurarSlot();
    }
    //rellena los pergaminos/ bloques
   public void ConfigurarSlot()
   {
        if (recipeData == null)
        {
            Debug.LogError($"[Cocina UI] Se intentó configurar el slot '{gameObject.name}' pero recipeData sigue siendo null.");
            return;
        }

        if (imgIconoComida != null) imgIconoComida.sprite = recipeData.recipeIcon;
        if (txtCostoRojo != null) txtCostoRojo.text = recipeData.redEssenceCost.ToString();
        if (txtCostoVerde != null) txtCostoVerde.text = recipeData.greenEssenceCost.ToString();

        if (txtCostoAzul != null) txtCostoAzul.text = recipeData.blueEssenceCost.ToString();

        if (btnCocinar != null)
        {
            btnCocinar.onClick.RemoveAllListeners();
            btnCocinar.onClick.AddListener(IntentarCocinar);
        }

    }

    private void IntentarCocinar()
    {
        if(CookingManager.Instance != null)
        {
            CookingManager.Instance.CocinarReceta(recipeData);
        }
    }
}
