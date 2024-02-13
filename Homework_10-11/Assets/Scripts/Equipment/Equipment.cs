using System;
using System.Collections.Generic;
using System.Linq;
using Sample;
using Sirenix.OdinInspector.Editor.Validation;

namespace Equipment
{
    //TODO: Реализовать экипировку
    public sealed class Equipment
    {
        public event Action<Item> OnItemAdded;
        public event Action<Item> OnItemRemoved;

        private List<KeyValuePair<EquipmentType, Item>> _equipedItems = new();
        private Inventory _inventory;

        public void Setup(Inventory inventory, params KeyValuePair<EquipmentType, Item>[] items)
        {
            _inventory = inventory;
            _equipedItems = items.ToList();
        }

        public Item GetItem(EquipmentType type)
        {
            if (TryGetItem(type, out var result))
            {
                return result;
            }

            return null;
        }

        public bool TryGetItem(EquipmentType type, out Item result)
        {
            result = _equipedItems.FirstOrDefault(p => p.Key == type).Value;
            return result != default;
        }

        public void RemoveItem(EquipmentType type, Item item)
        {
            if (!TryGetItem(type, out var result))
            {
                return;
            }

            if (_inventory != null)
            {
                _inventory.AddItem(item);
            }

            var pair = _equipedItems.First(p => p.Key == type);
            _equipedItems.Remove(pair);
            OnItemRemoved?.Invoke(item);
        }

        public void AddItem(EquipmentType type, Item item)
        {
            if (TryGetItem(type, out var result))
            {
                return;
            }

            if (_inventory != null)
            {
                if (!_inventory.FindItem(item.Name, out var _))
                {
                    return;
                }

                _inventory.RemoveItem(item.Name);
            }

            _equipedItems.Add(new(type, item));
            OnItemAdded?.Invoke(item);
        }

        public void ChangeItem(EquipmentType type, Item item)
        {
            if (TryGetItem(type, out var result))
            {
                RemoveItem(type, result);
                AddItem(type, item);
                return;
            }

            AddItem(type, item);
        }

        public List<KeyValuePair<EquipmentType, Item>> GetItems()
        {
            return _equipedItems;
        }
    }
}