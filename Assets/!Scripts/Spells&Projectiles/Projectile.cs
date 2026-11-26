using FishNet.Component.Transforming;
using FishNet.Object;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkObject), typeof(NetworkTransform), typeof(Rigidbody))]
public class Projectile : NetworkBehaviour
{
	[SerializeField] private float lifetime;

    public Rigidbody RB { get; private set; }

	public Action ObjectDestroyed;

	private void Awake()
	{
		RB = GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other)
	{

		ObjectDestroyed?.Invoke();
		ServerManager.Despawn(this.NetworkObject);
	}

	private IEnumerator DestroyOnLifetimeEnded()
	{
		yield return new WaitForSeconds(lifetime);

		ServerManager.Despawn(gameObject);
	}
}
