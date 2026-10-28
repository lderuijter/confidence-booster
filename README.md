# Confidence Booster

Een Windows-game in Unity (C#) voor positieve zelfreflectie. De speler registreert elke dag één prestatie waar hij trots op is, verdient daar coins mee en bouwt zo een eigen collectie op. Dit is mijn innovatieproject (BP5).

## Hoe werkt het

- De speler registreert maximaal één prestatie per dag en verdient daarvoor 10 coins.
- Het home-scherm toont het aantal coins.
- De historie toont alle prestaties met datum, de nieuwste bovenaan.
- Met coins koop je items in de shop, die in de inventory komen. 

De game heeft geen speelbaar personage. Alles gebeurt in één scene met vijf panels: Home, Prestatie toevoegen, Historie, Shop en Inventory.

## Techstack

- Unity 6.3 LTS
- C#
- Unity UI en TextMeshPro
- JSON-opslag via `JsonUtility`
- Git met issues, feature-branches en pull requests

## Projectstructuur

```
Assets/
  Scenes/MainScene.unity    de enige scene
  Resources/                JSON bestanden voor de game
  Scripts/
    Data/                   datamodel (GameData, AchievementData)
    Managers/               logica (SaveManager, AchievementManager, UIManager)
    Screens/                scripts per scherm (AddAchievementScreen, HomeScreen, HistoryScreen)
  Prefabs/                  herbruikbare UI-onderdelen (HistoryItem)
```
