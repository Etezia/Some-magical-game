using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using UnityEngine;

public class BasicWand : Wand
{
	[SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Projectile projectile;

	[SerializeField] private float projectileLaunchForce = 25f;

	private readonly SyncVar<float> currMana = new();

	public float CurrMana { get => currMana.Value;
		private set => currMana.Value = Mathf.Clamp(value, 0, wandData.MaxMana);
	}

	public event Action ManaChanged;

	private void Awake()
	{
		CurrMana = wandData.MaxMana;
	}

	public override void OnStartClient()
	{
		base.OnStartClient();

		if (IsOwner)
			enabled = true;
	}

	public override void CastMainSpell()
	{
		LaunchProjectile();
	}

	[ServerRpc(RequireOwnership = true)]
	public void LaunchProjectile()
    {
		var currProjectile = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
		Spawn(currProjectile.NetworkObject);

		currProjectile.RB.AddForce(currProjectile.transform.forward * projectileLaunchForce, ForceMode.VelocityChange);
    }
}