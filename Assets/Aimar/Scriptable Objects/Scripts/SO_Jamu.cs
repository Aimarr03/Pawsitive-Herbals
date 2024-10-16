using AimarWork;
using Sirenix.OdinInspector;
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
        [Title("General Data")]
        public string nama;
        public int level = 0;
        public Sprite ikon;
        [TextArea(5, 5)]
        public string deskripsi;
        [TextArea(5, 5)]
        public string manfaat;
        
        [Title("Bahan Bahan Yang Relevansi")]
        public List<SO_BahanBase> List_Bahan_Yang_Diperlukan;
        public List<SO_BahanMentah> List_Bahan_Mentah;
        public List<SO_BahanOlahan> List_BahanOlahan;
        public List<Metode> List_Metode;

        [Title("Data Keuntungan dan EXP")]
        [Range(1, 2f)]
        public float multiplierBase = 1f;
        [Range(0.3f,1f)]
        public float multiplierPerLevel = 0.5f;
        public bool terbuka;
        public int kualitas;
        public int base_keuntungan;
        public int base_exp;
        public int exp_dibuka;

        #region Cek Keuntungan dan EXP
        [Button("Test Keuntungan Berapa")]
        public void CheckKeuntungan()
        {
            Debug.Log("Perbandingan Keuntungan");
            int rawBaseKeuntungan = 0;
            foreach (SO_BahanMentah bahan in List_Bahan_Mentah)
            {
                rawBaseKeuntungan += bahan.hargaPerSatuan;
            }
            Debug.Log("Raw Keuntungan! " + rawBaseKeuntungan);
            
            
            base_keuntungan = GetBaseKeuntungan();
            Debug.Log("Dengan Multiplier " + base_keuntungan);
        }
        
        [Button("Text Exp Berapa")]
        public void CheckExp()
        {
            Debug.Log("Exp adalah " + GetExpProfit());
        }
        [Button("Text Exp Dibutuhkan Untuk Diriset")]
        public void CheckExpDibutuhkan()
        {
            Debug.Log("Exp adalah " + GetExpDiperlukan());
        }

        public int GetExpDiperlukan()
        {
            int rawExp = (int)(exp_dibuka * (multiplierBase + (multiplierPerLevel * level)));
            int remainder = rawExp % 100;
            rawExp = rawExp - remainder;
            rawExp += remainder > 50 ? 100 : 0;
            return rawExp;
        }
        
        public int GetExpProfit() => (int)(base_exp * (multiplierBase + (multiplierPerLevel * level)));
        public int GetBaseKeuntungan()
        {
            int base_keuntungan = 0;
            foreach (SO_BahanMentah bahan in List_Bahan_Mentah)
            {
                base_keuntungan += bahan.hargaPerSatuan;
            }

            int totalRaw = (int)(base_keuntungan * (multiplierBase + (level * multiplierPerLevel)));
            int sisaSeratusan = totalRaw % 1000;

            if (sisaSeratusan > 500)
            {
                totalRaw = totalRaw - sisaSeratusan + 1000;
            }
            else if (sisaSeratusan <= 500)
            {
                totalRaw = totalRaw - sisaSeratusan + 500;
            }

            return totalRaw;
        }
        #endregion

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

