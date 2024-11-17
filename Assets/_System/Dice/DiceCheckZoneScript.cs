using UnityEngine;

/// <summary>
/// @todo
/// </summary>

public class DiceCheckZoneScript : MonoBehaviour 
{

    [SerializeField]
    private DiceManagerScript _diceScript = null;
    public static int finalCount = 0;
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.GetComponentInParent<Rigidbody>().linearVelocity.x == 0f && col.gameObject.GetComponentInParent<Rigidbody>().linearVelocity.y == 0f && col.gameObject.GetComponentInParent<Rigidbody>().linearVelocity.z == 0f)
        {
            switch (col.gameObject.name)
            {
                case "side1":
                    finalCount += 6;
                    break;
                case "side2":
                    finalCount += 5;
                    break;
                case "side3":
                    finalCount += 4;
                    break;
                case "side4":
                    finalCount += 3;
                    break;
                case "side5":
                    finalCount += 2;
                    break;
                case "side6":
                    finalCount += 1;
                    break;
            }

            Destroy(col.gameObject);
            _diceScript.nbDiceRolling -= 1;

        }
    }
}
