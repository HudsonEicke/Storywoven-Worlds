using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;

// just a class to load in skill data
[System.Serializable]
public class Skill {
    public string name;
    public string description;
    public int attack;
    public int cost;
    public string type;
    public int healAmt;
}
[System.Serializable]
public class SkillListPlayer1 {
    public List<Skill> P1Skills;
}

public class SkillSystemPlayer : MonoBehaviour {
    public TextAsset jsonFile;

    public SkillListPlayer1 Load() {
        return LoadSkills();
    }

    SkillListPlayer1 LoadSkills() {
        Debug.Log("[SkillSystemPlayer] LOADING SKILLS");
        string json = jsonFile.ToString();
        SkillListPlayer1 skillList = JsonUtility.FromJson<SkillListPlayer1>(json);
        // Print out the skill data
        foreach (var skill in skillList.P1Skills) {
            Debug.Log($"Name: {skill.name}, Description: {skill.description}, Attack: {skill.attack}, Cost: {skill.cost}, Type: {skill.type}, Heal Amount: {skill.healAmt}");
        }

        return skillList;
    }
}