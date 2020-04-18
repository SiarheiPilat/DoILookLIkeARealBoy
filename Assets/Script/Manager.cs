using UnityEngine;
using NaughtyAttributes;
using TMPro;
using ScriptableObjectArchitecture;
using UnityEngine.UI;

// IDEA:
// - Virus aesthetics
// - Voiceover
// - https://www.youtube.com/watch?v=nM2Wz6NdPZs

// MVP:
// - Depleting energy bar (done)
// - Loot: energy source, electronics
// - Day timer (done)
// - Voice lines:
//      "Do I look like a real boy, papa?"
//      "Thank you, papa"
//      "I must sleep... and grow, papa"
//      "I want to be like you when I grow up, papa"
//      "I must consume your soul... to become flesh and bone" (Game over state)
// - Loot buttons with loot progress bar
// - Loot areas
// - Some sort of inventory (done)
// - Drag and drop to feed the "boy" (done)
// - Drag and drop for inventory (done)
// - Item text popup
// - Different types of loot items:
//      - Batteries
//      - Scrapes
//      - Drill
//      - Broken laptop
//      - Keyboard
//      - Box of nails
//      - Toilet paper
//      - Hammer
//      - Test tube
//      - Petri dish

// Removed from the MVP:
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

    void Start()
    {
        instance = this;
        InvokeRepeating("HourTick", 3.0f, HourTickRate.Value);
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
