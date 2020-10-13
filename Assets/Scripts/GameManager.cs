using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Time till the first camera change
    [SerializeField] private float timeTillInitialCameraChange = default;

    // Cooldown between camera changes
    [SerializeField] private float timeTillCameraChange = default;

    // Time till the first background change
    [SerializeField] private float timeTillInitialBackgroundChange = default;

    // Cooldown between background changes
    [SerializeField] private float timeTillBackgroundChange = default;

    // Time till the first circle change
    [SerializeField] private float timeTillInitialCircleChange = default;

    // Cooldown between circle changes
    [SerializeField] private float timeTillCircleChange = default;

    // Should the camera be changed
    private bool changeCamera = false;

    // Should the background be changed
    private bool changeBackground = false;

    // Should the circle be changed
    private bool changeCircle = false;

    // is the circle rotating independently of the background
    private bool circlesIndependent = false;

    // Rotating camera
    [SerializeField] private RotatingObject cameraRotatingObject = default;

    // Rotating background
    [SerializeField] private RotatingObject backgroundRotatingObject = default;

    // Rotating circle
    [SerializeField] private RotatingObject circleRotatingObject = default;

    // Rotating inner circles
    [SerializeField] private RotatingObject[] innerCirclesRotatingObject = default;

    // Oppossite otating inner circle
    [SerializeField] private RotatingObject innerCircleOppositeRotatingObject = default;

    private Coroutine backgroundRotationCoroutine = default;
    private Coroutine cameraRotationCoroutine = default;

    [SerializeField] private Platform[] platforms = default;

    private bool reversePlatforms = default;

    [SerializeField] private float timeTillInitialReversal = default;
    [SerializeField] private float timeTillReversal = default;

    private ScoreCounter scoreCounter = default;

    // Start is called before the first frame update
    void Start()
    {
        scoreCounter = FindObjectOfType<ScoreCounter>();
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;
        
        scoreCounter.ResetScore();
        scoreCounter.StartAddingToScore();

        cameraRotationCoroutine = StartCoroutine(CountdownTillCameraChange(timeTillInitialCameraChange));
        backgroundRotationCoroutine = StartCoroutine(CountdownTillBackgroundChange(timeTillInitialBackgroundChange));
        StartCoroutine(CountdownTillCircleChange(timeTillInitialCircleChange));
        StartCoroutine(CountdownTillReversal(timeTillInitialReversal));
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && Input.GetKey(KeyCode.Return))
            StartGame();

        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        if (Input.GetKey(KeyCode.Escape))
        {
            GoToMainMenu();
            return;
        }

        if (reversePlatforms)
        {
            if (Random.Range(0, 1000) <= 40)
            {
                foreach (Platform platform in platforms)
                {
                    platform.reverse = !platform.reverse;
                }
            }

            StartCoroutine(CountdownTillReversal(timeTillReversal));
        }

        if (!circlesIndependent && changeCircle)
        {
            int rotation = Random.Range(-1, 2);
            ChangeRotation(circleRotatingObject, rotation);
            ChangeRotation(innerCircleOppositeRotatingObject, GetOppositeDirection(rotation));
            StartCoroutine(CountdownTillCircleChange(timeTillCircleChange));
        }

        if (changeCamera || changeBackground)
        {
            bool syncUp = Random.Range(0, 9000) % 2 == 0;

            int? cameraRotation = null;
            int? backgroundRotation = null;

            if (syncUp)
            {
                int program = Random.Range(0, 9000) % 2;

                int newDirection = Random.Range(-1, 2);

                if (program == 0)
                {
                    cameraRotation = newDirection;
                    backgroundRotation = newDirection;
                }
                else {
                    cameraRotation = newDirection;
                    backgroundRotation = GetOppositeDirection(newDirection);
                }
            }

            if (changeCamera || syncUp)
            {
                cameraRotation = (cameraRotation == null) ? Random.Range(-1, 2) : cameraRotation;
                ChangeRotation(cameraRotatingObject, cameraRotation);
                StopCoroutine(cameraRotationCoroutine);
                cameraRotationCoroutine = StartCoroutine(CountdownTillCameraChange(timeTillCameraChange));

                int oppositeDirection = cameraRotation ?? 0;
                oppositeDirection = GetOppositeDirection(oppositeDirection);

                foreach (RotatingObject rotatingObject in innerCirclesRotatingObject)
                {
                    ChangeRotation(rotatingObject, oppositeDirection);
                }
            }
            if (changeBackground || syncUp)
            {
                ChangeRotation(backgroundRotatingObject, backgroundRotation);
                StopCoroutine(backgroundRotationCoroutine);
                backgroundRotationCoroutine = StartCoroutine(CountdownTillBackgroundChange(timeTillBackgroundChange));

                if (!circlesIndependent)
                    ChangeRotation(circleRotatingObject, backgroundRotation);
            }
        }
    }

    private int GetOppositeDirection(int direction)
    {
        switch (direction)
        {
            case -1:
                return 2;
            case 0:
                return 1;
            case 1:
                return 0;
            default:
                return -1;
        }
    }

    // Change rotation
    private void ChangeRotation(RotatingObject rotatingObject, int? newAngle = null)
    {
        newAngle = (newAngle == null) ? Random.Range(-1, 2) : newAngle;

        switch (newAngle)
        {
            case -1:
                rotatingObject.rotationDegrees = -90;
                break;
            case 0:
                rotatingObject.rotationDegrees = 0;
                break;
            case 1:
                rotatingObject.rotationDegrees = (Random.Range(0, 9000) % 2 == 0) ? 180 : -180;
                break;
            default:
                rotatingObject.rotationDegrees = 90;
                break;
        }
    }

    // Countdown till camera change
    private IEnumerator CountdownTillCameraChange(float time)
    {
        changeCamera = false;
        yield return new WaitForSeconds(time);
        changeCamera = true;
    }

    // Countdown till background change
    private IEnumerator CountdownTillBackgroundChange(float time)
    {
        changeBackground = false;
        yield return new WaitForSeconds(time);
        changeBackground = true;
    }

    // Countdown till circle change
    private IEnumerator CountdownTillCircleChange(float time)
    {
        changeCircle = false;
        yield return new WaitForSeconds(time);
        changeCircle = true;
        circlesIndependent = false;
    }

    // Countdown till reversals
    private IEnumerator CountdownTillReversal(float time)
    {
        reversePlatforms = false;
        yield return new WaitForSeconds(time);
        reversePlatforms = true;
    }

    // StartGame
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Go to main menu
    public void GoToMainMenu()
    {
        scoreCounter.StopAddingToScore();
        scoreCounter.UpdateScores();
        SceneManager.LoadScene(0);
    }
}
