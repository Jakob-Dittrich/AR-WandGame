using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public float moveSpeed = 0.1f; // Adjustable movement speed

    public void MoveLeft()
    {
        Vector3 moveVector = new Vector3(-moveSpeed * Time.deltaTime, 0, 0); // Only modify x
        transform.Translate(moveVector, Space.World);
    }

    public void MoveRight()
    {
        Vector3 moveVector = new Vector3(moveSpeed * Time.deltaTime, 0, 0); // Only modify x
        transform.Translate(moveVector, Space.World);
    }

    public void MoveUp()
    {
        Vector3 moveVector = new Vector3(0, 0, moveSpeed * Time.deltaTime); // Only modify z
        transform.Translate(moveVector, Space.World);
    }

    public void MoveDown()
    {
        Vector3 moveVector = new Vector3(0, 0, -moveSpeed * Time.deltaTime); // Only modify z
        transform.Translate(moveVector, Space.World);
    }
}
