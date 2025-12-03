using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public PlayerController Player1 { get; private set; }
    public PlayerController Player2 { get; private set; }
    public PlayerController ActivePlayer { get; private set; }
    public PlayerController InactivePlayer => (ActivePlayer == Player1) ? Player2 : Player1;

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
}
