using UnityEngine;

public class DiceManagerScript : MonoBehaviour
{
    public GameObject diceObject;
    public int nbDiceRolling = 0;
    public int resultFinal = 0;
    public bool diceLaunch = false;
    
    public void create_dice(int nbDice)
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
