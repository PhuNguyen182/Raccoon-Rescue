using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmBox : BaseBox<ConfirmBox>
{
    [Header("Notice Texts")]
    [SerializeField] private TMP_Text titleMessage;
    [SerializeField] private TMP_Text yesText;
    [SerializeField] private TMP_Text noText;
    [SerializeField] private TMP_Text messageText;

    [Header("Executionable Buttons")]
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject backGround;

    private Action _onYesClick;
    private Action _onNoClick;

    private bool _isAutoLockEscape = false;

    protected override void OnAwake()
    {
        yesButton.onClick.AddListener(OnClickYesButton);
        noButton.onClick.AddListener(OnClickNoButton);
        closeButton.onClick.AddListener(Close);
    }

    public void OnClickYesButton()
    {
        _onYesClick?.Invoke();
        Close();
    }

    public void OnClickNoButton()
    {
        _onNoClick?.Invoke();
        Close();
    }

    public ConfirmBox AddMessage(string titleString, string messageString = "", string yesString = null,
    string noString = null)
    {
        AddMessageYesNo(titleString, messageString, null, null, yesString, noString);
        noButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        return ShowCloseButton(false);
    }

    public ConfirmBox AddMessageUpdate(string messageString = "", bool isAutoLockEscape = false)
    {
        messageText.text = messageString;
        return ShowCloseButton(false);
    }

    public ConfirmBox AddMessageOK(string titleString, string messageString = "", Action onOKClick = null,
        string okString = null, bool isAutoLockEscape = false)
    {
        titleMessage.text = titleString;
        messageText.text = messageString;

        noButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(true);

        yesText.text = string.IsNullOrEmpty(okString) ? "OK" : okString;
        OnCloseBox = onOKClick;

        return this;
    }

    public ConfirmBox AddMessageYesNo(string titleString, string messageString = "", Action onYesClick = null,
        Action onNoClick = null, string yesString = null, string noString = null, Action OnCloseBoxAction = null)
    {
        titleMessage.text = titleString;
        messageText.text = messageString;

        yesText.text = string.IsNullOrEmpty(yesString) ? "Yes" : yesString;
        noText.text = string.IsNullOrEmpty(noString) ? "No" : noString;

        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);

        OnCloseBox = OnCloseBoxAction;
        _onYesClick = onYesClick;
        _onNoClick = onNoClick;

        return EnableBackground();
    }

    public ConfirmBox EnableBackground()
    {
        backGround.SetActive(true);
        return this;
    }

    public ConfirmBox ShowCloseButton(bool isShow)
    {
        closeButton.gameObject.SetActive(isShow);
        return this;
    }

    public ConfirmBox SetPopupAutoLockEscape(bool isLockEscape = false)
    {
        _isAutoLockEscape = isLockEscape;
        BoxController.Instance.isLockEscape = isLockEscape;
        return this;
    }

    public override void Close()
    {
        if (_isAutoLockEscape)
        {
            BoxController.Instance.isLockEscape = false;
        }

        base.Close();
    }
}
