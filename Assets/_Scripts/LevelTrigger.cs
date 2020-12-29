using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    public SceneField level;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trig:" + other.gameObject.name);
        if (other.GetComponent<TriggerActivator>())
        {
            SceneManager.LoadScene(level.SceneName);
        }
    }
}
