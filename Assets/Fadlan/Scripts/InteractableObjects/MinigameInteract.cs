using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace FadlanWork
{
    public class MinigameInteract : BaseInteractableObject
    {
        public string minigameCode;
        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            StoreMinigameManager.Instance.StartMinigame(minigameCode);
        }
    }
}