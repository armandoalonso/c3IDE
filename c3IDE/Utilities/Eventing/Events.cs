using c3IDE.Models;

namespace c3IDE.Utilities.Eventing
{
    //TODO: convert app to use eventing for communication
    public class AddonCreatedEvent : GenericTinyMessage<C3Addon>
    {
        public C3Addon Addon { get; set; }
        public AddonCreatedEvent(object sender, C3Addon content) : base(sender, content)
        {
            Addon = this.Content;
        }
    }
}
