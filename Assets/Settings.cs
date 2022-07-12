using System.Diagnostics;
using System.Globalization;
using HSVPicker;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;
using Random = System.Random;
using Random2 = Unity.Mathematics.Random;

public class Settings : MonoBehaviour {
    // GLOBAL VARIABLES
    public int lengthOfArray;
    public int step;
    public Sprite[] patternLayers;
    public Sprite[] extraLayers;
    public Sprite[] eyesLayers;
    
    // COLOR VARIABLES
    public ColorPicker picker;
    public Color32[] imageColor = new Color32[13];
    private Color32[] _colorsFromPalette = new Color32[110];

    // UI VARIABLES
    public Image[] colorPalette;
    public Image[] refLayer;
    public Dropdown layerDropdown;
    public Dropdown colorsetDropdown;
    public GameObject[] colorsets;
    public GameObject changeButtonsGameObject;
    public Button[] colorButtons;
    public InputField sergalNameInput;
    public InputField sergalTypeInput;
    public Text sergalName;
    public Text sergalType;
    
    public bool isGenerated = false;
    public Random SystemRandom;
    private RandomNames _randomNames;

    // UNITY STUFF
    public void Awake() {
        SystemRandom = new Random();
        _randomNames = new RandomNames();
        int i = 0;
        
        // REGISTER EVENT LISTENERS
        colorsetDropdown.onValueChanged.AddListener(delegate { ChangeColorSet();} );
        layerDropdown.onValueChanged.AddListener(   delegate { ChangeDropdown(); });
        
        sergalNameInput.onValueChanged.AddListener( delegate { ChangeName(); });
        sergalTypeInput.onValueChanged.AddListener( delegate { ChangeType(); });
        
        picker.onValueChanged.AddListener(delegate { ChangeColorFromPicker(); });
        //picker.onValueChanged.AddListener(color => { imageColor[layerDropdown.value] = (Color32)color; });

        foreach (var btn in colorButtons) {
            _colorsFromPalette[i] = btn.GetComponent<Image>().color;
            btn.onClick.AddListener(delegate { SetToBtnColor(btn.GetComponent<Image>().color); });
            i++;
        }
    }
    
    public void FixedUpdate() {
        foreach (var item in colorPalette) item.gameObject.SetActive(item.GetComponent<Image>().sprite != patternLayers[0]);

        while (!isGenerated) RandomSergal();
    }

    // FUNCTIONS
    public void ChangeTypeOfImage(int type) {
        switch (layerDropdown.value) {
            case 0:
                break;
            case 1: //Pattern
                lengthOfArray = patternLayers.Length;
                break;
            case 2: //Mouth
                break;
            case 3: //Extras
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                lengthOfArray = extraLayers.Length;
                break;
            case 11: // Eyes
                lengthOfArray = eyesLayers.Length;
                break;
        }
        
        if (type == 0) { // "-"
            step--;
            if (step < 0) step = lengthOfArray;

        } else { // "+"
            step++;
            if (step > lengthOfArray) step = 0;
        }

        switch (layerDropdown.value) {
            case 0:
                break;
            case 1: //Pattern
                refLayer[layerDropdown.value].GetComponent<Image>().sprite = patternLayers[step];
                break;
            case 3: //Extras
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                refLayer[layerDropdown.value].GetComponent<Image>().sprite = extraLayers[step];
                break;
            case 11: // Eyes
                refLayer[layerDropdown.value].GetComponent<Image>().sprite = eyesLayers[step];
                break;
        }
    }

    private void RandomPattern(int stage, int max, int cT) {
        imageColor[stage] = _colorsFromPalette[RandomColorTable(cT)];
        refLayer[stage].GetComponent<Image>().color = imageColor[stage];
        refLayer[stage].GetComponent<Image>().sprite = patternLayers[new Random().Next(1, max)];
        picker.AssignColor(refLayer[stage].GetComponent<Image>().color);
        colorPalette[stage].GetComponent<Image>().color = imageColor[stage];
    }
    private void RandomExtras(int stage, int max, int cT) {
        imageColor[stage] = _colorsFromPalette[RandomColorTable(cT)];
        refLayer[stage].GetComponent<Image>().color = imageColor[stage];
        refLayer[stage].GetComponent<Image>().sprite = extraLayers[new Random().Next(1, max)];
        picker.AssignColor(refLayer[stage].GetComponent<Image>().color);
        colorPalette[stage].GetComponent<Image>().color = imageColor[stage];
    }
    private void RandomSingle(int stage, int cT) {
        imageColor[stage] = _colorsFromPalette[RandomColorTable(cT)];
        refLayer[stage].GetComponent<Image>().color = imageColor[stage];
        picker.AssignColor(refLayer[stage].GetComponent<Image>().color);
        colorPalette[stage].GetComponent<Image>().color = imageColor[stage];
    }
    private int RandomColorTable(int cT) {
        switch (cT) {
            case 1:  return SystemRandom.Next(0, 14);
            case 2:  return SystemRandom.Next(15, 34);
            case 3:  return SystemRandom.Next(35, 54);
            case 4:  return SystemRandom.Next(55, 74);
            case 5:  return SystemRandom.Next(75, 89);
            case 6:  return SystemRandom.Next(90, 109);
            default: return SystemRandom.Next(0, 109);
        }
    }
    
