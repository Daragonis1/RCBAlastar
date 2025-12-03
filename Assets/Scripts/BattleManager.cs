using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public PlayerController Player1 { get; private set; }
    public PlayerController Player2 { get; private set; }
    public PlayerController ActivePlayer { get; private set; }
    public PlayerController InactivePlayer => (ActivePlayer == Player1) ? Player2 : Player1;

    // ----- Added for test card generation -----
    // Drag a CardDatabase asset here in the inspector (optional: used to lookup by name)
    public CardDatabase cardDatabase;
    // Optional prefab containing a CardObject component to visualise the generated card
    public GameObject cardObjectPrefab;
    // ------------------------------------------

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartBattle();
    }

    /// <summary>
    /// Initialise une nouvelle bataille.
    /// </summary>
    public void StartBattle()
    {
        Debug.Log("Initialisation de la bataille…");

        CreatePlayers();
        SetupPlayersDecks();
        GiveStartingHand();
        GiveInitialMana();

        ActivePlayer = Player1; // Pour l’instant Player 1 commence

        PhaseBegin();
    }

    // --------------------------------------------------------------
    // ----------------------- INIT PHASE ---------------------------
    // --------------------------------------------------------------

    private void CreatePlayers()
    {
        Player1 = new PlayerController("Player 1");
        Player2 = new PlayerController("Player 2");

        Player1.InitializeLocations();
        Player2.InitializeLocations();
    }

    private void SetupPlayersDecks()
    {
        // ❗ A remplacer plus tard par une construction de deck réelle
        //Player1.Deck.FillWithDebugCards();
        //Player2.Deck.FillWithDebugCards();

        Player1.Deck.Shuffle();
        Player2.Deck.Shuffle();
    }

    private void GiveStartingHand()
    {
        for (int i = 0; i < 5; i++)
        {
            Player1.DrawCard();
            Player2.DrawCard();
        }
    }

    private void GiveInitialMana()
    {
        Player1.Mana = 1;
        Player2.Mana = 1;
    }

    // --------------------------------------------------------------
    // -------------------------- PHASES ----------------------------
    // --------------------------------------------------------------

    private void PhaseBegin()
    {
        Debug.Log($"[Beginning Phase] - {ActivePlayer.Name}");
        // Untap / Ready all cards
        ActivePlayer.UntapAll();
        PhaseDraw();
    }

    private void PhaseDraw()
    {
        Debug.Log($"[Draw Phase] - {ActivePlayer.Name}");
        ActivePlayer.DrawCard();
        PhaseMain();
    }

    private void PhaseMain()
    {
        Debug.Log($"[Main Phase] - {ActivePlayer.Name}");
        // Plus tard : actions, effets, combats
        // Pour l’instant : rien
        //SpawnTestCard("Elven Archer");
    }
    private void PhaseFinishMain()
    {
        Debug.Log($"[Finish Main Phase] - {ActivePlayer.Name}");
        // Actions de fin de phase plus tard
        PhaseEnd();
    }

    private void PhaseEnd()
    {
        Debug.Log($"[End Phase] - {ActivePlayer.Name}");
        // Actions de fin de tour plus tard
        SwitchActivePlayer();
        PhaseBegin();
    }

    // --------------------------------------------------------------
    // -------------------------- UTILS -----------------------------
    // --------------------------------------------------------------

    private void SwitchActivePlayer()
    {
        ActivePlayer = InactivePlayer;
    }

    private void CheckVictory()
    {
        if (Player1.Points >= 10)
            EndBattle(Player1);

        if (Player2.Points >= 10)
            EndBattle(Player2);
    }

    private void EndBattle(PlayerController winner)
    {
        Debug.Log($"La bataille est terminée ! Vainqueur : {winner.Name}");
    }

    /// <summary>
    /// Génère une carte pour tests :
    /// - récupère les CardData (par nom si fourni, sinon carte aléatoire si cardDatabase set)
    /// - construit l'objet Card
    /// - (optionnel) l'ajoute à la Location fournie
    /// - (optionnel) instancie cardObjectPrefab et initialise son UI via CardObject.Init
    /// </summary>
    public Card SpawnTestCard(string cardName = null, Location targetLocation = null)
    {
        CardData data = null;

        if (!string.IsNullOrEmpty(cardName))
        {
            if (cardDatabase != null)
                data = cardDatabase.GetCardByName(cardName);
            if (data == null)
                Debug.LogWarning($"SpawnTestCard: card '{cardName}' not found in cardDatabase.");
        }

        if (data == null && cardDatabase != null)
        {
            data = cardDatabase.GetRandomCard();
        }

        if (data == null)
        {
            Debug.LogError("SpawnTestCard: No CardData available (no cardDatabase set or empty).");
            return null;
        }

        var card = new Card(data);

        // Try to place in target location (Location.AddCard expected in project)
        if (targetLocation != null)
        {
            try
            {
                targetLocation.AddCard(card);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"SpawnTestCard: failed to add card to location: {ex.Message}");
            }
        }

        // Instantiate visual if prefab assigned
        if (cardObjectPrefab != null)
        {
            var go = Instantiate(cardObjectPrefab);
            var cardObj = go.GetComponent<CardObject>();
            if (cardObj != null)
            {
                cardObj.Init(card);
            }
            else
            {
                Debug.LogWarning("SpawnTestCard: cardObjectPrefab has no CardObject component.");
            }
        }

        Debug.Log($"SpawnTestCard: generated '{card.Name}' (Type: {card.Type})");
        return card;
    }
}
