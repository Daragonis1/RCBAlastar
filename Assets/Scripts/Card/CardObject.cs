using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtType;
    public TextMeshProUGUI txtCost;
    public TextMeshProUGUI txtAttack;
    public TextMeshProUGUI txtHealth;
    public TextMeshProUGUI txtDescription;

    public Card RelatedCard { get; private set; }

    public void Init(Card card)
    {
        RelatedCard = card;
        RefreshUI();
    }

    public void RefreshUI()
    {
        txtName.text = RelatedCard.Name;
        if(RelatedCard.Type == CardType.Unit)
            txtType.text = "Unit";
        else if(RelatedCard.Type == CardType.Spell)
            txtType.text = "Spell";
        else if(RelatedCard.Type == CardType.Hero)
            txtType.text = "Hero";
        txtCost.text = RelatedCard.Cost.ToString();
        txtAttack.text = RelatedCard.CurrentAttack.ToString();
        txtHealth.text = RelatedCard.CurrentHealth.ToString();
        txtDescription.text = RelatedCard.Description;
    }
}
