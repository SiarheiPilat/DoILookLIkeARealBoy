using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class RecyclerProgressBar : MonoBehaviour
{
    public Slider slider;
    public FloatReference HowLong;
    float t, r;

    void OnEnable()
    {
        Manager.instance.SetWaitCursor();
        Manager.instance.OpenBackpackButton.SetActive(false);
        Manager.instance.OpenMapButton.SetActive(false);
        slider.value = 0.0f;
        t = 0.0f;
        r = 0.0f;
        slider.maxValue = HowLong.Value;
    }

    void Update()
    {
        slider.value = Mathf.Lerp(0.0f, HowLong.Value, t);
        if (r <= HowLong.Value)
        {
            r += Time.deltaTime;
            t += Time.deltaTime / HowLong.Value;
        }
        else
        {
            Manager.instance.SetNormalCursor();
            Manager.instance.FinalizeRecycle();
            gameObject.SetActive(false);
        }

    }
}
