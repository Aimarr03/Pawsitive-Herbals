using FadlanWork;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AimarWork
{
    public class PlayerInventory : MonoBehaviour
    {
        public List<SO_BahanBase> ListBahan;
        public SO_Jamu jamu;
        public static event Action<List<SO_BahanBase>> List_Berubah;
        public int maximum = 6;
        private void Start()
        {
            StoreMinigameManager.SelesaiMengolah += StoreMinigameManager_SelesaiMengolah;
        }
        private void OnDisable()
        {
            StoreMinigameManager.SelesaiMengolah -= StoreMinigameManager_SelesaiMengolah;
        }

        public void BersihkanSemuaBahanDiInventory()
        {
            ListBahan.Clear();
            jamu = null;
            List_Berubah?.Invoke(ListBahan);
        }
        public bool CheckBahan(SO_BahanMentah bahan_dicek)
        {
            if (ListBahan.Contains(bahan_dicek))
            {
                return false;
            }
            return true;
        }
        public void PenambahanBahan(List<SO_BahanBase> penambahan_bahan)
        {
            foreach (SO_BahanBase bahan in penambahan_bahan)
            {
                if (ListBahan.Contains(bahan)) continue;
                ListBahan.Add(bahan);
            }
            ListBahan = ListBahan.OrderBy(bahan_kini => bahan_kini.nama).ToList();
            
            List_Berubah?.Invoke(ListBahan);
        }
        public void PenambahanBahan(List<SO_BahanMentah> penambahan_bahan)
        {
            Debug.Log("Player Menambah Inventory");
            foreach (SO_BahanBase bahan in penambahan_bahan)
            {
                if (ListBahan.Contains(bahan)) continue;
                ListBahan.Add(bahan);
            }
            ListBahan = ListBahan.OrderBy(bahan_kini => bahan_kini.nama).ToList();
            
            List_Berubah?.Invoke(ListBahan);
        }
        public void PenambahanBahan(SO_BahanMentah bahanMentah)
        {
            if (ListBahan.Contains(bahanMentah) || maximum < 6)
            {
                return;
            }
            ListBahan.Add(bahanMentah);
            bahanMentah.kuantitasKini--;
            
            List_Berubah?.Invoke(ListBahan);
            maximum++;
        }
        public void PenguranganBahan(SO_BahanMentah bahanMentah)
        {
            if (ListBahan.Contains(bahanMentah))
            {
                ListBahan.Remove(bahanMentah);
                bahanMentah.kuantitasKini++;
                
                List_Berubah?.Invoke(ListBahan);
                maximum--;
            }
        }
        public void PembarisanInventory()
        {
            ListBahan = ListBahan.OrderBy(bahan_kini => bahan_kini.nama).ToList();

            List_Berubah?.Invoke(ListBahan);
        }

        private void StoreMinigameManager_SelesaiMengolah(ENUM_Tipe_Pengolahan tipe, float score)
        {
            ListBahan.Add(Manager_TokoJamu.instance.SelesaiProsesOlahan(tipe, score));

            List_Berubah?.Invoke(ListBahan);
        }
    }
}

