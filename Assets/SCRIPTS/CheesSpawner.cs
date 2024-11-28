using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheesespawner : MonoBehaviour
{
    public GameObject[] cheese; // Array of possible cheese targets
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 15f;

    private List<GameObject> spawnedCheese = new List<GameObject>(); // List to track spawned cheese objects
    public GameObject capsule; // Reference to the capsule (or the object that moves towards cheese)

    void Start()
    {
        // Start the coroutine to spawn cheese
        StartCoroutine(SpawnCheese());
    }

    IEnumerator SpawnCheese()
    {
        while (true) // Loop forever
        {
            // Ensure the cheese array is not empty
            if (cheese.Length == 0)
            {
                yield break; // Stop the coroutine if the array is empty
            }

            // Pick a random cheese object from the array
            int randomIndex = Random.Range(0, cheese.Length);
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-50f, 50f), 0.5f, Random.Range(-50f, 50f));

            GameObject spawned = Instantiate(cheese[randomIndex], randomSpawnPosition, Quaternion.identity);  // Spawn the object
            spawnedCheese.Add(spawned);  // Add to list of spawned cheese

            // Set the capsule to target the first cheese initially
            if (spawnedCheese.Count == 1 && capsule != null)
            {
                EatingCheese eatingScript = capsule.GetComponent<EatingCheese>();  // Get the EatingCheese script from the capsule
                if (eatingScript != null)
                {
                    eatingScript.SetTarget(spawned.transform);  // Set target for the capsule
                }
            }

            // Wait for a random interval before spawning the next cheese
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    // Call this method when a target (cheese) is destroyed
    public void OnTargetDestroyed(GameObject destroyedTarget)
    {
        // Remove the destroyed target from the list
        spawnedCheese.Remove(destroyedTarget);

        // If there are still targets, find the nearest one
        if (spawnedCheese.Count > 0 && capsule != null)
        {
            GameObject closestTarget = FindClosestTarget();
            EatingCheese eatingScript = capsule.GetComponent<EatingCheese>();  // Get the EatingCheese script from the capsule
            if (eatingScript != null)
            {
                eatingScript.SetTarget(closestTarget.transform);  // Set the new target for the capsule
            }
        }
        else
        {
            // If no targets left, you can either stop the capsule or handle it however you like
            Debug.Log("No more cheese to target!");
        }
    }

    // Method to find the closest target from the list
    GameObject FindClosestTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in spawnedCheese)
        {
            float distance = Vector3.Distance(capsule.transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}
