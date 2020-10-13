using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = default;
    public float rotationDegrees = default;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowards(rotationDegrees);
    }

    private void RotateTowards(float degrees)
    {

        //transform.rotation = Quaternion.Lerp(
        //    startPosition,
        //    Quaternion.Euler(
        //        transform.rotation.x,
        //        transform.rotation.y,
        //        degrees
        //    ),
        //    rotationSpeed * Time.deltaTime
        //);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(
                transform.rotation.x,
                transform.rotation.y,
                degrees
            ),
            rotationSpeed * Time.deltaTime);
    }
}
