﻿using System.Collections.Generic;
using System.Linq;

namespace BossMod.Endwalker.Ultimate.DSW2
{
    // TODO: hints?..
    class P4Hatebound : BossComponent
    {
        public enum Color { None, Red, Blue }

        private List<(Actor orb, Color color, bool exploded)> _orbs = new(); // 'red' is actually 'yellow orb'
        private Color[] _playerColors = new Color[PartyState.MaxPartySize];

        public bool ColorReady(Color c) => _orbs.Any(o => o.color == c && OrbReady(o.orb));
        public bool YellowReady => ColorReady(Color.Red);
        public bool BlueReady => ColorReady(Color.Blue);

        public override void DrawArenaForeground(BossModule module, int pcSlot, Actor pc, MiniArena arena)
        {
            foreach (var o in _orbs.Where(o => !o.exploded))
            {
                arena.Actor(o.orb, ArenaColor.Object, true);
                if (OrbReady(o.orb))
                    arena.AddCircle(o.orb.Position, 6, _playerColors[pcSlot] == Color.Red ? ArenaColor.Safe : ArenaColor.Danger);
            }
        }

        public override void OnActorCreated(BossModule module, Actor actor)
        {
            Color color = (OID)actor.OID switch
            {
                OID.AzurePrice => Color.Blue,
                OID.GildedPrice => Color.Red,
                _ => Color.None
            };
            if (color != Color.None)
                _orbs.Add((actor, color, false));
        }

        public override void OnStatusGain(BossModule module, Actor actor, ActorStatus status)
        {
            var color = (SID)status.ID switch
            {
                SID.Clawbound => Color.Red,
                SID.Fangbound => Color.Blue,
                _ => Color.None
            };
            if (color != Color.None && module.Raid.FindSlot(actor.InstanceID) is var slot && slot >= 0)
                _playerColors[slot] = color;
        }

        public override void OnEventCast(BossModule module, Actor caster, ActorCastEvent spell)
        {
            if ((AID)spell.Action.ID is AID.FlareStar or AID.FlareNova or AID.FlareNovaFail && _orbs.FindIndex(o => o.orb == caster) is var index && index >= 0)
                _orbs.AsSpan()[index].exploded = true;
        }

        private bool OrbReady(Actor orb) => orb.HitboxRadius > 1.501f; // TODO: verify...
    }
}
