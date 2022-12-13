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
    [SerializeField] CanvasGroup cardCanvasGroup, shineCanvasGroup;



    Card card;
    public Card data => card;
    bool used; //used when card is used
    public bool Used => used;

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

    public void ChangeValue(ValueType value, int changeBy)
    {
        used = true;

        switch (value)
        {
            case ValueType.Damage:
                card.Damage += changeBy;
                break;
            case ValueType.Energy:
                card.Energy += changeBy;
                break;
            case ValueType.Health:
            default:
                card.HP += changeBy;
                break;
        }

        Unparent();
    }


    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (pointerEventData.button != PointerEventData.InputButton.Left || GameManager.Instance.CurrentState == GameState.Busy) return;

        cardCanvasGroup.blocksRaycasts = false;

        Unparent();

        transform.rotation = Quaternion.identity;

        cardCanvasGroup.alpha = 0.8f;

        LeanTween.cancel(shineCanvasGroup.gameObject);
        LeanTween.value(shineCanvasGroup.gameObject, shineCanvasGroup.alpha, 1f, .2f)
    .setOnUpdate((float transparency) =>
    {
        shineCanvasGroup.alpha = transparency;

    });

        //TODO make transparent when dragging, circles around the slots (sphere collider)

        //activate component
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (used) return;

        cardCanvasGroup.blocksRaycasts = true;
        if (pointerEventData.button != PointerEventData.InputButton.Left) return;

        ParentBack();
        //transform.localPosition = Vector3.zero;

        cardCanvasGroup.alpha = 1f;
        //deactivate drag component

        LeanTween.cancel(shineCanvasGroup.gameObject);
        LeanTween.value(shineCanvasGroup.gameObject, shineCanvasGroup.alpha, 0f, .2f)
    .setOnUpdate((float transparency) =>
    {
        shineCanvasGroup.alpha = transparency;

    });
    }



    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState == GameState.Busy) return;
        transform.position = Input.mousePosition;
    }


    public void Unparent()
    {
        parentRect = transform.parent.GetComponent<RectTransform>();
        siblingIndex = transform.GetSiblingIndex();


        transform.SetParent(GameManager.Instance.CardCanvas);
    }

    public void ParentBack()
    {
        transform.SetParent(parentRect);
        transform.SetSiblingIndex(siblingIndex);
        used = false;
    }

    public void DisableCard()
    {
        used = true;
    }


    //UI stuff

    //basically the same method - ideally redo this
    public void UpdateDamage()
    {
        LeanTween.rotate(gameObject, Vector3.zero, .2f);
        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.move(gameObject, GameManager.Instance.MiddleOfTheScreen(), .3f));
        sequence.append(LeanTween.scale(damageText.gameObject, Vector3.one * 3f, .4f).setLoopPingPong(1));
        sequence.append(LeanTween.value(damageText.gameObject, int.Parse(damageText.text), card.Damage, 1f)
    .setOnUpdate((float newNumber) =>
    {
        damageText.text = Mathf.RoundToInt(newNumber).ToString();

    }));
        sequence.append(ParentBack);

    }
    public void UpdateEnergy()
    {
        LeanTween.rotate(gameObject, Vector3.zero, .2f);
        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.move(gameObject, GameManager.Instance.MiddleOfTheScreen(), .3f));
        sequence.append(LeanTween.scale(energyText.gameObject, Vector3.one * 3f, .4f).setLoopPingPong(1));
        sequence.append(LeanTween.value(energyText.gameObject, int.Parse(energyText.text), card.Energy, 1f)
    .setOnUpdate((float newNumber) =>
    {
        energyText.text = Mathf.RoundToInt(newNumber).ToString();

    }));
        sequence.append(ParentBack);

    }

    public void UpdateHP()
    {
        healthText.text = $"{card.HP}/{card.MaxHP}";
        LeanTween.cancel(healthFill.gameObject);

        LeanTween.rotate(gameObject, Vector3.zero, .2f);

        LTSeq sequence = LeanTween.sequence();

        sequence.append(LeanTween.move(gameObject, GameManager.Instance.MiddleOfTheScreen(), .3f));

        if (card.HP >= card.MaxHP)
        {
            sequence.append(LeanTween.scale(healthText.gameObject, Vector3.one * 2f, .4f).setLoopPingPong(1));
        }

        sequence.append(
        LeanTween.value(healthFill.gameObject, healthFill.fillAmount, card.HP / (float)card.MaxHP, 1f)
            .setOnUpdate((float newFill) =>
            {
                healthFill.fillAmount = newFill;

            }));



        if(card.HP <= 0)
        {
            //if hp is < 0 add die to sequence
            sequence.append(LeanTween.scale(gameObject, Vector3.zero, .3f).setEaseInElastic());
            sequence.append(SetUnused);
            sequence.append(.1f);
            sequence.append(Die);
        }
        else
        {
            sequence.append(ParentBack);
        }

    }
    public void SetUnused()
    {
        ToggleUsed(false);
    }
    public void ToggleUsed(bool toggle = false)
    {
        used = toggle;
    }

    public void Die()
    {
        
        GameManager.Instance.RemoveCard(this);
        Destroy(gameObject);
    }
    
}

public enum ValueType
{
    Health,
    Damage,
    Energy
}
