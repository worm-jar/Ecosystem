using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of movement
    public float lookSpeed = 2f; // Speed of mouse look
    public float minY = -90f; // Min vertical angle
    public float maxY = 90f; // Max vertical angle

    private float yaw = 0f; // Horizontal rotation
    private float pitch = 0f; // Vertical rotation

    void Update()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxis("Vertical"); // W/S or Up/Down
        float moveY = 0;

        if (Input.GetKey(KeyCode.E)) moveY = 1; // Move up
        if (Input.GetKey(KeyCode.Q)) moveY = -1; // Move down

        Vector3 move = transform.right * moveX + transform.up * moveY + transform.forward * moveZ;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

    }
}
