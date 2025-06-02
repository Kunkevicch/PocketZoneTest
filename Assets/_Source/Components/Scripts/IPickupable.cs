namespace PocketZoneTest
{
    public interface IPickupable
    {
        public int Quantity { get; }
        public ItemData Pickup();
    }
}
