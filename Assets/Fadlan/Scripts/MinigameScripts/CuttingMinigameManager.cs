using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    class CutTiming
    {
        public GameObject timingGameObject;
        public Slider timingSlider;
        public float timing;
    }

    public class CuttingMinigameManager : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private GameObject CutTimingPrefab;
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
        private bool begin = false;
        private bool ended = false;
        private List<CutTiming> cutTimings = new();

        void CheckForCuts()
        {
            CutTiming closestCutTiming = null;
            float closestCutTimingDistance = 0f;

            foreach (CutTiming cutTiming in cutTimings)
            {
                if (cutTiming.timing > closestCutTimingDistance)
                {
                    closestCutTiming = cutTiming;
                    closestCutTimingDistance = cutTiming.timing;
                }
            }

            if (closestCutTiming == null)
                return;

            if (Math.Abs(closestCutTiming.timing - TimingPosition) <= PerfectRange / 2)
            {
                score++;
            }

            Destroy(closestCutTiming.timingGameObject);
            cutTimings.Remove(closestCutTiming);

            CutText.text = "Cut: " + cutCounter + "/" + CutCount;

            CheckEnded();
        }


        void CheckForSpawn()
        {
            if (cutCounter >= CutCount)
                return;
            if (Time.time < nextCutSpawnTime)
                return;

            nextCutSpawnTime = Time.time + UnityEngine.Random.Range(MinInterval, MaxInterval);

            CutTiming cutTiming = new()
            {
                timing = 0f,
                timingGameObject = Instantiate(CutTimingPrefab, CutTimingParent),
            };
            cutTiming.timingSlider = cutTiming.timingGameObject.GetComponent<Slider>();
            cutTimings.Add(cutTiming);

            cutCounter++;

            CutText.text = "Cut: " + cutCounter + "/" + CutCount;
        }

        void UpdateCutTimings()
        {
            for (int i = cutTimings.Count - 1; i >= 0; i--)
            {
                CutTiming cutTiming = cutTimings[i];
                cutTiming.timing += Time.deltaTime * CutSpeed;
                cutTiming.timingSlider.value = cutTiming.timing;
                if (cutTiming.timing >= 1f)
                {
                    Destroy(cutTiming.timingGameObject);
                    cutTimings.RemoveAt(i);

                    CutText.text = "Cut: " + cutCounter + "/" + CutCount;
                    
                    CheckEnded();
                }
            }
        }

        void CheckEnded()
        {
            if (cutCounter >= CutCount && cutTimings.Count == 0)
            {
                CutText.text = "Score: " + score + "/" + CutCount;
                ended = true;
            }
        }


        void Start()
        {
            PerfectRectTransform.anchorMin = new Vector2(PerfectRectTransform.anchorMin.x, 1 - (TimingPosition + PerfectRange / 2));
            PerfectRectTransform.anchorMax = new Vector2(PerfectRectTransform.anchorMax.x, 1 - (TimingPosition - PerfectRange / 2));
        }

        void Update()
        {
            if (!begin)
                return;
            if (ended)
                return;

            CheckForSpawn();
            UpdateCutTimings();
        }

        public void CutPressed()
        {
            if (ended)
                return;

            if (!begin)
            {
                begin = true;
                return;
            }

            CheckForCuts();
        }
    }
}