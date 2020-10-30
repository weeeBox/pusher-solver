using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;


[RequireComponent(typeof(GameLevel))]
public class PushAgent : Agent
{
    private const int ACTION_UP = 1;
    private const int ACTION_DOWN = 2;
    private const int ACTION_LEFT = 3;
    private const int ACTION_RIGHT = 4;

    private GameLevel _level;

    public override void Initialize()
    {
        _level = gameObject.GetComponent<GameLevel>();
    }

    public override void OnEpisodeBegin()
    {
        _level.ResetLevel();
    }

    public override void Heuristic(float[] actionsOut)
    {
        int action = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            action = ACTION_UP;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            action = ACTION_DOWN;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            action = ACTION_LEFT;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            action = ACTION_RIGHT;
        }

        actionsOut[0] = action;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        int action = Mathf.RoundToInt(vectorAction[0]);
        switch (action)
        {
            case ACTION_UP:
                _level.MoveUp();
                break;
            case ACTION_DOWN:
                _level.MoveDown();
                break;
            case ACTION_LEFT:
                _level.MoveLeft();
                break;
            case ACTION_RIGHT:
                _level.MoveRight();
                break;
        }
    }
}