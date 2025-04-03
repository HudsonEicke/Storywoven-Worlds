using UnityEngine;
using System.Collections.Generic;

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

public class SkillListPlayer2 {
    public List<Skill> P2Skills;
}

public class SkillListPlayer3 {
    public List<Skill> P3Skills;
}

public class SkillSystemPlayer : MonoBehaviour {
    public TextAsset jsonFile;
    public TextAsset jsonFile2;
    public TextAsset jsonFile3;

    public SkillListPlayer1 Load() {
        return LoadSkills();
    }

    public SkillListPlayer2 Load2() {
        return LoadSkills2();
    }
    public SkillListPlayer3 Load3() {
        return LoadSkills3();
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

    SkillListPlayer2 LoadSkills2() {
        Debug.Log("[SkillSystemPlayer] LOADING SKILLS2");
        string json = jsonFile2.ToString();
        SkillListPlayer2 skillList = JsonUtility.FromJson<SkillListPlayer2>(json);
        // Print out the skill data
        foreach (var skill in skillList.P2Skills) {
            Debug.Log($"Name: {skill.name}, Description: {skill.description}, Attack: {skill.attack}, Cost: {skill.cost}, Type: {skill.type}, Heal Amount: {skill.healAmt}");
        }

        return skillList;
    }

    SkillListPlayer3 LoadSkills3() {
        Debug.Log("[SkillSystemPlayer] LOADING SKILLS3");
        string json = jsonFile3.ToString();
        SkillListPlayer3 skillList = JsonUtility.FromJson<SkillListPlayer3>(json);
        // Print out the skill data
        foreach (var skill in skillList.P3Skills) {
            Debug.Log($"Name: {skill.name}, Description: {skill.description}, Attack: {skill.attack}, Cost: {skill.cost}, Type: {skill.type}, Heal Amount: {skill.healAmt}");
        }

        return skillList;
    }
}