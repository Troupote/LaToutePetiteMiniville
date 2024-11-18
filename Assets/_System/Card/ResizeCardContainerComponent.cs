using UnityEngine;
using UnityEngine.UI;

public class ResizeCardContainerComponent : MonoBehaviour
{
    [SerializeField]
    private HorizontalLayoutGroup _horizontaldLayout;

    public void Resize()
    {
        // Compter le nombre d'enfants actifs
        int count = 0;
        foreach (Transform child in _horizontaldLayout.transform)
        {
            if (child.gameObject.activeSelf)
            {
                count++;
            }
        }

        /* Calculer le nouvel espacement
        if (count > 0) // Éviter la division par zéro
        {
            float newSpacingX = (1200 - (120 * count)) / (float)count;
            _horizontaldLayout.spacing = newSpacingX; // Assigner un nouveau Vector2
        }
        */

        if (count > 9)
        {
            float newSpacingX = -200;
            _horizontaldLayout.spacing = (1200 - (120 * count)) / (float)count; // Assigner un nouveau Vector2
        }
    }
}
