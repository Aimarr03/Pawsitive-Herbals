using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AimarWork
{
    public class PlayerInventory : MonoBehaviour
    {
        public List<SO_BahanBase> ListBahan;

        public void RemoveBahan()
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
    }
}

