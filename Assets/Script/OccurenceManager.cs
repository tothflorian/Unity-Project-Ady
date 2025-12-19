using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public enum Resource
{
    Money, Moral, Reputation, Environment
}

public enum Response
{
    Ignore, Solve
}

public class OccurenceManager : MonoBehaviour
{
    public static OccurenceManager Instance { get; private set;}
    public GameObject title;
    public GameObject description;
    public GameObject declineButton;
    public GameObject occurenceIcon;
    private List<Occurence> _occurenceList;
    public List<Occurence> OccurenceList { get { return _occurenceList; } }


    public void BindUI()
    {
        title = GameObject.Find("Title");
        description = GameObject.Find("OccurenceText");
        declineButton = GameObject.Find("Ignore");
        occurenceIcon = GameObject.Find("OccurenceIcon");
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _occurenceList = new List<Occurence>();

        using (StreamReader sr = new StreamReader("Assets/Media/occurences.txt"))
        {
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] data = line.Split('|');

                List<Response> __tempResp = new List<Response>();
                List<object> __tempResource = new List<object>();
                string[] __tempResType = data[4].Split();
                string[] __tempResValue = data[5].Split();
                string[] __tempResValue2 = null;

                switch (data[2])
                {
                    case "ok":
                        __tempResp.Add(Response.Solve);
                        break;
                    case "s/i":
                        __tempResp.Add(Response.Solve); __tempResp.Add(Response.Ignore);
                        __tempResValue2 = data[6].Split();
                        break;
                }
                for(int i = 0; i < __tempResType.Count(); i++)
                {
                    switch(__tempResType[i])
                    {
                        case "money":
                            if (__tempResValue2 == null)
                                __tempResource.Add((Resource.Money, Convert.ToInt16(__tempResValue[i])));
                            else
                                __tempResource.Add((Resource.Money, Convert.ToInt16(__tempResValue[i]),Convert.ToInt16(__tempResValue2[i])));
                        break;
                        case "moral":
                            if (__tempResValue2 == null)
                                __tempResource.Add((Resource.Moral, Convert.ToInt16(__tempResValue[i])));
                            else
                                __tempResource.Add((Resource.Moral, Convert.ToInt16(__tempResValue[i]),Convert.ToInt16(__tempResValue2[i])));
                        break;
                        case "reputation":
                            if (__tempResValue2 == null)
                                __tempResource.Add((Resource.Reputation, Convert.ToInt16(__tempResValue[i])));
                            else
                                __tempResource.Add((Resource.Reputation, Convert.ToInt16(__tempResValue[i]),Convert.ToInt16(__tempResValue2[i])));
                        break;
                        case "environment":
                            if (__tempResValue2 == null)
                                __tempResource.Add((Resource.Environment, Convert.ToInt16(__tempResValue[i])));
                            else
                                __tempResource.Add((Resource.Environment, Convert.ToInt16(__tempResValue[i]),Convert.ToInt16(__tempResValue2[i])));
                        break;
                    }
                }

                _occurenceList.Add(new Occurence(
                        data[0], data[1],
                        data[3],
                        __tempResp,
                        __tempResource
                    ));
                
            }
        }
    }
}

public class Occurence
{
    private string _title;
    private string _description;
    private string _icon;
    private List<(Resource, short)> _resourcesOk = new List<(Resource, short)>();    
    private List<(Resource, short, short)> _resourcesSI = new List<(Resource, short, short)>();
    private List<Response> _responses;
    public string Title { get { return _title; } }
    public string Description { get { return _description; } }
    public string Icon { get {return _icon; } }
    public List<(Resource, short)> ResourcesOk { get { return _resourcesOk; } }
    public List<(Resource, short, short)> ResourcesSI { get { return _resourcesSI; } }
    public List<Response> Responses { get { return _responses; } }
    public Sprite CurrentSprite;
    public Response response;

    public Occurence(string title, string description, string icon, List<Response> responses,
                     List<object> resource)
    {
        _title = title;
        _description = description;
        _icon = icon;
        _responses = responses;
        if (resource[0].GetType() == typeof((Resource, short)))
        {
            foreach((Resource,short) res in resource)
            {
                _resourcesOk.Add(res);
            }
        }else
        {
            foreach((Resource,short,short) res in resource)
            {
                _resourcesSI.Add(res);
            }
        }
    }
    public Occurence(string spritetext, Sprite sprite, string title, Response response)
    {
        _icon = spritetext;
        CurrentSprite = sprite;
        _title = title;
        this.response = response;
    }
}
public class OccurenceEntry
{
    public GameObject UIObject;
    public Occurence Occurence;
    public int daysPassed = 0;
    public OccurenceEntry(GameObject UI, Occurence occ, int daysPassed)
    {
        UIObject = UI;
        Occurence = occ;
        this.daysPassed = daysPassed;
    }
}
