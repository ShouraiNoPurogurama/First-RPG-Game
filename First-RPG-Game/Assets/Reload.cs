using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Reload : MonoBehaviour
{
    public Button swapButton; 

    private void Start()
    {
        if (swapButton != null)
        {
            swapButton.onClick.AddListener(OnSwapButtonClicked);
        }
    }

    private async void OnSwapButtonClicked()
    {
        if (swapButton != null)
        {
            swapButton.interactable = false; 
        }

        await ReloadRuby();

        if (swapButton != null)
        {
            swapButton.interactable = true; 
        }
    }

    public async Task ReloadRuby()
    {
        try
        {
            await APITrigger.Instance.LoadRubyDB();

        }
        catch (System.Exception ex)
        {
            Debug.LogError($"{ex.Message}");
        }
    }
}
