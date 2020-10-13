using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCam : MonoBehaviour
{
    [SerializeField] private float followspeed = 1f;
    private Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followspeed);
    }
}
