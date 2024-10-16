using AimarWork;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_BahanMentah : MonoBehaviour
{
    public SO_BahanMentah so_BahanMentah;
    public Button button;

    public Image GambarBahan;
    public Image IndikasiDipilih;

    public static event Action<SO_BahanMentah> TambahBahanMentah;
    public static event Action<SO_BahanMentah> KurangBahanMentah;
    public Color lockedCurrentImage;
    private void Awake()
    {
        GambarBahan = GetComponent<Image>();
        button = GetComponent<Button>();
    }
    private void Start()
    {
        Pengambilan_Bahan.UpdateBahan += Pengambilan_Bahan_UpdateBahan;
    }
    private void OnDisable()
    {
        Pengambilan_Bahan.UpdateBahan -= Pengambilan_Bahan_UpdateBahan;
    }

    public void SetUpBahanMentah(SO_BahanMentah so_bahanMentah)
    {
        so_BahanMentah = so_bahanMentah;
        if(so_BahanMentah.ikon_resep != null)
        {
            GambarBahan.sprite = so_BahanMentah.ikon_resep;
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(TambahBahan);
    }
    private void Pengambilan_Bahan_UpdateBahan()
    {
        
    }

    private void TambahBahan()
    {
        Debug.Log("Tambah Bahan Mentah " + so_BahanMentah.nama);
        TambahBahanMentah?.Invoke(so_BahanMentah);
        IndikasiDipilih.gameObject.SetActive(true);

        button.onClick.RemoveListener(TambahBahan);
        button.onClick.AddListener(KurangBahan);
    }
    private void KurangBahan()
    {
        Debug.Log("Kurang Bahan Mentah " + so_BahanMentah.nama);
        KurangBahanMentah?.Invoke(so_BahanMentah);
        IndikasiDipilih.gameObject.SetActive(false);

        button.onClick.RemoveListener(KurangBahan);
        button.onClick.AddListener(TambahBahan);
    }
    public void UpdateBahanMentah(bool adaBahanMentahDiPlayerInventory)
    {
        button.onClick.RemoveAllListeners();
        IndikasiDipilih.gameObject.SetActive(adaBahanMentahDiPlayerInventory);
        if (adaBahanMentahDiPlayerInventory)
        {
            button.onClick.AddListener(KurangBahan);
        }
        else
        {
            button.onClick.AddListener(TambahBahan);
        }
    }
}
