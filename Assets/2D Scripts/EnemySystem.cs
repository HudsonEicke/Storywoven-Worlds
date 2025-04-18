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
    [SerializeField] private List<Text> enemyStat;
    [SerializeField] private List<Text> enemyStun;
    public static EnemySystem Instance;

    // class to store all of the stuff we need for one enemy
    public class EnemyHealthAndInfo {

        // this deals with what we see of the enemy
        public GameObject enemy;        
        public Text enemyHud;
        public Text enemyStat;
        public Text enemyStun;
        public Unit enemyUnit; // this is where we can start filling the stats of the enemy

        // this deals with the health aspect of enemy
        public GameObject enemyHealth;
        public Transform healthPanel;
    }

    private void Awake()
    {
        Instance = this;
    }

    public List<EnemyHealthAndInfo> loadEnemies(int enemyCount) {
        List<EnemyHealthAndInfo> enemyList = new List<EnemyHealthAndInfo>(); // we will load this list up in here
        // for (int i = 0; i < enemyCount; i ++) {
        EnemyHealthAndInfo newEnemy = new EnemyHealthAndInfo();
        GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation[0]);
        newEnemy.enemy = enemy;
        newEnemy.enemyUnit = enemy.GetComponent<Unit>();
        newEnemy.enemyUnit.SetStats(100, 20, 5, 100, "Warrior", 1, 100, 10);
        newEnemy.enemyHud = enemyHud[0];
        newEnemy.enemyStat = enemyStat[0];
        newEnemy.enemyStun = enemyStun[0];
        newEnemy.enemyStat.text = "Burn";
        newEnemy.enemyHud.text = newEnemy.enemyUnit.getName();
        newEnemy.enemyHealth = Instantiate(healthBarEnemy, healthBarEnemyPanels[0]);
        newEnemy.healthPanel = healthBarEnemyPanels[0];
        newEnemy.enemyHealth.GetComponent<Slider>().maxValue = newEnemy.enemyUnit.getMaxHP();
        newEnemy.enemyHealth.GetComponent<Slider>().value = newEnemy.enemyUnit.getCurrentHP();
        enemyList.Add(newEnemy);
        enemyList[0].enemyUnit.gameObject.SetActive(false);
        enemyList[0].enemyHealth.gameObject.SetActive(false);
        enemyList[0].healthPanel.gameObject.SetActive(false);
        enemyList[0].enemyHud.gameObject.SetActive(false);
        enemyList[0].enemyStat.gameObject.SetActive(false);
        enemyList[0].enemyStun.gameObject.SetActive(false);
        // }

        EnemyHealthAndInfo newEnemyD = new EnemyHealthAndInfo();
        GameObject enemyD = Instantiate(enemyPrefab, enemyBattleStation[1]);
        newEnemyD.enemy = enemyD;
        newEnemyD.enemyUnit = enemyD.GetComponent<Unit>();
        newEnemyD.enemyUnit.SetStats(100, 20, 5, 100, "Defender", 1, 100, 10);
        newEnemyD.enemyHud = enemyHud[1];
        newEnemyD.enemyStat = enemyStat[1];
        newEnemyD.enemyStun = enemyStun[1];
        newEnemyD.enemyStat.text = "Burn";
        newEnemyD.enemyHud.text = newEnemyD.enemyUnit.getName();
        newEnemyD.enemyHealth = Instantiate(healthBarEnemy, healthBarEnemyPanels[1]);
        newEnemyD.healthPanel = healthBarEnemyPanels[1];
        newEnemyD.enemyHealth.GetComponent<Slider>().maxValue = newEnemyD.enemyUnit.getMaxHP();
        newEnemyD.enemyHealth.GetComponent<Slider>().value = newEnemyD.enemyUnit.getCurrentHP();
        enemyList.Add(newEnemyD);
        enemyList[1].enemyUnit.gameObject.SetActive(false);
        enemyList[1].enemyHealth.gameObject.SetActive(false);
        enemyList[1].healthPanel.gameObject.SetActive(false);
        enemyList[1].enemyHud.gameObject.SetActive(false);
        enemyList[1].enemyStat.gameObject.SetActive(false);
        enemyList[1].enemyStun.gameObject.SetActive(false);

        EnemyHealthAndInfo newEnemyH = new EnemyHealthAndInfo();
        GameObject enemyH = Instantiate(enemyPrefab, enemyBattleStation[2]);
        newEnemyH.enemy = enemyH;
        newEnemyH.enemyUnit = enemyH.GetComponent<Unit>();
        newEnemyH.enemyUnit.SetStats(100, 20, 5, 100, "Healer", 1, 100, 10);
        newEnemyH.enemyHud = enemyHud[2];
        newEnemyH.enemyStat = enemyStat[2];
        newEnemyH.enemyStun = enemyStun[2];
        newEnemyH.enemyStat.text = "Burn";
        newEnemyH.enemyHud.text = newEnemyH.enemyUnit.getName();
        newEnemyH.enemyHealth = Instantiate(healthBarEnemy, healthBarEnemyPanels[2]);
        newEnemyH.healthPanel = healthBarEnemyPanels[2];
        newEnemyH.enemyHealth.GetComponent<Slider>().maxValue = newEnemyH.enemyUnit.getMaxHP();
        newEnemyH.enemyHealth.GetComponent<Slider>().value = newEnemyH.enemyUnit.getCurrentHP();
        enemyList.Add(newEnemyH);
        enemyList[2].enemyUnit.gameObject.SetActive(false);
        enemyList[2].enemyHealth.gameObject.SetActive(false);
        enemyList[2].healthPanel.gameObject.SetActive(false);
        enemyList[2].enemyHud.gameObject.SetActive(false);
        enemyList[2].enemyStat.gameObject.SetActive(false);
        enemyList[2].enemyStun.gameObject.SetActive(false);

        return enemyList;
    }

    public List<Transform> getEnemyBattleStation() {
        return enemyBattleStation;
    }


}