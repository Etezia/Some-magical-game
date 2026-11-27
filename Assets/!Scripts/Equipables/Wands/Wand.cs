using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using UnityEngine.InputSystem;


public abstract class Wand : NetworkBehaviour, IWand
{
	[field: SerializeField] public EquipableItemType ItemType { get; protected set; }
	[field: SerializeField] public Transform HoldingPoint { get; protected set; }

	[SerializeField] protected WandData wandData;

	protected readonly SyncVar<bool> isReadyToSpell;

    public abstract void CastMainSpell();
}
