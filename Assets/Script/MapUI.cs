using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public GameObject[] LocationButtons;

    void OnEnable()
    {
        switch (Manager.instance.LocationIndex)
        {
            case 0:
                // lab
                
                break;
            case 1:
                // basement
                EnableAll();
                DisableButton(1);
                DisableButton(3);
                break;
            case 2:
                // kitchen
                
                break;
            case 3:
                // attick
                EnableAll();
                DisableButton(1);
                DisableButton(3);
                break;
            default:
                break;
        }
    }

    void DisableButton(int index)
    {
        LocationButtons[index].GetComponent<Button>().interactable = false;
        LocationButtons[index].GetComponent<Image>().raycastTarget = false;
    }

    void EnableAll()
    {
        for (int i = 0; i < LocationButtons.Length; i++)
        {
            LocationButtons[i].GetComponent<Button>().interactable = true;
            LocationButtons[i].GetComponent<Image>().raycastTarget = true;
        }
    }
}
