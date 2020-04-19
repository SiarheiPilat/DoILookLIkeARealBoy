using UnityEngine;
using ScriptableObjectArchitecture;

public class LootableSpot : MonoBehaviour
{
    public FloatReference LootingTime;

    public GameObject[] LootItems;

    void Start()
    {
        
    }

    public void FillUpLootUI()
    {
        Manager.instance.CurrentlyLooted = this;
        for (int i = 0; i < LootItems.Length; i++)
        {
            if(LootItems[i] != null)
                Instantiate(LootItems[i], Manager.instance.ContainerCells[i].transform);
        }
    }

    void OnMouseOver()
    {
        if(Manager.instance.CanLoot) Manager.instance.SetSearchCursor();
    }

    void OnMouseExit()
    {
        Manager.instance.SetNormalCursor();
    }

    void OnMouseDown()
    {
        if (Manager.instance.CanLoot)
        {
            Manager.instance.SetWaitCursor();
            Manager.instance.LootingUIDialogue.GetComponent<LootingUI>().HowLong = LootingTime.Value;
            Manager.instance.LootingUIDialogue.SetActive(true);
            FillUpLootUI();
        }
    }


}
