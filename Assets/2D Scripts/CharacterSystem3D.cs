using UnityEngine;
using System.Collections.Generic;


// just a class to load in character data
[System.Serializable]
public class Character3D {

    // normal stats for character
    public string name;
    public int health;
    public int attack;
    public int defense;
    public int energy;
}
[System.Serializable]
public class CharacterList3D {
    public List<Character> characters;
}

public class CharacterSystem3D : MonoBehaviour {
    public TextAsset jsonFile;

    public CharacterList3D Load(int characterCount) {
        return LoadCharacters3D(characterCount);
    }

    CharacterList3D LoadCharacters3D(int characterCount) {
        Debug.Log("[CharacterSystem] LOADING CHARACTERS");
        string json = jsonFile.ToString();
        CharacterList3D characterList = JsonUtility.FromJson<CharacterList3D>(json);
        // Print out the character data
        foreach (Character character in characterList.characters) {
            Debug.Log($"[CharacterSystem] Character: {character.name}");
            Debug.Log($"[CharacterSystem] Health: {character.health}");
            Debug.Log($"[CharacterSystem] Attack: {character.attack}");
            Debug.Log($"[CharacterSystem] Defense: {character.defense}");
            Debug.Log($"[CharacterSystem] Energy: {character.energy}");
        }

        return characterList;
    }
}
