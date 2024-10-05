using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    public class BoilMinigameManager : MonoBehaviour
    {

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
        private bool begin = false;
        private bool ended = false;


        void EndBoil()
        {
            BoilText.text = "Score: " + score + "/" + BoilCount;
            ended = true;
        }

        void NextBoil()
        {
            if (boilCounter < BoilCount)
            {
                boilCounter++;

                BoilSpeed += BoilSpeedIncrease;
                BoilText.text = "Boil " + boilCounter + "/" + BoilCount;
                boilTiming = 0;

                PerfectRectTransform.anchorMin = new Vector2(TimingPosition - PerfectRange / 2, PerfectRectTransform.anchorMin.y);
                PerfectRectTransform.anchorMax = new Vector2(TimingPosition + PerfectRange / 2, PerfectRectTransform.anchorMax.y);
            }
            else
            {
                EndBoil();
            }
        }

        void PerfectBoil()
        {
            score++;
            NextBoil();
        }

        void BadBoil()
        {
            NextBoil();
        }

        void Update()
        {
            if (!begin)
                return;
            if (ended)
                return;

            TimingSlider.value = boilTiming;

            boilTiming += Time.deltaTime * BoilSpeed;
            if (boilTiming > 1)
                BadBoil();
        }

        public void BoilPressed()
        {
            if (ended)
                return;

            if (!begin)
            {
                begin = true;
                NextBoil();
                return;
            }

            if (Math.Abs(boilTiming - TimingPosition) <= PerfectRange / 2)
            {
                PerfectBoil();
            }
            else
            {
                BadBoil();
            }
        }
    }
}
