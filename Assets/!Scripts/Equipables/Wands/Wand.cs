using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class Wand : NetworkBehaviour, IWand
{
	[field: SerializeField] public EquipableItemType ItemType { get; protected set; }
	[field: SerializeField] public Transform HoldingPoint { get; protected set; }

	protected bool isReadyToSpell;

    public abstract void CastMainSpell();
}
