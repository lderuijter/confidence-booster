using System;
using System.IO;
using Data;
using UnityEngine;

namespace Managers
{
    public static class SaveManager
    {
        private const string FileName = "savegame.json";

        // Bepaalt waar het savebestand op de computer wordt opgeslagen.
        private static string FilePath =>
            Path.Combine(Application.persistentDataPath, FileName);

        // Bevat de gedeelde GameData, zodat managers dezelfde data gebruiken.
        private static GameData _data;

        public static GameData Load()
        {
            // Als de data al geladen is, gebruiken we dezelfde instantie.
            // Zo maken managers niet onbedoeld hun eigen kopie van de data.
            if (_data != null)
            {
                return _data;
            }

            // Bestaat er nog geen savebestand? Dan beginnen we met nieuwe data.
            if (!File.Exists(FilePath))
            {
                _data = new GameData();
                return _data;
            }
            
            Debug.Log(FilePath);

            try
            {
                // Leest de opgeslagen JSON-tekst uit het bestand.
                string json = File.ReadAllText(FilePath);

                // Zet de JSON-tekst om naar een GameData-object.
                // Als dat niet lukt en null wordt teruggegeven, gebruiken we nieuwe data.
                _data = JsonUtility.FromJson<GameData>(json)
                        ?? new GameData();
            }
            catch (Exception e)
            {
                // Geeft een foutmelding als het bestand niet gelezen kan worden.
                // Daarna starten we met nieuwe data, zodat het spel door kan gaan.
                Debug.LogError($"Laden mislukt: {e.Message}");
                _data = new GameData();
            }

            // Geeft de gedeelde data terug aan de manager die erom vraagt.
            return _data;
        }

        public static void Save(GameData data)
        {
            try
            {
                // Bewaart de verwijzing naar dezelfde data die de managers gebruiken.
                _data = data;

                // Zet de huidige GameData om naar leesbare JSON-tekst.
                string json = JsonUtility.ToJson(_data, true);

                // Schrijft de JSON-tekst naar het savebestand.
                File.WriteAllText(FilePath, json);
            }
            catch (Exception e)
            {
                // Geeft een foutmelding als het opslaan niet lukt.
                Debug.LogError($"Opslaan mislukt: {e.Message}");
            }
        }
    }
}
