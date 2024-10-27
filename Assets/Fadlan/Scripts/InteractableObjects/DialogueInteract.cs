using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

namespace FadlanWork
{
    public class InteractDialogue : BaseInteractableObject
    {
        public NPCConversation conversation;
        public override void Interact(PlayerController player)
        {
            base.Interact(player);
            
            ConversationManager.Instance.StartConversation(conversation);
        }
    }
}