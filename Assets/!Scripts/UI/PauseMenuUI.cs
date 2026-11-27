
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenuObjs;
    [SerializeField] private GameObject gameUI;

    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;

    [Header("Managers")]
    [SerializeField] private PauseMenu pauseMenuManager;
    [SerializeField] private SaveManager saveManager;

    private void Awake()
    {
        pauseMenuObjs.SetActive(false);

        resumeButton.onClick.AddListener(
            pauseMenuManager.Resume);

        saveButton.onClick.AddListener(
            SaveGame);

        loadButton.onClick.AddListener(
            LoadGame);
    }

    private void OnEnable()
    {
        pauseMenuManager.OnPause += OnPause;
        pauseMenuManager.OnResume += OnResume;
    }

    private void OnDisable()
    {
        pauseMenuManager.OnPause -= OnPause;
        pauseMenuManager.OnResume -= OnResume;
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(
            pauseMenuManager.Resume);

        saveButton.onClick.RemoveListener(SaveGame);
        loadButton.onClick.RemoveListener(LoadGame);
    }

    private void OnPause()
    {
        pauseMenuObjs.SetActive(true);

        if (gameUI != null)
            gameUI.SetActive(false);
    }

    private void OnResume()
    {
        pauseMenuObjs.SetActive(false);

        if (gameUI != null)
            gameUI.SetActive(true);
    }

    private void SaveGame()
    {
        if (saveManager != null)
            saveManager.SaveGame();
    }

    private void LoadGame()
    {
        if (saveManager != null)
            saveManager.LoadGame();
    }
}
