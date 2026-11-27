using System;
using System.IO;
using UnityEngine;

/// <summary>
/// Отвечает только за чтение и запись JSON-файла сохранения.
/// Не знает о сетевых объектах и игровом процессе.
/// </summary>
public static class SaveSystem
{
    private const string SaveFolderName = "Saves";
    private const string SaveFileName = "prototype_save.json";
    private const int CurrentSaveVersion = 1;

    public static string SavePath
    {
        get
        {
            return Path.Combine(
                Application.persistentDataPath,
                SaveFolderName,
                SaveFileName);
        }
    }

    /// <summary>
    /// Записывает данные во временный файл, затем заменяет основное сохранение.
    /// </summary>
    public static bool TrySave(SaveData saveData, out string errorMessage)
    {
        errorMessage = null;

        if (saveData == null)
        {
            errorMessage = "Данные сохранения отсутствуют.";
            return false;
        }

        var temporaryPath = SavePath + ".tmp";
        var backupPath = SavePath + ".bak";

        try
        {
            var directoryPath = Path.GetDirectoryName(SavePath);
            Directory.CreateDirectory(directoryPath);

            var json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(temporaryPath, json);

            if (File.Exists(SavePath))
            {
                if (File.Exists(backupPath))
                    File.Delete(backupPath);

                File.Replace(temporaryPath, SavePath, backupPath);

                if (File.Exists(backupPath))
                    File.Delete(backupPath);
            }
            else
            {
                File.Move(temporaryPath, SavePath);
            }

            return true;
        }
        catch (Exception exception)
        {
            errorMessage = exception.Message;

            if (File.Exists(temporaryPath))
            {
                try
                {
                    File.Delete(temporaryPath);
                }
                catch (Exception cleanupException)
                {
                    Debug.LogWarning(
                        $"Не удалось удалить временный файл сохранения: {cleanupException.Message}");
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Читает JSON и проверяет базовую совместимость сохранения.
    /// </summary>
    public static bool TryLoad(out SaveData saveData, out string errorMessage)
    {
        saveData = null;
        errorMessage = null;

        try
        {
            if (!File.Exists(SavePath))
            {
                errorMessage = "Файл сохранения не найден.";
                return false;
            }

            var json = File.ReadAllText(SavePath);
            saveData = JsonUtility.FromJson<SaveData>(json);

            if (saveData == null)
            {
                errorMessage = "Не удалось прочитать данные сохранения.";
                return false;
            }

            if (saveData.saveVersion != CurrentSaveVersion)
            {
                errorMessage =
                    $"Версия сохранения {saveData.saveVersion} не поддерживается. " +
                    $"Текущая версия: {CurrentSaveVersion}.";
                saveData = null;
                return false;
            }

            if (saveData.players == null)
            {
                errorMessage = "В сохранении отсутствует список игроков.";
                saveData = null;
                return false;
            }

            return true;
        }
        catch (Exception exception)
        {
            errorMessage = exception.Message;
            saveData = null;
            return false;
        }
    }
}
