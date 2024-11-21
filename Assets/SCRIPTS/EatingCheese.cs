using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EatingCheese : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform currentTarget;
    private bool isEating = false;  // Flag to indicate if the capsule is eating

    public float stopDistance = 2f;  // Distance to stop at the target
    public Cheesespawner cheeseSpawner;  // Reference to the Cheesespawner script

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (currentTarget != null && !isEating)
        {
            // Move towards the target
            agent.SetDestination(currentTarget.position);

            // Check if the capsule is close enough to the target
            if (Vector3.Distance(transform.position, currentTarget.position) <= stopDistance)
            {
                StartCoroutine(EatCheese());
            }
        }
    }

    // Coroutine for eating process (waiting 4 seconds)
    private IEnumerator EatCheese()
    {
        isEating = true;
        agent.isStopped = true;  // Stop the agent's movement

        // Wait for 4 seconds (eating process)
        yield return new WaitForSeconds(4f);

        // Destroy the target and call OnTargetDestroyed to find a new target
        Destroy(currentTarget.gameObject);

        // Notify Cheesespawner that the target was destroyed
        if (cheeseSpawner != null)
        {
            cheeseSpawner.OnTargetDestroyed(currentTarget.gameObject);
        }

        OnTargetDestroyed();

        isEating = false;
        agent.isStopped = false;  // Allow movement again
    }

    // Set a new target for the capsule
    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    // Called when the target is destroyed
    private void OnTargetDestroyed()
    {
        currentTarget = null;
    }
}
