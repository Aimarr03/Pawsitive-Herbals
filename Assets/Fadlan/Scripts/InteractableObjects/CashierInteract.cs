using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FadlanWork
{
    public class CashierInteract : BaseInteractableObject
    {
        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            Customer firstCustomer = CustomersQueueManager.Instance.GetFirst();
            if (firstCustomer != null)
            {
                if (firstCustomer.wantToOrder)
                {
                    firstCustomer.AskOrder();
                }
            }
        }
    }
}