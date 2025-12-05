using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vue générique représentant une Location côté UI.
/// S'abonne aux événements de Location pour créer/détruire les éléments UI.
/// </summary>
public class LocationView : MonoBehaviour
{
    [Tooltip("Player index à suivre (1 ou 2) — utile si tu as plusieurs joueurs dans la scène)")]
    public int playerIndex = 1;

    [Tooltip("Type de Location que cette vue représente (Hand, Deck, ...)")]
    public LocationType locationType = LocationType.Hand;

    protected Location boundLocation;
    protected Dictionary<Card, GameObject> cardToGameObject = new();

    private void Awake()
    {
        TrySubscribe();
    }

    protected virtual void OnDisable()
    {
        if (BattleManager.Instance != null)
            BattleManager.Instance.OnBattleStarted -= BindToLocation;

        Unbind();
    }

    private void TrySubscribe()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStarted -= BindToLocation; // évite double binding
            BattleManager.Instance.OnBattleStarted += BindToLocation;

            Debug.Log($"[LocationView] Subscribed to OnBattleStarted ({gameObject.name})");
            BindToLocation();
        }
        else
        {
            // BattleManager pas encore prêt → on retente dans un frame
            Debug.Log($"[LocationView] BattleManager not ready, retry next frame… ({gameObject.name})");
            StartCoroutine(RetrySubscribeNextFrame());
        }
    }

    private System.Collections.IEnumerator RetrySubscribeNextFrame()
    {
        yield return null; // attendre 1 frame

        TrySubscribe(); // essayer à nouveau
    }

    protected virtual void BindToLocation()
    {
        Debug.Log($"[LocationView] BindToLocation called on {gameObject.name}");
        Unbind();

        PlayerController p = (playerIndex == 1) ? BattleManager.Instance.Player1 : BattleManager.Instance.Player2;
        Debug.Log($"[LocationView] Player resolved: {p}");
        if (p == null)
            return;

        // Sélectionner la Location demandée
        switch (locationType)
        {
            case LocationType.Hand:
                Debug.Log($"[LocationView] Binding to Hand of Player {playerIndex}");
                boundLocation = p.Hand;
                break;
            case LocationType.Deck:
                boundLocation = p.Deck;
                break;
            case LocationType.Battlefield:
                // si plusieurs battlefields, prendre 0 par défaut
                boundLocation = p.Battlefields != null && p.Battlefields.Length > 0 ? p.Battlefields[0] : null;
                break;
            case LocationType.Show:
                boundLocation = p.Show;
                break;
            default:
                // ajouter d'autres cas si nécessaire
                boundLocation = null;
                break;
        }

        if (boundLocation != null)
        {
            Debug.Log($"[LocationView] Bound to location: {boundLocation}");
            boundLocation.OnCardAdded += OnCardAdded;
            boundLocation.OnCardRemoved += OnCardRemoved;
            boundLocation.OnCleared += OnCleared;

            // Hydrate la vue avec les cartes déjà présentes
            foreach (var c in boundLocation.Cards)
            {
                Debug.Log($"[LocationView] Hydrating existing card: {c.Name}");
                OnCardAdded(c);
            }
        }
        else
        {
            Debug.LogWarning($"[LocationView] Could not bind to location of type {locationType} for Player {playerIndex}");
        }
    }

    protected virtual void Unbind()
    {
        if (boundLocation != null)
        {
            boundLocation.OnCardAdded -= OnCardAdded;
            boundLocation.OnCardRemoved -= OnCardRemoved;
            boundLocation.OnCleared -= OnCleared;
            boundLocation = null;
        }

        // supprimer objets UI existants
        foreach (var go in cardToGameObject.Values)
            if (go != null) Destroy(go);
        cardToGameObject.Clear();
    }

    protected virtual void OnCardAdded(Card card)
    {
        // méthode à surcharger : par défaut on ne fait rien
    }

    protected virtual void OnCardRemoved(Card card)
    {
        if (cardToGameObject.TryGetValue(card, out var go))
        {
            Destroy(go);
            cardToGameObject.Remove(card);
        }
    }

    protected virtual void OnCleared()
    {
        foreach (var go in cardToGameObject.Values)
            if (go != null) Destroy(go);
        cardToGameObject.Clear();
    }
}
