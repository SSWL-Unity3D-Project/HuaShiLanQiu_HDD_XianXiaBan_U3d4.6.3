using UnityEngine;
using System.Collections;

public class InputEventCtrl : MonoBehaviour
{
    public enum ButtonState : int
    {
        UP = 1,
        DOWN = -1
    }

    public enum InputDevice
    {
        Null = -1,
        PC = 0,        //电脑键盘鼠标.
        HDD = 1,       //红点点微信手柄.
    }
    /// <summary>
    /// 输入设备枚举.
    /// </summary>
    InputDevice _InputDevice = InputDevice.PC;
    [HideInInspector]
    public InputDevice m_InputDevice { get { return _InputDevice; } }

    static private InputEventCtrl _Instance = null;
    static public InputEventCtrl GetInstance()
    {
        if (_Instance == null)
        {
            GameObject obj = new GameObject("_InputEventCtrl");
            _Instance = obj.AddComponent<InputEventCtrl>();
        }
        return _Instance;
    }

    public delegate void EventHandel(SSGameDataCtrl.PlayerIndex index, ButtonState val);
    /// <summary>
    /// 开始按键被点击事件.
    /// </summary>
    public event EventHandel OnClickStartBtEvent;
    public void ClickStartBt(SSGameDataCtrl.PlayerIndex index, ButtonState val)
    {
        if (OnClickStartBtEvent != null)
        {
            OnClickStartBtEvent(index, val);
        }
    }
    
    /// <summary>
    /// 水平向左移动按键被点击事件.
    /// </summary>
    public event EventHandel OnClickLeftHorBtEvent;
    public void ClickLeftHorBt(SSGameDataCtrl.PlayerIndex index, ButtonState val)
    {
        if (OnClickLeftHorBtEvent != null)
        {
            OnClickLeftHorBtEvent(index, val);
        }
    }

    /// <summary>
    /// 水平向右移动按键被点击事件.
    /// </summary>
    public event EventHandel OnClickRightHorBtEvent;
    public void ClickRightHorBt(SSGameDataCtrl.PlayerIndex index, ButtonState val)
    {
        if (OnClickRightHorBtEvent != null)
        {
            OnClickRightHorBtEvent(index, val);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (m_InputDevice != InputDevice.PC)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ClickStartBt(SSGameDataCtrl.PlayerIndex.Player01, ButtonState.DOWN);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            ClickStartBt(SSGameDataCtrl.PlayerIndex.Player02, ButtonState.DOWN);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ClickLeftHorBt(SSGameDataCtrl.PlayerIndex.Player01, ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            ClickLeftHorBt(SSGameDataCtrl.PlayerIndex.Player01, ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ClickRightHorBt(SSGameDataCtrl.PlayerIndex.Player01, ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            ClickRightHorBt(SSGameDataCtrl.PlayerIndex.Player01, ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ClickLeftHorBt(SSGameDataCtrl.PlayerIndex.Player02, ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            ClickLeftHorBt(SSGameDataCtrl.PlayerIndex.Player02, ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ClickRightHorBt(SSGameDataCtrl.PlayerIndex.Player02, ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            ClickRightHorBt(SSGameDataCtrl.PlayerIndex.Player02, ButtonState.UP);
        }
    }
}