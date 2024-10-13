using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AimarWork
{
    public class Manager_Jamu : MonoBehaviour
    {
        public List<SO_Jamu> List_Jamu;
        public SO_Jamu jamu_difoksukan;
        public SO_BahanOlahan olahanGagal;
        private Queue<SO_Jamu.Metode> langkah_langkah_pengolahan;
        private PlayerInventory playerInventory;

        public static Manager_Jamu instance;
        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            playerInventory = FindObjectOfType<PlayerInventory>();
        }
        private void Start()
        {
            SetJamu(List_Jamu[1]);
        }
        public List<SO_BahanMentah> GetSemuaBahanMentah()
        {
            List<SO_BahanMentah> List_Bahan_Mentah = new List<SO_BahanMentah>();
            foreach(SO_Jamu jamu in List_Jamu)
            {
                foreach(SO_BahanMentah bahan_mentah in jamu.List_Bahan_Mentah)
                {
                    if (List_Bahan_Mentah.Contains(bahan_mentah)) continue;
                    List_Bahan_Mentah.Add(bahan_mentah);
                }
            }
            List_Bahan_Mentah = List_Bahan_Mentah.OrderBy(bahan => bahan.nama).ToList();
            return List_Bahan_Mentah;
        }
        public void SetJamu(SO_Jamu jamu_difokuskan)
        {
            this.jamu_difoksukan = jamu_difokuskan;
            langkah_langkah_pengolahan = new Queue<SO_Jamu.Metode>(jamu_difoksukan.List_Metode);
        }
        public void PemesananJamu()
        {
            SO_Jamu jamu_dicari = List_Jamu[Random.Range(0, List_Jamu.Count)];
            if (!jamu_dicari.CheckJamuMasihAda() || !jamu_dicari.terbuka)
            {
                PemesananJamu();
            }
            else
            {
                SetJamu(jamu_dicari);
            }
        }

        public SO_BahanOlahan SelesaiProsesOlahan(ENUM_Tipe_Pengolahan tipePengolahan)
        {
            SO_Jamu.Metode metode_kini = langkah_langkah_pengolahan.Dequeue();
            SO_BahanOlahan output = metode_kini.OutPut;
            bool benar = true;
            for(int index = 0; index <output.Bahan_Original.Count; index++)
            {
                SO_BahanBase cek_bahan = output.Bahan_Original[index];
                if (!playerInventory.ListBahan.Contains(cek_bahan))
                {
                    benar = false;
                    break;
                }
                else
                {
                    playerInventory.ListBahan.Remove(cek_bahan);
                }
            }
            return benar && metode_kini.tipePengolahan == tipePengolahan ? output : olahanGagal;
        }
    }
}

