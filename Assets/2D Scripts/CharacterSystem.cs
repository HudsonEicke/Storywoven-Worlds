using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;


[System.Serializable]
public class Character {
    public string name;
    public int health;
    public int attack;
    public int defense;
    public int energy;
}
[System.Serializable]
public class CharacterList {
    public List<Character> characters;
}

public class CharacterSystem : MonoBehaviour {
    public TextAsset jsonFile;

    public CharacterList Load() {
        return LoadCharacters();
    }

    CharacterList LoadCharacters() {
        Debug.Log("[CharacterSystem] LOADING CHARACTERS");
        string json = jsonFile.ToString();
        CharacterList characterList = JsonUtility.FromJson<CharacterList>(json);
        
        // Print out the character data
        foreach (var character in characterList.characters) {
            Debug.Log($"Name: {character.name}, Health: {character.health}, Attack: {character.attack}, Defense: {character.defense}, Energy: {character.energy}");
        }

        return characterList;
    }
}
