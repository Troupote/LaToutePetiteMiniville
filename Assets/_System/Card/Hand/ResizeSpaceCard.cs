using UnityEngine;
using UnityEngine.UI;

public class ResizeSpaceCard : MonoBehaviour
{
    [SerializeField]
    private HorizontalLayoutGroup _gridLayout;

    public void Resize()
    {
        // Compter le nombre d'enfants actifs
        int count = 0;
        foreach (Transform child in _gridLayout.transform)
        {
            if (child.gameObject.activeSelf)
                count++;
        }

        // Calculer le nouvel espacement
        if (count > 0) // Éviter la division par zéro
        {
            float newSpacingX = (1200 - (120 * count)) / (float)count;
            _gridLayout.spacing = newSpacingX; // Assigner un nouveau Vector2
        }
        else if (count == 9)
        {
            float newSpacingX = -200;
            _gridLayout.spacing =newSpacingX; // Assigner un nouveau Vector2


        }
    }
}
