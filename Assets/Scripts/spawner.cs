using UnityEngine;
public class spawner : MonoBehaviour
{
    // result to be spawned
    public GameObject objectToSpawn;

    // how far away result is spawned
    public float spawnDistance = 3f;

    void OnCollisionEnter(Collision collision)
    {
        // Check if this object is "ingredient1" and the other is "ingredient2"
        if ((gameObject.CompareTag("ingredient1") && collision.gameObject.CompareTag("ingredient2")))
        {
            // Destroy both objects
            Destroy(gameObject);
            Destroy(collision.gameObject);

            // Spawn the result
            SpawnResult();
        }
    }

    public void SpawnResult()
    {
        Instantiate(
            objectToSpawn,
            Camera.main.transform.position + Camera.main.transform.forward * spawnDistance,
            Camera.main.transform.rotation
        );
    }
}