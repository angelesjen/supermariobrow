using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    AudioManager audioManager;
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private DeathAnimation DeadAnim;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead => DeadAnim.enabled;

    private void Awake()
    {
        DeadAnim = GetComponent<DeathAnimation>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

    }
    public void Hit()
    {
        if (!dead)
        {
            if (big)
            {
                Shrink();
            }
            else
            {
                audioManager.PlaySFX(audioManager.die);
                Die();
            }
        }
    }

    private void Shrink()
    {
        
    }

    public void Die()
    {
        audioManager.PlaySFX(audioManager.die);
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        DeadAnim.enabled = true;

        GameManager.Instance.ResetLevel(2f);
    }
}
