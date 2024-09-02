﻿namespace BossMod.AI;

[ConfigDisplay(Name = "AI Configuration", Order = 6)]
sealed class AIConfig : ConfigNode
{
    // ui settings
    [PropertyDisplay("Enable AI", tooltip: "Disclaimer: AI is very experimental, use at your own risk!")]
    public bool Enabled = false;

    [PropertyDisplay("Show status in DTR bar")]
    public bool ShowDTR = false;

    [PropertyDisplay("Show AI management window")]
    public bool DrawUI = true;

    [PropertyDisplay("Focus target master")]
    public bool FocusTargetLeader = true;

    [PropertyDisplay("Show AI/Navi status on TitleBar")]
    public bool ShowStatusOnTitlebar = true;

    [PropertyDisplay("Broadcast keypresses to other windows")]
    public bool BroadcastToSlaves = false;

    [PropertyDisplay("Follow party slot")]
    public int FollowSlot = 0;

    [PropertyDisplay("Forbid actions")]
    public bool ForbidActions = false;

    [PropertyDisplay("Forbid movement")]
    public bool ForbidMovement = false;

    [PropertyDisplay("Follow during combat")]
    public bool FollowDuringCombat = false;

    [PropertyDisplay("Follow during active boss module")]
    public bool FollowDuringActiveBossModule = false;

    [PropertyDisplay("Follow out of combat")]
    public bool FollowOutOfCombat = false;

    [PropertyDisplay("Follow target")]
    public bool FollowTarget = false;

    [PropertyDisplay("Desired positional when following target")]
    [PropertyCombo(["Any", "Flank", "Rear", "Front"])]
    public Positional DesiredPositional = Positional.Any;

    [PropertyDisplay("Max distance to slot")]
    public float MaxDistanceToSlot = 1;

    [PropertyDisplay("Max distance to target")]
    public float MaxDistanceToTarget = 2.6f;
}
