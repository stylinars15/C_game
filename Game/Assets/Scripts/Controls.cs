using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Transform move;
    public Transform jump;
    public Transform defend;
    public Transform attack;
    public Transform s_attack;
    public Transform wait_attack;
    public PlayerCombat playerCombat;
    public PlayerController playerController;
    
    private enum State
    {
        Move,
        Jump,
        Defend,
        Attack,
        W_Attack,
        S_Attack,
        Done
    }

    private State currentState = State.Move;

    private void Start()
    {
        Deactivate();
    }

    void FixedUpdate()
    {
        if (!playerController._canMove)
        {
            move.gameObject.SetActive(false);
            Deactivate();
            this.enabled = false;
        }
        switch (currentState)
        {
            case State.Move:
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
                {
                    move.gameObject.SetActive(false);
                    attack.gameObject.SetActive(true);
                    currentState = State.Attack; // Move to next state
                }
                break;

            case State.Attack:
                if (Input.GetKey(KeyCode.Space))
                {
                    attack.gameObject.SetActive(false);
                    defend.gameObject.SetActive(true);
                    currentState = State.Defend; // Move to next state
                }
                break;

            case State.Defend:
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    defend.gameObject.SetActive(false);
                    jump.gameObject.SetActive(true);
                    currentState = State.Jump; // Move to next state
                }
                break;

            case State.Jump:
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    jump.gameObject.SetActive(false);
                    wait_attack.gameObject.SetActive(true);
                    currentState = State.W_Attack; // Presumably move to S_Attack
                }
                break;

            case State.W_Attack:
                if (playerCombat._resolveBuildUp > 99)
                {
                    wait_attack.gameObject.SetActive(false);
                    s_attack.gameObject.SetActive(true);
                    currentState = State.S_Attack; // 
                }
                break;
            case State.S_Attack:
                if (Input.GetKey(KeyCode.S))
                {
                    s_attack.gameObject.SetActive(false);
                    currentState = State.Done;
                }
                break;
            case State.Done:
                this.enabled = false; // Disables this script
                break;
        }
    }

    private void Deactivate()
    {
        jump.gameObject.SetActive(false);
        defend.gameObject.SetActive(false);
        s_attack.gameObject.SetActive(false);
        attack.gameObject.SetActive(false);
        wait_attack.gameObject.SetActive(false);
    }
}
