namespace SAB.Item
{
    public class WeaponTemplateData
    {
        public readonly int ID;
        public readonly WeaponType WeaponType;
        public readonly WeaponHandedness Handeness;
        public readonly float Damage;
        public readonly int UpgradeSlot;

        public WeaponTemplateData(int id, /*WeaponType weaponType, WeaponHandedness weaponHandedness,*/ float damage, int upgradeSlot)
        {
            ID = id;
            WeaponType = WeaponType.Sword;
            Handeness = WeaponHandedness.OneHand;
            Damage = damage;
            UpgradeSlot = upgradeSlot;
        }
    }
}
