using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    int hour;
    int minute;
    int second;
    int millisecond;
    public Text text_timeSpend;
    public bool success = false;

    // 已经花费的时间
    float timeSpend = 0.0f;

    // 显示时间区域的文本

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (!success)
        {
            timeSpend += Time.deltaTime;
            //GlobalSetting.timeSpent = timeSpend;

            hour = (int)timeSpend / 3600;
            minute = ((int)timeSpend - hour * 3600) / 60;
            second = (int)timeSpend - hour * 3600 - minute * 60;
            millisecond = (int)((timeSpend - (int)timeSpend) * 1000);

            text_timeSpend.text = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hour, minute, second, millisecond);
        }
        else
        {
            text_timeSpend.text = "游戏成功，用时："+string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hour, minute, second, millisecond);
        }
    }
}
