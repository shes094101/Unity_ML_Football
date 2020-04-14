using UnityEngine;

public class ball : MonoBehaviour
{
    /// <summary>
    /// 足球是否進入球門
    /// </summary>
    public static bool complete;


    /// <summary>
    /// 觸發開始事件,球碰到感應區執行一次
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //碰到的物件名稱是感應區的話
        if (other.name == "touchRange") {
            //進入球門
            complete = true;
        }
    }
}
