using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;

public class SavesController : MonoBehaviour
{
    public static SavesController Instance {get;private set;}
    private GameObject[] games;
    private string[] names;
    public GameObject buttonPrefab;
    private GameObject parent = null;
    private Toggle toggle;
    private string filePath = "Assets/Texts/";
    private string fileExtension = ".txt";
    public int selectedGame;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        games = new GameObject[3];
        Init();
    }
    public void Init()
    {
        if((parent = GameObject.Find("GameSelectorMenu")) != null)
        {
            BindUI(parent);

            using (StreamReader sr = new StreamReader(filePath + "saves" + fileExtension))
            {
                List<string> lines = new List<string>();
                
                string line = null;
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line.Trim());
                names = lines.ToArray();
                SetButtons(names);
            }
        }
    }
    public void BindUISelector()
    {
        toggle = GameObject.Find("Toggle").GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(b => SelectDelete(b));
    }
    private void SelectDelete(bool selected)
    {
        foreach (GameObject game in games)
        {
            if(selected)
            {
                Outline outline = game.AddComponent<Outline>();
                
                outline.effectColor = Color.red;
                outline.effectDistance = new Vector2(2, 2);
                outline.enabled = selected;
            }else
            {
                Destroy(game.GetComponent<Outline>());
            }
        }
    }
    public void BindUI(GameObject parent)
    {
        for (int i = 0; i < 3; i++)
        {
            games[i] = Instantiate(buttonPrefab);
            games[i].transform.SetParent(parent.transform);
            games[i].GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }
        BindUISelector();
    }
    private void SetButtons(string[] names)
    {
        if(games[0] != null)
        {
            for(int i = 0; i < names.Length; i++)
            {
                int index = i;
                if (names[index] == "-")
                {
                    games[index].transform.GetComponent<Button>().onClick.AddListener(() => ClickOnNewGame(index));
                    games[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "New Game";
                }else
                {
                    games[index].transform.GetComponent<Button>().onClick.AddListener(() => LoadGame(index));
                    games[index].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = names[index];
                }
            }
        }
    }
    public void ClickOnNewGame(int index)
    {
        if (toggle.isOn) toggle.isOn = false;
        selectedGame = index;
        GameObject inputGameName = games[selectedGame].transform.GetChild(1).gameObject;
        inputGameName.SetActive(true);
        games[selectedGame].transform.GetChild(0).gameObject.SetActive(false);
        inputGameName.GetComponent<TMP_InputField>().onEndEdit.AddListener(name => CreateGame(name));
        inputGameName.GetComponent<TMP_InputField>().onFocusSelectAll = true;
    }
    public void Save()
    {
        SaveName("saves");
        SaveGame();
    }
    private void SaveName(string path)
    {
        using (StreamWriter sw = new StreamWriter(filePath + path + fileExtension))
        {
            foreach(string name in names)
            {
                sw.WriteLine(name);
            }
        }
    }
    private void SaveGame()
    {
        using(StreamWriter sw = new StreamWriter(filePath + names[selectedGame] + fileExtension))
        {
            sw.WriteLine(TimeManager.Instance._currentDay);
            
            sw.WriteLine(GameManager.Instance.Money + " " + GameManager.Instance.Moral + " " +
                         GameManager.Instance.Reputation + " " + GameManager.Instance.Environment);

            foreach(OccurenceEntry occEntry in GameManager.Instance.occurenceList)
            {
                Occurence occ = occEntry.Occurence;
                sw.WriteLine(occ.Title + "|" + occ.response + "|" + occEntry.daysPassed + "|" + occ.Icon);
            }
        }
    }
    public void LoadGame(int index)
    {
        if (toggle.isOn)
        {
            Delete(index);
            SetButtons(names);
        }else if (File.Exists(filePath + names[index] + fileExtension))
        {
            MenuController.Instance.StartGame(filePath + names[index] + fileExtension);
            selectedGame = index;
        }else
        {
            MenuController.Instance.StartGame("new");
        }
    }
    public void Delete(int index)
    {
        File.Delete(filePath + names[index] + fileExtension);
        File.Delete(filePath + names[index] + fileExtension + ".meta");
        names[index] = "-";
        SaveName("saves");
    }
    public void CreateGame(string name)
    {
        ClearParent();
        if (name != null)
        {
            for (int i = 0; i < games.Length; i++)
            {
                if (games[i].transform.GetChild(1).gameObject.activeSelf)
                {
                    names[i] = name;
                }
            }
            parent = GameObject.Find("GameSelectorMenu");
            BindUI(parent);
            SetButtons(names);
        }
    }
    private void ClearParent()
    {
        parent = GameObject.Find("GameSelectorMenu");
        foreach(Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
