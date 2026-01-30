namespace SAB.Item
{
    public class WeaponTemplateData
    {
        public readonly int ID;
        public readonly float Damage;
        public readonly int UpgradeSlot;

        public WeaponTemplateData(int id, float damage, int upgradeSlot)
        {
            ID = id;
            Damage = damage;
            UpgradeSlot = upgradeSlot;
        }
    }
}
