using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : Singleton<MatchController> {

    public MatchSession match { get; private set; }
	
    public void StartSession()
    {
        match = new MatchSession(5,5);
    }


}
