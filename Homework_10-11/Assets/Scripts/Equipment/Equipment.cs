using System;
using System.Collections.Generic;
using Sample;

namespace Equipment
{
    //TODO: Реализовать экипировку
    public sealed class Equipment
    {
        private Dictionary<EquipmentType, Item> _equipedItems = new();
        private Inventory _inventory;

        public IReadOnlyDictionary<EquipmentType, Item> EquipedItems => _equipedItems;

        public event Action<Item> OnItemEquiped;
        public event Action<Item> OnItemUnequiped;

        public void Setup(Inventory inventory, params KeyValuePair<EquipmentType, Item>[] items)
        {
            _inventory = inventory;

            foreach(var item in items)
            {
                _equipedItems.Add(item.Key, item.Value);
            }
        }

        public void Unequip(EquipmentType type, Item item)
        {
            _equipedItems.TryGetValue(type, out var valueItem);

            if (valueItem != item)
            {
                throw new Exception($"{type} dont have item {item.Name}");
            }

            if (_inventory == null)
            {
                throw new Exception("Inventory in equipment is null");
            }

            _inventory.AddItem(item);
            _equipedItems.Remove(type);
            OnItemUnequiped?.Invoke(item);
        }

        public void Equip(EquipmentType type, Item item)
        {
            if (_inventory == null)
            {
                throw new Exception("Inventory in equipment is null");
            }

            if (!_inventory.FindItem(item.Name, out var _))
            {
                throw new Exception($"Inventory doesnt have {item.Name}");
            }

            _equipedItems.TryGetValue(type, out var valueItem);

            if (valueItem != null)
            {
                Unequip(type, valueItem);
            }

            _inventory.RemoveItem(item.Name);
            _equipedItems.Add(type, item);
            OnItemEquiped?.Invoke(item);
        }
    }
}