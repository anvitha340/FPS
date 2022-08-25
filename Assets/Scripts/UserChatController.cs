using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserChatController : MonoBehaviour
{
    public static List<string> approvedList = new List<string>();
    public void OnApprove()
    {
        approvedList.Add(SmartfoxManager.sender);
    }
}
