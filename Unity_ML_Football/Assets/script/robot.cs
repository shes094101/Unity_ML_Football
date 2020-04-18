using UnityEngine;
using MLAgents;
using MLAgents.Sensors;

public class robot : Agent
{
    [Header("速度"), Range(1, 50)]
    public float speed = 10;

    /// <summary>
    /// 機器人剛體
    /// </summary>
    private Rigidbody rigRobot;
    /// <summary>
    /// 足球剛體
    /// </summary>
    private Rigidbody rigBall;

    /// <summary>
    /// 遊戲一開始就獲取兩者的剛體
    /// </summary>
    private void Start()
    {
        rigRobot = GetComponent<Rigidbody>();
        rigBall = GameObject.Find("Soccer Ball").GetComponent<Rigidbody>();
    }


    /// <summary>
    /// 事件開始時:重新設定機器人與足球位置
    /// </summary>
    public override void OnEpisodeBegin()
    {
        //直線加速度與角加速度歸零
        rigRobot.velocity = Vector3.zero;
        rigRobot.angularVelocity = Vector3.zero;
        rigBall.velocity = Vector3.zero;
        rigBall.angularVelocity = Vector3.zero;

        //隨機位置:機器人跟球
        Vector3 posRobot = new Vector3(Random.Range(-2f, 2f), 0.1f, Random.Range(-2f, 0f));
        //指定座標
        transform.position = posRobot;

        Vector3 posBall = new Vector3(Random.Range(-0f, 0f), 0.1f, Random.Range(-1f, 2f));
        rigBall.position = posBall;

        //足球未進門
        ball.complete = false;
    }


    /// <summary>
    /// 收集觀測資料
    /// </summary>
    public override void CollectObservations(VectorSensor sensor)
    {
        //觀測資料:機器人、足球座標、機器人加速度、x、z
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rigBall.position);
        sensor.AddObservation(rigRobot.velocity.x);
        sensor.AddObservation(rigRobot.velocity.z);
    }

    /// <summary>
    /// 控制機器人與回饋
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)
    {
        //用參數控制機器人
        Vector3 control = Vector3.zero;
        control.x = vectorAction[0];
        control.z = vectorAction[1];
        rigRobot.AddForce(control * speed);


        //成功+1分(球進球門)
        if (ball.complete) {
            SetReward(1);
            EndEpisode();
        }

        //失敗-1分(人或球掉到場外)
        if (transform.position.y < 0 || rigBall.position.y < 0) {
            SetReward(-1);
            EndEpisode();
        }
    }

    /// <summary>
    /// 開發者測試環境
    /// </summary>
    /// <returns></returns>
    public override float[] Heuristic()
    {
        //提供開發者控制
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }

}
