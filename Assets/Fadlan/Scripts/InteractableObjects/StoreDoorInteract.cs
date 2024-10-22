using System.Collections;
using System.Collections.Generic;
using AimarWork;
using AimarWork.GameManagerLogic;
using FadlanWork;
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
        public Image VisualProgress;
        public RectTransform VisualContainer;
        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            if (openingCoroutine != null)
                StopCoroutine(openingCoroutine);

            openTimer = 0f;

            openingCoroutine = StartCoroutine(OpenDoor());
        }

        IEnumerator OpenDoor()
        {
            Transform playerTransform = PlayerController.Instance.transform;

            Vector3 startPos = playerTransform.position;

            bool cancelled = false;
            VisualContainer.gameObject.SetActive(true);
            while (openTimer < SecondsToOpen)
            {
                Vector3 newPos = playerTransform.position;

                if (Vector3.Distance(startPos, newPos) > 1f)
                {
                    cancelled = true;
                    VisualContainer.gameObject.SetActive(false);
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
                Manager_TokoJamu.instance.BukaToko();
            }
            else
            {
                Debug.Log("Cancelled");
                VisualContainer.gameObject.SetActive(false);
            }
        }
    }

}