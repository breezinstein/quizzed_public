using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class MessageBox : MonoBehaviour
{
    public BreezeButton closeButton;
    public BreezeButton okayButton;
    public TextMeshProUGUI messageText;

    private void Start()
    {
        closeButton.onClick.AddListener(Close);
    }

    public void SetMessage(string _message,bool _isLoading = false, UnityAction okayAction = null)
    {
        messageText.text = _message;
        closeButton.gameObject.SetActive(!_isLoading);
        if(okayAction == null || _isLoading)
        {
            okayButton.gameObject.SetActive(false);
        }
        else
        {
            okayButton.gameObject.SetActive(true);
            okayButton.onClick.AddListener(okayAction);
            okayButton.onClick.AddListener(Close);
        }

    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        okayButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
