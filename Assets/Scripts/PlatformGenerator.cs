using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private float timeTillPlatformSpawn = default;
    [SerializeField] public int verticalDirection = default;
    [SerializeField] public int horizontalDirection = default;
    [SerializeField] private Platform platform = default;
    [SerializeField] private Vector2 maxAddedFromLastPlatform = default;
    [SerializeField] private Vector2 minimalAddedFromLastPlatform = default;
    Vector3 lastPlatformPosition = default;
    bool spawn = true;

    // Start is called before the first frame update
    void Start()
    {
        lastPlatformPosition = FindObjectOfType<Platform>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn)
            SpawnPlatform();
    }

    private void SpawnPlatform()
    {
        int horizontalDirection = (this.horizontalDirection != 0) ? this.horizontalDirection : Random.Range(0, 2) * 2 - 1;
        int verticalDirection = (this.verticalDirection != 0) ? this.verticalDirection : Random.Range(-1, 1);

        Vector3 newPlatformPosition = lastPlatformPosition;

        float addedHorizontal = Random.Range(minimalAddedFromLastPlatform.x, maxAddedFromLastPlatform.x);
        float addedHeight = Random.Range(minimalAddedFromLastPlatform.y, maxAddedFromLastPlatform.y);

        newPlatformPosition = new Vector3(
            (newPlatformPosition.x + (addedHorizontal * horizontalDirection)),
            (newPlatformPosition.y + (addedHeight * verticalDirection)),
            newPlatformPosition.z
        );

        Instantiate(platform, newPlatformPosition, Quaternion.identity);

        lastPlatformPosition = newPlatformPosition;

        StartCoroutine(Cooldown(timeTillPlatformSpawn));
    }

    private IEnumerator Cooldown(float time)
    {
        spawn = false;
        yield return new WaitForSecondsRealtime(time);
        spawn = true;
    }
}
