﻿using Sample;
using System;
using System.Collections.Generic;
using Game.GameEngine.Mechanics;

namespace Equipment
{
    public sealed class EquipmentController
    {
        private Equipment _equipment;
        private Character _character;

        private readonly List<EffectId> _effectsID = new();

        public EquipmentController(Equipment equipment, Character character)
        {
            _equipment = equipment;
            _character = character;

            var effectsIDStrings = Enum.GetNames(typeof(EffectId));

            foreach (var effectId in effectsIDStrings)
            {
                _effectsID.Add(Enum.Parse<EffectId>(effectId));
            }

            Subcribe();
        }

        ~EquipmentController()
        {
            Unsubcribe();
        }

        private void Subcribe()
        {
            _equipment.OnItemAdded += OnItemAdded;
            _equipment.OnItemRemoved += OnItemRemoved;
        }

        private void Unsubcribe()
        {
            _equipment.OnItemAdded -= OnItemAdded;
            _equipment.OnItemRemoved -= OnItemRemoved;
        }

        #region EquipmentEventsListiners
        private void OnItemAdded(Item item)
        {
            var effect = item.GetComponent<Component_Effect>();

            foreach (var effectId in _effectsID)
            {
                if (effect.Effect.TryGetParameter<int>(effectId, out int value))
                {
                    string effectName = effectId.ToString();
                    int currentValue = _character.GetStat(effectName);
                    _character.SetStat(effectName, currentValue + value);
                }
            }
        }

        private void OnItemRemoved(Item item)
        {
            var effect = item.GetComponent<Component_Effect>();

            foreach (var effectId in _effectsID)
            {
                if (effect.Effect.TryGetParameter<int>(effectId, out int value))
                {
                    string effectName = effectId.ToString();
                    int currentValue = _character.GetStat(effectName);
                    _character.SetStat(effectName, currentValue - value);
                }
            }
        }
        #endregion
    }
}