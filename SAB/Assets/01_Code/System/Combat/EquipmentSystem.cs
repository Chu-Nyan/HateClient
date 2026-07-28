using SAB.Item;

namespace SAB.GameSystem
{
    public class EquipmentSystem
    {
        private IHasEquipmentData[] _parts;
        private WeaponStance _stance;

        public WeaponStance Stance
        {
            get => _stance;
        }

        public EquipmentSystem()
        {
            _parts = new IHasEquipmentData[5];
        }

        public void Equip(IHasEquipmentData item)
        {
            _parts[(int)item.EquipmentTemplateData.Template.Slot] = item;
            _stance = ChangeStance();
        }

        private WeaponStance ChangeStance()
        {
            bool hasShield = _parts[(int)EquipSlot.Shield] != null;
            if (hasShield == true)
                return WeaponStance.SwordAndShield;
            if (_parts[(int)EquipSlot.Weapon] != null)
                return WeaponStance.Sword;

            return WeaponStance.Unarmed;
        }
    }
}
