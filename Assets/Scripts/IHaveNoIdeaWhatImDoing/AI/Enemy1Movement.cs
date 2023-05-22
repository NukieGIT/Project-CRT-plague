using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy1Movement : MonoBehaviour
{
    [SerializeField] private Transform Target;
    private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    private const string isInRange = "isInRange";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        StartCoroutine(followTarget());
    }

    private IEnumerator followTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (enabled)
        {
            agent.SetDestination(Target.transform.position);
            yield return wait;
        }
        animator.SetBool(isInRange, agent.velocity == Vector3.zero);
    }
}
