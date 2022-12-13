using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Field : MonoBehaviour, IDropHandler
{
    [SerializeField] TextMeshProUGUI damageDealt;
    CardContainer currentCard;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.TryGetComponent(out CardContainer card))
            {
                damageDealt.text = $"dealt { card.data.Damage} damage";

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
}
