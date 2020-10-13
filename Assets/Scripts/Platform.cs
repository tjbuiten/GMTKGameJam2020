using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Time from contact with player till platform crumbles
    [SerializeField] private float crumbleSpeed = default;

    // Time from contact with player till platform respawn
    [SerializeField] private float timeTillRespawn = default;

    private SpriteRenderer spriteRenderer = default;

    private BoxCollider2D boxCollider = default;

    [SerializeField] private Vector3[] positions = default;

    [SerializeField] private int nextPosition = 0;

    [SerializeField] private float speed = default;

    [SerializeField] public bool reverse = false;

    public bool crumbling = false;

    private Color defaultColor = default;
    private Color invisible = default;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        defaultColor = spriteRenderer.color;
        invisible = defaultColor;
        invisible.a = 0;
    }

    private void FixedUpdate()
    {

        if (crumbling)
        {
            if (spriteRenderer.color.a > 0)
                spriteRenderer.color = new Color(
                    defaultColor.r,
                    defaultColor.g,
                    defaultColor.b,
                    spriteRenderer.color.a - crumbleSpeed * Time.deltaTime
                );
            else
            {
                transform.DetachChildren();
                boxCollider.enabled = false;
                spriteRenderer.enabled = false;
                crumbling = false;
                StartCoroutine(RespawnTimer(timeTillRespawn));
            }

        }

        transform.position = Vector3.MoveTowards(transform.position, positions[nextPosition], speed * Time.deltaTime);

        if (transform.position == positions[nextPosition])
        {
            if (!reverse)
                nextPosition = (nextPosition + 1 < positions.Length) ? nextPosition + 1 : 0;
            else
                nextPosition = (nextPosition - 1 >= 0) ? nextPosition - 1 : positions.Length - 1;
        }
    }

    // Timer before crumble
    private IEnumerator RespawnTimer(float time)
    {
        yield return new WaitForSeconds(time);
        boxCollider.enabled = true;
        spriteRenderer.enabled = true;
        spriteRenderer.color = defaultColor;
    }
}
