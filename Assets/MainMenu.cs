using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    /*      ID | SCENENAME
     *      0  | _Menu
     *      1  | _SergalOldEditor
     *      2  | _SergalNewEditor
     *      3  | _NevreanEditor
     */

    public GameObject panelMain;
    public GameObject panelChangelog;
    
    // 4.0 and lower
    public void BtnClickSergalOldEditor() { SceneManager.LoadScene(1); }

    // 4.1 and higher
    public void BtnClickMainMenu() { SceneManager.LoadScene(0); }
    public void BtnClickSergalEditor() { SceneManager.LoadScene(2); }
    public void BtnClickChangelog() {
        panelChangelog.SetActive(true);
        panelMain.SetActive(false);
    }
    public void BtnClickChangelogBackToMenu() {
        panelChangelog.SetActive(false);
        panelMain.SetActive(true);
    }

    
    //TODO: Create Scene in 5.0
    public void BtnClickNevreanEditor() { SceneManager.LoadScene(0); }
    

}