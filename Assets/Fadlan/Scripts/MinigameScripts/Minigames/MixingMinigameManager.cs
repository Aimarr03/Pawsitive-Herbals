using AimarWork;
using Sirenix.OdinInspector;
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
        [SerializeField] private Animator mixAnimator;

        [Header("Minigame Config")]
        public float MixSpeed = 1f;
        public float MixSpeedIncrease = 0.1f;
        public float PerfectRange = 0.25f;
        public int MixCount = 4;
        public int MaxTurnCount = 8;

        [Title("Format Max Mixing")]
        [SerializeField] private RectTransform FormatMixing;
        [SerializeField] private RectTransform containerMaxMixing;
        public AudioClip mixingSound;
        public Button CancelButton;

        private float mixTiming = 0f;
        private int mixCounter = 0;
        private int turnCounter = 0;
        private int score = 0;
        private int direction = 1;
        private float timingPosition = 0.5f;

        private GameState currentState = GameState.NotStarted;
        private void Awake()
        {
            mixAnimator = GetComponent<Animator>();
        }
        void Start()
        {
            PerfectRectTransform.anchorMin = new Vector2(timingPosition - PerfectRange / 2, PerfectRectTransform.anchorMin.y);
            PerfectRectTransform.anchorMax = new Vector2(timingPosition + PerfectRange / 2, PerfectRectTransform.anchorMax.y);
            containerMaxMixing.gameObject.SetActive(false);
            CancelButton.onClick.AddListener(StoreMinigameManager.Instance.CancelMiniGame);
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
                UpdateMaxMixing();
            }
            else if (mixTiming <= 0f)
            {
                direction = 1;
                turnCounter++;
                UpdateMaxMixing();
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
            CancelButton.interactable = false;
            ResettingMaxMixing();
        }

        private void HandleMixPress()
        {
            mixAnimator.SetTrigger("Mixing");
            Manager_Audio.instance.PlaySFX(mixingSound);
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
            StoreMinigameManager.Instance.EndMinigame(score/mixCounter);
        }
        private void ResettingMaxMixing()
        {
            for(int index = 0; index < containerMaxMixing.childCount; index++)
            {
                Transform childFormat = containerMaxMixing.transform.GetChild(index);
                if(childFormat != FormatMixing.transform)
                {
                    Destroy(childFormat.gameObject);
                }
            }
            for(int index = 0; index < MaxTurnCount; index++)
            {
                Transform childFormat = Instantiate(FormatMixing, containerMaxMixing);
                childFormat.gameObject.SetActive(true);
            }
            containerMaxMixing.gameObject.SetActive(true);
        }
        private void UpdateMaxMixing()
        {
            int bufferIndex = MaxTurnCount - turnCounter + 1;
            Transform childMix = containerMaxMixing.GetChild(bufferIndex);
            Image activeVisual = childMix.GetChild(1).GetComponent<Image>();
            activeVisual.gameObject.SetActive(false);
        }
    }
}
