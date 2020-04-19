using UnityEngine;
using UnityEngine.UI;

public class LootingUI : MonoBehaviour
{
    public Slider slider;
    public float HowLong;
    float t, r;

    void OnEnable()
    {
        Manager.instance.CanLoot = false;
        Manager.instance.SearchingTextContentUI.text = "Searching...";
        Manager.instance.SetWaitCursor();
        Manager.instance.OpenBackpackButton.SetActive(false);
        Manager.instance.OpenMapButton.SetActive(false);
        slider.value = 0.0f;
        t = 0.0f;
        r = 0.0f;
        slider.maxValue = HowLong;
    }

    void Update()
    {
        slider.value = Mathf.Lerp(0.0f, HowLong, t);
        if(r <= HowLong)
        {
            r += Time.deltaTime;
            t += Time.deltaTime / HowLong;
            Debug.Log(t);
        }
        else
        {
            Manager.instance.SetNormalCursor();
            Manager.instance.SearchingTextContentUI.text = "Done!";
            Manager.instance.OKLootConfirmButton.GetComponent<Button>().interactable = true;
            Manager.instance.OKLootConfirmButton.GetComponent<Image>().raycastTarget = true;
        }
            
    }
}
