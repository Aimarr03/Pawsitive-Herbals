using AimarWork;
using DG.Tweening;
using Sirenix.OdinInspector;
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

        [Title("Visual Representation Duration")]
        public TextMeshProUGUI textVisual;
        public Button CancelButton;
        public AudioSource audioSource;
        public Animator animator;

        private float boilTiming = 0f;
        private int boilCounter = 0;
        private int score = 0;

        private GameState currentState = GameState.NotStarted;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        void Start()
        {
            PerfectRectTransform.anchorMin = new Vector2(PerfectRectTransform.anchorMin.x, TimingPosition - PerfectRange / 2);
            PerfectRectTransform.anchorMax = new Vector2(PerfectRectTransform.anchorMax.x, TimingPosition + PerfectRange / 2);
            CancelButton.onClick.AddListener(StoreMinigameManager.Instance.CancelMiniGame);

            TimingSlider.value = boilTiming;
            textVisual.text = "--/--";

            Manager_Audio.MuteSFX += Manager_Audio_MuteSFX;

            animator.speed = 0;
        }
        private void OnDisable()
        {
            Manager_Audio.MuteSFX -= Manager_Audio_MuteSFX;
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

                PerfectRectTransform.anchorMin = new Vector2(PerfectRectTransform.anchorMin.x, TimingPosition - PerfectRange / 2);
                PerfectRectTransform.anchorMax = new Vector2(PerfectRectTransform.anchorMax.x, TimingPosition + PerfectRange / 2);

                TimingSlider.value = boilTiming;
                textVisual.text = $"{boilCounter}/{BoilCount}";
            }
            else
            {
                animator.speed = 0;
                audioSource.DOFade(0, 1.3f);
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
            textVisual.text = $"{boilCounter}/{BoilCount}";
            CancelButton.interactable = false;
            audioSource.DOFade(1, 1.3f);
            animator.speed = 1;
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
            StoreMinigameManager.Instance.EndMinigame(score/BoilCount);
        }
        private void UpdateVisualTimer()
        {

        }
        private void Manager_Audio_MuteSFX(bool isMute)
        {
            audioSource.mute = isMute;
        }
    }
}
