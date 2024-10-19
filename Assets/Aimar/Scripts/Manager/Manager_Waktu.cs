using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    namespace GameManagerLogic
    {
        public class Manager_Waktu : MonoBehaviour
        {
            public static Manager_Waktu instance;
            public List<DayStateData> List_DayStateData = new List<DayStateData>
            {
                new DayStateData(DayState.Day, 7, 8),
                new DayStateData(DayState.Night, 4, 6)
            };
            public DayStateData DataStatusHariKini;
            public DateTime TanggalKini;
            public bool IsPaused = false;

            //private int MaksimumJam = 24;
            //private int MaksimumJamAnalog = 12;
            private float DerajatRotasiPerJam = 30;
            private float DurasiWaktu = 30;
            private float KecepatanPutaranJam;
            private float KecepatanPutaranMenit;


            private bool IsOpen = false;
            private int Segmen_Kini = 0;
            public int Segmen_Max = 6;

            private float DurasiKini = 0;
            public GameObject TestRotasi;
            public GameObject TestRotasiMenit;

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
                DurasiKini = 0;
                DataStatusHariKini = List_DayStateData[0];
                
            }
            private void Update()
            {
                switch (DataStatusHariKini.dayState)
                {
                    case DayState.Day:
                        Logika_Siang();
                        break;
                    case DayState.Night:
                        break;
                }
                
            }
            private void Logika_Siang()
            {
                if (IsOpen && DurasiKini < DurasiWaktu)
                {
                    float RotasiJamPerFrame = Time.deltaTime * KecepatanPutaranJam;
                    float RotasiMenitPerFrame = Time.deltaTime * KecepatanPutaranMenit;
                    DurasiKini += Time.deltaTime;
                    //Debug.Log("Melakukan Rotasi " + RotasiPerFrame);
                    /*TestRotasi.transform.Rotate(0, 0, -RotasiJamPerFrame);
                    TestRotasiMenit.transform.Rotate(0, 0, -RotasiMenitPerFrame);*/
                }
                else
                {
                    IsOpen = false;
                }
            }
            public void BukaToko()
            {
                if (DataStatusHariKini.dayState != DayState.Day) return;
                DurasiKini = 0;
                IsOpen = true;
            }
            public void GantiStatusHari()
            {
                DataStatusHariKini = DataStatusHariKini.dayState == DayState.Day ? List_DayStateData[1] : List_DayStateData[0];
                if(DataStatusHariKini.dayState == DayState.Night)
                {
                    Segmen_Kini = Segmen_Max;
                }
                else
                {
                    DurasiKini = 0;
                    float RotasiZ = (DataStatusHariKini.startingHour * DerajatRotasiPerJam);
                    TestRotasi.transform.Rotate(0, 0, -RotasiZ);
                    SetRotasiKecepatanJam();
                    SetRotasiKecepatanMenit();
                }
            }
            public void GantiHari()
            {
                TanggalKini.AddDays(1);
            }
            public bool CekAktifitas(int harga) => Segmen_Kini >= harga;

            public bool CekTokoBuka() => IsOpen;
            private void SetRotasiKecepatanJam() => KecepatanPutaranJam = (DataStatusHariKini.maxHour * DerajatRotasiPerJam) / DurasiWaktu;
            private void SetRotasiKecepatanMenit() => KecepatanPutaranMenit = 360/(DurasiWaktu/DataStatusHariKini.maxHour);

            public void IncrementPausedSegment()
            {

            }
        }
        public struct DayStateData
        {
            public DayState dayState;
            public int startingHour;
            public int maxHour;
            
            public DayStateData(DayState dayState, int startingHour, int maxHour)
            {
                this.dayState = dayState;
                this.startingHour = startingHour;
                this.maxHour = maxHour;
            }
        }
        public enum DayState
        {
            Day,
            Night
        }
    }
    
}

