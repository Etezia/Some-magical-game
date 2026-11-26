using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private GameMenuManager gameMenuManager;
	[SerializeField] private MusicPlayer musicPlayer;

	[SerializeField] private Button exitGameButton;
	[SerializeField] private Button nextTrackButton;

	[SerializeField] private Slider musicVolumeSlider;

	private void Awake()
	{
		exitGameButton.onClick.AddListener(gameMenuManager.Exit);
	}
}