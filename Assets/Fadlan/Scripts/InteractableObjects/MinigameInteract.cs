using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FadlanWork
{
    public class MinigameInteract : BaseInteractableObject
    {
        public string minigameCode;

        public override void Interact()
        {
            base.Interact();

            StoreMinigameManager.Instance.StartMinigame(minigameCode);
        }
    }
}