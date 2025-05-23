using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


// just a class to load in character data
[System.Serializable]
public class Character {

    // normal stats for character
    public string name;
    public int health;
    public int attack;
    public int defense;
    public int energy;
    public int weight;

    // player UI stuff
    public GameObject player;
    public Unit playerUnit;
    public GameObject playerHealth;
    public Text playerHud;
    public Text playerHudAttack;
    public Text playerHudSkill;
    public Text playerHudItem;
    public Transform healthBarPanel;
    public Transform manaBarPanel;
    public GameObject playerMana;
}
[System.Serializable]
public class CharacterList {
    public List<Character> characters;
}

public class CharacterSystem : MonoBehaviour {
    [SerializeField] List<Transform> playerBattleStations;
    [SerializeField] List<GameObject> playerPrefab;
    [SerializeField] List<Transform> healthBarPanels;
    [SerializeField] GameObject healthBarsAllies;
    [SerializeField] List<Text> playerHuds;
    [SerializeField] List<Text> playerHudsAttack;
    [SerializeField] List<Text> playerHudsSkill;
    [SerializeField] List<Text> playerHudsItem;
    [SerializeField] GameObject manaBarsAllies;
    [SerializeField] List<Transform> manaBarPanels;
    public TextAsset jsonFile;

    public CharacterList Load(int characterCount) {
        return LoadCharacters(characterCount);
    }

    CharacterList LoadCharacters(int characterCount) {
        Debug.Log("[CharacterSystem] LOADING CHARACTERS");
        string json = jsonFile.ToString();
        CharacterList characterList = JsonUtility.FromJson<CharacterList>(json);
        // Print out the character data
        foreach (Character character in characterList.characters) {
            Debug.Log($"[CharacterSystem] Character: {character.name}");
            Debug.Log($"[CharacterSystem] Health: {character.health}");
            Debug.Log($"[CharacterSystem] Attack: {character.attack}");
            Debug.Log($"[CharacterSystem] Defense: {character.defense}");
            Debug.Log($"[CharacterSystem] Energy: {character.energy}");
            Debug.Log($"[CharacterSystem] Weight: {character.weight}");
        }
        
        for (int i = 0; i < 3; i++) {
            Debug.Log("Creating player: " + characterList.characters[i].name);
            GameObject newPlayer = Instantiate(playerPrefab[i], playerBattleStations[i]);
            characterList.characters[i].player = newPlayer;
            characterList.characters[i].playerUnit = newPlayer.GetComponent<Unit>();
            characterList.characters[i].playerUnit.SetStats(characterList.characters[i].health, characterList.characters[i].attack, characterList.characters[i].defense, characterList.characters[i].health, characterList.characters[i].name, 0,  characterList.characters[i].energy, characterList.characters[i].weight);
            characterList.characters[i].playerHud = playerHuds[i];
            characterList.characters[i].playerHud.text = characterList.characters[i].playerUnit.getName();
            characterList.characters[i].playerHudAttack = playerHudsAttack[i];
            characterList.characters[i].playerHudAttack.text = "Attack: ";
            characterList.characters[i].playerHudAttack.gameObject.SetActive(false);
            characterList.characters[i].playerHudSkill = playerHudsSkill[i];
            characterList.characters[i].playerHudSkill.text = "Skill: ";
            characterList.characters[i].playerHudSkill.gameObject.SetActive(false);
            characterList.characters[i].playerUnit.gameObject.SetActive(false);
            characterList.characters[i].playerHudItem = playerHudsItem[i];
            characterList.characters[i].playerHudItem.text = "Item";
            characterList.characters[i].playerHudItem.gameObject.SetActive(false);


            GameObject healthBar = Instantiate(healthBarsAllies, healthBarPanels[i]);
            characterList.characters[i].healthBarPanel = healthBarPanels[i];
            healthBar.GetComponent<Slider>().maxValue = characterList.characters[i].playerUnit.getMaxHP();
            healthBar.GetComponent<Slider>().value = characterList.characters[i].playerUnit.getCurrentHP();
            characterList.characters[i].playerHealth = healthBar;
            characterList.characters[i].playerHealth.gameObject.SetActive(false);
            characterList.characters[i].healthBarPanel.gameObject.SetActive(false);
            characterList.characters[i].playerHud.gameObject.SetActive(false);

            GameObject manaBar = Instantiate(manaBarsAllies, manaBarPanels[i]);
            characterList.characters[i].manaBarPanel = manaBarPanels[i];
            characterList.characters[i].playerMana = manaBar;
            manaBar.GetComponent<Slider>().maxValue = characterList.characters[i].playerUnit.getEnergy();
            manaBar.GetComponent<Slider>().value = characterList.characters[i].playerUnit.getEnergy();
            characterList.characters[i].manaBarPanel.gameObject.SetActive(false);
            characterList.characters[i].playerMana.gameObject.SetActive(false);
        }

        return characterList;
    }
}
