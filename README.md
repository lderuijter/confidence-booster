# Confidence Booster

Een Windows-game in Unity (C#) voor positieve zelfreflectie. De speler registreert elke dag één prestatie waar hij trots op is, verdient daar coins mee en bouwt zo een eigen collectie op. Dit is mijn innovatieproject (BP5).

## Hoe werkt het

- De speler registreert maximaal één prestatie per dag en verdient daarvoor 10 coins. Lege invoer wordt geweigerd.
- Het overzicht toont het aantal coins en of er vandaag al een prestatie is toegevoegd.
- De geschiedenis toont alle prestaties met datum, de nieuwste bovenaan.
- In de winkel koop je met coins items uit een lijst van 20 items. Elk item kan één keer worden gekocht.
- Gekochte items komen in de inventaris.
- De voortgang wordt automatisch opgeslagen en blijft behouden tussen speelsessies.
- Met de knop "Delete all progress" wordt alle voortgang gewist (bedoeld voor de demonstratie).

De game heeft geen speelbaar personage en geen wereld. Alles gebeurt in één scene met vijf panels:

| Panel | Functie |
| --- | --- |
| Overzicht | Welkomsttekst, aantal coins en dagstatus |
| Prestatie toevoegen | Invoerveld om de prestatie van vandaag te registreren |
| Geschiedenis | Lijst met alle eerdere prestaties |
| Winkel | Items bekijken en kopen |
| Inventaris | Items die de speler bezit |

## Techstack

- Unity 6.3 LTS (6000.3.25f1)
- C#
- Unity UI en TextMeshPro
- JSON-opslag via `JsonUtility`
- Git met issues, feature-branches en pull requests

## Installeren en spelen

1. Ga naar de [Releases](https://github.com/lderuijter/confidence-booster/releases) en download de nieuwste versie (v1.0.0).
2. Pak het bestand uit en start het `.exe`-bestand.

De voortgang wordt opgeslagen als `savegame.json` in de standaard opslagmap van Unity (`Application.persistentDataPath`).

## Project openen in Unity

1. Installeer Unity Hub en Unity 6.3 LTS (6000.3.25f1) met Windows Build Support.
2. Clone de repository: `git clone https://github.com/lderuijter/confidence-booster.git`
3. Voeg de map toe in Unity Hub en open het project.
4. Open `Assets/Scenes/MainScene.unity` en druk op Play.

Voor een Windows-build gebruik je in Unity de Build Profiles (File > Build Profiles > Windows).

## Projectstructuur

```
Assets/

  Scenes/

    MainScene.unity             de enige scene, met alle panels

  Resources/

    shop_items.json             de 20 shop-items (id, naam, beschrijving, prijs)

  Prefabs/

    HistoryItem.prefab          één regel in de geschiedenis

    ShopItem.prefab             één item in de winkel (met koopknop)

    InventoryItem.prefab        één item in de inventaris (zonder koopknop)

  Scripts/

    Data/

      GameData.cs               datamodellen: GameData, AchievementData, ShopItemData

    Managers/

      GameSession.cs            houdt de speldata op één plek, zodat alle managers dezelfde data gebruiken

      SaveManager.cs            laadt en bewaart de speldata als JSON-bestand

      AchievementManager.cs     prestaties toevoegen, coins toekennen, maximaal één per dag

      ShopManager.cs            laadt de shop-items en verwerkt aankopen

      InventoryManager.cs       bepaalt welke items de speler bezit

      UIManager.cs              toont één panel tegelijk, regelt de navigatie en de reset

    Screens/

      HomeScreen.cs             overzicht: coins en dagstatus

      AddAchievementScreen.cs   invoerveld, knop en meldingen bij het toevoegen

      HistoryScreen.cs          bouwt de lijst met prestaties op

      ShopScreen.cs             bouwt de winkel op en koppelt de koopknoppen

      InventoryScreen.cs        bouwt de inventaris op en toont een melding als die leeg is

  Settings/                     standaardinstellingen van Unity (render pipeline, input)

Packages/                       Unity-packages van het project

ProjectSettings/                projectinstellingen van Unity
```

## Hoe de onderdelen samenwerken

- **Data:** `GameData` bevat de coins, de datum van de laatste prestatie, de lijst met prestaties en de id's van gekochte items. `GameSession` laadt deze data één keer via `SaveManager` en deelt hem met alle managers. Zo overschrijven coins en aankopen elkaar niet.
- **Logica:** de managers bevatten de spelregels en geven een resultaat terug (bijvoorbeeld `Success`, `NotEnoughCoins` of `AlreadyAddedToday`). Na elke wijziging wordt de data opgeslagen.
- **Schermen:** de scripts in `Screens` tonen de data en geven meldingen aan de speler. Ze luisteren naar events van de managers (zoals `CoinsChanged`), zodat de weergave direct wordt bijgewerkt.
- **Navigatie:** `UIManager` toont steeds één panel en verbergt de andere.

## Werkwijze

Ik werk met GitHub-issues, een aparte feature-branch per onderdeel en pull requests naar `main`. Versie 1.0.0 is gepubliceerd via GitHub Releases.
