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
        ConfigurarSlot();
    }
    //rellena los pergaminos/ bloques
   public void ConfigurarSlot()
   {
        if (recipeData == null) return;
        
        imgIconoComida.sprite = recipeData.recipeIcon;
        txtCostoRojo.text = recipeData.redEssenceCost.ToString();
        txtCostoVerde.text = recipeData.greenEssenceCost.ToString();
        txtCostoAzul.text = recipeData.blueEssenceCost.ToString();

        btnCocinar.onClick.RemoveAllListeners();
        btnCocinar.onClick.AddListener(IntentarCocinar);

    }

    private void IntentarCocinar()
    {
        if(CookingManager.Instance != null)
        {
            CookingManager.Instance.CocinarReceta(recipeData);
        }
    }
}