    public void RandomSergal() {
        picker.onValueChanged.RemoveAllListeners();
        ResetSergal();

        int paletteType = SystemRandom.Next(0, 6);

        sergalName.text = _randomNames.GenerateCustomName().ToUpper();
        switch (paletteType) {
            case 0: sergalType.text = "GENERATED SEGAL"; break;
            case 1: sergalType.text = "PURE NORTHERN SEGAL"; break;
            case 2: sergalType.text = "CIVILIZED NORTHERN SEGAL"; break;
            case 3: sergalType.text = "WESTERN SEGAL"; break;
            case 4: sergalType.text = "EASTERN SEGAL"; break;
            case 5: sergalType.text = "PURE SOUTHERN SEGAL"; break;
            case 6: sergalType.text = "CIVILIZED SOUTHERN SEGAL"; break;
        }
        
        // BASE
        RandomSingle(0, paletteType);

        // PATTERN
        RandomPattern(1, patternLayers.Length, paletteType);
        
        // MOUTH
        RandomSingle(2, paletteType);
        
        // EXTRAS
        if (SystemRandom.Next(0, 10) > 3) {
            RandomExtras(3, extraLayers.Length, paletteType);
            if (SystemRandom.Next(0, 10) > 4) {
                RandomExtras(4, extraLayers.Length, paletteType);
                if (SystemRandom.Next(0, 10) > 5) {
                    RandomExtras(5, extraLayers.Length, paletteType);
                    if (SystemRandom.Next(0, 10) > 6) {
                        RandomExtras(6, extraLayers.Length, paletteType);
                    }
                }
            }
        }

        // EYES
        RandomSingle(11, paletteType);

        // PADS
        RandomSingle(12, paletteType);
        
        // GENERATION COMPLETED
        Debug.Log("Generation Completed --> Sergal should now have the colors.");
        isGenerated = true;
        
        picker.onValueChanged.AddListener(delegate { ChangeColorFromPicker(); });
    }
    private void ResetSergal() {
        for (int i = 0; i < imageColor.Length; i++) {
            imageColor[i] = new Color32(0,0,0, 255);
        }
        
        for (int i = 3; i < 10; i++) {
            refLayer[i].GetComponent<Image>().sprite = extraLayers[0];
        }
        
    }
    
    // LISTENERS
    private void SetToBtnColor(Color32 color) {
        imageColor[layerDropdown.value] = color;
        refLayer[layerDropdown.value].GetComponent<Image>().color = imageColor[layerDropdown.value];
        
        picker.AssignColor(refLayer[layerDropdown.value].GetComponent<Image>().color);
        colorPalette[layerDropdown.value].GetComponent<Image>().color = imageColor[layerDropdown.value];
    }

    private void ChangeColorFromPicker() {
        imageColor[layerDropdown.value] = picker.CurrentColor;
        refLayer[layerDropdown.value].GetComponent<Image>().color = imageColor[layerDropdown.value];
        
        colorPalette[layerDropdown.value].GetComponent<Image>().color = imageColor[layerDropdown.value];
    }
    
    private void ChangeColorSet() {
        var setNum =colorsetDropdown.value;

        foreach (var set in colorsets) {
            set.SetActive(false);
        }

        colorsets[setNum].SetActive(true);
    }
    private void ChangeDropdown() {
        if (layerDropdown.value == 0 || layerDropdown.value == 2 || layerDropdown.value == 11 || layerDropdown.value == 12) 
            changeButtonsGameObject.SetActive(false);
        else
            changeButtonsGameObject.SetActive(true);
        
        step = 0;
        refLayer[layerDropdown.value].GetComponent<Image>().color = imageColor[layerDropdown.value];
        picker.AssignColor(imageColor[layerDropdown.value]);
    }

    private void ChangeName() { sergalName.text = sergalNameInput.text; }
    private void ChangeType() { sergalType.text = sergalTypeInput.text; }
}