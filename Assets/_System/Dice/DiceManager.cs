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
    
    public void CreateDice(int nbDice)
    {
        diceLaunch = true;
        nbDiceRolling = nbDice;
        for (int i = 0; i < nbDice; i++) 
        {
            Instantiate(diceObject);
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
