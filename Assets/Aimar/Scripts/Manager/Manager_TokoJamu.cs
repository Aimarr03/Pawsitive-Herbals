using AimarWork.GameManagerLogic;
using FadlanWork;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AimarWork
{
    public class Manager_TokoJamu : MonoBehaviour
    {
        public List<SO_Jamu> List_Jamu;
        private List<SO_Jamu> Jamu_Terbuka;
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

        public int pelangganMax = 0;
        public int pelangganDihidangkan = 0;
        public float kualitasPerforma() 
        { 
            float kalkulasi = (pelangganDihidangkan * 1f) / pelangganMax;
            return kalkulasi;
        }
        public int uangDiperoleh;
        public int expDiperoleh;

        public static Manager_TokoJamu instance;
        [Title("Data Waktu")]
        public GameObject RotasiJarumJam;
        public GameObject RotasiJarumMenit;
        
        private float DurasiKini;
        [SerializeField] private float DurasiWaktu = 60;
        private bool IsOpen;

        private float KecepatanPutaranJam;
        private float KecepatanPutaranMenit;
        private float DerajatRotasiPerJam = 30;

        public event Action<SO_Jamu> MenghidangkanDenganBenar;
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
            Jamu_Terbuka = new List<SO_Jamu>();
            foreach(SO_Jamu jamu in List_Jamu)
            {
                if (jamu.terbuka)
                {
                    Jamu_Terbuka.Add(jamu);
                }
            }
        }
        private void Start()
        {
            SetJam();
            SetRotasiKecepatanJam();
            SetRotasiKecepatanMenit();
            CustomersQueueManager.Instance.OnAddedQueue += Instance_OnQueueChanged;
        }
        private void OnDisable()
        {
            CustomersQueueManager.Instance.OnAddedQueue -= Instance_OnQueueChanged;
        }
        private void Update()
        {
            if (Manager_Game.instance.IsPaused) return;
            switch (Manager_Waktu.instance.DataStatusHariKini.dayState)
            {
                case DayState.Day:
                    Logika_Siang();
                    break;
                case DayState.Night:
                    break;
            }
        }
        private void Instance_OnQueueChanged()
        {
            pelangganMax++;
        }
        public void HandleMenghidangiJamu(Customer customer)
        {
            Debug.Log($"{jamu_difokuskan} {playerInventory.jamu}");
            if (CheckJamuBenar())
            {
                Debug.Log("Jamu Dihidangkan benar");
                int uangDiperoleh = GetKeuntungan();
                this.uangDiperoleh += uangDiperoleh;
                Manager_Game.instance.TambahProfit(uangDiperoleh);

                int expDiperoleh = GetEXP();
                this.expDiperoleh += expDiperoleh;
                Manager_Game.instance.TambahExp(expDiperoleh);

                
                customer.GettingDeliveredRightJamu();
                MenghidangkanDenganBenar?.Invoke(jamu_difokuskan);
                pelangganDihidangkan++;
            }
            else
            {
                Debug.Log("Jamu Dihidangkan salah");
                customer.GettingDeliveredWrongJamu();
            }
            playerInventory.BersihkanSemuaBahanDiInventory();
            jamu_difokuskan = null;
            
        }
        #region Logika Jamu
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
            Debug.Log("Pemesanan Jamu " + jamu_difokuskan.nama);
            this.jamu_difokuskan = jamu_difokuskan;
            kualitas = 0;
            maxIndex = 0;
            langkah_langkah_pengolahan = new Queue<SO_Jamu.Metode>(this.jamu_difokuskan.List_Metode);
        }
        public SO_Jamu MencariPemesanan()
        {
            SO_Jamu jamu_dicari = null;
            int buffer = 0;
            do
            {
                buffer++;
                if (buffer > 8) break;
                jamu_dicari = Jamu_Terbuka[UnityEngine.Random.Range(0, Jamu_Terbuka.Count)];
                Debug.Log($"{jamu_dicari.nama} Terbuka {jamu_dicari.terbuka} Ada {jamu_dicari.CheckJamuMasihAda()}");
            } while (!jamu_dicari.terbuka || !jamu_dicari.CheckJamuMasihAda());
            return jamu_dicari;
        }
        public bool CheckJamuBenar()
        {
            return jamu_difokuskan == playerInventory.jamu;
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
                keuntungan = (int)(keuntungan * 0.75f);
            }
            else if(kualitas >= 0.75f && kualitas < .85f)
            {
                keuntungan = (int)(keuntungan * 0.9f);
            }
            else if(kualitas >= 85 && kualitas <= 0.95)
            {
                keuntungan = (int)(keuntungan * 1f);
            }
            else if(kualitas >= 0.95)
            {
                keuntungan = (int)(keuntungan * 1.25f);
            }
            else
            {
                keuntungan = 0;
            }
            
            int sisapuluhan = keuntungan % 100;
            keuntungan = keuntungan - sisapuluhan;
            return keuntungan;
        }
        public int GetEXP()
        {
            SO_Jamu jamuDijual = playerInventory.jamu;
            int exp = jamuDijual.GetExpProfit();
            if (kualitas < 0.75f)
            {
                exp = (int)(exp * 0.75f);
            }
            else if (kualitas >= 0.75f && kualitas < .85f)
            {
                exp = (int)(exp * 0.9f);
            }
            else if (kualitas >= 85 && kualitas <= 0.95)
            {
                exp = (int)(exp * 1f);
            }
            else if (kualitas >= 0.95)
            {
                exp = (int)(exp * 1.25f);
            }
            else
            {
                exp = 0;
            }

            int sisapuluhan = exp % 100;
            exp = exp - sisapuluhan;
            return exp;
        }
        
        public void CheckJamu()
        {
            bool benar = false;
            foreach(SO_Jamu jamuKini in List_Jamu)
            {
                Debug.Log("Pengecekkan" + jamuKini.name);
                if (!jamuKini.terbuka) continue;
                if (jamuKini.CheckBahan(playerInventory.ListBahan))
                {
                    Debug.Log("Dapat Jamu yang Benar");
                    benar = true;
                    break;
                }
            }
            playerInventory.jamu = benar? jamu_difokuskan : jamuGagal;
            playerInventory.ListBahan.Clear();
        }
        #endregion
        #region Logika waktu
        private void Logika_Siang()
        {
            if (IsOpen && DurasiKini < DurasiWaktu)
            {
                float RotasiJamPerFrame = Time.deltaTime * KecepatanPutaranJam;
                float RotasiMenitPerFrame = Time.deltaTime * KecepatanPutaranMenit;
                DurasiKini += Time.deltaTime;
                //Debug.Log("Melakukan Rotasi " + RotasiPerFrame);
                RotasiJarumJam.transform.Rotate(0, 0, -RotasiJamPerFrame);
                RotasiJarumMenit.transform.Rotate(0, 0, -RotasiMenitPerFrame);
            }
            else
            {
                IsOpen = false;
            }
        }
        public void BukaToko()
        {
            if (Manager_Waktu.instance.DataStatusHariKini.dayState != DayState.Day) return;
            Debug.Log("Buka Toko Jamu!");
            DurasiKini = 0;
            IsOpen = true;
        }
        
        public bool CekTokoBuka() => IsOpen;
        private void SetJam()
        {
            float derajatRotasiJam = DerajatRotasiPerJam * Manager_Waktu.instance.DataStatusHariKini.startingHour;
            //Debug.Log(derajatRotasiJam);
            RotasiJarumJam.transform.rotation = Quaternion.Euler(0, 0, -derajatRotasiJam);
        }
        private void SetRotasiKecepatanJam() => KecepatanPutaranJam = (Manager_Waktu.instance.DataStatusHariKini.maxHour * DerajatRotasiPerJam) / DurasiWaktu;
        private void SetRotasiKecepatanMenit() => KecepatanPutaranMenit = 360 / (DurasiWaktu / Manager_Waktu.instance.DataStatusHariKini.maxHour);
        #endregion
    }
}

