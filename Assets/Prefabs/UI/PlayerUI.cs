using TMPro;
using UnityEngine;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI JuiceText;
    [SerializeField] TextMeshProUGUI OxygenText;
    public void UpdateJuiceText(int _amount)
    {
        JuiceText.text = "Juice: " + _amount.ToString();
    }
    public void UpdateOxygenText(int _oxygen)
    {
        OxygenText.text = "Oxygen: " + _oxygen.ToString();
    }
}