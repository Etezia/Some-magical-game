using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicWand : Wand
{
	[SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Projectile projectile;

	[SerializeField] private float projectileLaunchForce = 25f;

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

	[ServerRpc(RequireOwnership = true, RunLocally = true)]
	public void LaunchProjectile()
    {
		var currProjectile = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
		Spawn(currProjectile.NetworkObject);

		currProjectile.RB.AddForce(currProjectile.transform.forward * projectileLaunchForce, ForceMode.VelocityChange);
		print("gooo cast");
    }
}