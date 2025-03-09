using UnityEngine;
using System.Collections.Generic;

// Define the character structure separately
[System.Serializable]
public class Character3D {
    public string name;
    public int health;
    public int attack;
    public int defense;
    public int energy;
    public Unit playerUnit;
    public GameObject player;
}

[System.Serializable]
public class CharacterList3D {
    public List<Character3D> characters;
}

public class CharacterSystem3D : MonoBehaviour {
    [SerializeField] List<Transform> TransformForHealth;
    [SerializeField] List<GameObject> GameObjectForHealth;
    public TextAsset jsonFile3;

    public CharacterList3D Load(int characterCount) {
        return LoadCharacters3D(characterCount);
    }

    private CharacterList3D LoadCharacters3D(int characterCount) {


        Debug.Log("[CharacterSystem] LOADING CHARACTERS");

        string json = jsonFile3.text;
        CharacterList3D characterList = JsonUtility.FromJson<CharacterList3D>(json);

        // Print out the character data
        foreach (Character3D character3D in characterList.characters) {
            Debug.Log($"[CharacterSystem] Character: {character3D.name}");
            Debug.Log($"[CharacterSystem] Health: {character3D.health}");
            Debug.Log($"[CharacterSystem] Attack: {character3D.attack}");
            Debug.Log($"[CharacterSystem] Defense: {character3D.defense}");
            Debug.Log($"[CharacterSystem] Energy: {character3D.energy}");
        }

        for (int i = 0; i < characterCount; i++) {
            if (GameObjectForHealth.Count > 0) {
                GameObject newPlayer = Instantiate(GameObjectForHealth[i], TransformForHealth[i]);
                characterList.characters[i].player = newPlayer;
                characterList.characters[i].playerUnit = newPlayer.GetComponent<Unit>();
                characterList.characters[i].playerUnit.SetStats(characterList.characters[i].health, characterList.characters[i].attack, characterList.characters[i].defense, 50, characterList.characters[i].name, 0,  characterList.characters[i].energy);
            }
        }

        return characterList;
    }
}
