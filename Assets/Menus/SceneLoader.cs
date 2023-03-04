using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transitions;

    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            LoadNextLevel();
    }

    void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(1));
    }

    IEnumerator LoadLevel(int index)
    {
        transitions.SetTrigger("Start");

        yield return new WaitForSeconds(1.5f);
        
        SceneManager.LoadScene(index);
        yield return null;
    }
}
