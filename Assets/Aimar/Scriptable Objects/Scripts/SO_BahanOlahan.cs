using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    [CreateAssetMenu(fileName ="Bahan Olahan Baru", menuName ="Bahan/Buat Bahan Olahan Baru")]
    public class SO_BahanOlahan : SO_BahanBase
    {
        public List<SO_BahanBase> Bahan_Original;
        public ENUM_Tipe_Pengolahan tipePengolahan;
        public ENUM_Status_Pengolahan statusPengolahan;
    }
    public enum ENUM_Tipe_Pengolahan
    {
        Merebus,
        Mengaduk,
        Memotong,
        Memblender
    }
    public enum ENUM_Status_Pengolahan
    {
        Gagal,
        Berhasil
    }
}

