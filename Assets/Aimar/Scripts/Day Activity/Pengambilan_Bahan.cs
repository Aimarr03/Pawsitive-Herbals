using FadlanWork;
using System.Collections.Generic;

namespace AimarWork
{
    public class Pengambilan_Bahan : BaseInteractableObject
    {
        public List<SO_BahanMentah> List_BahanMentah;
        private void Start()
        {
            List_BahanMentah = Manager_Jamu.instance.GetBahanMentah();
        }
        public override void Interact()
        {
            base.Interact();

        }
    }
}

