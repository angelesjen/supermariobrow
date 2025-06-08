using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;
    private bool allowBacktracking = false;
    private float previousCameraX;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        previousCameraX = transform.position.x;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;

        if (allowBacktracking)
        {
            cameraPosition.x = player.position.x;
        }
        else
        {
            cameraPosition.x = Mathf.Max(previousCameraX, player.position.x);
        }

        previousCameraX = cameraPosition.x;
        transform.position = cameraPosition;
    }

    public void EnableBacktracking(bool enable)
    {
        allowBacktracking = enable;
    }
}