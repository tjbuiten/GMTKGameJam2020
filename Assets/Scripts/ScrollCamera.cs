using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCamera : MonoBehaviour
{
    [SerializeField] public int direction = default;
    [SerializeField] private float horizontalScrollSpeed = default;
    [SerializeField] private float verticalScrollSpeed = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveInDirection(direction);
    }

    private void MoveInDirection(int direction)
    {
        float horizontalSpeed = 0;
        float verticalSpeed = 0;

        switch (direction)
        {
            case -1:
                horizontalSpeed = -horizontalScrollSpeed;
                break;
            case 0:
                verticalSpeed = -verticalScrollSpeed;
                break;
            case 1:
                verticalSpeed = verticalScrollSpeed;
                break;
            default:
                horizontalSpeed = horizontalScrollSpeed;
                break;
        }

        transform.Translate(new Vector3(horizontalSpeed, verticalSpeed, 0) * Time.deltaTime);
    }
}
