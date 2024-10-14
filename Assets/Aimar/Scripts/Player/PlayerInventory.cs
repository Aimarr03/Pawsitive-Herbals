using FadlanWork;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AimarWork
{
    public class PlayerInventory : MonoBehaviour
    {
        public List<SO_BahanBase> ListBahan;
        public SO_Jamu jamu;

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
        }
        public void PenambahanBahan(SO_BahanMentah bahanMentah)
        {
            if (ListBahan.Contains(bahanMentah)) return;
            ListBahan.Add(bahanMentah);
            bahanMentah.kuantitasKini--;
        }
        public void PenguranganBahan(SO_BahanMentah bahanMentah)
        {
            if (ListBahan.Contains(bahanMentah))
            {
                ListBahan.Remove(bahanMentah);
                bahanMentah.kuantitasKini++;
            }
        }
        public void PembarisanInventory() => ListBahan = ListBahan.OrderBy(bahan_kini => bahan_kini.nama).ToList();

        private void StoreMinigameManager_SelesaiMengolah(ENUM_Tipe_Pengolahan obj)
        {
            ListBahan.Add(Manager_Jamu.instance.SelesaiProsesOlahan(obj));
        }
    }
}

