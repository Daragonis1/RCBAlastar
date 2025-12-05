using UnityEngine;

public class HandView : LocationView
{
    [Header("Hand Layout Settings")]
    public GameObject cardObjectPrefab;
    public Transform container;

    public float cardWidth = 1f;
    public float spacing = 0.2f;
    public float baseScale = 0.9f;
    public bool leftAligned = true;

    // Global offset for the whole hand (appliqué à toutes les cartes)
    [Header("Hand Global Offset")]
    [Tooltip("Décalage en X appliqué à toute la main")]
    public float handOffsetX = 0f;
    [Tooltip("Décalage en Y appliqué à toute la main")]
    public float handOffsetY = 0f;

    protected override void OnCardAdded(Card card)
    {
        if (container == null || cardObjectPrefab == null)
        {
            Debug.LogWarning("HandView: container ou prefab manquant.");
            return;
        }

        var go = Instantiate(cardObjectPrefab, container);
        var cardObj = go.GetComponent<CardObject>();
        if (cardObj != null)
        {
            int sorting = ComputeSortingOrder(card);
            cardObj.Init(card, sorting);
        }

        cardToGameObject[card] = go;
        LayoutCards();
    }

    protected override void OnCardRemoved(Card card)
    {
        base.OnCardRemoved(card);  
        LayoutCards();
    }

    private void LayoutCards()
    {
        if (boundLocation == null) return;

        int count = boundLocation.Cards.Count;

        for (int i = 0; i < count; i++)
        {
            Card c = boundLocation.Cards[i];
            if (!cardToGameObject.TryGetValue(c, out var go) || go == null)
                continue;

            float x = leftAligned
                ? i * (cardWidth + spacing)
                : -((count - 1) * (cardWidth + spacing)) / 2f + i * (cardWidth + spacing);

            // Apply global hand offset to each card
            go.transform.localPosition = new Vector3(x + handOffsetX, handOffsetY, 0f);
            go.transform.localScale = Vector3.one * baseScale;

            // Update sorting order according to current index/location
            var cardObj = go.GetComponent<CardObject>();
            if (cardObj != null)
            {
                int sorting = ComputeSortingOrder(c);
                cardObj.ApplyLocalSorting(sorting);
            }
        }
    }

    // Calcule un sorting order en fonction de la location et de l'index dans la location.
    // Exemple: Hand => base 200 + 10 * index
    private int ComputeSortingOrder(Card card)
    {
        // détermine la location à utiliser (préfère la position du modèle, sinon la boundLocation)
        Location loc = card.Position ?? boundLocation;

        LocationType locType = loc != null ? loc.Type : LocationType.Hand;

        int index = -1;
        if (loc != null)
            index = loc.IndexOf(card);
        if (index < 0)
        {
            // fallback : recherche manuelle si nécessaire
            var list = loc?.Cards;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == card)
                    {
                        index = i;
                        break;
                    }
                }
            }
        }
        if (index < 0) index = 0;

        int baseOrder;
        switch (locType)
        {
            case LocationType.Deck:
                baseOrder = 100;
                break;
            case LocationType.Hand:
                baseOrder = 200;
                break;
            case LocationType.Battlefield:
                baseOrder = 300;
                break;
            case LocationType.Show:
                baseOrder = 400;
                break;
            case LocationType.Graveyard:
                baseOrder = 50;
                break;
            default:
                baseOrder = 0;
                break;
        }

        return baseOrder + 10 * index;
    }
}
