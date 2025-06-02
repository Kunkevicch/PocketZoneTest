namespace PocketZoneTest
{
    public interface IWeapon
    {
        public ItemData GetAmmoData();
        public void Reload(int ammo);
        public int GetMaxAmmo();
        public int GetAmmo();
        public void Attack();
    }
}
