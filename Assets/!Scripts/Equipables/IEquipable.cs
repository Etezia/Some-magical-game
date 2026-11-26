using UnityEngine;

public interface IEquipable
{
    Transform HoldingPoint { get; }
    EquipableItemType ItemType { get; }
}
