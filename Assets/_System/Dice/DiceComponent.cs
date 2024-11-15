using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DiceComponent : MonoBehaviour
{
    static Rigidbody rb;
    float elapsedTime = 0;
    float ratio = 0;

    [SerializeField]
    private Material _mainMaterial = null;

    [SerializeField]
    private Material _secondaryMaterial = null;

    [SerializeField]
    private float _duration;

    private Coroutine _fadeDiceCoroutine = null;

    private void Start()
    {
        _mainMaterial.color = new Color(1,1,1, 1);
        _secondaryMaterial.color = new Color(0, 0, 0, 1);
        rb = GetComponent<Rigidbody>();
        float dirX = Random.Range(0, 100);
        float dirY = Random.Range(0, 100);
        float dirZ = Random.Range(0, 100);
        transform.position = new Vector3(Random.Range(-3, 3), Random.Range(2, 3), Random.Range(-3, 3));
        rb.AddForce(transform.up * 100);
        rb.AddTorque(dirX, dirY, dirZ);
        rb.linearVelocity = new Vector3(Random.Range(-3, 3), Random.Range(2, 3), Random.Range(-3, 3));
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.x == 0f && rb.linearVelocity.y == 0f && rb.linearVelocity.z == 0f)
        {
            _mainMaterial.SetFloat("_SurfaceType", 3.0f);
            ratio = elapsedTime / _duration;
            elapsedTime += Time.deltaTime;
            _mainMaterial.color = new Color(_mainMaterial.color.r, _mainMaterial.color.g, _mainMaterial.color.b, 1 - ratio);
            _secondaryMaterial.color = new Color(_secondaryMaterial.color.r, _secondaryMaterial.color.g, _secondaryMaterial.color.b, 1 - ratio);
            if (ratio > 1)
            {
                Destroy(gameObject);
            }
        }
    }
    

}
