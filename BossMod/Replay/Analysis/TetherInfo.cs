﻿using ImGuiNET;
using Lumina.Excel.Sheets;
using System.Text;

namespace BossMod.ReplayAnalysis;

class TetherInfo : CommonEnumInfo
{
    private class TetherData
    {
        public HashSet<uint> SourceOIDs = [];
        public HashSet<uint> TargetOIDs = [];
    }

    private readonly Type? _tidType;
    private readonly Dictionary<uint, TetherData> _data = [];

    public TetherInfo(List<Replay> replays, uint oid)
    {
        var moduleInfo = BossModuleRegistry.FindByOID(oid);
        _oidType = moduleInfo?.ObjectIDType;
        _tidType = moduleInfo?.TetherIDType;
        foreach (var replay in replays)
        {
            foreach (var enc in replay.Encounters.Where(enc => enc.OID == oid))
            {
                foreach (var tether in replay.EncounterTethers(enc))
                {
                    var data = _data.GetOrAdd(tether.ID);
                    data.SourceOIDs.Add(tether.Source.OID);
                    data.TargetOIDs.Add(tether.Target.OID);
                }
            }
        }
    }

    public void Draw(UITree tree)
    {
        UITree.NodeProperties map(KeyValuePair<uint, TetherData> kv)
        {
            var name = _tidType?.GetEnumName(kv.Key);
            return new($"{kv.Key} ({name})", false, name == null ? Colors.TextColor2 : Colors.TextColor1);
        }
        foreach (var (tid, data) in tree.Nodes(_data, map))
        {
            tree.LeafNode($"Source IDs: {OIDListString(data.SourceOIDs)}");
            tree.LeafNode($"Target IDs: {OIDListString(data.TargetOIDs)}");
            tree.LeafNode($"VFX: {Service.LuminaRow<Channeling>(tid)?.File}");
        }
    }

    public void DrawContextMenu()
    {
        if (ImGui.MenuItem("Generate enum for boss module"))
        {
            var sb = new StringBuilder("public enum TetherID : uint\n{\n");
            foreach (var (tid, data) in _data)
                sb.Append($"    {EnumMemberString(tid, data)}\n");
            sb.Append("}\n");
            ImGui.SetClipboardText(sb.ToString());
        }

        if (ImGui.MenuItem("Generate missing enum values for boss module"))
        {
            var sb = new StringBuilder();
            foreach (var (tid, data) in _data.Where(kv => _tidType?.GetEnumName(kv.Key) == null))
                sb.AppendLine(EnumMemberString(tid, data));
            ImGui.SetClipboardText(sb.ToString());
        }
    }

    private string EnumMemberString(uint tid, TetherData data)
    {
        var name = _tidType?.GetEnumName(tid) ?? $"_Gen_Tether_{tid}";
        return $"{name} = {tid}, // {OIDListString(data.SourceOIDs)}->{OIDListString(data.TargetOIDs)}";
    }
}
