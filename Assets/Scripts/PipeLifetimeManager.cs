using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeLifetimeManager : MonoBehaviour
{
    [SerializeField] public Camera mainCamera;
    [SerializeField] public GameObject pipePrefab;
    public float distanceBetweenPipes;
    public float respawnTime = 2f;

    // This represents the top right corner of mainCamera in world coordinates
    private Vector2 leftBound;
    private Vector2 rightBound;
    private const float BOUND_OFFSET = 3f;
    // Time between respawn/despawn checks
    private List<GameObject> spawnedPipes = new List<GameObject>();
    private float randomVerticalOffset;
    private Vector3 topPipePosition;
    private Vector3 previousTopPipePosition;
    private Vector3 bottomPipePosition;
    private Vector3 previousBottomPipePosition;
    private Vector3 cameraPosition;
    private float pipeVerticalLengthFromCenter;
    private Bounds pipeColliderBounds;
    private float topPipeVerticalPosition;
    private float bottomPipeVerticalPosition;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (pipePrefab == null)
            pipePrefab = Resources.Load<GameObject>("Prefabs/Pipe");
    }

    private void OnEnable()
    {
        UpdateCameraBounds();
        CalculatePipeDistances();
        StartCoroutine(SpawnPipeCoroutine());
        StartCoroutine(DespawnPipeCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(SpawnPipeCoroutine());
        StopCoroutine(DespawnPipeCoroutine());
    }

    private void UpdateCameraBounds()
    {
        cameraPosition = mainCamera.transform.position;
        // Transform the top right corner of the camera into world coordinates
        Vector3 topRightCorner =
            mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        // Pipes are moving from right to left, so make the bound be left side
        leftBound = new Vector2(-topRightCorner.x, cameraPosition.y);
        rightBound = new Vector2(topRightCorner.x, cameraPosition.y);
    }

    private IEnumerator SpawnPipeCoroutine()
    {
        /*
         * Method spawns pipes every 2seconds
         */
        while (true)
        {
            SpawnPipe();
            yield return new WaitForSeconds(respawnTime);
        }
    }

    private IEnumerator DespawnPipeCoroutine()
    {
        /*
         * Method checks whether the pipes are off-screen and despawns the off-screen pipes every 2s
         */
        while (true)
        {
            for (var i = 0; i < spawnedPipes.Count; ++i)
            {
                if (spawnedPipes[i].transform.position.x < leftBound.x - BOUND_OFFSET || spawnedPipes[i].transform.position.x > rightBound.x + BOUND_OFFSET)
                {
                    Destroy(spawnedPipes[i]);
                    spawnedPipes.RemoveAt(i);
                }
            }

            yield return new WaitForSeconds(respawnTime);
        }
    }

    private void SpawnPipe()
    {
        /*
         * Takes care of the spawn logic of the pipes, including where to spawn them, the distance between them,
         * spawning relative to last set of pipes, etc
         */
        randomVerticalOffset = Random.Range(-3f, 3f);
        bottomPipeVerticalPosition = -(distanceBetweenPipes / 2) - pipeVerticalLengthFromCenter + randomVerticalOffset;
        bottomPipePosition = new Vector3(rightBound.x + BOUND_OFFSET, bottomPipeVerticalPosition, 0);
        topPipeVerticalPosition = distanceBetweenPipes / 2 + pipeVerticalLengthFromCenter + randomVerticalOffset;
        topPipePosition = new Vector3(rightBound.x + BOUND_OFFSET, topPipeVerticalPosition, 0);
        GameObject bottomPipe = Instantiate(pipePrefab, bottomPipePosition, Quaternion.identity);
        GameObject topPipe = Instantiate(pipePrefab, topPipePosition, Quaternion.identity);
        Vector3 newTopPipeScale = topPipe.transform.localScale;
        newTopPipeScale.y = -newTopPipeScale.y;
        topPipe.transform.localScale = newTopPipeScale;
        spawnedPipes.Add(bottomPipe);
        spawnedPipes.Add(topPipe);
    }

    private void CalculatePipeDistances()
    {
        if (pipePrefab != null)
        {
            // Get the bounds of the pipe collider
            GameObject testPipe = Instantiate(pipePrefab, new Vector3(1000, 1000, 0), Quaternion.identity);
            pipeColliderBounds = testPipe.GetComponent<BoxCollider2D>().bounds;
            pipeVerticalLengthFromCenter = pipeColliderBounds.extents.y;
            Destroy(testPipe);
        }
    }
}