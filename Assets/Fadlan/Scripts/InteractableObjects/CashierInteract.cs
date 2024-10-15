using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FadlanWork
{
    public class CashierInteract : BaseInteractableObject
    {
        public override void Interact()
        {
            base.Interact();

            Customer firstCustomer = CustomersQueueManager.Instance.GetFirst();
            if (firstCustomer != null)
            {
                firstCustomer.AskOrder();
            }
        }
    }
}