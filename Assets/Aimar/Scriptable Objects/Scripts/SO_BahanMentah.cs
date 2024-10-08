using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    [CreateAssetMenu(fileName ="Bahan Mentah Baru", menuName = "Bahan/Buat Bahan Mentah Baru")]
    public class SO_BahanMentah : SO_BahanBase
    {
        public int kuantitasKini;

        public int hargaPerSatuan;
        public bool terbuka;
    }
}

