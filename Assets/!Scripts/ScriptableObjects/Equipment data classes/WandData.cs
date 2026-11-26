using UnityEngine;

[CreateAssetMenu(fileName = "WandData", menuName = "Scriptable Objects/Wand Data")]
public class WandData : ScriptableObject
{
    [field: SerializeField] public float MaxMana { get; private set; }
	/// <summary>
	/// Mana recovery per second
	/// </summary>
	[field: SerializeField] public float ManaRecovery { get; private set; }

	[field: SerializeField] public int SpellsCountInCharge { get; private set; }
	[field: SerializeField] public float FullReloadTime { get; private set; }
	[field: SerializeField] public float DelayBetweenSpells { get; private set; }
}
