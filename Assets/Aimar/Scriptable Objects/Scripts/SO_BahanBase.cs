using System;
using UnityEngine;

namespace AimarWork
{
    [Serializable]
    public abstract class SO_BahanBase : ScriptableObject
    {
        public string nama;

        [TextArea(4, 6)]
        public string deskripsi;
        public Sprite ikon_resep;
        public Sprite ikon_gameplay;
    }
}

