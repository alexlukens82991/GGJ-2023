using LukensUtils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class HackerComputer : Singleton<HackerComputer>
{
    [SerializeField] private string hackerTextMaster;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private TextMeshProUGUI hackerTxt;

    string[] expanded;
    private int currIndex;

    private void Start()
    {
        expanded = hackerTextMaster.Split(' ');
        hackerTxt.text = "█";
    }

    private void Update()
    {
        if (Input.anyKeyDown && cg.interactable)
        {
            Type();
        }
    }

    private void Type()
    {
        hackerTxt.text += expanded[currIndex];
        hackerTxt.text += " ";
        hackerTxt.text += "█";
    }

    public void SetTargetPlayer(Transform player)
    {
        targetPlayer = player;
    }
}
