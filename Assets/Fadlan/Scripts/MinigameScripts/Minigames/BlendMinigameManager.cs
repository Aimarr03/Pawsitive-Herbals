using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    public class BlendMinigameManager : MonoBehaviour
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
        [SerializeField] private TextMeshProUGUI BlendText;

        [Header("Minigame Config")]

        public float BlendDrainSpeed = 0.35f;
        public float BlendGainPerClick = 0.1f;
        public float BlendPerfectTime = 5f;
        public float BlendEndTime = 8f;
        public float PerfectRange = 0.25f;
        public float TimingPosition = 0.7f;

        private float blendTiming = 0f;
        private float blendTimer = 0f;
        private float score = 0f;

        private GameState currentState = GameState.NotStarted;

        void Start()
        {
            PerfectRectTransform.anchorMin = new Vector2(PerfectRectTransform.anchorMin.x, TimingPosition - PerfectRange / 2);
            PerfectRectTransform.anchorMax = new Vector2(PerfectRectTransform.anchorMax.x, TimingPosition + PerfectRange / 2);
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
            TimingSlider.value = blendTiming;

            blendTiming -= Time.deltaTime * BlendDrainSpeed;
            blendTiming = Mathf.Clamp(blendTiming, 0f, 1f);

            if (Mathf.Abs(blendTiming - TimingPosition) <= PerfectRange / 2)
            {
                score += Time.deltaTime;
                BlendText.text = $"Blending: {score:F1}/{BlendPerfectTime}";

                if (score >= BlendPerfectTime)
                {
                    EndBlend();
                }
            }
            else
            {
                BlendText.text = $"Blending: {score:F1}/{BlendPerfectTime}";
            }

            blendTimer += Time.deltaTime;
            if (blendTimer >= BlendEndTime)
            {
                EndBlend();
            }
        }

        private void EndBlend()
        {
            currentState = GameState.Ended;
            BlendText.text = $"Blend Score: {score:F1}/{BlendPerfectTime}";
        }

        public void BlendPressed()
        {
            switch (currentState)
            {
                case GameState.NotStarted:
                    StartBlend();
                    break;

                case GameState.Running:
                    GainBlend();
                    break;

                case GameState.Ended:
                    CloseGame();
                    break;
            }
        }

        private void StartBlend()
        {
            currentState = GameState.Running;
            blendTiming = 0f;
            blendTimer = 0f;
            score = 0f;
            BlendText.text = $"Blending: {score:F1}/{BlendPerfectTime}";
            TimingSlider.value = blendTiming;
        }

        private void GainBlend()
        {
            blendTiming += BlendGainPerClick;
            blendTiming = Mathf.Clamp(blendTiming, 0f, 1f);
        }

        private void CloseGame()
        {
            StoreMinigameManager.Instance.EndMinigame();
        }
    }
}