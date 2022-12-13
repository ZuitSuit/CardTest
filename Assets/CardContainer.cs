using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CardContainer : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] TextMeshProUGUI healthText, damageText, energyText;
    [SerializeField] Image healthFill, cardImage;
    [SerializeField] CanvasGroup canvasGroup;


    Card card;
    bool used; //used when card is used

    //drag reference to return the card to
    RectTransform parentRect;
    int siblingIndex;

    public void SetupCard(Card _card, Sprite sprite)
    {
        card = _card;
        cardImage.sprite = sprite;

        healthText.text = $"{ card.HP}/{ card.MaxHP}";
        damageText.text = card.Damage.ToString();
        energyText.text = card.Energy.ToString();

        card.HpChangedAction += UpdateHP;
        card.DamageChangedAction += UpdateDamage;
        card.EnergyChangedAction += UpdateEnergy;
    }




    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (pointerEventData.button != PointerEventData.InputButton.Left || GameManager.Instance.CurrentState == GameState.Busy) return;

        canvasGroup.blocksRaycasts = false;
        parentRect = transform.parent.GetComponent<RectTransform>();
        siblingIndex = transform.GetSiblingIndex();


        transform.SetParent(GameManager.Instance.CardCanvas);


        transform.rotation = Quaternion.identity;

        canvasGroup.alpha = 0.8f;

        //TODO make transparent when dragging, circles around the slots (sphere collider)

        //activate component
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (used) return;

        canvasGroup.blocksRaycasts = true;
        if (pointerEventData.button != PointerEventData.InputButton.Left) return;

        transform.SetParent(parentRect);
        transform.SetSiblingIndex(siblingIndex);
        //transform.localPosition = Vector3.zero;

        canvasGroup.alpha = 1f;
        //deactivate drag component
    }



    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState == GameState.Busy) return;
        transform.position = Input.mousePosition;
    }

    public void DisableCard()
    {
        used = true;
    }


    //UI stuff

    public void UpdateDamage()
    {
        LeanTween.value(damageText.gameObject, int.Parse(damageText.text), card.Damage, 1f)
    .setOnUpdate((float newNumber) =>
    {
        damageText.text = Mathf.CeilToInt(newNumber).ToString();

    });
    }
    public void UpdateEnergy()
    {
        LeanTween.value(damageText.gameObject, int.Parse(energyText.text), card.Energy, 1f)
    .setOnUpdate((float newNumber) =>
    {
        energyText.text = Mathf.CeilToInt(newNumber).ToString();

    });
    }
    public void UpdateHP()
    {
        healthText.text = $"{card.HP}/{card.MaxHP}";
        LeanTween.cancel(healthFill.gameObject);    
        
        LTSeq sequence = LeanTween.sequence();

        sequence.append(
        LeanTween.value(healthFill.gameObject, healthFill.fillAmount, card.HP / (float)card.MaxHP, 1f)
            .setOnUpdate((float newFill) =>
            {
                healthFill.fillAmount = newFill;

            }));

        if(card.HP <= 0)
        {
            sequence.append(LeanTween.scale(gameObject, Vector3.zero, .3f).setEaseInElastic());
            sequence.append(Die);
        }

        //if hp is < 0 add die to sequence
    }



    public void Die()
    {
        Destroy(gameObject);
    }
    
}
