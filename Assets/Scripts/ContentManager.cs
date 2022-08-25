using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    [SerializeField] GameObject[] contents;
    [SerializeField] GameObject[] scrollview; 
    [SerializeField] GameObject requestItemPrefab;
    List<string> senders = new List<string>();

    public static ContentManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void MakeContentActive(int index)
    {
        foreach (var content in contents)
        {
            content.SetActive(false);
        }
        foreach(var gameobj in scrollview)
        {
            gameobj.SetActive(false);
        }
        scrollview[index].SetActive(true);
        contents[index].SetActive(true);
        if(!string.IsNullOrWhiteSpace(SmartfoxManager.sender) && !senders.Contains(SmartfoxManager.sender) && index == 3)
        {
            GameObject item = Instantiate(requestItemPrefab);
            item.GetComponent<Text>().text = SmartfoxManager.sender + " sent a chat request";
            senders.Add(SmartfoxManager.sender);
            item.transform.parent = contents[index].transform;
        }
        
    }
}
