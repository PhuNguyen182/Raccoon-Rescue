using System;
using UnityEngine;

public abstract class BaseBox : MonoBehaviour
{
    public RectTransform ContentPanel;
    [SerializeField] protected RectTransform mainPanel;

    [Header("Config Box")]
    public bool IsPopup;
    public bool IsNotStack;

    [SerializeField] protected bool isAnim = true;

    private bool _isApplicationQuitting; 

    protected Canvas popupCanvas;
    protected Action actionOpenSaveBox;

    protected virtual string IDPopup => $"{GetType()}{gameObject.GetInstanceID()}";

    public Action OnCloseBox;
    public Action<int> OnChangeLayer;

    public bool IsBoxSave { get; set; }
    public int IDLayerPopup { get; set; }

    private void Awake()
    {
        popupCanvas = GetComponent<Canvas>();

        if (popupCanvas != null && IsPopup)
        {
            popupCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            popupCanvas.worldCamera = Camera.main;
            popupCanvas.sortingLayerID = SortingLayer.NameToID("Popup");
        }

        OnAwake();
    }

    protected virtual void OnAwake() { }

    public void InitBoxSave(bool isBoxSave, Action actionOpenSaveBox)
    {
        this.IsBoxSave = isBoxSave;
        this.actionOpenSaveBox = actionOpenSaveBox;
    }

    #region Init Open Handle
    protected virtual void OnEnable()
    {
        if (!IsNotStack)
        {
            BoxController.Instance.AddNewBackObj(this);
        }

        DoAppear();
        OnStart();
    }

    protected virtual void DoAppear() { }

    protected virtual void OnStart() { }
    #endregion

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Move the popup into Save Stack
    /// </summary>
    public virtual void SaveBox()
    {
        if (IsBoxSave)
            BoxController.Instance.AddBoxSave(IDPopup, actionOpenSaveBox);
    }

    /// <summary>
    /// Call Remove Save Box in specific scenarios
    /// </summary>
    public virtual void RemoveSaveBox()
    {
        BoxController.Instance.RemoveBoxSave(IDPopup);
    }

    #region Close Box
    public virtual void Close()
    {
        if (!IsNotStack)
            BoxController.Instance.Remove();

        DoClose();
    }

    protected virtual void DoClose()
    {
        SimplePool.Despawn(gameObject);
    }

    protected virtual void OnDisable()
    {
        if (!_isApplicationQuitting)
        {
            OnCloseBox?.Invoke();
            OnCloseBox = null;
        }

        BoxController.Instance.OnActionOnClosedOneBox();
    }

    private void OnApplicationQuit()
    {
        _isApplicationQuitting = true;
    }

    protected void DestroyBox()
    {
        OnCloseBox?.Invoke();
        Destroy(gameObject);
    }
    #endregion

    #region Change layer Box
    public void ChangeLayerHandle(int indexInStack)
    {
        if (IsPopup)
        {
            if (popupCanvas != null)
            {
                popupCanvas.planeDistance = 5;
                popupCanvas.sortingOrder = indexInStack;
                OnChangeLayer?.Invoke(indexInStack);
                IDLayerPopup = indexInStack;
            }
        }

        else
        {
            if (ContentPanel != null)
                transform.SetAsLastSibling();
        }
    }

    #endregion

    public virtual bool IsActive()
    {
        return true;
    }
}

public class BaseBox<T> : BaseBox where T : BaseBox
{
    public static string ResorcePath { get; }

    public static T Create()
    {
        return Create($"Boxes/{typeof(T).FullName}");
    }
    
    public static T Create(string path)
    {
        T prefab = Resources.Load<T>(path);
        T instance = SimplePool.Spawn(prefab);
        instance.gameObject.SetActive(true);
        return instance;
    }
}