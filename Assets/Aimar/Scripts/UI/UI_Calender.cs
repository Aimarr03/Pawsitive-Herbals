using AimarWork.GameManagerLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Calender : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Text;
    void Start()
    {
        Manager_Waktu.instance.ChangeHari += Instance_ChangeHari;
        Instance_ChangeHari();
    }
    private void OnDisable()
    {
        Manager_Waktu.instance.ChangeHari -= Instance_ChangeHari;
    }
    private void Instance_ChangeHari()
    {
        DateTime tanggalKini = Manager_Waktu.instance.TanggalKini;
        Text.text = $"{tanggalKini.Day}/{tanggalKini.Month}";
    }
}
