using UnityEngine;
using NaughtyAttributes;
using TMPro;
using ScriptableObjectArchitecture;
using UnityEngine.UI;
using System.Collections.Generic;

// IDEA:
// - Virus aesthetics
// - Voiceover
// - https://www.youtube.com/watch?v=nM2Wz6NdPZs

// MVP:
// - Depleting energy bar (done)
// - Day timer (done)
// - Voice lines:
//      "Do I look like a real boy, papa?"
//      "Thank you, papa"
//      "I must sleep... and grow, papa"
//      "I want to be like you when I grow up, papa"
//      "I must consume your soul... to become flesh and bone" (Game over state)
// - Loot areas:
//      - Travel through areas using map list (done)
//      - Lootable spots (loot buttons with loot progress bar)
//      - Loot manager
// - Some sort of inventory (done)
// - Drag and drop to feed the "boy" (done)
// - Drag and drop for inventory (done)
// - Item text popup (done)
// - Different types of loot items: (done)
//      - Batteries (done)
//      - Scrapes (done)
//      - Drill (done)
//      - Broken laptop (done)
//      - Keyboard (done)
//      - Box of nails (done)
//      - Toilet paper (done)
//      - Hammer (done)
//      - Test tube (done)
//      - Petri dish (done)
// - Game balance spreadsheet

// Early removed from the MVP:
// - Several levels of the "boy"

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public FloatReference HourTickRate, EnergyDepleteRate;
    public int Hour, Day;
    public TextMeshProUGUI DayText, HourText;
    public Slider EnergySlider;

    // Inventory
    public int Batteries, Electronics;
    public DraggableSprite DraggableElectronics, DraggableBatteries;
    public GameObject FloatingTextUI;
    public Vector3 FloatingTextUIOffset;
    public TextMeshProUGUI FloatingTextContent;

    // Control
    public bool isDraggingUI;

    // Boy Stats
    public int BoyLevel, BoyXP;

    // Locations
    [HorizontalLine]
    public int LocationIndex;
    public TextMeshProUGUI LabMapButtonText, BasementMapButtonText, KitchenMapButtonText, AttickMapButtonText, ConfirmationTextConent;
    public GameObject ConfirmationDialogue;

    // Cursor
    [HorizontalLine]
    public GameObject LootingUIDialogue;
    public Texture2D SearchCursor, GrabCursor, NormalCursor, WaitCursor;

    // UI
    [HorizontalLine]
    public TextMeshProUGUI SearchingTextContentUI;
    public GameObject OpenMapButton, OpenBackpackButton, OKLootConfirmButton;
    public GameObject[] BackpacksAndContent;

    // Loot
    [HorizontalLine]
    public bool CanLoot;
    public LootableSpot CurrentlyLooted;

    public GameObject[] ContainerCells;

    void Start()
    {
        AllowToLoot();
        SetNormalCursor();
        instance = this;
        InvokeRepeating("HourTick", 3.0f, HourTickRate.Value);
    }

    public void OpenBackPackAndContainer()
    {
        OpenMapButton.SetActive(false);
        OpenBackpackButton.SetActive(false);
        for (int i = 0; i < BackpacksAndContent.Length; i++)
        {
            BackpacksAndContent[i].SetActive(true);
        }
        CanLoot = false;
    }

    public List<GameObject> tempList;

    public void RemoveLoot()
    {
        // the following code is just a candy <3
        int temp = 0;
        //List<GameObject> tempList = new List<GameObject>();
        //tempList = new List<GameObject>();
        for (int i = 0; i < ContainerCells.Length; i++)
        {
            if (ContainerCells[i].transform.childCount > 0)
            {
                temp++;
                // this is wrong because it's not a prefab (fixed? NO)
                tempList.Add(ContainerCells[i].transform.GetChild(0).gameObject.GetComponent<ItemUI>().OwnPrefab);
                Destroy(ContainerCells[i].transform.GetChild(0).gameObject);
            }
        }
        Debug.Log(tempList.Count);
        if (CurrentlyLooted)
        {
            //Debug.Log(tempList.Count);
            CurrentlyLooted.LootItems = new GameObject[temp];
            for (int i = 0; i < temp; i++)
            {
                CurrentlyLooted.LootItems[i] = tempList[i];
            }
            //tempList = new List<GameObject>();
        }
    }

    public void AllowToLoot()
    {
        CanLoot = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }

    public void SetSearchCursor()
    {
        Cursor.SetCursor(SearchCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetGrabCursor()
    {
        Cursor.SetCursor(GrabCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetNormalCursor()
    {
        Cursor.SetCursor(NormalCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetWaitCursor()
    {
        Cursor.SetCursor(WaitCursor, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDraggingUI = false;
            DraggableElectronics.ReturnToPlace();
            DraggableBatteries.ReturnToPlace();
        }
    }

    void HourTick()
    {
        if(Hour + 1 == 24)
        {
            Hour = 0;
            Day++;
            CompleteDay();
        }
        else
        {
            Hour++;
        }
        
        UpdateTimeUI();
        DepleteEnergy();
    }

    void DepleteEnergy()
    {
        if(EnergySlider.value - EnergyDepleteRate.Value <= 0.0f)
        {
            EnergySlider.value = 0.0f;
            GameOver();
        }
        else
        {
            EnergySlider.value -= EnergyDepleteRate.Value;
        }
    }

    public void AddEnergy(float amount)
    {
        EnergySlider.value += amount;
    }

    void GameOver()
    {
        Debug.Log("Boom");
    }

    void UpdateTimeUI()
    {
        DayText.text = "Day: " + Day.ToString();
        HourText.text = "Hour: " + Hour.ToString();
    }

    void CompleteDay()
    {

    }
}
