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

        [Title("FUCK OFF")]
        [ListDrawerSettings(ShowIndexLabels = true)]
        public List<string> minigameNames = new List<string>
        {
            "test",
            "test2"
        };


        [Button]
        public void DoSomething()
        {
            Debug.Log("jnscdijbnc");
        }

        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            StoreMinigameManager.Instance.StartMinigame(minigameCode);
        }
    }
}