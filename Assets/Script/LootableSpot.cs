using UnityEngine;
using ScriptableObjectArchitecture;
using System.Collections.Generic;

public class LootableSpot : MonoBehaviour
{
    public FloatReference LootingTime;

    public List<GameObject> SetupLootItems;

    public bool IsRecycler;

    public string ThisContainerName;

    public void FillUpLootUI()
    {
        Manager.instance.CurrentlyLooted = this;
        for (int i = 0; i < SetupLootItems.Count; i++)
        {
            GameObject NewItem = Instantiate(SetupLootItems[i], Manager.instance.ContainerCells[i].transform);
        }
    }

    void OnMouseOver()
    {
        if(Manager.instance.CanLoot && !IsRecycler) Manager.instance.SetSearchCursor();
        if (Manager.instance.CanLoot && IsRecycler) Manager.instance.SetPointCursor();
    }

    void OnMouseExit()
    {
        if(Manager.instance.IsSearching != true) Manager.instance.SetNormalCursor();
    }

    void OnMouseDown()
    {
        if (Manager.instance.CanLoot && !IsRecycler)
        {
            Manager.instance.IsSearching = true;
            Manager.instance.SetWaitCursor();
            Manager.instance.LootingUIDialogue.GetComponent<LootingUI>().HowLong = LootingTime.Value;
            Manager.instance.LootingUIDialogue.SetActive(true);
            Manager.instance.RecycleButton.SetActive(false);
            FillUpLootUI();
        }

        if (Manager.instance.CanLoot && IsRecycler)
        {
            Manager.instance.SetNormalCursor();
            Manager.instance.OpenBackPackAndContainer();
            Manager.instance.RecycleButton.SetActive(true);
            FillUpLootUI();
        }

        if (ThisContainerName == "") ThisContainerName = "Container";
        Manager.instance.ContainerName.text = ThisContainerName;
        Manager.instance.CanLoot = false;
    }


}
