using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class ItemUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    Vector3 StartingPos;
    public GameObject OccupyingSlot;
    CanvasGroup CanvasGroup;
    public string ItemName;

    void Start()
    {
        OccupyingSlot = transform.parent.gameObject;
        UpdateStartingPos();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    //void Update()
    //{
    //    CanvasGroup.blocksRaycasts = Manager.instance.isDraggingUI ? false : true;
    // }

    public void Drag()
    {
        transform.position = Input.mousePosition;
    }

    public void ReturnToOriginalPosition()
    {
        transform.position = StartingPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Manager.instance.isDraggingUI = true;
        OccupyingSlot.GetComponent<SlotUI>().CurrentOccupant = null;
        CanvasGroup.blocksRaycasts = false;
        transform.parent = GetComponentInParent<Canvas>().transform;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Manager.instance.isDraggingUI = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void UpdateStartingPos()
    {
        StartingPos = transform.position;
    }

    // WARNING:
    // WITH THIS APPROACH, ITEM ICONS MUST BE STRICTLY SQUARE SHAPED AND FILL IN THE ENTIRY UI SLOT

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            // item place swapping:
            // - when you drag and drop another item on me
            //      - I will cache your info
            //      - Place you in my place
            //      - Use your cached info to go to your previous place

            Debug.Log("dropped into another item");

            GameObject TempOccupyingSlot = eventData.pointerDrag.GetComponent<ItemUI>().OccupyingSlot;

            eventData.pointerDrag.transform.parent = OccupyingSlot.transform;
            eventData.pointerDrag.GetComponent<ItemUI>().OccupyingSlot = OccupyingSlot;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;


            //eventData.pointerDrag.transform.parent = OccupyingSlot.transform;
            //eventData.pointerDrag.GetComponent<ItemUI>().UpdateStartingPos();

            //Debug.Log(TempOccupyingSlot.name);

            transform.parent = TempOccupyingSlot.transform;
            OccupyingSlot = TempOccupyingSlot;
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            OccupyingSlot.GetComponent<SlotUI>().CurrentOccupant = gameObject;
            eventData.pointerDrag.GetComponent<ItemUI>().OccupyingSlot.GetComponent<SlotUI>().CurrentOccupant = eventData.pointerDrag;

            //GameObject temp = OccupyingSlot;
            //OccupyingSlot = eventData.pointerDrag.GetComponent<ItemUI>().OccupyingSlot;
            //eventData.pointerDrag.GetComponent<ItemUI>().OccupyingSlot = temp;

            //UpdateStartingPos();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Manager.instance.FloatingTextContent.text = ItemName;
        Manager.instance.FloatingTextUI.SetActive(true);
        Manager.instance.FloatingTextUI.transform.position = transform.position + Manager.instance.FloatingTextUIOffset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Manager.instance.FloatingTextUI.SetActive(false);
    }
}
