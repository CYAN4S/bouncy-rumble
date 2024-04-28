using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;

public class OpenTaskManager : Singleton<OpenTaskManager>
{
    [SerializeField] private List<UnityEvent> onTaskComplete;
    [SerializeField] private UnityEvent<int> onAnyTaskComplete;
    [SerializeField] private GameObject Congr;

    [SerializeField] private int task01 = 2;
    [SerializeField] private int task02 = 5;
    [SerializeField] private int taskCount = 4;

    [SerializeField] private AudioClip completeSound;

    public void Clear(int taskCode)
    {
        onTaskComplete[taskCode]?.Invoke();
        onAnyTaskComplete?.Invoke(taskCode);
        AudioSource.PlayClipAtPoint(completeSound, Vector3.zero);
        taskCount -= 1;
        if (taskCount == 0)
        {
            Congr.SetActive(true);
            StartCoroutine(TMP());
        }
    }

    private IEnumerator TMP()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(0);
    }

    public void MannequinKilled(int taskCode)
    {
        if (taskCode == 1)
        {
            task01 -= 1;
            if (task01 == 0)
                Clear(1);
        }
        else if (taskCode == 2)
        {
            task02 -= 1;
            if (task02 == 0)
                Clear(2);
        }
        
    }
}
