﻿namespace RTS;

public partial class Slime
{
    GTimer timerIdle;

    State Idle()
    {
        var state = new State("Idle");

        state.Enter = () =>
        {
            sprite.Play("idle");
            timerIdle = new GTimer(this, DurationIdle);
            timerIdle.Finished += () => SwitchState(PreJump());
            timerIdle.Start();
        };

        return state;
    }
}
