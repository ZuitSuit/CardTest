using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDropField : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{

    CardContainer currentCard;
    public RectTransform rectTransform;
    [SerializeField] Color highlightedColor;
    [SerializeField] Image background;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //&& player.TryUseMana(card.data.Energy)
            if (eventData.pointerDrag.TryGetComponent(out CardContainer card) )
            {


                card.ToggleUsed(true); //prevent dragging and stuf
                currentCard = card;
                float time = .3f;
                LeanTween.rotateZ(card.gameObject, Random.Range(-350f, 350f), time);
                LeanTween.move(card.gameObject, transform.position, time).setEaseInSine();
                LeanTween.scale(card.gameObject, Vector3.zero, time);
                LeanTween.delayedCall(time, UseUpCard);
         
            }
        }
    }



    public void UseUpCard()
    {
        currentCard.Die();
        currentCard = null;
    }

    public void ToggleHighlight(bool toggle)
    {
        background.color = toggle ? highlightedColor : Color.clear;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleHighlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        ToggleHighlight(false);
    }
}
