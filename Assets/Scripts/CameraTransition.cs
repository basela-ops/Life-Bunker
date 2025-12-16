using UnityEngine;
using Unity.Cinemachine;

public class CameraTransition : MonoBehaviour
{
    // Code attributed to Google Gemini and GitHub Copilot
    [Header("Inscribed")]
    public Transform isometricView;
    public Transform deskView;
    public float smoothTime = 2f;
    public float firstPersonViewOrthographicSize = 2f;
    [Header("Dynamic")]
    public float isometricViewOrthographicSize;
    public CinemachineCamera cinemachineCamera;
    public bool isFirstPerson = false;
    public Vector3 velocity = Vector3.zero;

    void Start() {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        isometricViewOrthographicSize = cinemachineCamera.Lens.OrthographicSize;
    }

    void Update() {
        // Determine the target position and rotation based on the current view
        Vector3 targetPosition;
        Quaternion targetRotation;

        if (isFirstPerson) {
            targetPosition = deskView.position;
            targetRotation = deskView.rotation;
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, firstPersonViewOrthographicSize, Time.deltaTime * smoothTime);
            cinemachineCamera.Follow = deskView;
        } else {
            targetPosition = isometricView.position;
            targetRotation = isometricView.rotation;
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, isometricViewOrthographicSize, Time.deltaTime * smoothTime);
            cinemachineCamera.Follow = isometricView;
        }

        // Smoothly move and rotate the camera
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 - Mathf.Pow(0.001f, Time.deltaTime / smoothTime)); // Using Slerp for rotation

        // Alternatively, use Lerp for position
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / smoothTime);
    }

    public void CameraSwitch() {
        isFirstPerson = !isFirstPerson;
    }
}
