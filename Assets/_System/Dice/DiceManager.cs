using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject diceObject;

    [HideInInspector]
    public int nbDiceRolling = 0;
    [HideInInspector]
    public int resultFinal = 0;
    [HideInInspector]
    public bool diceLaunch = false;

    [SerializeField]
    private Transform _diceParent;
    public void CreateDice(int nbDice)
    {
        diceLaunch = true;
        nbDiceRolling = nbDice;
        DiceCheckZoneScript.finalCount = 0;
        for (int i = 0; i < nbDice; i++) 
        {
            Instantiate(diceObject, _diceParent);
        }
    }
    public void Update()
    {
        if (diceLaunch && nbDiceRolling == 0)
        { 
            resultFinal = DiceCheckZoneScript.finalCount;
        }
    }
}
