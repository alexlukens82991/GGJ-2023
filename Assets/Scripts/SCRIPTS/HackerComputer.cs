﻿using LukensUtils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HackerComputer : Singleton<HackerComputer>
{
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private TextMeshProUGUI hackerTxt;
    [SerializeField] private ScrollRect scrollRect;

    string[] expanded;
    private int currIndex;
    private bool hackComplete;

    private void Start()
    {
        expanded = HACKER_TEXT.Split(' ');
        hackerTxt.text = "█";
    }

    private void Update()
    {
        if (hackComplete) return;

        if (Input.anyKeyDown && cg.interactable)
        {
            Type();
        }
    }

    private void Type()
    {
        hackerTxt.text = hackerTxt.text.Replace("█", "");

        hackerTxt.text += expanded[currIndex];
        hackerTxt.text += " ";
        hackerTxt.text += "█";
        currIndex++;
        scrollRect.verticalNormalizedPosition = 0;

        if (currIndex == expanded.Length)
            TriggerHackSuccess();
    }

    private void TriggerHackSuccess()
    {
        print("HACK COMPLETE. SPAWNING PLAYER");
        hackComplete = true;
    }

    public void SetTargetPlayer(Transform player)
    {
        targetPlayer = player;
    }

    private const string HACKER_TEXT = "ACCESS DATABASE { DB239158-235f } identity: NONE \n\n PURGE CACHE; \n\n ECHO PING_RATE () => DISSCONNECT_OBSERVERS; " +
        "\n\n CONFIRM_ACCESS () => Grant_Access(); \n\n Grant_Access():\n 	\tfor(int i = 0; i <= DB_CONTENTS; i+++)\n 	\t{\n 		\t\tUSER[I].READ;\n 		" +
        "\t\tRETRIEVE USER DATA_ENCRYPTED\n 		\t\tDESTORY_TRACERS\n 	\t}\n \n REMOVE_TRACKER(Iso_248885) \n\n BEGIN_BRUTE_FORCE:\n 	" +
        "\tGenerate_KEY: Encryption: MD5 \n\n USE_KEYS: \n\n 543892758094275980472\n 798543289057409832759\n 897654380967580943705\n 689754389076543876668\n " +
        "598430580943890590439\n 509490856049390909649 \n\n IF (KEY_DESTORYED)\n 	\tWayBackMachine_ACCESS_REMOTE\n 	\tRetrieve_Destroyed_Data(HASH-ID: 34895409865063)\n \n " +
        "DEFRAG_HD_RETURN: TRUE(NO_ACCESS); \n\n RETURN FRAGGED:\n 	\tLAUNCH_HALO_2\n RETURN_BYTE(42069) \n\n DESTROY_ALL(HashMap: D12_MY_BAND)\n 	" +
        "\t() => MyBand ? END : RETURN FALSE \n\n ENTER_SPOOFED KEYS: \n\n 3544385\n 5437543\n 6543789\n 5438539\n 5438982\n 5438979\n 2758950\n 1935259\n 6954869\n \n " +
        "REVERT_CONVERT_FROZEN ? TRUE : FALSE \n\n MAX_IP = FLOAT.MAX - FLOAT.MIN \n\n MIN_IP = YUR.MOHM.EXE\n 	\t() => ANON_ARROW_FUNC_NOT_NEEDED \n\n OPEN_ACCESS_POINT " +
        "\n\n RETURN_PING_SUCCESSFUL \n\n ACCESS GRANTED.";
}
