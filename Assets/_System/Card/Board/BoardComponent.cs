using UnityEngine;
using System.Collections;

public class BoardComponent : MonoBehaviour
{
    [SerializeField]
    private GameRunner _gameRunner = null;

    [SerializeField]
    private RectTransform _container = null;

    [SerializeField]
    private BoardCardComponent _boardCardPrefab = null;

    [SerializeField]
    private float _delayBetweenCards = 0.1f; 

    private void Start()
    {
        StartCoroutine(DisplayCardsOneByOne());
    }

    private IEnumerator DisplayCardsOneByOne()
    {
        foreach (BuildingSO asset in _gameRunner.Config.CardsArchetypes)
        {
            BoardCardComponent boardCard = Instantiate(_boardCardPrefab, _container);
            boardCard.SetBoardCard(asset);

            yield return new WaitForSeconds(_delayBetweenCards);
        }
    }
}
