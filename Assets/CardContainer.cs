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
    }



    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        if (pointerEventData.button != PointerEventData.InputButton.Left) return;

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
        transform.localPosition = Vector3.zero;

        canvasGroup.alpha = 1f;
        //deactivate drag component
    }



    public void OnDrag(PointerEventData eventData)
    {

        transform.position = Input.mousePosition;
    }

    public void DisableCard()
    {
        used = true;
    }

}
