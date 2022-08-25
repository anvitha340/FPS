using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [SerializeField] Toggle toggle;

    List<string> toggles = new List<string> { "AllUsers", "AvailableUsers", "PlayingUsers", "Requests" };
    //ContentManager contentManager = new ContentManager();

    #region Mono Methods
    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(OnTabSwitch);
    }
    void Start()
    {
        if (toggle.isOn)
            toggle.image.color = Color.red;

    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(OnTabSwitch);
    }
    #endregion

    #region Private Methods

    /// <summary>
    /// Tab switch along with colour change
    /// </summary>
    /// <param name="isSelected"></param>
    private void OnTabSwitch(bool isSelected)
    {
        if (isSelected)
        {
            toggle.image.color = Color.red;
            int a = toggles.IndexOf(toggle.gameObject.name);
            Debug.Log("a"+a);
            ContentManager.instance.MakeContentActive(a);

        }
        else
        {
            toggle.image.color = Color.white;
        }
    }

    #endregion
    

}
