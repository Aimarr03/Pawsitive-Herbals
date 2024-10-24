using System.Collections;
using System.Collections.Generic;
using AimarWork;
using AimarWork.GameManagerLogic;
using DG.Tweening;
using FadlanWork;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace FadlanWork
{
    public class StoreDoorInteract : BaseInteractableObject
    {
        public float SecondsToOpen = 3f;

        private float openTimer = 0f;
        private Coroutine openingCoroutine;

        public AudioClip openDoor;
        [Title("VIsual Progress Buka Pintu")]
        public Image VisualProgress;
        public RectTransform VisualContainer;

        [Title("Visual Bantuan")]
        public RectTransform VisualGuideBukaPintu;

        private void Awake()
        {
            VisualGuideBukaPintu.DOAnchorPosY(VisualGuideBukaPintu.anchoredPosition.y + 0.35f, 1.5f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
        }
        public override void Interact(PlayerController player)
        {
            if (Manager_TokoJamu.instance.CekTokoBuka()) return;
            base.Interact(player);

            if (openingCoroutine != null)
            {
                StopCoroutine(openingCoroutine);
                VisualGuideBukaPintu.gameObject.SetActive(true);
            }

            openTimer = 0f;

            openingCoroutine = StartCoroutine(OpenDoor());
            VisualGuideBukaPintu.gameObject.SetActive(false);
        }

        IEnumerator OpenDoor()
        {
            Transform playerTransform = PlayerController.Instance.transform;

            Vector3 startPos = playerTransform.position;

            bool cancelled = false;
            VisualContainer.gameObject.SetActive(true);
            VisualGuideBukaPintu.gameObject.SetActive(false);
            while (openTimer < SecondsToOpen)
            {
                Vector3 newPos = playerTransform.position;

                if (Vector3.Distance(startPos, newPos) > 1.5f)
                {
                    cancelled = true;
                    VisualContainer.gameObject.SetActive(false);
                    VisualGuideBukaPintu.gameObject.SetActive(true);
                    break;
                }

                yield return new WaitForSeconds(0.1f);

                openTimer += 0.1f;
                VisualProgress.fillAmount = openTimer / SecondsToOpen;
            }

            if (!cancelled)
            {
                Debug.Log("Opened");
                VisualContainer.gameObject.SetActive(false);
                VisualGuideBukaPintu.gameObject.SetActive(false);
                Manager_TokoJamu.instance.BukaToko();
            }
            else
            {
                Debug.Log("Cancelled");
                VisualGuideBukaPintu.gameObject.SetActive(false);
                VisualContainer.gameObject.SetActive(false);
            }
        }
    }

}