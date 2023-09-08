using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HGame : MonoBehaviour
{
    // 进度条 image
    public Image m_Image;
    // 显示的进度文字 100%
    public Text m_Text;
    // 控制进度
    float m_CurProgressValue = 0;
    float m_TargetProgressValue = 50; // 目标进度值
    float m_ProgressSpeed = 1.0f; // 进度条移动速度
    float progressSpeed;

    private float totalScrollAmount = 0.0f; // 记录滚动总幅度
    public float ScrollOneLow = 2.0f;
    public float ScrollOneHigh = 10.0f;

    void Update()
    {
        // 获取鼠标滚轮滚动输入
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            // 获取本次滚动的幅度并添加到总幅度上
            totalScrollAmount += scrollInput;
        if (scrollInput != 0) {
            Debug.Log(totalScrollAmount);
        }

            // 在达到阈值时执行操作
        if (totalScrollAmount >= ScrollOneLow && totalScrollAmount <= ScrollOneHigh)
        {
           // 计算进度条移动的速度，根据滚轮滚动的总幅度
           progressSpeed = m_ProgressSpeed + 2;
           Debug.Log("找到甜蜜點");
        }
        else
        {
           progressSpeed = m_ProgressSpeed;
        }
        
    

        // 更新进度条的当前值
        m_CurProgressValue += progressSpeed * Time.deltaTime;

        // 限制进度值在0到目标值之间
        m_CurProgressValue = Mathf.Clamp(m_CurProgressValue, 0, m_TargetProgressValue);

        // 实时更新进度百分比的文本显示 
        m_Text.text = Mathf.RoundToInt(m_CurProgressValue) + "%";
        // 实时更新滑动进度图片的fillAmount值  
        m_Image.fillAmount = m_CurProgressValue / m_TargetProgressValue / 2;

        if (m_CurProgressValue == m_TargetProgressValue)
        {
            m_Text.text = "OK";
            // 这一块可以写上场景加载的脚本
        }
    }
}
