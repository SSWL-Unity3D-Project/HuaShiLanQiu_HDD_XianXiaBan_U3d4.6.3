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
        /// <summary>
        /// 电脑键盘鼠标.
        /// </summary>
        PC = 0,
        /// <summary>
        /// 红点点微信手柄.
        /// </summary>
        HDD = 1,
    }
    InputDevice _InputDevice = InputDevice.Null;
    /// <summary>
    /// 输入设备枚举.
    /// </summary>
    [HideInInspector]
    public InputDevice m_InputDevice
    {
        set { _InputDevice = value; }
        get { return _InputDevice; }
    }

    static private InputEventCtrl _Instance = null;
    static public InputEventCtrl GetInstance()
    {
        if (_Instance == null)
        {
            GameObject obj = new GameObject("_InputEventCtrl");
            _Instance = obj.AddComponent<InputEventCtrl>();
            _Instance.Init();
            pcvr.GetInstance();
        }
        return _Instance;
    }

    void Init()
    {
        m_InputDevice = pcvr.m_InputDevice;
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

    /// <summary>
    /// 电视遥控器按键消息.
    /// </summary>
    public delegate void EventHandelTV(ButtonState val);
    public event EventHandelTV ClickTVYaoKongExitBtEvent;
    public void ClickTVYaoKongExitBt(ButtonState val)
    {
        if (ClickTVYaoKongExitBtEvent != null)
        {
            ClickTVYaoKongExitBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongEnterBtEvent;
    public void ClickTVYaoKongEnterBt(ButtonState val)
    {
        if (ClickTVYaoKongEnterBtEvent != null)
        {
            ClickTVYaoKongEnterBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongLeftBtEvent;
    public void ClickTVYaoKongLeftBt(ButtonState val)
    {
        if (ClickTVYaoKongLeftBtEvent != null)
        {
            ClickTVYaoKongLeftBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongRightBtEvent;
    public void ClickTVYaoKongRightBt(ButtonState val)
    {
        if (ClickTVYaoKongRightBtEvent != null)
        {
            ClickTVYaoKongRightBtEvent(val);
        }
    }
    
    public event EventHandelTV ClickTVYaoKongUpBtEvent;
    public void ClickTVYaoKongUpBt(ButtonState val)
    {
        if (ClickTVYaoKongUpBtEvent != null)
        {
            ClickTVYaoKongUpBtEvent(val);
        }
    }

    public event EventHandelTV ClickTVYaoKongDownBtEvent;
    public void ClickTVYaoKongDownBt(ButtonState val)
    {
        if (ClickTVYaoKongDownBtEvent != null)
        {
            ClickTVYaoKongDownBtEvent(val);
        }
    }

    class KeyCodeTV
    {
        /// <summary>
        /// 遥控器确定键的键值.
        /// </summary>
        public static KeyCode PadEnter01 = (KeyCode)10;
        public static KeyCode PadEnter02 = (KeyCode)66;
    }

    /// <summary>
    /// 红点点微信手柄消息.
    /// </summary>
    public delegate void EventHandelHDD(SSGameDataCtrl.PlayerIndex index, float val);
    /// <summary>
    /// 移动红点点微信手柄篮筐事件.
    /// </summary>
    public event EventHandelHDD ClickHddPadLanKuangBtEvent;
    public void ClickHddPadLanKuangBt(SSGameDataCtrl.PlayerIndex index, float val)
    {
        if (ClickHddPadLanKuangBtEvent != null)
        {
            ClickHddPadLanKuangBtEvent(index, val);
        }
    }

    void Update()
    {        
        //(KeyCode)10 -> acbox虚拟机的遥控器确定键消息.
        if (Input.GetKeyDown(KeyCode.KeypadEnter)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCodeTV.PadEnter01)
            || Input.GetKeyDown(KeyCodeTV.PadEnter02)
            || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            //遥控器的确定键消息.
            ClickTVYaoKongEnterBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.KeypadEnter)
            || Input.GetKeyUp(KeyCode.Return)
            || Input.GetKeyUp(KeyCodeTV.PadEnter01)
            || Input.GetKeyUp(KeyCodeTV.PadEnter02)
            || Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            //遥控器的确定键消息.
            ClickTVYaoKongEnterBt(ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //接收遥控器的返回键/键盘上的Esc按键信息.
            ClickTVYaoKongExitBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI == null)
            {
                //创建退出游戏的窗口.
                SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnExitGameDlg();
            }
            else
            {
                //接收遥控器的返回键/键盘上的Esc按键信息.
                ClickTVYaoKongExitBt(ButtonState.UP);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            //接收遥控器/键盘上的向左按键信息.
            ClickTVYaoKongLeftBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.Keypad4))
        {
            //接收遥控器/键盘上的向左按键信息.
            ClickTVYaoKongLeftBt(ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            //接收遥控器/键盘上的向右按键信息.
            ClickTVYaoKongRightBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.Keypad6))
        {
            //接收遥控器/键盘上的向右按键信息.
            ClickTVYaoKongRightBt(ButtonState.UP);
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            //接收遥控器/键盘上的向上按键信息.
            ClickTVYaoKongUpBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Keypad2))
        {
            //接收遥控器/键盘上的向上按键信息.
            ClickTVYaoKongUpBt(ButtonState.UP);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            //接收遥控器/键盘上的向下按键信息.
            ClickTVYaoKongDownBt(ButtonState.DOWN);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.Keypad8))
        {
            //接收遥控器/键盘上的向下按键信息.
            ClickTVYaoKongDownBt(ButtonState.UP);
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