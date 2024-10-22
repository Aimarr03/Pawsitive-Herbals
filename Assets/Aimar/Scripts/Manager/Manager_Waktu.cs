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

            //private int MaksimumJam = 24;
            //private int MaksimumJamAnalog = 12;
            private int Segmen_Kini = 0;
            public int Segmen_Max = 6;

            public event Action OnChangeStatusHari;
            public event Action ChangeHari;
            

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
                
                DataStatusHariKini = List_DayStateData[0];
                TanggalKini = new DateTime(2024, 10, 1);
            }


            public void GantiStatusHari()
            {
                DataStatusHariKini = DataStatusHariKini.dayState == DayState.Day ? List_DayStateData[1] : List_DayStateData[0];
                OnChangeStatusHari?.Invoke();
            }
            public void GantiHari()
            {
                TanggalKini.AddDays(1);
            }
            public bool CekAktifitas(int harga) => Segmen_Kini >= harga;
            public void IncrementPausedSegment()
            {

            }
        }
        [Serializable]
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
        [Serializable]
        public enum DayState
        {
            Day,
            Night
        }
    }
    
}

