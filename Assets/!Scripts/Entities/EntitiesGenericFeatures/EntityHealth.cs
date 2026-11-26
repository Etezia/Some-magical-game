using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EntityHealth : NetworkBehaviour
{
    [SerializeField] private EntityHealthData healthData;

	private readonly SyncVar<float> currHealth = new();
	private readonly SyncVar<float> currArmor = new();

	public EntityType Type => healthData.EntityType;

	public event Action EntityDestroyed;

	public float CurrHealth
	{
		get => currHealth.Value;
		private set => currHealth.Value = Mathf.Clamp(value, 0, healthData.MaxHealth);
	}
	public float CurrArmor
	{
		get => currArmor.Value;
		private set => currArmor.Value = Mathf.Clamp(value, 0, healthData.MaxArmor);
	}

	private void Awake()
	{
		CurrHealth = healthData.MaxHealth;
	}

	[Server]
	public void GetDamage(float value)
	{
		CurrHealth -= value;
		Debug.Log(CurrHealth);
		if (CurrHealth <= 0)
		{
			EntityDestroyed?.Invoke();
			Despawn(gameObject);
		}
	}

	[Server]
	public void HealUp(float value)
	{
		CurrHealth += value;
	}
}
