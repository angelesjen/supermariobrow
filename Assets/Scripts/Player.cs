using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    private DeathAnimation DeadAnim;

    public bool big => bigRenderer.enabled;
    public bool small => smallRenderer.enabled;
    public bool dead => DeadAnim.enabled;

    private void Awake()
    {
        DeadAnim = GetComponent<DeathAnimation>();
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
                Die();
            }
        }
    }

    private void Shrink()
    {
        
    }

    public void Die()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        DeadAnim.enabled = true;

        GameManager.Instance.ResetLevel(2f);
    }
}
