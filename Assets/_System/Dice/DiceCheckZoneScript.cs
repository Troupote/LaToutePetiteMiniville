using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NewEmptyCSharpScript : MonoBehaviour 
{
    Vector3 diceVelocity;
    public TextMeshPro diceText;

    void FixedUpdate()
    {
        diceVelocity = DiceScript.diceVelocity;
    }

    void OnTriggerStay(Collider col)
    {
        if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
        {
            switch (col.gameObject.name)
            {
                case "side1":
                    diceText.text = "6";
                    break;
                case "side2":
                    diceText.text = "5";
                    break;
                case "side3":
                    diceText.text = "4";
                    break;
                case "side4":
                    diceText.text = "3";
                    break;
                case "side5":
                    diceText.text = "2";
                    break;
                case "side6":
                    diceText.text = "1";
                    break;
            }

        }
    }
}
