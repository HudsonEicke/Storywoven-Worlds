using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;

public class HideSpriteOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private EnemySystem enemySystem;
    private List<Transform> enemyBattleStation;

    void Start()
    {
        // Assuming the EnemySystem script is attached to the same GameObject
        enemySystem = GetComponent<EnemySystem>();

        if (enemySystem != null)
        {
            // Now you can call the method
            enemyBattleStation = enemySystem.getEnemyBattleStation();
        }
        else
        {
            Debug.LogError("EnemySystem component not found on this GameObject.");
        }
        // Hide the enemy sprite when the mouse pointer enters the button area
        foreach (Transform enemy in enemyBattleStation)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
