using AimarWork;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class Night_DropdownBeliBahan : MonoBehaviour
{
    public SO_BahanMentah pilihanBahanMentah;
    
    [SerializeField] private TMP_Dropdown tmp_dropdown;
    [SerializeField] private Night_BeliBahan logika_BeliBahan;
    
    public int kuantitas_pembelianbahan = 0;
    public int totalharga => kuantitas_pembelianbahan * pilihanBahanMentah.hargaPerSatuan;

    [Header("Penambahan dan Pengurangan Kuantitas")]
    [SerializeField] private RectTransform kontainer_kuantitas;
    [SerializeField] private TextMeshProUGUI teks_kuantitasbahan;
    [SerializeField] private Button tombol_inkremen;
    [SerializeField] private Button tombol_dekremen;

    [Header("Teks Harga Pembelian")]
    [SerializeField] private TextMeshProUGUI teks_hargaperkuantitas;
    [SerializeField] private TextMeshProUGUI teks_totalharga;


    public static event Action MemasukkanPerubahanPemesanan;
    public static event Action<Night_DropdownBeliBahan> Event_MengurangiBahan;
    private void Awake()
    {
        kuantitas_pembelianbahan = 0;
        kontainer_kuantitas.gameObject.SetActive(false);
        teks_hargaperkuantitas.gameObject.SetActive(false);
        teks_totalharga.gameObject.SetActive(false);
    }
    private void Start()
    {
        Update_ListBahanData();
    }
    public void Update_ListBahanData()
    {
        tmp_dropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> dropdownOptions = new List<TMP_Dropdown.OptionData>();
        foreach (SO_BahanMentah bahanMentah in logika_BeliBahan.List_BahanMentah)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = bahanMentah.nama;
            dropdownOptions.Add(optionData);
        }
        tmp_dropdown.AddOptions(dropdownOptions);
    }
    public void Set_JamuSO(int index)
    {
        pilihanBahanMentah = logika_BeliBahan.List_BahanMentah[index];
        kuantitas_pembelianbahan = 1;
        
        teks_kuantitasbahan.text = kuantitas_pembelianbahan.ToString();
        teks_hargaperkuantitas.text = $"{pilihanBahanMentah.hargaPerSatuan.ToString("N0")}";
        
        Update_TotalHarga();
        
        kontainer_kuantitas.gameObject.SetActive(true);
        teks_hargaperkuantitas.gameObject.SetActive(true);
        teks_totalharga.gameObject.SetActive(true);

        MemasukkanPerubahanPemesanan?.Invoke();
        Debug.Log("Pilihan Bahan Mentah " + pilihanBahanMentah.nama);
    }
    public void Inkremen_Jumlah()
    {
        kuantitas_pembelianbahan++;
        teks_kuantitasbahan.text = kuantitas_pembelianbahan.ToString();
        Cek_Tombol();
        Update_TotalHarga();
        MemasukkanPerubahanPemesanan?.Invoke();
    }
    public void Dekremen_Jumlah()
    {
        kuantitas_pembelianbahan--;
        teks_kuantitasbahan.text = kuantitas_pembelianbahan.ToString();
        Cek_Tombol();
        Update_TotalHarga();
        MemasukkanPerubahanPemesanan?.Invoke();
    }
    private void Cek_Tombol()
    {
        tombol_dekremen.interactable = (kuantitas_pembelianbahan > 0);
        tombol_inkremen.interactable = (kuantitas_pembelianbahan < 20);
    }
    private void Update_TotalHarga()
    {
        teks_totalharga.text = $"{totalharga.ToString("N0")}";
    }

    public void MengurangiBahan()
    {
        Event_MengurangiBahan?.Invoke(this);
    }
}
