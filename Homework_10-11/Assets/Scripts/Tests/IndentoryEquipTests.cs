using NUnit.Framework;
using UnityEngine;
using Sample;
using Equipment;
using Game.GameEngine.Mechanics;
using System.Collections.Generic;

[TestFixture]
public class IndentoryEquipTests
{
    [Test]
    public void WhenEquipHelmetOnHead_AndHeadHasNoItem_ThanShouldBeHelmetOnHead()
    {
        //arange
        var helmet = Resources.Load<ItemConfig>("Helmet").item.Clone();
        var equipment = new Equipment.Equipment();
        var equipmentType = EquipmentType.HEAD;
        //action
        var itemOnEquipSlot = equipment.GetItem(equipmentType);
        Item result = null;

        if (itemOnEquipSlot == null)
        {
            equipment.AddItem(equipmentType, helmet);
            equipment.TryGetItem(equipmentType, out result);
        }

        //assert
        Assert.AreEqual(helmet, result);
    }

    [Test]
    public void WhenEquipSwordOnRightHand_AndRigthHandHasNoItem_ThanSwordSholdBeEquipAndCharacterMustIncreaseDamageStat()
    {
        //arange
        var sword = Resources.Load<ItemConfig>("Sword").item.Clone();
        var equipment = new Equipment.Equipment();
        KeyValuePair<string, int> damageStatArrange = new(EffectId.DAMAGE.ToString(), 0);
        var character = new Character(damageStatArrange);
        var equipController = new EquipmentController(equipment, character);
        var equipmentType = EquipmentType.RIGHT_HAND;
        //action
        equipment.AddItem(equipmentType, sword);
        equipment.TryGetItem(equipmentType, out var result);
        //assert
        var damageIncrease = sword.GetComponent<Component_Effect>().Effect.GetParameter<int>(EffectId.DAMAGE);
        var damageStat = character.GetStat(EffectId.DAMAGE.ToString());
        Assert.AreEqual(sword, result);
        Assert.AreEqual(damageIncrease, damageStat);
    }

    [Test]
    public void WhenEquipHelmetOnHead_AndHasItemOnHead_ThanHelmetSholdntBeEquip()
    {
        //arange
        var helmet1 = Resources.Load<ItemConfig>("Helmet").item.Clone();
        var helmet2 = Resources.Load<ItemConfig>("Helmet").item.Clone();
        var equipment = new Equipment.Equipment();
        var equipmentType = EquipmentType.HEAD;
        //action
        equipment.AddItem(equipmentType, helmet1);
        equipment.AddItem(equipmentType, helmet2);
        equipment.TryGetItem(equipmentType, out var result);
        //assert
        Assert.AreNotEqual(helmet2, result);
    }

    [Test]
    public void WhenEquipBoots_AndInventoryHasNoBoots_ThanBootsShooldntBeEquip()
    {
        //arange
        var boots = Resources.Load<ItemConfig>("Boots").item.Clone();
        var inventory = new Inventory();
        var equip = new Equipment.Equipment();
        var equipType = EquipmentType.LEGS;
        //action
        equip.Setup(inventory);
        equip.AddItem(equipType, boots);
        equip.TryGetItem(equipType, out var result);
        //assert
        Assert.AreNotEqual(boots, result);
    }

    [Test]
    public void WhenEquipArmor_AndInventoryHasArmor_ThanArmorShooldBeEquip()
    {
        //arange
        var armor = Resources.Load<ItemConfig>("Armor").item.Clone();
        var inventory = new Inventory();
        var equip = new Equipment.Equipment();
        var equipType = EquipmentType.BODY;
        //action
        inventory.AddItem(armor);
        equip.Setup(inventory);
        equip.AddItem(equipType, armor);
        equip.TryGetItem(equipType, out var result);
        //assert
        Assert.AreEqual(armor, result);
    }

    [Test]
    public void WhenRemoveShieldFromLeftHand_AndEquipHasShieldOnLeftHand_ThanLeftHandSholdntHaveShieldAndInventoryMustHaveShield()
    {
        //arange
        var shield = Resources.Load<ItemConfig>("Shield").item.Clone();
        var inventory = new Inventory();
        var equip = new Equipment.Equipment();
        var equipType = EquipmentType.LEFT_HAND;
        //action
        inventory.AddItem(shield);
        equip.Setup(inventory);
        equip.AddItem(equipType, shield);
        equip.RemoveItem(equipType, shield);
        equip.TryGetItem(equipType, out var result);
        //assert
        inventory.FindItem("Shield", out var itemResult);
        Assert.AreNotEqual(shield, result);
        Assert.AreEqual(shield, itemResult);
    }

    [Test]
    public void WhenAddShieldToLeftHand_AndInventoryHasNoShield_ThanShoudBeNoEquip()
    {
        //arange
        var shield = Resources.Load<ItemConfig>("Shield").item.Clone();
        var inventory = new Inventory();
        var equip = new Equipment.Equipment();
        var equipType = EquipmentType.LEFT_HAND;
        //action
        equip.Setup(inventory);
        equip.AddItem(equipType, shield);
        equip.TryGetItem(equipType, out var result);
        //assert
        Assert.AreNotEqual(shield, result);
    }

    [Test]
    public void WhenReplaceShieldToSwordInRightHand_AndInventoryHasSwordAndEquipHasShield_ThanSwordShouldBeEquipedAndShieldMustBeRemovedAndStatsMustChangesAndShieldMustReturnToInventory()
    {
        //arange
        var shield = Resources.Load<ItemConfig>("Shield").item.Clone();
        var sword = Resources.Load<ItemConfig>("Sword").item.Clone();
        var inventory = new Inventory();
        var equip = new Equipment.Equipment();
        var equipType = EquipmentType.RIGHT_HAND;
        KeyValuePair<string, int> damageStatArrange = new(EffectId.DAMAGE.ToString(), 0);
        KeyValuePair<string, int> healthStatArrange = new(EffectId.HEALTH.ToString(), 0);
        var character = new Character(damageStatArrange, healthStatArrange);
        var equipController = new EquipmentController(equip, character);
        //action
        inventory.AddItem(sword);
        inventory.AddItem(shield);
        equip.Setup(inventory);
        equip.AddItem(equipType, shield);
        equip.ChangeItem(equipType, sword);
        var result = equip.GetItem(equipType);
        //assert
        var damageIncrease = sword.GetComponent<Component_Effect>().Effect.GetParameter<int>(EffectId.DAMAGE);
        var damageStat = character.GetStat(EffectId.DAMAGE.ToString());
        var healthStat = character.GetStat(EffectId.HEALTH.ToString());
        inventory.FindItem("Shield", out var resultInventoryItem);
        Assert.AreEqual(sword, result);
        Assert.AreEqual(damageIncrease, damageStat);
        Assert.AreEqual(shield, resultInventoryItem);
    }
}