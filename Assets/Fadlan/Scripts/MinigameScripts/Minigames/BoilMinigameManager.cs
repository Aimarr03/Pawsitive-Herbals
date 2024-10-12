using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    public class BoilMinigameManager : MonoBehaviour
    {
        private enum GameState
        {
            NotStarted,
            Running,
            Ended
        }

        [Header("UI Components")]
        [SerializeField] private Slider TimingSlider;
        [SerializeField] private RectTransform PerfectRectTransform;
        [SerializeField] private TextMeshProUGUI BoilText;

        [Header("Minigame Config")]
        public float BoilSpeed = 0.5f;
        public float BoilSpeedIncrease = 0.1f;
        public float TimingPosition = 0.7f;
        public float PerfectRange = 0.2f;
        public int BoilCount = 3;

        private float boilTiming = 0f;
        private int boilCounter = 0;
        private int score = 0;

        private GameState currentState = GameState.NotStarted;

        void Start()
        {
            PerfectRectTransform.anchorMin = new Vector2(TimingPosition - PerfectRange / 2, PerfectRectTransform.anchorMin.y);
            PerfectRectTransform.anchorMax = new Vector2(TimingPosition + PerfectRange / 2, PerfectRectTransform.anchorMax.y);

            TimingSlider.value = boilTiming;
        }

        void Update()
        {
            switch (currentState)
            {
                case GameState.Running:
                    UpdateRunningState();
                    break;
                default:
                    break;
            }
        }

        private void UpdateRunningState()
        {
            TimingSlider.value = boilTiming;

            boilTiming += Time.deltaTime * BoilSpeed;

            if (boilTiming > 1f)
            {
                BadBoil();
                return;
            }
        }

        private void EndBoil()
        {
            currentState = GameState.Ended;
            BoilText.text = $"Score: {score}/{BoilCount}";
        }

        private void NextBoil()
        {
            if (boilCounter < BoilCount)
            {
                boilCounter++;
                BoilSpeed += BoilSpeedIncrease;
                boilTiming = 0f;

                BoilText.text = $"Boil {boilCounter}/{BoilCount}";

                PerfectRectTransform.anchorMin = new Vector2(TimingPosition - PerfectRange / 2, PerfectRectTransform.anchorMin.y);
                PerfectRectTransform.anchorMax = new Vector2(TimingPosition + PerfectRange / 2, PerfectRectTransform.anchorMax.y);

                TimingSlider.value = boilTiming;
            }
            else
            {
                EndBoil();
            }
        }

        private void PerfectBoil()
        {
            score++;
            NextBoil();
        }

        private void BadBoil()
        {
            NextBoil();
        }

        public void BoilPressed()
        {
            switch (currentState)
            {
                case GameState.NotStarted:
                    StartBoil();
                    break;

                case GameState.Running:
                    HandleBoilPress();
                    break;

                case GameState.Ended:
                    CloseGame();
                    break;
            }
        }

        private void StartBoil()
        {
            currentState = GameState.Running;
            boilTiming = 0f;
            boilCounter = 0;
            score = 0;
            TimingSlider.value = boilTiming;
            NextBoil();
        }

        private void HandleBoilPress()
        {
            if (Mathf.Abs(boilTiming - TimingPosition) <= PerfectRange / 2)
            {
                PerfectBoil();
            }
            else
            {
                BadBoil();
            }
        }

        private void CloseGame()
        {
            StoreMinigameManager.Instance.EndMinigame();
        }
    }
}
