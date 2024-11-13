using UnityEngine;

public class DiceScript : MonoBehaviour
{
    static Rigidbody rb;

    public static Vector3 diceVelocity;
    public bool isStop = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        float dirX = Random.Range(0, 100);
        float dirY = Random.Range(0, 100);
        float dirZ = Random.Range(0, 100);
        transform.position = new Vector3(Random.Range(-3, 3), Random.Range(2,3), Random.Range(-3,3));
        rb.AddForce(transform.up * 100);
        rb.AddTorque(dirX, dirY, dirZ);
        rb.linearVelocity = new Vector3(Random.Range(-3, 3), Random.Range(2, 3), Random.Range(-3, 3));
    }

    private void Update()
    {
        diceVelocity = rb.linearVelocity;
        if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f) 
        {
            isStop = true;
        }
    }
}
