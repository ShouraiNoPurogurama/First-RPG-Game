using MainCharacter;
using Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LoadRuby : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI.text = PlayerManager.Instance.player.GetComponent<PlayerStats>().Ruby.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI.text = PlayerManager.Instance.player.GetComponent<PlayerStats>().Ruby.ToString();
    }
}
