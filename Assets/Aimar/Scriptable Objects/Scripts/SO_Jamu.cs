using AimarWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AimarWork
{
    [CreateAssetMenu(fileName = "Jamu Baru", menuName = "Jamu/Buat Jamu Baru")]
    public class SO_Jamu : ScriptableObject
    {
        [Serializable]
        public struct Metode
        {
            public ENUM_Tipe_Pengolahan tipePengolahan;
            public SO_BahanOlahan OutPut;
            public string langkah;
        }
        public string nama;
        public int level = 0;
        public Sprite ikon;
        [TextArea(5, 5)]
        public string deskripsi;
        [TextArea(5, 5)]
        public string manfaat;

        public List<SO_BahanBase> List_Bahan_Yang_Diperlukan;
        public List<SO_BahanMentah> List_Bahan_Mentah;
        public List<SO_BahanOlahan> List_BahanOlahan;
        public List<Metode> List_Metode;
        [Range(1, 2f)]
        public float multiplierBaseKeuntungan = 1f;
        public int GetBaseKeuntungan()
        {
            int base_keuntungan = 0;
            foreach(SO_BahanMentah bahan in List_Bahan_Mentah)
            {
                base_keuntungan += bahan.hargaPerSatuan;
            }
            return (int)(base_keuntungan *multiplierBaseKeuntungan);
        }
        public List<SO_BahanOlahan> GetBahanOlahan(ENUM_Tipe_Pengolahan enum_tipe_pengolahan)
        {
            List<SO_BahanOlahan> List_TipeBahanOlahan = new List<SO_BahanOlahan>();
            foreach(SO_BahanOlahan bahan_olahan in List_BahanOlahan)
            {
                if(bahan_olahan.tipePengolahan == enum_tipe_pengolahan) List_TipeBahanOlahan.Add(bahan_olahan);
            }
            return List_TipeBahanOlahan;
        }

        public bool terbuka;
        public int kualitas;
        public int base_keuntungan;
        public int base_exp;

        public bool CheckBahan(List<SO_BahanBase> List_BahanPembuatan)
        {
            List_BahanPembuatan.OrderBy(bahan => bahan.nama).ToList();
            List<SO_BahanBase> List_Bahan_Yang_Diperlukan = this.List_Bahan_Yang_Diperlukan.OrderBy(bahan => bahan.nama).ToList();
            if (List_BahanPembuatan.Count > List_Bahan_Yang_Diperlukan.Count) return false;
            for (int i = 0; i < List_BahanPembuatan.Count; i++)
            {
                if (List_BahanPembuatan[i] != List_Bahan_Yang_Diperlukan[i]) return false;
            }
            return true;
        }
        public bool CheckJamuMasihAda()
        {
            foreach(SO_BahanMentah bahan_kini in List_Bahan_Mentah)
            {
                if (bahan_kini.kuantitasKini == 0) return false;
            }
            return true;
        }
    }
}

