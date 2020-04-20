using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

// The areas are:
//  - Lab
//  - Basement
//  - Kitchen
//  - Attick

public class CameraControl : MonoBehaviour
{
    public Transform Lab, Basement, Kitchen, Attick;
    bool CanMove;
    public Transform NewDestination;
    public FloatReference CameraSpeed, ApproachThreshold;
    public GameObject[] AllMapButtons;

    //float tempt;

    void Update()
    {
        // If anyone sees it, don't tell anyone, please... lol... sending love <3
        if (CanMove)
        {
            //tempt += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, NewDestination.position, Time.deltaTime * CameraSpeed.Value * 0.1f);
            if(Vector3.Distance(transform.position, NewDestination.position) < ApproachThreshold.Value)
            {
                
                CanMove = false;
                Debug.Log("arrived");
                NewDestination = null;

                string temp = "";

                switch (Manager.instance.LocationIndex)
                {
                    case 0:
                        // lab
                        temp = "Lab";
                        break;
                    case 1:
                        // basement
                        temp = "Basement";
                        break;
                    case 2:
                        // kitchen
                        temp = "Kitchen";
                        break;
                    case 3:
                        // attick
                        temp = "Attick";
                        break;
                    default:
                        break;
                }

                Manager.instance.ConfirmationTextConent.text = "You arrived to the " + temp;
                Manager.instance.ConfirmationDialogue.SetActive(true);
                Time.timeScale = 0.0f;

                //Debug.Log(tempt);
                //tempt = 0.0f;
            }
        }
    }

    public void MoveTo(Transform spot)
    {
        NewDestination = spot;
        CanMove = true;
        Manager.instance.CanLoot = false;
    }

    private void EnableAllButtons()
    {
        for (int i = 0; i < AllMapButtons.Length; i++)
        {
            AllMapButtons[i].GetComponent<Button>().interactable = true;
            AllMapButtons[i].GetComponent<Image>().raycastTarget = true;
        }
    }

    public void DisableButton(GameObject button)
    {
        EnableAllButtons();
        button.GetComponent<Button>().interactable = false;
        button.GetComponent<Image>().raycastTarget = false;
    }

    public void ChangeLocationIndex(int i)
    {
        Manager.instance.LocationIndex = i;

        Manager.instance.LabMapButtonText.text = "Lab";
        Manager.instance.BasementMapButtonText.text = "Basement";
        Manager.instance.KitchenMapButtonText.text = "Kitchen";
        Manager.instance.AttickMapButtonText.text = "Attick";

        switch (i)
        {
            case 0:
                // lab
                Manager.instance.LabMapButtonText.text = "Lab (You are here!)";
                break;
            case 1:
                // basement
                Manager.instance.BasementMapButtonText.text = "Basement (You are here!)";
                break;
            case 2:
                // kitchen
                Manager.instance.KitchenMapButtonText.text = "Kitchen (You are here!)";
                break;
            case 3:
                // attick
                Manager.instance.AttickMapButtonText.text = "Attick (You are here!)";
                break;
            default:
                break;
        }
    }
}
