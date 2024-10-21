using AimarWork;
using AimarWork.GameManagerLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FadlanWork
{
    public class StoreMinigameManager : MonoBehaviour
    {
        public static StoreMinigameManager Instance {get; private set;}

        [Header("Minigame Prefabs")]
        public GameObject BlendMinigamePrefab;
        public GameObject BoilMinigamePrefab;
        public GameObject MixMinigamePrefab;
        public GameObject CutMinigamePrefab;

        public ENUM_Tipe_Pengolahan tipe_pengolahan;
        public static event Action<ENUM_Tipe_Pengolahan, float> SelesaiMengolah;
        public bool IsMinigameActive {get; private set;}
        public GameObject ActiveMinigameObject {get; private set;}

        void Awake()
        {
            if (Instance != null)
                throw new Exception("More than one instance of StoreMinigameManager");

            Instance = this;
        }

        public void StartMinigame(string minigameName)
        {
            if (IsMinigameActive)
                return;

            switch(minigameName){
                case "blend":
                    tipe_pengolahan = ENUM_Tipe_Pengolahan.Memblender;
                    ActiveMinigameObject = Instantiate(BlendMinigamePrefab);
                    break;
                case "mix":
                    tipe_pengolahan = ENUM_Tipe_Pengolahan.Mengaduk;
                    ActiveMinigameObject = Instantiate(MixMinigamePrefab);
                    break;
                case "cut":
                    tipe_pengolahan = ENUM_Tipe_Pengolahan.Memotong;
                    ActiveMinigameObject = Instantiate(CutMinigamePrefab);
                    break;
                case "boil":
                    tipe_pengolahan = ENUM_Tipe_Pengolahan.Merebus;
                    ActiveMinigameObject = Instantiate(BoilMinigamePrefab);
                    break;
            }

            IsMinigameActive = true;
        }
        public void CancelMiniGame()
        {
            Destroy(ActiveMinigameObject);
            IsMinigameActive = false;
        }
        public void EndMinigame(float score)
        {
            if (!IsMinigameActive)
                return;
            
            Destroy(ActiveMinigameObject);
            SelesaiMengolah?.Invoke(tipe_pengolahan, score);
            IsMinigameActive = false;
        }
    }
}
