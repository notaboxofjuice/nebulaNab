using UnityEngine;
public class DebugCanvas : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI debugText;
    private void Start()
    {
        // set text to parent object's name
        debugText.text = transform.parent.name;
    }
}