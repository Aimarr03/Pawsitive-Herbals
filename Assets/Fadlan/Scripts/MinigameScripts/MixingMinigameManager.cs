using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    public class MixingMinigameManager : MonoBehaviour
    {

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
        private bool begin = false;
        private bool ended = false;
        private int direction = 1;
        private float timingPosition = 0.5f;


        void EndMix()
        {
            MixText.text = "Score: " + score + "/" + MixCount;
            ended = true;
        }

        void NextMix()
        {
            if (mixCounter < MixCount)
            {
                mixCounter++;

                MixSpeed += MixSpeedIncrease;
                MixText.text = "Mix " + mixCounter + "/" + MixCount;

                timingPosition = UnityEngine.Random.Range(0 + PerfectRange / 2, 1 - PerfectRange / 2);

                PerfectRectTransform.anchorMin = new Vector2(timingPosition - PerfectRange / 2, PerfectRectTransform.anchorMin.y);
                PerfectRectTransform.anchorMax = new Vector2(timingPosition + PerfectRange / 2, PerfectRectTransform.anchorMax.y);
            }
            else
            {
                EndMix();
            }
        }

        void PerfectMix()
        {
            score++;
            NextMix();
        }

        void BadMix()
        {
            NextMix();
        }

        void Update()
        {
            if (!begin)
                return;
            if (ended)
                return;

            TimingSlider.value = mixTiming;

            mixTiming += Time.deltaTime * MixSpeed * direction;

            if (mixTiming > 1)
            {
                direction = -1;
                turnCounter++;
            }

            if (mixTiming < 0)
            {
                direction = 1;
                turnCounter++;
            }

            if (turnCounter >= MaxTurnCount)
            {
                EndMix();
            }
        }

        public void MixPressed()
        {
            if (ended)
                return;

            if (!begin)
            {
                begin = true;
                NextMix();
                return;
            }

            if (Math.Abs(mixTiming - timingPosition) <= PerfectRange / 2)
            {
                PerfectMix();
            }
            else
            {
                BadMix();
            }
        }
    }
}
