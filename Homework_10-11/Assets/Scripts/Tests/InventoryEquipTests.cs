using NUnit.Framework;
using UnityEngine;
using Sample;
using Equipment;
using Game.GameEngine.Mechanics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TextCore.Text;
using System;
using UnityEngine.Assertions;

public enum ItemType
{
    helmet,
}

[TestFixture]
public class InventoryEquipTests
{
    private Inventory _inventory;
    private Sample.Character _character;
    private Equipment.Equipment _equipment;
    private List<Item> _items;
    private List<EffectId> _effectsID;

    [SetUp]
    public void Setup()
    {
        _inventory = new();
        _equipment = new();
        _equipment.Setup(_inventory);
        _character = new();
        _effectsID = new();

        var effectsIDStrings = Enum.GetNames(typeof(EffectId));

        foreach (var effectId in effectsIDStrings)
        {
            _effectsID.Add(Enum.Parse<EffectId>(effectId));
        }

        _items = new()
        {
            Resources.Load<ItemConfig>("Helmet").item.Clone(),
            Resources.Load<ItemConfig>("Sword").item.Clone(),
            Resources.Load<ItemConfig>("Armor").item.Clone(),
            Resources.Load<ItemConfig>("Boots").item.Clone(),
            Resources.Load<ItemConfig>("Shield").item.Clone()
        };
    }

    [TestCase(EquipmentType.HEAD)]
    [TestCase(EquipmentType.LEGS)]
    [TestCase(EquipmentType.LEFT_HAND)]
    [TestCase(EquipmentType.RIGHT_HAND)]
    [TestCase(EquipmentType.BODY)]
    public void EquipItem(EquipmentType equipmentType)
    {
        var item = _items.First(i => i.GetComponent<EquipTypeComponent>().EquipmentType == equipmentType);
        _inventory.Clear();
        _equipment.Clear();

        _inventory.AddItem(item);
        _equipment.Equip(equipmentType, item);

        var itemInEquipment = _equipment.EquipedItems.FirstOrDefault(p => p.Key == equipmentType).Value;
        _inventory.FindItem(item.Name, out var itemInInventory);
        NUnit.Framework.Assert.AreEqual(item, itemInEquipment);
        NUnit.Framework.Assert.AreNotEqual(item, itemInInventory);
    }

    [TestCase(EquipmentType.HEAD)]
    [TestCase(EquipmentType.LEGS)]
    [TestCase(EquipmentType.LEFT_HAND)]
    [TestCase(EquipmentType.RIGHT_HAND)]
    [TestCase(EquipmentType.BODY)]
    public void UnequipItem(EquipmentType equipmentType)
    {
        var item = _items.First(i => i.GetComponent<EquipTypeComponent>().EquipmentType == equipmentType);
        _inventory.Clear();
        _equipment.Clear();

        _inventory.AddItem(item);
        _equipment.Equip(equipmentType, item);
        _equipment.Unequip(equipmentType, item);

        var itemInEquipment = _equipment.EquipedItems.FirstOrDefault(p => p.Key == equipmentType).Value;
        _inventory.FindItem(item.Name, out var itemInInventory);
        NUnit.Framework.Assert.AreNotEqual(item, itemInEquipment);
        NUnit.Framework.Assert.AreEqual(item, itemInInventory);
    }

    [TestCase(EquipmentType.HEAD, EquipmentType.HEAD)]
    [TestCase(EquipmentType.LEGS, EquipmentType.LEGS)]
    [TestCase(EquipmentType.LEFT_HAND, EquipmentType.LEFT_HAND)]
    [TestCase(EquipmentType.RIGHT_HAND, EquipmentType.RIGHT_HAND)]
    [TestCase(EquipmentType.BODY, EquipmentType.BODY)]
    public void SwapItems(EquipmentType equipmentTypeItem1, EquipmentType equipmentTypeItem2)
    {
        var item1 = _items.First(i => i.GetComponent<EquipTypeComponent>().EquipmentType == equipmentTypeItem1);
        var item2 = _items.First(i => i.GetComponent<EquipTypeComponent>().EquipmentType == equipmentTypeItem2);
        _inventory.Clear();
        _equipment.Clear();

        _inventory.AddItem(item1);
        _equipment.Equip(equipmentTypeItem1, item1);
        _inventory.AddItem(item2);
        _equipment.Equip(equipmentTypeItem1, item2);

        var itemInEquipment = _equipment.EquipedItems.FirstOrDefault(p => p.Key == equipmentTypeItem1).Value;
        NUnit.Framework.Assert.AreEqual(item2, itemInEquipment);
    }

    [TestCase(EquipmentType.HEAD)]
    [TestCase(EquipmentType.LEGS)]
    [TestCase(EquipmentType.LEFT_HAND)]
    [TestCase(EquipmentType.RIGHT_HAND)]
    [TestCase(EquipmentType.BODY)]
    public void IncreaseCharacterStats(EquipmentType equipmentType)
    {
        var item = _items.First(i => i.GetComponent<EquipTypeComponent>().EquipmentType == equipmentType);
        var effect = item.GetComponent<Component_Effect>();

        _inventory.Clear();
        _equipment.Clear();
        _character.Reset();

        _inventory.AddItem(item);
        _equipment.Equip(equipmentType, item);

        foreach (var effectId in _effectsID)
        {
            if (effect.Effect.TryGetParameter<int>(effectId, out int value))
            {
                string effectName = effectId.ToString();
                _character.SetStat(effectName, value);
            }
        }

        var itemInEquipment = _equipment.EquipedItems.FirstOrDefault(p => p.Key == equipmentType).Value;
        NUnit.Framework.Assert.AreEqual(item, itemInEquipment);

        foreach (var effectId in _effectsID)
        {
            if (effect.Effect.TryGetParameter<int>(effectId, out int value))
            {
                string effectName = effectId.ToString();

                if (_character.TryGetStat(effectName, out var currentValue))
                {
                    NUnit.Framework.Assert.AreEqual(currentValue, value);
                }
            }
        }
    }
}