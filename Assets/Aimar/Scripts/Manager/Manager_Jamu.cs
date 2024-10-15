using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

namespace AimarWork
{
    public class Manager_Jamu : MonoBehaviour
    {
        public List<SO_Jamu> List_Jamu;
        public SO_Jamu jamu_difokuskan;

        public SO_Jamu jamuGagal;
        private Queue<SO_Jamu.Metode> langkah_langkah_pengolahan;

        [Title("Tipe-Tipe Bahan Olahan")]
        public List<SO_BahanOlahan> List_BlenderOlahan;
        public List<SO_BahanOlahan> List_MotongOlahan;
        public List<SO_BahanOlahan> List_RebusOlahan;
        public List<SO_BahanOlahan> List_AdukOlahan;
        public Dictionary<ENUM_Tipe_Pengolahan, List<SO_BahanOlahan>> Dictionary_BahanOlahan;

        public SO_BahanOlahan olahanGagal;
        private PlayerInventory playerInventory;
        private float kualitas = 0f;
        private int maxIndex = 1;

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
            Dictionary_BahanOlahan = new Dictionary<ENUM_Tipe_Pengolahan, List<SO_BahanOlahan>>
            {
                {ENUM_Tipe_Pengolahan.Memblender, List_BlenderOlahan },
                {ENUM_Tipe_Pengolahan.Memotong, List_MotongOlahan},
                {ENUM_Tipe_Pengolahan.Merebus, List_RebusOlahan},
                {ENUM_Tipe_Pengolahan.Mengaduk, List_AdukOlahan}
            };
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
            this.jamu_difokuskan = jamu_difokuskan;
            kualitas = 0;
            maxIndex = 0;
            langkah_langkah_pengolahan = new Queue<SO_Jamu.Metode>(this.jamu_difokuskan.List_Metode);
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

        public SO_BahanOlahan SelesaiProsesOlahan(ENUM_Tipe_Pengolahan tipePengolahan, float kualitas)
        {
            maxIndex++;
            this.kualitas = (this.kualitas + kualitas) / maxIndex;
            List<SO_BahanOlahan> List_BahanOlahan = Dictionary_BahanOlahan[tipePengolahan];
            List<SO_BahanBase> List_PengambilanBahan = new List<SO_BahanBase>();
            int bufferIndex = 0;
            foreach(SO_BahanOlahan bahanOlahan in List_BahanOlahan)
            {
                List<SO_BahanBase> List_BahanDibutuhkan = bahanOlahan.Bahan_Original;
                bool pengecekkan = true;
                for (int index = 0; index < List_BahanDibutuhkan.Count; index++)
                {
                    SO_BahanBase cek_bahan = List_BahanDibutuhkan[index];
                    bufferIndex = index;
                    if(!playerInventory.ListBahan.Contains(cek_bahan))
                    {
                        pengecekkan = false;
                        break;
                    }
                }
                if (pengecekkan)
                {
                    List_PengambilanBahan = List_BahanOlahan[bufferIndex].Bahan_Original;
                    break;
                }
                else
                {
                    bufferIndex++;
                }
                
            }
            if(List_PengambilanBahan.Count == 0)
            {
                Debug.Log("Gagal Membuat Olahan");
                return olahanGagal;    
            }
            else
            {
                for (int index = 0; index < List_PengambilanBahan.Count; index++)
                {
                    playerInventory.ListBahan.Remove(List_PengambilanBahan[index]);
                }
                Debug.Log("Berhasil Membuat Olahan " + List_BahanOlahan[bufferIndex].name);
                return List_BahanOlahan[bufferIndex];
            }   
        }
        public int GetKeuntungan()
        {
            SO_Jamu jamuDijual = playerInventory.jamu;
            int keuntungan = jamuDijual.GetBaseKeuntungan();
            if(kualitas < 0.75f)
            {
                return (int)(keuntungan * 0.75f);
            }
            else if(kualitas >= 0.75f && kualitas < .85f)
            {
                return (int)(keuntungan * 0.9f);
            }
            else if(kualitas >= 85 && kualitas <= 0.95)
            {
                return (int)(keuntungan * 1f);
            }
            else if(kualitas >= 0.95)
            {
                return (int)(keuntungan * 1.25f);
            }
            else
            {
                return 0;
            }
        }
        
        public void CheckJamu()
        {
            playerInventory.jamu = jamu_difokuskan.CheckBahan(playerInventory.ListBahan) ? jamu_difokuskan : jamuGagal;
            playerInventory.ListBahan.Clear();
        }
    }
}

