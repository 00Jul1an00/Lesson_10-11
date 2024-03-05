namespace Equipment
{
    public static class EquipmentExtension
    {
        public static void Clear(this Equipment equipment)
        {
            var items = equipment.EquipedItems;

            foreach (var item in items) 
            {
                equipment.Unequip(item.Key, item.Value);
            }
        }
    }
}