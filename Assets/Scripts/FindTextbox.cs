using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindTextbox : MonoBehaviour {

    public GameObject GoalArea1,GoalArea2,S_Ball,Redparent;
    public GoalCount goalCount1,goalCount2;
    public GameSetup gameSetup;

	void Start () {
       
 //       GoalArea2 = GameObject.FindGameObjectWithTag("Blueteam");
 //       GoalArea1 = GameObject.FindGameObjectWithTag("Redteam");

 //      goalCount2 = GoalArea2.GetComponent<GoalCount>();
 //       goalCount1 = GoalArea1.GetComponent<GoalCount>();

 //       goalCount2.ScoreText = GameObject.FindWithTag("Bluescore").GetComponent<Text>();
 //       goalCount1.ScoreText = GameObject.FindWithTag("Redscore").GetComponent<Text>();

 //       goalCount2.S_Ball = GameObject.FindGameObjectWithTag("Ball");
 //       goalCount1.S_Ball = GameObject.FindGameObjectWithTag("Ball");

 //      goalCount2.Ballrigidbody = goalCount2.S_Ball.GetComponent<Rigidbody2D>();
 //      goalCount1.Ballrigidbody = goalCount1.S_Ball.GetComponent<Rigidbody2D>();

 //        goalCount2.ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
 //        goalCount1.ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();

 //       goalCount2.isGoal = false;
 //       goalCount1.isGoal = false;

        gameSetup = GameObject.FindGameObjectWithTag("GameSetup").GetComponent<GameSetup>();
        gameSetup.RedParent = GameObject.FindGameObjectWithTag("RedParent");
        gameSetup.IsCreated = false;
	}
	
	// Update is called once per frame
	void Update () {
   //    GoalArea2.GetComponent<GoalCount>().Counttext();
   //    GoalArea1.GetComponent<GoalCount>().Counttext();	
	}
}
