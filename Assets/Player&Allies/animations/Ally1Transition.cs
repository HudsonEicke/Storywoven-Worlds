using System.Collections;
using UnityEngine;

public class Ally1Transition : MonoBehaviour
{
    private Animator Ally1Animator;
    public bool attack = false;

    private bool isAttacking = false;

    void Start()
    {
        Ally1Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Ally1Animator != null && attack && !isAttacking)
        {
            isAttacking = true;
            Ally1Animator.SetTrigger("AllyOneAttack");

            // Start coroutine to reset attack after animation
            StartCoroutine(ResetAttackAfterAnimation("AllyOneAttack"));
        }
    }

    private IEnumerator ResetAttackAfterAnimation(string animationName)
    {
        // Get the current clip length from Animator (assumes it's in layer 0)
        AnimatorStateInfo info = Ally1Animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = info.length;

        yield return new WaitForSeconds(animationLength);

        attack = false;
        isAttacking = false;
    }
}
