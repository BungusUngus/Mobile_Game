using UnityEngine;

public class PipeSpawner : MonoBehaviour, IStop, IRestart
{
    [SerializeField] private GameObject pipePrefab;

    [Tooltip("How far up or down the pipes can spawn from centre")]
    [SerializeField] private float pipeSpawnRange;

    [Tooltip("How long to wait between pipes")]
    [SerializeField] private float pipeSpawnDelay;

    private float pipeTimeLastSpawned;

    private bool isActive = true;

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        //if the current time is greater than the time we last spawned plus our delay, our delay is over. we should spawn
        if (Time.time > pipeTimeLastSpawned + pipeSpawnDelay)
        {
            SpawnPipe();
        }
    }

    private void SpawnPipe()
    {
        pipeTimeLastSpawned = Time.time;

        float yOffset = Random.Range(-pipeSpawnRange, pipeSpawnRange);

        GameObject pipes = Instantiate(pipePrefab);
        //if yOffset is -ve, this will spawn the pipes lower, if it's +ve, it will spawn higher
        pipes.transform.position = transform.position + Vector3.up * yOffset;

    }

    public void Stop()
    {
        isActive = false;
    }

    public void Restart()
    {
        isActive = true;
        pipeTimeLastSpawned = Time.time;
    }
}
