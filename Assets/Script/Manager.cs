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
//      "Do I look like a real boy, papa?" (done)
//      "Thank you, papa" (done)
//      "I must sleep... and grow, papa" (done)
//      "I want to be like you when I grow up, papa" (done)
//      "I must consume your soul... to become flesh and bone" (Game over state) (done)
// - Loot areas:
//      - Travel through areas using map list (done)
//      - Lootable spots (loot buttons with loot progress bar) (done)
//      - Loot manager that would randomize a bit of it
//      - Loot persistence (done)
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
// - Recycler (done)

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
    public TextMeshProUGUI BatteriesAmountText, ElectronicsAmountText;
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
    public Texture2D SearchCursor, GrabCursor, NormalCursor, WaitCursor, PointCursor, HoldCursor;

    // UI
    [HorizontalLine]
    public TextMeshProUGUI SearchingTextContentUI, ContainerName, GenericMessageDialogueText;
    public GameObject OpenMapButton, OpenBackpackButton, OKLootConfirmButton, RecycleButton, RecycleProgressDialogue, GenericMessageDialogue;
    public GameObject[] BackpacksAndContent;
    public GameObject[] ContainerCells;

    // Loot
    [HorizontalLine]
    public bool CanLoot, IsSearching;
    public LootableSpot CurrentlyLooted;
    public GameObject[] AllPossibleLoot;
    public List<int> indices;

    public void UpdateSuppliesTexts()
    {
        BatteriesAmountText.text = "Batteries: " + Batteries.ToString();
        ElectronicsAmountText.text = "Scraps: " + Electronics.ToString();
    }

    public void TryRecycle()
    {
        for (int i = 0; i < ContainerCells.Length; i++)
        {
            if (ContainerCells[i].transform.childCount > 0)
            {
                //there's something in the container
                Recycle();
                i = ContainerCells.Length;
            }
        }
    }

    void Recycle()
    {
        RecycleProgressDialogue.SetActive(true);
    }

    public void ShowGenericMessage(string messageText)
    {
        GenericMessageDialogue.SetActive(true);
        GenericMessageDialogueText.text = messageText;
    }

    public void FinalizeRecycle()
    {
        int el = 0;
        int bat = 0;

        for (int i = 0; i < ContainerCells.Length; i++)
        {
            if (ContainerCells[i].transform.childCount > 0)
            {
                el += ContainerCells[i].transform.GetChild(0).GetComponent<ItemUI>().ElectronicsYield;
                bat += ContainerCells[i].transform.GetChild(0).GetComponent<ItemUI>().BatteryYeild;
                Destroy(ContainerCells[i].transform.GetChild(0).gameObject);
            }
        }

        Batteries += bat;
        Electronics += el;
        UpdateSuppliesTexts();
        ShowGenericMessage("Result: " + el.ToString() + " Scraps, " + bat.ToString() + " Batteries");
    }

    [HorizontalLine]
    public GameObject[] BackpackCells;

    public void CheckIfReturnedToLab()
    {
        if(LocationIndex == 0)
        {
            CheckBackpackForConsumables();
        }
    }

    void CheckBackpackForConsumables()
    {
        int el = 0;
        int bat = 0;

        for (int i = 0; i < BackpackCells.Length; i++)
        {
            if (BackpackCells[i].transform.childCount > 0)
            {
                if(BackpackCells[i].transform.GetChild(0).gameObject.GetComponent<ItemUI>().ItemName == "Batteries")
                {
                    bat++;
                    Destroy(BackpackCells[i].transform.GetChild(0).gameObject);
                }
                if (BackpackCells[i].transform.GetChild(0).gameObject.GetComponent<ItemUI>().ItemName == "Scraps")
                {
                    el++;
                    Destroy(BackpackCells[i].transform.GetChild(0).gameObject);
                }
            }
        }

        Batteries += bat;
        Electronics += el;
        UpdateSuppliesTexts();
        if(bat > 0 || el > 0)
            ShowGenericMessage("Brought back: " + bat.ToString() + " Batteries, " + el.ToString() + " Scraps");
    }

    void Start()
    {
        AllowToLoot();
        SetNormalCursor();
        instance = this;
        InvokeRepeating("HourTick", 3.0f, HourTickRate.Value);
        UpdateSuppliesTexts();
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



    public void WhatHappensWhenCloseContainerButtonIsPressed()
    {
        for (int i = 0; i < ContainerCells.Length; i++)
        {
            if (ContainerCells[i].transform.childCount > 0)
            {
                indices.Add(ContainerCells[i].transform.GetChild(0).gameObject.GetComponent<ItemUI>().PrefabIndex);
                Destroy(ContainerCells[i].transform.GetChild(0).gameObject);
            }
        }

        CurrentlyLooted.SetupLootItems = new List<GameObject>();
        for (int i = 0; i < indices.Count; i++)
        {
            CurrentlyLooted.SetupLootItems.Add(AllPossibleLoot[indices[i]]);
        }

        indices = new List<int>();
    }

    public void AllowToLoot()
    {
        CanLoot = true;
    }

    public void DisalowToLoot()
    {
        CanLoot = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }

    public void SetSearchCursor()
    {
        Cursor.SetCursor(SearchCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetHoldCursor()
    {
        Cursor.SetCursor(HoldCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetPointCursor()
    {
        Cursor.SetCursor(PointCursor, Vector2.zero, CursorMode.Auto);
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
