using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    public class Manager_Jamu : MonoBehaviour
    {
        public SO_Jamu jamu_difoksukan;
        public SO_BahanOlahan olahanGagal;
        private Queue<SO_Jamu.Metode> langkah_langkah_pengolahan;

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
        }

        public void SetJamu(SO_Jamu jamu_difokuskan)
        {
            this.jamu_difoksukan = jamu_difokuskan;
            langkah_langkah_pengolahan = new Queue<SO_Jamu.Metode>(jamu_difoksukan.List_Metode);
        }

        public SO_BahanOlahan SelesaiProsesOlahan(ENUM_Tipe_Pengolahan tipePengolahan)
        {
            SO_Jamu.Metode metode_kini = langkah_langkah_pengolahan.Dequeue();
            SO_BahanOlahan output = metode_kini.OutPut;
            bool benar = true;
            for(int index = 0; index <output.Bahan_Original.Count; index++)
            {
                SO_BahanBase cek_bahan = output.Bahan_Original[index];
                //Mau melakukan pengecekkan inventory player
            }
            return benar ? output : olahanGagal;
        }
    }
}

