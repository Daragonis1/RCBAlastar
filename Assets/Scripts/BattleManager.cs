using System;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public PlayerController Player1 { get; private set; }
    public PlayerController Player2 { get; private set; }
    public PlayerController ActivePlayer { get; private set; }
    public PlayerController InactivePlayer => (ActivePlayer == Player1) ? Player2 : Player1;
    public CardDatabase cardDatabase;
    public GameObject cardObjectPrefab;
    public event Action OnBattleStarted;

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

    public void StartBattle()
    {
        CreatePlayers();
        SetupPlayersDecks();
        GiveStartingHand();
        GiveInitialMana();
        ActivePlayer = Player1;
        OnBattleStarted?.Invoke();
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
        for (int i = 0; i < 15; i++)
        {
            Card newCard = CreateCardByName("Elven Archer");
            if (newCard != null)
                Player1.Deck.AddCard(newCard);
        }
        Debug.Log("Nombre de carte dans deck Player 1 : " + Player1.Deck.Cards.Count);

        // Mélange du deck
        Player1.Deck.Shuffle();
    }

    private void GiveStartingHand()
    {
        Debug.Log("Pioche de la main initiale du Player 1...");

        var drawn = Player1.Deck.Draw(8);

        foreach (var card in drawn)
        {
            Player1.Hand.AddCard(card);
        }
        Debug.Log("Cartes dans la main du Player 1 : " + Player1.Hand.Cards.Count);
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
        // Untap / Ready all cards
        ActivePlayer.UntapAll();
        PhaseDraw();
    }

    private void PhaseDraw()
    {
        ActivePlayer.DrawCard();
        PhaseMain();
    }

    private void PhaseMain()
    {
        // Plus tard : actions, effets, combats
        // Pour l’instant : rien
        //SpawnTestCard("Elven Archer");
    }
    private void PhaseFinishMain()
    {
        // Actions de fin de phase plus tard
        PhaseEnd();
    }

    private void PhaseEnd()
    {
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
    private Card CreateCardByName(string cardName)
    {
        Debug.Log($"Création de la carte '{cardName}'...");
        var data = cardDatabase.GetCardByName(cardName);
        if (data == null)
        {
            Debug.LogError($"Card '{cardName}' not found in CardDatabase.");
            return null;
        }

        return new Card(data);
    }

}
