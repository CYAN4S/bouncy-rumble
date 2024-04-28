using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskDisplay : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> texts;

    public void TaskComplete(int code)
    {
        texts[code].color = Color.green;
    }
}
