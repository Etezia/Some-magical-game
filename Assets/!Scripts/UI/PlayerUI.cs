using FishNet.Example.ColliderRollbacks;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : NetworkBehaviour
{
    [SerializeField] private GameObject playerCanvas;

	private void Awake()
	{
		CheckForComponentReferences();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();

		if (IsOwner)
		{
			playerCanvas.SetActive(true);
			enabled = true;
		}
	}

	private void CheckForComponentReferences()
	{
		if (playerCanvas == null)
			Debug.Log($"Playr canvas is null in PlayerUI at player {this.OwnerId}");
	}
}
