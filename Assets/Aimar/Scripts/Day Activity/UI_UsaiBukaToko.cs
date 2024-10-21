using AimarWork;
using AimarWork.GameManagerLogic;
using FadlanWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UsaiBukaToko : MonoBehaviour
{
    public GameObject UI_Rangkuman;
    public Dictionary<SO_Jamu, int> hidanganDiberikan;

    public GameObject FormatPemesanan;
    public GameObject ContainerPemesanan;

    public TextMeshProUGUI tanggalTokoBuka;
    
    public TextMeshProUGUI uangYangDiperoleh;

    public GameObject ContainerStar;
    public TextMeshProUGUI overallCustomerDihidangkan;

    private void Awake()
    {
        hidanganDiberikan = new Dictionary<SO_Jamu, int>();
    }
    void Start()
    {
        Customer.PergiDariToko += Customer_PergiDariToko;
        Manager_TokoJamu.instance.MenghidangkanDenganBenar += Instance_MenghidangkanDenganBenar;
    }
    private void OnDisable()
    {
        Customer.PergiDariToko -= Customer_PergiDariToko;
        Manager_TokoJamu.instance.MenghidangkanDenganBenar -= Instance_MenghidangkanDenganBenar;
    }
    private void Instance_MenghidangkanDenganBenar(SO_Jamu obj)
    {
        if(!hidanganDiberikan.ContainsKey(obj))
        {
            hidanganDiberikan.Add(obj, 1);
        }
        else
        {
            int kuantitaskini = hidanganDiberikan[obj];
            kuantitaskini++;
            hidanganDiberikan[obj] = kuantitaskini;
        }
    }

    private void Customer_PergiDariToko()
    {
        OnDisplayRangkuman();
    }
    private async void OnDisplayRangkuman()
    {
        await Task.Delay(1200);
        DateTime tanggalKini = Manager_Waktu.instance.TanggalKini;
        tanggalTokoBuka.text = $"Tanggal: \t{tanggalKini.Date}/{tanggalKini.Month}/{tanggalKini.Year}";

        List<SO_Jamu> keys = new List<SO_Jamu>(hidanganDiberikan.Keys);
        List<int> values = new List<int>(hidanganDiberikan.Values);
        for (int index = 0; index < hidanganDiberikan.Count; index++)
        {
            SO_Jamu jamu = keys[index];
            int kuantitas = values[index];
            
            GameObject HidanganJamu = Instantiate(FormatPemesanan, ContainerPemesanan.transform);
            HidanganJamu.SetActive(true);

            TextMeshProUGUI tipeJamu = HidanganJamu.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            tipeJamu.text = $"{index +1}. {jamu.nama}:";
            TextMeshProUGUI berapaGelas = HidanganJamu.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            berapaGelas.text = $"{kuantitas} Gelas";
            TextMeshProUGUI hargaKuantitas = HidanganJamu.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            hargaKuantitas.text = $"Rp. {jamu.GetBaseKeuntungan().ToString("N2")}";
        }

        float performa = Manager_TokoJamu.instance.kualitasPerforma;
        int formatSatuan = (int)performa;
        float performaPecahan = performa % 1;
        for(int index = 0; index < formatSatuan; index++)
        {
            Transform starKini = ContainerPemesanan.transform.GetChild(0);
            starKini.GetChild(0).gameObject.SetActive(true);
        }
        if(formatSatuan < 5)
        {
            Transform starTerakhir = ContainerPemesanan.transform.GetChild(formatSatuan);
            GameObject gambarStarTerakhir = starTerakhir.GetChild(0).gameObject;
            gambarStarTerakhir.SetActive(true);
            gambarStarTerakhir.GetComponent<Image>().fillAmount = performaPecahan;
        }
        overallCustomerDihidangkan.text = $"{performa.ToString("F1")}/{5}";

        uangYangDiperoleh.text = $"Uang Yang Diperoleh: Rp. {Manager_TokoJamu.instance.uangDiperoleh.ToString("N2")}";
        UI_Rangkuman.SetActive(true);
    }
}
