using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FadlanWork
{
    public abstract class BaseInteractableObject : MonoBehaviour
    {
        public virtual void Interact(PlayerController player)
        {
            Debug.Log(gameObject.name + " interacted.");
        }
    }
}