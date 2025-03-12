using MainCharacter;
using Stats;
using TMPro;
using UnityEngine;

public class LoadGold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI.text = PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI.text = PlayerManager.Instance.player.GetComponent<PlayerStats>().Gold.ToString();
    }
}
