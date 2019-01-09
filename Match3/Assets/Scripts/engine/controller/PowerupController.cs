using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : Singleton<PowerupController> {

    public enum POWERUP_TYPE{
        ONE_LINE_PW,
        ONE_COLLUMN_PW,
        ONE_TYPE_PW
    }

    public List<Piece> ExecutePowerup(POWERUP_TYPE type, Piece p)
    {
        if(type==POWERUP_TYPE.ONE_LINE_PW)
        {
            return  MatchController.ME.ExecutePowerup(new OneLinePw().ExecutePowerup(p));
        }

        if (type == POWERUP_TYPE.ONE_COLLUMN_PW)
        {
            return MatchController.ME.ExecutePowerup(new OneCollumnPw().ExecutePowerup(p));
        }

        if (type == POWERUP_TYPE.ONE_TYPE_PW)
        {
            return MatchController.ME.ExecutePowerup(new OneTypePw().ExecutePowerup(p));
        }


        return null;
    }
}
