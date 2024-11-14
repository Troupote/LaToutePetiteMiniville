using UnityEngine;

public class DiceManagerScript : MonoBehaviour
{
    public int nbDice = 2;
    public GameObject diceObject;
    public void create_dice()
    {   
        for (int i = 0; i < nbDice; i++) 
        {
            Instantiate(diceObject);
          
        }
    }
}
