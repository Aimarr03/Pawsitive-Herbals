using System.Collections;
using System.Collections.Generic;
using AimarWork.GameManagerLogic;
using FadlanWork;
using UnityEngine;

namespace FadlanWork
{
    public class StoreDoorInteract : BaseInteractableObject
    {
        public float SecondsToOpen = 3f;

        private float openTimer = 0f;

        private Coroutine openingCoroutine;

        public override void Interact()
        {
            base.Interact();

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

            while (openTimer < SecondsToOpen)
            {
                Vector3 newPos = playerTransform.position;

                if (Vector3.Distance(startPos, newPos) > 1f)
                {
                    cancelled = true;
                    break;
                }

                yield return new WaitForSeconds(0.1f);

                openTimer += 0.1f;
            }

            if (!cancelled)
            {
                Debug.Log("Opened");
                Manager_Waktu.instance.BukaToko();
            }
            else
            {
                Debug.Log("Cancelled");
            }
        }
    }

}