using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    public class BlendMinigameManager : MonoBehaviour
    {

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
        private bool begin = false;
        private bool ended = false;


        void Start()
        {
            PerfectRectTransform.anchorMin = new Vector2(PerfectRectTransform.anchorMin.x, TimingPosition - PerfectRange / 2);
            PerfectRectTransform.anchorMax = new Vector2(PerfectRectTransform.anchorMax.x, TimingPosition + PerfectRange / 2);
        }

        void EndBlend()
        {
            ended = true;
            BlendText.text = "Blend Score: " + score.ToString("F1") + "/" + BlendPerfectTime;
        }

        void Update()
        {
            if (!begin)
                return;
            if (ended)
                return;

            TimingSlider.value = blendTiming;

            blendTiming -= Time.deltaTime * BlendDrainSpeed;
            if (blendTiming < 0f)
                blendTiming = 0f;

            if (Math.Abs(blendTiming - TimingPosition) <= PerfectRange / 2)
            {
                score += Time.deltaTime;
                BlendText.text = "Blending: " + score.ToString("F1") + "/" + BlendPerfectTime;

                if (score >= BlendPerfectTime)
                {
                    EndBlend();
                }
            }

            blendTimer += Time.deltaTime;
            if (blendTimer >= BlendEndTime)
            {
                EndBlend();
            }
        }

        public void BlendPressed()
        {
            if (ended)
                return;

            if (!begin)
            {
                begin = true;
                return;
            }

            blendTiming += BlendGainPerClick;
            if (blendTiming > 1f)
                blendTiming = 1f;
        }
    }
}
