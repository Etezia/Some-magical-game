using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class EquipmentManager : NetworkBehaviour
{
	[field: SerializeReference] public Wand CurrWand { get; private set; }

    private PlayerInput inputSystem;
    private InputAction mainCastAction;

	private void Awake()
	{
		inputSystem = GetComponent<PlayerInput>();

		mainCastAction = inputSystem.actions.FindAction("Attack");
		mainCastAction.performed += TryCastMainSpell;

		CheckForComponentReferences();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();

		if (IsOwner)
			enabled = true;
	}

	private void TryCastMainSpell(InputAction.CallbackContext context)
	{
		print("yep");
		if (CurrWand)
			CurrWand.CastMainSpell();
	}

	private void CheckForComponentReferences()
	{
		if (mainCastAction == null)
			Debug.Log("No input action for main cast");
	}
}