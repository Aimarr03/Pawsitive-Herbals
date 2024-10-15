using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FadlanWork
{
    public abstract class BaseInteractableObject : MonoBehaviour
    {
        public Vector2 StandPositionOffset;
        public virtual void Interact()
        {
            Debug.Log(gameObject.name + " interacted.");
        }
    }
}