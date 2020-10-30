using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(GameLevel))]
public class PushAgent : Agent
{
    private const int ACTION_UP = 0;
    private const int ACTION_DOWN = 1;
    private const int ACTION_LEFT = 2;
    private const int ACTION_RIGHT = 3;
    
    private GameLevel _level;

    public override void Initialize()
    {
        _level = gameObject.GetComponent<GameLevel>();
    }

    private void Update()
    {
        RequestDecision();
    }

    public override void OnEpisodeBegin()
    {
        _level.ResetLevel();
    }

    public override void Heuristic(float[] actionsOut)
    {
        int action = -1;
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
        // player pos (2)
        sensor.AddObservation(_level.playerI);
        sensor.AddObservation(_level.playerJ);

        // level state (11x8)
        for (int i = 0; i < _level.rows; ++i)
        {
            for (int j = 0; j < _level.cols; ++j)
            {
                sensor.AddObservation((int) _level[i, j]);
            }
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        int action = Mathf.RoundToInt(vectorAction[0]);
        bool changed = false;
        switch (action)
        {
            case ACTION_UP:
                changed = _level.MoveUp();
                break;
            case ACTION_DOWN:
                changed = _level.MoveDown();
                break;
            case ACTION_LEFT:
                changed = _level.MoveLeft();
                break;
            case ACTION_RIGHT:
                changed = _level.MoveRight();
                break;
        }
        if (changed)
        {
            AddReward(0.01f);
        }
    }
}