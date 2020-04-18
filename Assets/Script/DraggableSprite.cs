using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableSprite : MonoBehaviour
{
    Vector3 pos;
    Vector3 OriginalPos;
    bool canDrag, canFeed;

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

    void OnMouseDrag()
    {
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
        }
    }

    public void ReturnToPlace()
    {
        transform.position = OriginalPos;
    }

    // THIS DOESN'T WORK GOOD
    void OnTriggerStay2D(Collider2D other)
    {
        canFeed = true;
        //Debug.Log("entered");
        //if (Input.GetMouseButtonUp(0))
        //{
        //    FeedBoy();
        //}
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
                break;
            case FoodType.BATTERIES:
                Manager.instance.AddEnergy(25.0f);
                break;
            default:
                break;
        }
    }
}
