using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject diceObject;
    public int nbDiceRolling = 0;
    public int resultFinal = 0;
    public bool diceLaunch = false;
    
    public void CreateDice(int nbDice)
    {
        diceLaunch = true;
        nbDiceRolling = nbDice;
        for (int i = 0; i < nbDice; i++) 
        {
            Debug.Log("Dice value");
            Instantiate(diceObject);
        }
    }
    public void Update()
    {
        if (diceLaunch && nbDiceRolling == 0)
            resultFinal = DiceCheckZoneScript.finalCount;
    }
}
