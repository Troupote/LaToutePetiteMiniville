using UnityEngine;

public class CardComponent : MonoBehaviour
{
    [SerializeField]
    private CardSO _cardSO;
    private string _name;
    private Color _color;
    private int _price;
    private int _activationNumber;

    public void Effect()
    {
        _cardSO.ApplyEffect();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
