using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController{

    private float[] timeIntervals;
    private bool gravity;

    private List<RapidFireKey> rapidFireKeys;
    RapidFireKey temp;

    public class RapidFireKey
    {
        public bool[] stages;
        public bool gravityInfluenced;
        public bool gravityDir;

        public KeyCode key;
        public int xdir;
        public int ydir;
        public float time;
        

        public RapidFireKey(KeyCode _key,int _xdir, int _ydir)
        {
            key = _key;
            stages = new bool[2] { false, false };
            xdir = _xdir;
            ydir = _ydir;
            if (ydir == 0) {
                gravityInfluenced = false;
                gravityDir = false;
            }
            else {
                gravityInfluenced = true;
                gravityDir = ydir > 0 ? false : true;
            }
            
            time = Time.time;

        }
    }



    public InputController()
    {

        timeIntervals = new float[2] { 0.2f, .045f };

        rapidFireKeys = new List<RapidFireKey>(4) {
            new RapidFireKey(KeyCode.LeftArrow, -1, 0),
            new RapidFireKey(KeyCode.RightArrow, 1, 0),
            new RapidFireKey(KeyCode.UpArrow, 0, 1),
            new RapidFireKey(KeyCode.DownArrow, 0, -1)
        };
    }

	public void GetInput () {
        gravity = GameManager.instance.gravity;

        rapidFireKeys.ForEach(EvaluateRapidFire);


        if (Input.GetKeyDown("w")) { GameManager.instance.gravity = false; }
        if (Input.GetKeyDown("s")) { GameManager.instance.gravity = true; }
        if (Input.GetKeyDown("a")) { GameManager.instance.AttemptRotate(90); }
        if (Input.GetKeyDown("d")) { GameManager.instance.AttemptRotate(-90); }
    }

    private void EvaluateRapidFire(RapidFireKey assignedKey)
    {

        if (Input.GetKeyUp(assignedKey.key) || !Input.GetKey(assignedKey.key))
        {
            assignedKey.stages[0] = false;
            assignedKey.stages[1] = false;
        }

        if ((!assignedKey.gravityInfluenced) || assignedKey.gravityDir == gravity)
        {
            if (!assignedKey.stages[0] && Input.GetKey(assignedKey.key))
            {
                GameManager.instance.AttemptMove(assignedKey.xdir, assignedKey.ydir);
                assignedKey.stages[0] = true;
                assignedKey.time = Time.time;
            } else if (assignedKey.stages[0] && !assignedKey.stages[1] && ((Time.time - assignedKey.time) > timeIntervals[0]))
            {
                assignedKey.stages[1] = true;
                assignedKey.time += timeIntervals[0];
                GameManager.instance.AttemptMove(assignedKey.xdir, assignedKey.ydir);
                
            } else if (assignedKey.stages[0] && assignedKey.stages[1] && ((Time.time - assignedKey.time) > timeIntervals[1]))
            {
                assignedKey.time += timeIntervals[1];
                GameManager.instance.AttemptMove(assignedKey.xdir, assignedKey.ydir);
                
            }

        }


    }
}
