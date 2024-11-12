using UnityEngine;

public class DiceScript : MonoBehaviour
{
    static Rigidbody rb;

    public static Vector3 diceVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        diceVelocity = rb.linearVelocity;

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            float dirX = Random.Range(0, 100);
            float dirY = Random.Range(0, 100);
            float dirZ = Random.Range(0, 100);
            transform.position = new Vector3(0, 3, 0);
            rb.AddForce(transform.up * 100);
            rb.AddTorque(dirX, dirY, dirZ);
        }
    }
}
