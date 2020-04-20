using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableSprite : MonoBehaviour
{
    Vector3 pos;
    Vector3 OriginalPos;
    bool canDrag, canFeed;

    public GameObject DisplayItem;

    public enum FoodType
    {
        ELECTRONICS,
        BATTERIES
    }

    public FoodType foodType;

    void Start()
    {
        OriginalPos = transform.position;
    }

    void ControlDisplayItem()
    {
        switch (foodType)
        {
            case FoodType.ELECTRONICS:
                if (Manager.instance.Electronics < 2) DisplayItem.SetActive(false);
                break;
            case FoodType.BATTERIES:
                if (Manager.instance.Batteries < 2) DisplayItem.SetActive(false);
                break;
            default:
                break;
        }
    }

    void OnMouseOver()
    {
        if(!Manager.instance.isDraggingUI && canDrag)
            Manager.instance.SetGrabCursor();
    }

    void OnMouseExit()
    {
        Manager.instance.SetNormalCursor();
    }

    void OnMouseDown()
    {
        
    }

    void OnMouseUp()
    {
        Manager.instance.SetNormalCursor();
    }

    void OnMouseDrag()
    {
        ControlDisplayItem();

        switch (foodType)
        {
            case FoodType.ELECTRONICS:
                canDrag = Manager.instance.Electronics > 0 ? true : false;
                break;
            case FoodType.BATTERIES:
                canDrag = Manager.instance.Batteries > 0 ? true : false;
                break;
            default:
                break;
        }

        if (canDrag)
        {
            Manager.instance.isDraggingUI = true;
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, 0.0f);
            Manager.instance.SetHoldCursor();
        }
    }

    public void ReturnToPlace()
    {
        transform.position = OriginalPos;
        Manager.instance.SetNormalCursor();
        //Invoke("PatchNormalCursor", 0.5f);
    }

    //void PatchNormalCursor()
    //{
    //    Manager.instance.SetNormalCursor();
    //}

    // THIS DOESN'T WORK GOOD
    void OnTriggerStay2D(Collider2D other)
    {
        canFeed = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        canFeed = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && canFeed)
        {
            FeedBoy();
        }
    }

    void FeedBoy()
    {
        switch (foodType)
        {
            case FoodType.ELECTRONICS:
                Manager.instance.BoyXP += 25;
                Manager.instance.Electronics -= 1;
                Manager.instance.UpdateSuppliesTexts();
                break;
            case FoodType.BATTERIES:
                Manager.instance.AddEnergy(25.0f);
                Manager.instance.Batteries -= 1;
                Manager.instance.UpdateSuppliesTexts();
                break;
            default:
                break;
        }
    }
}
