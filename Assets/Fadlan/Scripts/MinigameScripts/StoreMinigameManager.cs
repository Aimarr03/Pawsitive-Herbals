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
                    ActiveMinigameObject = Instantiate(BlendMinigamePrefab);
                    break;
                case "mix":
                    ActiveMinigameObject = Instantiate(MixMinigamePrefab);
                    break;
                case "cut":
                    ActiveMinigameObject = Instantiate(CutMinigamePrefab);
                    break;
                case "boil":
                    ActiveMinigameObject = Instantiate(BoilMinigamePrefab);
                    break;
            }

            IsMinigameActive = true;
        }

        public void EndMinigame()
        {
            if (!IsMinigameActive)
                return;
            
            Destroy(ActiveMinigameObject);

            IsMinigameActive = false;
        }
    }
}
