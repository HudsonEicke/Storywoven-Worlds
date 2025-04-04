using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EnemySystem : MonoBehaviour {
    // some variables we need to set for Unity
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> enemyBattleStation;
    [SerializeField] private GameObject healthBarEnemy;
    [SerializeField] private List<Transform> healthBarEnemyPanels;
    [SerializeField] private List<Text> enemyHud;

    // class to store all of the stuff we need for one enemy
    public class EnemyHealthAndInfo {

        // this deals with what we see of the enemy
        public GameObject enemy;        
        public Text enemyHud;
        public Unit enemyUnit; // this is where we can start filling the stats of the enemy

        // this deals with the health aspect of enemy
        public GameObject enemyHealth;
        public Transform healthPanel;
    }

    public List<EnemyHealthAndInfo> loadEnemies(int enemyCount) {
        List<EnemyHealthAndInfo> enemyList = new List<EnemyHealthAndInfo>(); // we will load this list up in here
        for (int i = 0; i < enemyCount; i ++) {
            EnemyHealthAndInfo newEnemy = new EnemyHealthAndInfo();
            GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation[i]);
            newEnemy.enemy = enemy;
            newEnemy.enemyUnit = enemy.GetComponent<Unit>();
            newEnemy.enemyUnit.SetStats(100, 10, 5, 100, "Warrior " + (i + 1), 1, 100, 10);
            newEnemy.enemyHud = enemyHud[i];
            newEnemy.enemyHud.text = newEnemy.enemyUnit.getName();
            newEnemy.enemyHealth = Instantiate(healthBarEnemy, healthBarEnemyPanels[i]);
            newEnemy.healthPanel = healthBarEnemyPanels[i];
            newEnemy.enemyHealth.GetComponent<Slider>().maxValue = newEnemy.enemyUnit.getMaxHP();
            newEnemy.enemyHealth.GetComponent<Slider>().value = newEnemy.enemyUnit.getCurrentHP();
            enemyList.Add(newEnemy);
            enemyList[i].enemyUnit.gameObject.SetActive(false);
            enemyList[i].enemyHealth.gameObject.SetActive(false);
            enemyList[i].healthPanel.gameObject.SetActive(false);
            enemyList[i].enemyHud.gameObject.SetActive(false);
        }

        return enemyList;
    }


}