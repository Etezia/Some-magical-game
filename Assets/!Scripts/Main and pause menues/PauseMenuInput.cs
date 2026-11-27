
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputSystem;
    [SerializeField] private PauseMenu pauseMenuManager;

    private InputAction pauseAction;

    private void Awake()
    {
        if (inputSystem == null || pauseMenuManager == null)
        {
            Debug.LogError(
                "[PauseMenuInput] Не назначены необходимые ссылки.",
                this);

            return;
        }

        pauseAction = inputSystem.FindAction("Switch Pause", false);

        if (pauseAction == null)
        {
            Debug.LogError(
                "[PauseMenuInput] Не найдено действие Switch Pause.",
                this);
        }
    }

    private void OnEnable()
    {
        if (pauseAction == null)
            return;

        pauseAction.performed += OnPausePerformed;
        pauseAction.Enable();
    }

    private void OnDisable()
    {
        if (pauseAction == null)
            return;

        pauseAction.performed -= OnPausePerformed;
        pauseAction.Disable();
    }

    private void OnPausePerformed(InputAction.CallbackContext context)
    {
        pauseMenuManager.TogglePause();
    }
}
