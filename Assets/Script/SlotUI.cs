using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IDropHandler
{
    //public bool IsOccupied;
    public GameObject CurrentOccupant;
    CanvasGroup CanvasGroup;

    void Start()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
        if (transform.childCount == 1)
        {
            //GetComponent<CanvasGroup>().blocksRaycasts = false;
            UpdateOccupant();
            //IsOccupied = true;
        }
    }

    public void UpdateOccupant()
    {
        CurrentOccupant = transform.GetChild(0).gameObject;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.Log("dropped into slot");
            eventData.pointerDrag.transform.parent = transform;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            eventData.pointerDrag.GetComponent<ItemUI>().UpdateStartingPos();
            eventData.pointerDrag.GetComponent<ItemUI>().OccupyingSlot = gameObject;
            //GetComponent<CanvasGroup>().blocksRaycasts = false;

            //if (IsOccupied)
            //{
            //    CurrentOccupant.transform.parent = eventData.pointerDrag.GetComponent<ItemUI>().OccupyingSlot.transform;
            //    CurrentOccupant.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<ItemUI>().OccupyingSlot.GetComponent<RectTransform>().anchoredPosition;
            //    CurrentOccupant.GetComponent<ItemUI>().UpdateStartingPos();
            //}

            CurrentOccupant = eventData.pointerDrag;
            //IsOccupied = true;
        }
    }
}
