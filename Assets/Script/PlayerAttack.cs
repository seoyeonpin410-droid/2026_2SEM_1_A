using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    //공격 설정 변수 추가 

    [SerializeField] private Transform attackPoint;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 1.0f;

    //애니메이션 설정 관련 추가 관리 변수 
    //애니메이션 설정 관련 추가 관리 변수
    [SerializeField] private float attackDuration = 0.8f;
    [SerializeField] private float attackMoment = 0.3f;
    [SerializeField] private string attackAnimationName = "Attack";

    private bool isAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null || isAttacking) return;
        if (playerController.IsNormalState() == false) return;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(AttackAnimation());
        }
    }

    private IEnumerator AttackAnimation()
    {
        isAttacking = true;

        playerController.ChangeState(PlayerState.Attack);

        int attackState = Animator.StringToHash(attackAnimationName);

        if (animator.HasState(0, attackState))
        {
            animator.CrossFade(attackAnimationName, 0.1f);
        }
        else
        {
            Debug.LogWarning(attackAnimationName + " 애니메이션 상태가 없습니다. ");
        }

        yield return new WaitForSeconds(attackMoment);

        HitEnemy();

        yield return new WaitForSeconds(Mathf.Max(0f, attackDuration - attackMoment));

        animator.CrossFade("Blend Tree", 0.1f);
        playerController.ChangeState(PlayerState.Normal);
        isAttacking = false;
    }

    private void HitEnemy()
    {
        Collider[] targets = Physics.OverlapSphere(attackPoint.position, attackRange);

        foreach (Collider target in targets)
        {
            EnemyHealth enemy = target.GetComponentInParent<EnemyHealth>();

            if (enemy == null) continue;

            enemy.TakeDamage(attackDamage);
            //break;                    //break가 있으면 1명만 데미지가 들어가고 빠져 나온다.
        }
    }

    private void OnDrawGizmosSelected() //플레이어 선택시 공격 범위인 빨간색 구체가 보임
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}              