using Data;

namespace Managers
{
    // Houdt de speldata op één plek, zodat alle managers dezelfde gegevens gebruiken
    public static class GameSession
    {
        private static GameData _current;

        // Laadt de data de eerste keer dat iemand hem nodig heeft
        public static GameData Current
        {
            get
            {
                _current ??= SaveManager.Load();
                return _current;
            }
        }

        public static void Save()
        {
            SaveManager.Save(Current);
        }
    }
}