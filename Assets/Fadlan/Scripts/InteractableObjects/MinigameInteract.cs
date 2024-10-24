using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using System;

namespace FadlanWork
{
    public class MinigameInteract : BaseInteractableObject
    {
        public string minigameCode;
        public static event Action TidakBisa;
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            if(player.inventory.ListBahan.Count == 0)
            {
                Debug.Log("Player Inventory is Empty!");
                TidakBisa?.Invoke();
                return;
            }
            StoreMinigameManager.Instance.StartMinigame(minigameCode);
        }
    }
}