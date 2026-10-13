using System;
using System.IO;
using UnityEngine;

namespace Data
{
    public static class SaveManager
    {
        private const string FileName = "savegame.json";

        private static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public static GameData Load()
        {
            if (!File.Exists(FilePath))
            {
                return new GameData();
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonUtility.FromJson<GameData>(json) ?? new GameData();
            }
            catch (Exception e)
            {
                Debug.LogError($"Laden mislukt, nieuwe data gebruikt: {e.Message}");
                return new GameData();
            }
        }

        public static void Save(GameData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Opslaan mislukt: {e.Message}");
            }
        }
    }
}