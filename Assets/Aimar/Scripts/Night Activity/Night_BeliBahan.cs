using FadlanWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AimarWork
{
    public class Night_BeliBahan : BaseInteractableObject
    {
        [Header("Data Jamu")]
        public List<SO_BahanMentah> List_BahanMentah;
        [SerializeField] private List<SO_Jamu> List_Jamu;
        private List<Night_DropdownBeliBahan> List_Pembelian;

        [Header("Container Tipe Pembelian")]
        [SerializeField] private Night_DropdownBeliBahan formatPembelian;
        [SerializeField] private RectTransform PembelianContainer;

        [Header("UI")]
        [SerializeField] private Canvas UI_Container;
        [SerializeField] private TextMeshProUGUI text_TotalHarga;
        [SerializeField] private Button button_Pesan;
        [SerializeField] private Button button_TambahPesanan;
        [SerializeField] private TextMeshProUGUI JumlahPemesanan;

        private string STRING_bisa_pesan = "Tambah Pesanan";
        private string STRING_tidak_bisa_pesan= "Tidak Bisa Pesan";
        private int TotalBiaya = 0;
        private int maksPesanan = 10;
        private void Awake()
        {
            List_BahanMentah = new List<SO_BahanMentah>();
            List_Pembelian = new List<Night_DropdownBeliBahan>();
            MemasukkanDataBahanTerbuka();
            Menutup_UI_Pembelian();
        }
        private void Start()
        {
            Night_DropdownBeliBahan.MemasukkanPerubahanPemesanan += PerubahanPemesanan;
            Night_DropdownBeliBahan.Event_MengurangiBahan += Night_DropdownBeliBahan_Event_MengurangiBahan;
        }
        private void OnDisable()
        {
            Night_DropdownBeliBahan.MemasukkanPerubahanPemesanan -= PerubahanPemesanan;
            Night_DropdownBeliBahan.Event_MengurangiBahan -= Night_DropdownBeliBahan_Event_MengurangiBahan;
        }
        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Debug.Log("Mouse Enter");
        }
        private void OnMouseExit()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Debug.Log("Mouse Exit");
        }
        /*private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                
            }
        }*/
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            Membuka_UI_Pembelian();
            Debug.Log("Enter Beli Bahan");
        }
        private void MemasukkanDataBahanTerbuka()
        {
            foreach(SO_Jamu jamu in List_Jamu)
            {
                if (!jamu.terbuka) continue;
                foreach(SO_BahanMentah BahanMentah_jamu in jamu.List_Bahan_Mentah)
                {
                    if (List_BahanMentah.Contains(BahanMentah_jamu)) continue;
                    List_BahanMentah.Add(BahanMentah_jamu);
                }
            }
            List_BahanMentah = List_BahanMentah.OrderBy(jamu => jamu.nama).ToList();
        }
        private void BersihkanDaftarPembelian()
        {
            for(int index = 0; index < PembelianContainer.childCount; index++)
            {
                Night_DropdownBeliBahan tipe_pembelian_perIndex = PembelianContainer.GetChild(index).GetComponent<Night_DropdownBeliBahan>();
                if(tipe_pembelian_perIndex.gameObject != formatPembelian.gameObject)
                {
                    if (List_Pembelian.Contains(tipe_pembelian_perIndex)) List_Pembelian.Remove(tipe_pembelian_perIndex);
                    Destroy(tipe_pembelian_perIndex.gameObject);
                }
            }
        }
        private void PerubahanPemesanan()
        {
            TotalBiaya = 0;
            bool pengecekkanBisaPesan = true;
            foreach(Night_DropdownBeliBahan beliBahan in List_Pembelian)
            {
                if (beliBahan.pilihanBahanMentah == null)
                {
                    pengecekkanBisaPesan = false;
                    continue;
                }
                if (beliBahan.kuantitas_pembelianbahan == 0) pengecekkanBisaPesan = false;
                TotalBiaya += beliBahan.totalharga;
            }
            text_TotalHarga.text = $"Total Harga: Rp. {TotalBiaya.ToString("N2")}";
            button_Pesan.interactable = pengecekkanBisaPesan;

        }

        private void Night_DropdownBeliBahan_Event_MengurangiBahan(Night_DropdownBeliBahan obj)
        {
            List_Pembelian.Remove(obj);
            Destroy(obj.gameObject);
            PerubahanPemesanan();
            UpdateTombolMenambahPesanan();
        }

        public void MenambahkanDaftarBeli()
        {
            Night_DropdownBeliBahan tipe_pembelian_daftar = Instantiate(formatPembelian, PembelianContainer);
            tipe_pembelian_daftar.gameObject.SetActive(true);
            if (!List_Pembelian.Contains(tipe_pembelian_daftar)) List_Pembelian.Add(tipe_pembelian_daftar);
            UpdateTombolMenambahPesanan();
        }
        private void UpdateTombolMenambahPesanan()
        {
            if(List_Pembelian.Count < maksPesanan)
            {
                button_TambahPesanan.interactable = true;
                button_TambahPesanan.GetComponent<TextMeshProUGUI>().text = STRING_bisa_pesan;
            }
            else
            {
                button_TambahPesanan.interactable = false;
                button_TambahPesanan.transform.GetComponent<TextMeshProUGUI>().text = STRING_tidak_bisa_pesan;
            }
            JumlahPemesanan.text = $"maksimal pemesanan {List_Pembelian.Count}/{maksPesanan}";
        }
        
        #region Interaksi Mulai Pembelian
        public void Membuka_UI_Pembelian()
        {
            BersihkanDaftarPembelian();
            MenambahkanDaftarBeli();

            UI_Container.gameObject.SetActive(true);
        }
        public void Menutup_UI_Pembelian()
        {
            BersihkanDaftarPembelian();
            UI_Container.gameObject.SetActive(false);
        }
        #endregion
    }
}

