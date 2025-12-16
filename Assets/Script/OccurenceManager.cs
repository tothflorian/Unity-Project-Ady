using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public enum Resource
{
    Money, Moral, Reputation, Enviroment
}

public enum Response
{
    OK, Ignore, Solve
}

public class OccurenceManager : MonoBehaviour
{
    public GameObject title;
    public GameObject description;
    public GameObject declineButton;
    private List<Occurence> _occurenceList;

    public List<Occurence> OccurenceList { get { return _occurenceList; } }

    void Awake()
    {
        _occurenceList = new List<Occurence>();

        using (StreamReader sr = new StreamReader("Assets/Media/occurences.txt"))
        {
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] data = line.Split('|');

                List<(Resource?, short?)> __tempResource;
                if (data[2] == "")
                {
                    __tempResource = null;
                }
                else
                {
                    __tempResource = new List<(Resource?, short?)>();

                    string[] __tempResourceString = data[2].Split(' ');
                    string[] __tempResValueString = data[3].Split(' ');

                    for (int i = 0; i < __tempResourceString.Count(); i++)
                    {
                        switch (__tempResourceString[i])
                        {
                            case "money":
                                __tempResource.Add( (Resource.Money, short.Parse(__tempResValueString[i])) );
                                break;
                            case "moral":
                                __tempResource.Add( (Resource.Moral, short.Parse(__tempResValueString[i])) );
                                break;
                            case "reputation":
                                __tempResource.Add( (Resource.Reputation, short.Parse(__tempResValueString[i])) );
                                break;
                            case "environment":
                                __tempResource.Add( (Resource.Enviroment, short.Parse(__tempResValueString[i])) );
                                break;
                            default:
                                throw new System.ArgumentException();
                        }
                    }
                }

                List<Response> __tempResp = new List<Response>();
                switch (data[4])
                {
                    case "ok":
                        __tempResp.Add(Response.OK);
                        break;
                    case "s/i":
                        __tempResp.Add(Response.Solve); __tempResp.Add(Response.Ignore);
                        break;
                }

                _occurenceList.Add(new Occurence(
                        data[0], data[1],
                        __tempResource,
                        __tempResp
                    ));
            }
        }
    }
}

public class Occurence
{
    private string _title;
    private string _description;
    private List<(Resource?, short?)> _resources;
    private List<Response> _responses;

    public string Title { get { return _title; } }
    public string Description { get { return _description; } }
    public List<(Resource?, short?)> Resources { get { return _resources; } }
    public List<Response> Responses { get { return _responses; } }

    public Occurence(string title, string description, List<(Resource?, short?)> resources, List<Response> responses)
    {
        _title = title;
        _description = description;
        _resources = resources;
        _responses = responses;
    }
}
