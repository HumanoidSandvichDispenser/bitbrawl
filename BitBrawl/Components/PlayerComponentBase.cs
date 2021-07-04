using Nez;

namespace BitBrawl.Components
{
    public class PlayerComponentBase : Component
    {
        public Entities.Player Player => Entity as Entities.Player;

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            if (!(Entity is Entities.Player))
            {
                throw new System.Exception("This component is only compatible with Player entities.");
            }
        }
    }
}
