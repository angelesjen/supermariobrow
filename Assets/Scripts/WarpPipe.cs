using System.Collections;
using UnityEngine;

public class WarpPipe : MonoBehaviour
{
    [Header("Warp Settings")]
    public Transform destinationPipe;
    public float warpDelay = 0.5f;
    public KeyCode enterKey = KeyCode.S;

    private bool isWarping = false;
    private bool playerInRange = false;
    private PlayerMovement playerMovement;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (playerInRange && !isWarping && Input.GetKeyDown(enterKey) && playerMovement != null)
        {
            StartCoroutine(WarpPlayer());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerMovement = other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private IEnumerator WarpPlayer()
    {
        SideScrolling scrollingCamera = Camera.main.GetComponent<SideScrolling>();
        if (scrollingCamera != null) scrollingCamera.EnableBacktracking(true);
        isWarping = true;

        if (playerMovement == null || playerMovement.transform == null)
        {
            Debug.LogError("Player reference lost during warp!");
            isWarping = false;
            yield break;
        }

        Transform player = playerMovement.transform;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Player missing Rigidbody2D!");
            isWarping = false;
            yield break;
        }

        bool wasMovementEnabled = playerMovement.enabled;
        bool wasKinematic = rb.isKinematic;
        Vector3 originalScale = player.localScale;

        playerMovement.enabled = false;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.pipe);
        }

        Vector3 startPos = player.position;
        Vector3 endPos = transform.position;
        float timer = 0;

        while (timer < warpDelay / 2)
        {
            if (player == null) break; 

            player.position = Vector3.Lerp(startPos, endPos, timer / (warpDelay / 2));
            player.localScale = Vector3.Lerp(originalScale, new Vector3(0.8f, 0.5f, 1f), timer / (warpDelay / 2));
            timer += Time.deltaTime;
            yield return null;
        }

        if (player != null && destinationPipe != null)
        {
            player.position = destinationPipe.position;

            timer = 0;
            startPos = player.position;
            endPos = startPos + (Vector3.up * 1f);

            while (timer < warpDelay / 2)
            {
                if (player == null) break;

                player.position = Vector3.Lerp(startPos, endPos, timer / (warpDelay / 2));
                player.localScale = Vector3.Lerp(new Vector3(0.8f, 0.5f, 1f), originalScale, timer / (warpDelay / 2));
                timer += Time.deltaTime;
                yield return null;
            }
        }

        if (player != null && playerMovement != null && rb != null)
        {
            player.localScale = originalScale;
            rb.isKinematic = wasKinematic;
            rb.velocity = Vector2.zero;
            playerMovement.enabled = wasMovementEnabled;
            player.up = Vector2.up;
        }

        isWarping = false;

        yield return new WaitForSeconds(0.5f); 

        if (scrollingCamera != null) scrollingCamera.EnableBacktracking(false);
    }
}