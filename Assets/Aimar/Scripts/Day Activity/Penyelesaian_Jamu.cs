using FadlanWork;


namespace AimarWork
{
    public class Penyelesaian_Jamu : BaseInteractableObject
    {
        public override void Interact(PlayerController player)
        {
            base.Interact(player);

            Manager_TokoJamu.instance.CheckJamu();
        }
    }
}

