using System;
using FishNet;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Координирует сохранение и загрузку игрового состояния на стороне Host.
/// Не управляет отображением интерфейса.
/// </summary>
public sealed class SaveManager : MonoBehaviour
{
    private const string PrototypeSaveId = "prototype-slot-01";

    public void SaveGame()
    {
        if (!CanSaveOnThisInstance())
            return;

        var playerControllers =
            FindObjectsByType<MovementController>(FindObjectsSortMode.None);

        SaveSystem.TryLoad(out var previousSave, out _);

        var saveData = new SaveData
        {
            saveId = PrototypeSaveId,
            sceneName = SceneManager.GetActiveScene().name,
            createdAtUtc = previousSave != null
                ? previousSave.createdAtUtc
                : DateTime.UtcNow.ToString("O"),
            updatedAtUtc = DateTime.UtcNow.ToString("O")
        };

        foreach (var player in playerControllers)
        {
            if (player == null || !player.IsServerInitialized)
                continue;

            saveData.players.Add(new PlayerSaveData
            {
                ownerClientId = player.OwnerId,
                position = player.transform.position,
                rotation = player.transform.rotation
            });
        }

        if (saveData.players.Count == 0)
        {
            Debug.LogWarning("[SaveManager] Не найдено игроков для сохранения.", this);
            return;
        }

        if (!SaveSystem.TrySave(saveData, out var errorMessage))
        {
            Debug.LogError($"[SaveManager] Не удалось сохранить игру: {errorMessage}", this);
            return;
        }

        Debug.Log(
            $"[SaveManager] Сохранено игроков: {saveData.players.Count}. Файл: {SaveSystem.SavePath}",
            this);
    }

    public void LoadGame()
    {
        if (!CanSaveOnThisInstance())
            return;

        if (!SaveSystem.TryLoad(out var saveData, out var errorMessage))
        {
            Debug.LogWarning($"[SaveManager] Не удалось загрузить игру: {errorMessage}", this);
            return;
        }

        var currentSceneName = SceneManager.GetActiveScene().name;
        if (saveData.sceneName != currentSceneName)
        {
            Debug.LogError(
                $"[SaveManager] Сохранение создано для сцены '{saveData.sceneName}', " +
                $"а сейчас загружена '{currentSceneName}'.",
                this);
            return;
        }

        var playerControllers =
            FindObjectsByType<MovementController>(FindObjectsSortMode.None);

        var restoredPlayersCount = 0;

        foreach (var playerData in saveData.players)
        {
            var matchingPlayer = default(MovementController);

            foreach (var player in playerControllers)
            {
                if (player == null ||
                    !player.IsServerInitialized ||
                    player.OwnerId != playerData.ownerClientId)
                {
                    continue;
                }

                matchingPlayer = player;
                break;
            }

            if (matchingPlayer != null &&
                matchingPlayer.RestoreTransformFromSave(
                    playerData.position,
                    playerData.rotation))
            {
                restoredPlayersCount++;
            }
        }

        Debug.Log(
            $"[SaveManager] Запрос на восстановление отправлен для игроков: " +
            $"{restoredPlayersCount} из {saveData.players.Count}.",
            this);
    }

    private bool CanSaveOnThisInstance()
    {
        if (InstanceFinder.IsServerStarted)
            return true;

        Debug.LogWarning("[SaveManager] Сохранять и загружать игру может только Host.", this);
        return false;
    }
}
