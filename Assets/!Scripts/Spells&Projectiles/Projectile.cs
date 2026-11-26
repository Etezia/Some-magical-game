using FishNet.Component.Transforming;
using FishNet.Object;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(NetworkObject), typeof(NetworkTransform), typeof(Rigidbody))]
public class Projectile : NetworkBehaviour
{
	[SerializeField] private float damage = 40f;
	[SerializeField] private float lifetime = 10f;

	private NetworkTransform networkTransform;

    public Rigidbody RB { get; private set; }

	public event Action ObjectDestroyed;

	private void Awake()
	{
		networkTransform = GetComponent<NetworkTransform>();
		RB = GetComponent<Rigidbody>();
	}

	private void Reset()
	{
		damage = 40f;
		lifetime = 10f;
	}

	public override void OnStartServer()
	{
		base.OnStartServer();

		StartCoroutine(DestroyOnLifetimeEnded());
	}

	[Server]
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent<EntityHealth>(out var entityHealth))
		{
			entityHealth.GetDamage(damage);
			ObjectDestroyed?.Invoke();
			Despawn();
		}
	}

	private IEnumerator DestroyOnLifetimeEnded()
	{
		yield return new WaitForSeconds(lifetime);

		Despawn();
	}
}