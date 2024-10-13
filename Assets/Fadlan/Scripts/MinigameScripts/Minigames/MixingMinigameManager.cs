using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    public class MixingMinigameManager : MonoBehaviour
    {
        // Define the possible states of the minigame
        private enum GameState
        {
            NotStarted,
            Running,
            Ended
        }

        [Header("UI Components")]
        [SerializeField] private Slider TimingSlider;
        [SerializeField] private RectTransform PerfectRectTransform;
        [SerializeField] private TextMeshProUGUI MixText;

        [Header("Minigame Config")]
        public float MixSpeed = 1f;
        public float MixSpeedIncrease = 0.1f;
        public float PerfectRange = 0.25f;
        public int MixCount = 4;
        public int MaxTurnCount = 8;

        private float mixTiming = 0f;
        private int mixCounter = 0;
        private int turnCounter = 0;
        private int score = 0;
        private int direction = 1;
        private float timingPosition = 0.5f;

        private GameState currentState = GameState.NotStarted;

        void Start()
        {
            PerfectRectTransform.anchorMin = new Vector2(timingPosition - PerfectRange / 2, PerfectRectTransform.anchorMin.y);
            PerfectRectTransform.anchorMax = new Vector2(timingPosition + PerfectRange / 2, PerfectRectTransform.anchorMax.y);
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
            TimingSlider.value = mixTiming;

            mixTiming += Time.deltaTime * MixSpeed * direction;

            if (mixTiming >= 1f)
            {
                direction = -1;
                turnCounter++;
            }
            else if (mixTiming <= 0f)
            {
                direction = 1;
                turnCounter++;
            }

            if (turnCounter >= MaxTurnCount)
            {
                EndMix();
            }
        }

        private void EndMix()
        {
            currentState = GameState.Ended;
            MixText.text = $"Score: {score}/{MixCount}";
        }

        private void NextMix()
        {
            if (mixCounter < MixCount)
            {
                mixCounter++;
                MixSpeed += MixSpeedIncrease;

                timingPosition = UnityEngine.Random.Range(0 + PerfectRange / 2, 1 - PerfectRange / 2);

                PerfectRectTransform.anchorMin = new Vector2(timingPosition - PerfectRange / 2, PerfectRectTransform.anchorMin.y);
                PerfectRectTransform.anchorMax = new Vector2(timingPosition + PerfectRange / 2, PerfectRectTransform.anchorMax.y);

                MixText.text = $"Mix {mixCounter}/{MixCount}";
            }
            else
            {
                EndMix();
            }
        }

        private void PerfectMix()
        {
            score++;
            NextMix();
        }

        private void BadMix()
        {
            NextMix();
        }

        public void MixPressed()
        {
            switch (currentState)
            {
                case GameState.NotStarted:
                    StartMix();
                    break;

                case GameState.Running:
                    HandleMixPress();
                    break;

                case GameState.Ended:
                    CloseGame();
                    break;
            }
        }

        private void StartMix()
        {
            currentState = GameState.Running;
            mixTiming = 0f;
            mixCounter = 0;
            turnCounter = 0;
            score = 0;
            TimingSlider.value = mixTiming;
            NextMix();
        }

        private void HandleMixPress()
        {
            if (Mathf.Abs(mixTiming - timingPosition) <= PerfectRange / 2)
            {
                PerfectMix();
            }
            else
            {
                BadMix();
            }
        }

        private void CloseGame()
        {
            StoreMinigameManager.Instance.EndMinigame();
        }
    }
}
