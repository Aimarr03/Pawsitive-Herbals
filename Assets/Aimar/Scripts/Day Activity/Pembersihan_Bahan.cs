using FadlanWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AimarWork
{
    public class Pembersihan_Bahan : BaseInteractableObject
    {
        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            player.inventory.BersihkanSemuaBahanDiInventory();
        }
    }
}

