using UnityEngine;

public class ButtonMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private float amplitude = 50;
    private Vector3 target1;
    private Vector3 target2;
    private Vector3 currentTarget;
    private float baseY;

    private void Start()
    {
        baseY = transform.position.y;
        target1 = new Vector3(transform.position.x, transform.position.y - amplitude, transform.position.z);
        target2 = new Vector3(transform.position.x, transform.position.y + amplitude, transform.position.z);
        currentTarget = target1;
    }
    void Update()
    {

        if (transform.position.y > baseY + amplitude - (amplitude*0.1))
        {
            currentTarget = target1;
                
        }
        if (transform.position.y < baseY - amplitude + (amplitude * 0.1))
        {
            currentTarget = target2;
        }
        
        transform.position = Vector3.Lerp(transform.position,currentTarget, _speed*Time.deltaTime);
    }
}
