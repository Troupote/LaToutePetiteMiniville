using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CreditTextMove : MonoBehaviour
{
    public static string direction = "down";
    private float _speed = 5;

    private Vector3 _target1;
    private Vector3 _target2;
    private Vector3 _currentTarget;
    private void Start()
    {
        _target1 = new Vector3(600,-400,0);
        _target2 = new Vector3(600, 620,0);
        _currentTarget = _target1;
    }
    void Update()
    {
        if (direction == "down")
        {
            _currentTarget = _target1;
        }
        if (direction == "up")
        {
            _currentTarget = _target2;
        }

        transform.position = Vector3.Lerp(transform.position, _currentTarget, _speed * Time.deltaTime);
    }
}
