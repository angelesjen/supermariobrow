using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flagpole : MonoBehaviour
{
    AudioManager AudioManager;
    [SerializeField] private Transform flag;
    [SerializeField] private float slideSpeed = 3f;
    [SerializeField] private float flagLowerTime = 1.5f;
    [SerializeField] private Transform bottomPosition;

    private Vector3 flagStartPos;
    public static Flagpole Instance { get; private set; }
    public bool activated { get; private set; }

    private void Start()
    {
        flagStartPos = flag.position;
        AudioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(FlagSequence(other.GetComponent<PlayerMovement>()));
        }
    }

    private IEnumerator FlagSequence(PlayerMovement player)
    {
        player.enabled = false;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Animator anim = player.GetComponent<Animator>();

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0f;
        }

        if (anim != null)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", false);
            anim.SetTrigger("isSliding");
        }

        AudioManager.PlaySFX(AudioManager.flag);

        Vector3 flagEndPos = bottomPosition.position + Vector3.up * 0.5f;
        float flagTimer = 0f;

        Vector3 playerStartPos = player.transform.position;
        float playerTimer = 0f;

        while (flagTimer < 1f || playerTimer < 1f)
        {
            if (flagTimer < 1f)
            {
                flagTimer += Time.deltaTime / flagLowerTime;
                flag.position = Vector3.Lerp(flagStartPos, flagEndPos, flagTimer);
            }

            if (playerTimer < 1f)
            {
                playerTimer += Time.deltaTime * slideSpeed;
                player.transform.position = Vector3.Lerp(playerStartPos, bottomPosition.position, playerTimer);
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);


        StartCoroutine(player.WalkIntoCastle());
    }
}
