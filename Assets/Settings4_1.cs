using HSVPicker;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Random = System.Random;

public class Settings4_1 : MonoBehaviour {

    // New in 4.1
    public GameObject objectColorBox;
    public GameObject objectColorSet;
    
    // GLOBAL VARIABLES
    public int lengthOfArray = 0;
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
    private Random _systemRandom;
    private RandomNames _randomNames;

    // UNITY STUFF
    public void Awake() {
        _systemRandom = new Random();
        _randomNames = new RandomNames();
        int i = 0;
        
        // REGISTER EVENT LISTENERS
        colorsetDropdown.onValueChanged.AddListener(delegate { ChangeColorSet();} );
        layerDropdown.onValueChanged.AddListener(   delegate { ChangeDropdown(); });
        
        sergalNameInput.onValueChanged.AddListener( delegate { ChangeName(); });
        sergalTypeInput.onValueChanged.AddListener( delegate { ChangeType(); });
        
        picker.onValueChanged.AddListener(delegate { ChangeColorFromPicker(); });

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

    // FUNCTIONS 4.1 or updated
    public void ChangeColorMode() { 
        if (objectColorBox.activeSelf == false) {
            objectColorSet.SetActive(false);
            objectColorBox.SetActive(true);
        } else {
            objectColorSet.SetActive(true);
            objectColorBox.SetActive(false);
        }
    }
    
    private void RandomTile(int layer, int pT, int max = 0, string mode = "primary" ) {
        imageColor[layer] = _colorsFromPalette[RandomColorTable(pT)];
        refLayer[layer].GetComponent<Image>().color = imageColor[layer];
        
        if(mode == "secondary")
            refLayer[layer].GetComponent<Image>().sprite = patternLayers[new Random().Next(1, max)];
        if(mode == "pattern")
            refLayer[layer].GetComponent<Image>().sprite = extraLayers[new Random().Next(1, max)];
        
        picker.AssignColor(refLayer[layer].GetComponent<Image>().color);
        colorPalette[layer].GetComponent<Image>().color = imageColor[layer];
    }
    
    public void ChangeTypeOfImage(int type) {
        switch (layerDropdown.value) {
            case 0: break;
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
            if (step <= 0) step = lengthOfArray;
        } else { // "+"
            step++;
            if (step >= lengthOfArray) step = 0;
        }

        switch (layerDropdown.value) {
            case 0: break;
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
    
    public void RandomSergal() {
        picker.onValueChanged.RemoveAllListeners();
        ResetSergal();

        int paletteType = _systemRandom.Next(0, 6);

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
        
        RandomTile(0, paletteType);                                    // PRIMARY COLOR
        RandomTile(1, paletteType, patternLayers.Length, "secondary"); // SECONDARY COLOR
        RandomTile(2, paletteType);                                    // MOUTH
        if (_systemRandom.Next(0, 10) > 3) {
            RandomTile(3, paletteType, extraLayers.Length, "pattern"); // PATTERN 1
            if (_systemRandom.Next(0, 10) > 5) {
                RandomTile(4, paletteType, extraLayers.Length, "pattern"); // PATTERN 2
                if (_systemRandom.Next(0, 10) > 7) {
                    RandomTile(5, paletteType, extraLayers.Length, "pattern"); // PATTERN 3
                }   
            }   
        }
        RandomTile(11, paletteType); // EYES
        RandomTile(12, paletteType); // PADS
        
        // GENERATION COMPLETED
        isGenerated = true;
        
        picker.onValueChanged.AddListener(delegate { ChangeColorFromPicker(); });
    }
    
    // FUNCTIONS 4.0 below
    private int RandomColorTable(int cT) {
        switch (cT) {
            case 1:  return _systemRandom.Next(0, 14);
            case 2:  return _systemRandom.Next(15, 34);
            case 3:  return _systemRandom.Next(35, 54);
            case 4:  return _systemRandom.Next(55, 74);
            case 5:  return _systemRandom.Next(75, 89);
            case 6:  return _systemRandom.Next(90, 109);
            default: return _systemRandom.Next(0, 109);
        }
    }
    
    private void ResetSergal() {
        for (int i = 0; i < imageColor.Length; i++) imageColor[i] = new Color32(255,255,255, 255);

        for (int i = 3; i < 10; i++) refLayer[i].GetComponent<Image>().sprite = extraLayers[0];
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
        var setNum = colorsetDropdown.value;

        foreach (var set in colorsets)
            set.SetActive(false);

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

    private void ChangeName() => sergalName.text = sergalNameInput.text;
    private void ChangeType() => sergalType.text = sergalTypeInput.text; 
}