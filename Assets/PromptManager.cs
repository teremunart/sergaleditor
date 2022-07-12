using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PromptManager : MonoBehaviour {
    public GameObject objectPrompt;

    public void ChangeVisibility() => objectPrompt.SetActive(!objectPrompt.activeSelf);

    public void CancelBtn() => ChangeVisibility();
    public void YesBtn() => SceneManager.LoadScene(0);

}
