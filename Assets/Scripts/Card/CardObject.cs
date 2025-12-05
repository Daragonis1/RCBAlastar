using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
{
    public Canvas textCanvas;
    [Header("Textes BIG")]
    public TextMeshProUGUI txtNameBig;
    public TextMeshProUGUI txtCostBig;
    public TextMeshProUGUI txtAttackBig;
    public TextMeshProUGUI txtHealthBig;

    [Header("Textes SHORT")]
    public TextMeshProUGUI txtNameShort;
    public TextMeshProUGUI txtCostShort;
    public TextMeshProUGUI txtAttackShort;
    public TextMeshProUGUI txtHealthShort;

    [Header("Textes communs")]
    public TextMeshProUGUI txtType;
    public TextMeshProUGUI txtDescription;

    [Header("GameObjects / SpriteRenderers")]
    public SpriteRenderer Border;
    public SpriteRenderer Template;
    public SpriteRenderer Illustration_Big;
    public SpriteRenderer Illustration_Short;
    public SpriteRenderer DamageType_Big;
    public SpriteRenderer DamageType_Short;

    public bool IsShort { get; private set; }
    public Card RelatedCard { get; private set; }
    [Header("Internal layers order")]
    public int templateOrder = 1;
    public int artworkOrder = 0;
    public int textOrder = 2;
    public int borderOrder = 4;
    public int damagetypeOrder = 3;

    public void Init(Card card, int baseSortingOrder = 0)
    {
        RelatedCard = card;
        IsShort = false;
        ApplyLocalSorting(baseSortingOrder);
        RefreshUI();
    }
    public void ApplyLocalSorting(int baseOrder)
    {
        Border.sortingOrder = baseOrder + borderOrder;
        Template.sortingOrder = baseOrder + templateOrder;
        Illustration_Big.sortingOrder = baseOrder + artworkOrder;
        Illustration_Short.sortingOrder = baseOrder + artworkOrder;
        DamageType_Big.sortingOrder = baseOrder + damagetypeOrder;
        DamageType_Short.sortingOrder = baseOrder + damagetypeOrder;
        textCanvas.sortingOrder = baseOrder + textOrder;
    }
    public void RefreshUI()
    {
        if (IsShort)
        {
            RefreshShortUI();
            SetupShortSprites();
        }
        else
        {
            RefreshBigUI();
            SetupBigSprites();
        }
    }

    // -------------------------
    // UI TEXTES
    // -------------------------

    private void RefreshShortUI()
    {
        txtNameShort.text = RelatedCard.Name;
        txtCostShort.text = RelatedCard.Cost.ToString();
        txtAttackShort.text = RelatedCard.CurrentAttack.ToString();
        txtHealthShort.text = RelatedCard.CurrentHealth.ToString();

        txtType.text = "";
        txtDescription.text = "";
    }

    private void RefreshBigUI()
    {
        txtNameBig.text = RelatedCard.Name;
        txtCostBig.text = RelatedCard.Cost.ToString();
        txtAttackBig.text = RelatedCard.CurrentAttack.ToString();
        txtHealthBig.text = RelatedCard.CurrentHealth.ToString();
        txtDescription.text = RelatedCard.Description;

        switch (RelatedCard.Type)
        {
            case CardType.Unit:
                txtType.text = "Unit";
                break;

            case CardType.Spell:
                txtType.text = "Spell";
                break;

            case CardType.Hero:
                txtType.text = "Hero";
                break;
        }
    }

    // -------------------------
    // SPRITES (Border / Template / DamageType)
    // -------------------------

    private void SetupShortSprites()
    {
        // BORDER
        Border.sprite = LoadSprite("UI/Card/Border_UltraShort");

        // TEMPLATE
        Template.sprite = LoadSprite("UI/Card/AllyNeutral_UltraShort");

        // Illustration : short only when IsShort, big cleared
        Illustration_Short.sprite = LoadCardIllustration();
        Illustration_Big.sprite = null;

        // DAMAGE TYPE
        DamageType_Short.sprite = LoadSprite("UI/Card/DamageMelee");
        DamageType_Big.sprite = null;
    }

    private void SetupBigSprites()
    {
        // BORDER
        Border.sprite = LoadSprite("UI/Card/Border");

        // TEMPLATE
        switch (RelatedCard.Type)
        {
            case CardType.Unit:
                Template.sprite = LoadSprite("UI/Card/AllyNeutral");
                break;

            case CardType.Spell:
                Template.sprite = LoadSprite("UI/Card/Ability");
                break;

            case CardType.Hero:
                Template.sprite = LoadSprite("UI/Card/HeroNeutral");
                break;
        }

        // Illustration : big only when not short, short cleared
        Illustration_Big.sprite = LoadCardIllustration();
        Illustration_Short.sprite = null;

        // DAMAGE TYPE
        DamageType_Big.sprite = LoadSprite("UI/Card/DamageMelee");
        DamageType_Short.sprite = null;
    }

    // Charge l'illustration correspondant au type et au nom de la carte depuis Resources/CardImgDatabase/{Type}/{Name}
    private Sprite LoadCardIllustration()
    {
        if (RelatedCard == null)
            return null;

        string folder = RelatedCard.Type.ToString(); // "Unit", "Spell", "Hero"
        string path = $"CardImgDatabase/{folder}/{RelatedCard.Name}";
        Sprite s = Resources.Load<Sprite>(path);
        if (s == null)
            Debug.LogWarning("Illustration introuvable : Resources/" + path);
        return s;
    }

    // Utility loader
    private Sprite LoadSprite(string path)
    {
        Sprite s = Resources.Load<Sprite>(path);
        if (s == null)
            Debug.LogWarning("Sprite introuvable : Resources/" + path);

        return s;
    }
}
