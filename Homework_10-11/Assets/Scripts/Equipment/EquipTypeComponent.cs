using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public sealed class EquipTypeComponent
    {
        public EquipmentType EquipmentType => _equipmentType;

        [SerializeReference]
        private EquipmentType _equipmentType;
    }
}
