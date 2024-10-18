using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    class CutTiming
    {
        public GameObject TimingGameObject;
        public Slider TimingSlider;
        public float Timing;
    }

    public class CuttingMinigameManager : MonoBehaviour
    {
        private enum GameState
        {
            NotStarted,
            Running,
            Ended
        }

        [Header("UI Components")]
        [SerializeField] private GameObject CutTimingPrefab;
        [SerializeField] private Sprite spritePrefab;
        [SerializeField] private Transform CutTimingParent;
        [SerializeField] private RectTransform PerfectRectTransform;
        [SerializeField] private TextMeshProUGUI CutText;

        [Header("Minigame Config")]
        public float CutSpeed = 1f;
        public float TimingPosition = 0.75f;
        public float PerfectRange = 0.2f;
        public int CutCount = 8;
        public float MinInterval = 0.1f;
        public float MaxInterval = 0.5f;

        private int cutCounter = 0;
        private float nextCutSpawnTime = 0f;
        private int score = 0;
        private List<CutTiming> cutTimings = new();
        private Animator cuttingAnimator;

        private GameState currentState = GameState.NotStarted;

        private void Awake()
        {
            cuttingAnimator = GetComponent<Animator>();
        }
        void Start()
        {
            PerfectRectTransform.anchorMin = new Vector2(PerfectRectTransform.anchorMin.x, 1 - (TimingPosition + PerfectRange / 2));
            PerfectRectTransform.anchorMax = new Vector2(PerfectRectTransform.anchorMax.x, 1 - (TimingPosition - PerfectRange / 2));
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
            CheckForSpawn();
            UpdateCutTimings();
        }

        private void CheckForSpawn()
        {
            if (cutCounter >= CutCount)
                return;

            if (Time.time < nextCutSpawnTime)
                return;

            nextCutSpawnTime = Time.time + UnityEngine.Random.Range(MinInterval, MaxInterval);

            CutTiming cutTiming = new()
            {
                Timing = 0f,
                TimingGameObject = Instantiate(CutTimingPrefab, CutTimingParent),
            };

            cutTiming.TimingSlider = cutTiming.TimingGameObject.GetComponent<Slider>();
            cutTiming.TimingGameObject.transform.GetChild(0).GetComponent<Image>().sprite = spritePrefab;
            if (cutTiming.TimingSlider == null)
            {
                Debug.LogError("CutTimingPrefab does not have a Slider component.");
                Destroy(cutTiming.TimingGameObject);
                return;
            }

            cutTimings.Add(cutTiming);
            cutCounter++;

            CutText.text = $"Cut: {cutCounter}/{CutCount}";
        }

        private void UpdateCutTimings()
        {
            for (int i = cutTimings.Count - 1; i >= 0; i--)
            {
                CutTiming cutTiming = cutTimings[i];
                cutTiming.Timing += Time.deltaTime * CutSpeed;
                cutTiming.TimingSlider.value = cutTiming.Timing;

                if (cutTiming.Timing >= 1f)
                {
                    Destroy(cutTiming.TimingGameObject);
                    cutTimings.RemoveAt(i);

                    CutText.text = $"Cut: {cutCounter}/{CutCount}";

                    CheckEnded();
                }
            }
        }

        private void CheckForCuts()
        {
            if (cutTimings.Count == 0)
                return;

            CutTiming closestCutTiming = null;
            float closestCutTimingDistance = -1f;

            foreach (CutTiming cutTiming in cutTimings)
            {
                if (cutTiming.Timing > closestCutTimingDistance)
                {
                    closestCutTiming = cutTiming;
                    closestCutTimingDistance = cutTiming.Timing;
                }
            }

            if (closestCutTiming == null)
                return;

            if (Mathf.Abs(closestCutTiming.Timing - TimingPosition) <= PerfectRange / 2)
            {
                score++;
            }

            Destroy(closestCutTiming.TimingGameObject);
            cutTimings.Remove(closestCutTiming);

            CutText.text = $"Cut: {cutCounter}/{CutCount}";

            CheckEnded();
        }

        private void CheckEnded()
        {
            if (cutCounter >= CutCount && cutTimings.Count == 0)
            {
                currentState = GameState.Ended;
                CutText.text = $"Score: {score}/{CutCount}";
            }
        }

        public void CutPressed()
        {
            switch (currentState)
            {
                case GameState.NotStarted:
                    StartCutting();
                    break;

                case GameState.Running:
                    HandleCutPress();
                    break;

                case GameState.Ended:
                    CloseGame();
                    break;
            }
        }

        private void StartCutting()
        {
            currentState = GameState.Running;
            cutCounter = 0;
            score = 0;
            nextCutSpawnTime = Time.time + UnityEngine.Random.Range(MinInterval, MaxInterval);

            CutText.text = $"Cut: {cutCounter}/{CutCount}";
        }

        private void HandleCutPress()
        {
            cuttingAnimator.SetTrigger("Cutting");
            CheckForCuts();
        }

        private void CloseGame()
        {
            StoreMinigameManager.Instance.EndMinigame(cutCounter/CutCount);
        }
    }
}
