using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isOnPause;

    private readonly List<InputAction> disabledActions = new();

    private CursorLockMode previousCursorLockState;
    private bool previousCursorVisible;

    public event Action OnPause;
    public event Action OnResume;

    public void TogglePause()
    {
        if (isOnPause)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        if (isOnPause)
            return;

        isOnPause = true;

        previousCursorLockState = Cursor.lockState;
        previousCursorVisible = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetPlayerInputEnabled(false);

        OnPause?.Invoke();
    }

    public void Resume()
    {
        if (!isOnPause)
            return;

        isOnPause = false;

        SetPlayerInputEnabled(true);

        Cursor.lockState = previousCursorLockState;
        Cursor.visible = previousCursorVisible;

        OnResume?.Invoke();
    }

    private void SetPlayerInputEnabled(bool enabled)
    {
        if (!enabled)
        {
            var players = FindObjectsByType<MovementController>(
                FindObjectsSortMode.None);

            foreach (var player in players)
            {
                if (!player.IsOwner)
                    continue;

                var playerInput = player.GetComponent<PlayerInput>();

                if (playerInput == null)
                    continue;

                foreach (var actionMap in playerInput.actions.actionMaps)
                {
                    foreach (var action in actionMap.actions)
                    {
                        // ESC должен продолжать работать,
                        // чтобы можно было закрыть меню.
                        if (action.name == "Switch Pause")
                            continue;

                        if (!action.enabled)
                            continue;

                        disabledActions.Add(action);
                        action.Disable();
                    }
                }

                break;
            }
        }
        else
        {
            foreach (var action in disabledActions)
            {
                if (action != null)
                    action.Enable();
            }

            disabledActions.Clear();
        }
    }

    private void OnDisable()
    {
        if (isOnPause)
            Resume();
    }
}
