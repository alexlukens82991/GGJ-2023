using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatPrefab : NetworkBehaviour
{
    [SerializeField] private TMP_Text m_playerName;
    [SerializeField] private TMP_Text m_bitsNumber;
    [SerializeField] private TMP_Text m_progressPercent;
    [SerializeField] private Image m_healthBar;

    public void SetName(string name)
    {
        m_playerName.text = name;
    }

    public void SetBits(NetworkVariable<int> numBits)
    {
        m_bitsNumber.text = numBits.Value.ToString();
    }

    public void SetProgressPercent(float percent)
    {
        m_progressPercent.text = percent.ToString() + " %";
    }

    public void SetHealthPercent(float percent)
    {
        m_healthBar.fillAmount = percent / 1;
    }
}
