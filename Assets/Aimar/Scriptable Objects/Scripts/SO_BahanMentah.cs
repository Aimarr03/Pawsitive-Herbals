using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    [CreateAssetMenu(fileName ="Bahan Mentah Baru", menuName = "Bahan/Buat Bahan Mentah Baru")]
    public class SO_BahanMentah : SO_BahanBase
    {
        public ENUM_Tipe_BahanMentah tipe;
        
        public int kuantitasKini;
        public ENUM_TipeKuantitasFormat tipeKuantitasFormat;

        public int hargaPerSatuan;
        public bool terbuka;
    }
    public enum ENUM_Tipe_BahanMentah
    {
        Kuantitas,
        Takaran
    }
    public enum ENUM_TipeKuantitasFormat
    {
        Satuan,
        Gram,
        Mililiter
    }
}

