using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данные одного тестового сохранения.
/// Пока сохраняет только текущую сцену и состояния игроков.
/// </summary>
[Serializable]
public sealed class SaveData
{
    public int saveVersion = 1;
    public string saveId;
    public string sceneName;
    public string createdAtUtc;
    public string updatedAtUtc;
    public List<PlayerSaveData> players = new List<PlayerSaveData>();
}

/// <summary>
/// Сохраняемое состояние одного игрока.
/// OwnerClientId используется только для прототипа в рамках текущей сетевой сессии.
/// </summary>
[Serializable]
public sealed class PlayerSaveData
{
    public int ownerClientId;
    public Vector3 position;
    public Quaternion rotation;
}
