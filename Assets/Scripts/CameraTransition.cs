using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    // Code attributed to Google Gemini
    [Header("Inscribed")]
    public Transform isometricView;
    public Transform firstPersonView;
    public float smoothTime = 2f;
    [Header("Dynamic")]
    public bool isFirstPerson = false;
    public Vector3 velocity = Vector3.zero;

    void Update() {
        // Determine the target position and rotation based on the current view
        Vector3 targetPosition;
        Quaternion targetRotation;

        if (isFirstPerson) {
            targetPosition = firstPersonView.position;
            targetRotation = firstPersonView.rotation;
        } else {
            targetPosition = isometricView.position;
            targetRotation = isometricView.rotation;
        }

        // Smoothly move and rotate the camera
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 - Mathf.Pow(0.001f, Time.deltaTime / smoothTime)); // Using Slerp for rotation

        // Alternatively, use Lerp for position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / smoothTime);
    }

    public void CameraSwitch() {
        isFirstPerson = !isFirstPerson;
    }
}
