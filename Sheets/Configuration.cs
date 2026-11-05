using System;
using System.Collections.Generic;
using System.IO;
using Faust;
using Dalamud.Configuration;
using Faust.Base.Global;
using FFXIVClientStructs;
using FFXIVClientStructs.FFXIV.Client.Graphics;
using Lumina.Excel.Sheets;
using Newtonsoft.Json;
#pragma warning disable CA1416 // Validate platform compatibility

namespace Faust.Sheets;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public class JobArray (bool gla = false, bool mrd = false, bool cnj = false, bool pgl = false, bool lnc = false,
        bool rog = false, bool arc = false, bool thm = false, bool acn = false, bool pld = false, bool war = false,
        bool drk = false, bool gnb = false, /*bool bas = false,*/ bool whm = false, bool sch = false, bool ast = false, bool sge = false,
        bool mnk = false, bool drg = false, bool nin = false, bool sam = false, bool rpr = false, bool vpr = false,
        bool bst = false, bool brd = false, bool mch = false, bool dnc = false, /*bool zph = false,*/  bool blm = false, bool smn = false,
        bool rdm = false, bool pct = false, bool blu = false, bool crp = false, bool bsm = false, bool arm = false,
        bool gsm = false, bool ltw = false, bool wvr = false, bool alc = false, bool cul = false, bool min = false,
        bool btn = false, bool fsh = false)
    {
        public bool GLA = gla;
        public bool MRD = mrd;

        public bool CNJ = cnj;

        public bool PGL = pgl;
        public bool LNC = lnc;
        public bool ROG = rog;

        public bool ARC = arc;

        public bool THM = thm;
        public bool ACN = acn;


        public bool PLD = pld;
        public bool WAR = war;
        public bool DRK = drk;
        public bool GNB = gnb;
        //public bool BAS = bas;


        public bool WHM = whm;
        public bool SCH = sch;
        public bool AST = ast;
        public bool SGE = sge;


        public bool MNK = mnk;
        public bool DRG = drg;
        public bool NIN = nin;
        public bool SAM = sam;
        public bool RPR = rpr;
        public bool VPR = vpr;
        public bool BST = bst;

        public bool BRD = brd;
        public bool MCH = mch;
        public bool DNC = dnc;
        //public bool ZPH = zph;

        public bool BLM = blm;
        public bool SMN = smn;
        public bool RDM = rdm;
        public bool PCT = pct;
        public bool BLU = blu;


        public bool CRP = crp;
        public bool BSM = bsm;
        public bool ARM = arm;
        public bool GSM = gsm;
        public bool LTW = ltw;
        public bool WVR = wvr;
        public bool ALC = alc;
        public bool CUL = cul;

        public bool MIN = min;
        public bool BTN = btn;
        public bool FSH = fsh;

        public bool CheckJob(uint job)
        {
            switch (job)
            {
                case 1: return this.GLA;
                case 3: return this.MRD;
                case 6: return this.CNJ;
                case 2: return this.PGL;
                case 4: return this.LNC;
                case 29: return this.ROG;
                case 5: return this.ARC;
                case 7: return this.THM;
                case 26: return this.ACN;
                case 19: return this.PLD;
                case 21: return this.WAR;
                case 32: return this.DRK;
                case 37: return this.GNB;
                //case "BAS": return this.BAS;
                case 24: return this.WHM;
                case 28: return this.SCH;
                case 33: return this.AST;
                case 40: return this.SGE;
                case 20: return this.MNK;
                case 22: return this.DRG;
                case 30: return this.NIN;
                case 34: return this.SAM;
                case 39: return this.RPR;
                case 41: return this.VPR;
                case 43: return this.BST;
                case 23: return this.BRD;
                case 31: return this.MCH;
                case 38: return this.DNC;
                //case "ZPH": return this.ZPH;
                case 25: return this.BLM;
                case 27: return this.SMN;
                case 35: return this.RDM;
                case 42: return this.PCT;
                case 36: return this.BLU;
                case 8: return this.CRP;
                case 9: return this.BSM;
                case 10: return this.ARM;
                case 11: return this.GSM;
                case 12: return this.LTW;
                case 13: return this.WVR;
                case 14: return this.ALC;
                case 15: return this.CUL;
                case 16: return this.MIN;
                case 17: return this.BTN;
                case 18: return this.FSH;
                default:
                    {
                        Service.Log.Info(job + " does not exist in checkjob switch");
                        return false;
                    }
            }
        }

        public void SetAll(bool value)
        {
        this.GLA = value;
        this.MRD = value;

        this.CNJ = value;

        this.PGL = value;
        this.LNC = value;
        this.ROG = value;

        this.ARC = value;

        this.THM = value;
        this.ACN = value;


        this.PLD = value;
        this.WAR = value;
        this.DRK = value;
        this.GNB = value;
        //this.BAS = value;


        this.WHM = value;
        this.SCH = value;
        this.AST = value;
        this.SGE = value;


        this.MNK = value;
        this.DRG = value;
        this.NIN = value;
        this.SAM = value;
        this.RPR = value;
        this.VPR = value;
        this.BST = value;

        this.BRD = value;
        this.MCH = value;
        this.DNC = value;
        //this.ZPH = value;

        this.BLM = value;
        this.SMN = value;
        this.RDM = value;
        this.PCT = value;
        this.BLU = value;


        this.CRP = value;
        this.BSM = value;
        this.ARM = value;
        this.GSM = value;
        this.LTW = value;
        this.WVR = value;
        this.ALC = value;
        this.CUL = value;

        this.MIN = value;
        this.BTN = value;
        this.FSH = value;
        }
        
    }
    //TOSETUP: add new dictionary here
    public class ReplacementSet(string name, bool enabled, int priority,
    JobArray jobarray,
    string charactername,
    uint homeworld,
    Dictionary<uint, ActionTransientsConfig> actiontransientswriter,
    Dictionary<uint, CompanionDetourConfig> Companiondetourwriter,
    Dictionary<uint, CompanionTransientsConfig> companiontransientswriter,
    Dictionary<uint, JobTransientsConfig> jobtransientswriter,
    Dictionary<uint, MountLightConfig> mountlightwriter,
    Dictionary<uint, MountDetourConfig> mountdetourwriter,
    Dictionary<uint, MountTransientsConfig> mounttransientswriter,
    Dictionary<uint, OrnamentTransientsConfig> ornamenttransientswriter,
    Dictionary<uint, StatusTransientsConfig> statustransientswriter,
    Dictionary<uint, TraitTransientsConfig> traittransientswriter)
    {
        public string Name = name;

        public JobArray Jobs = jobarray;

        public bool Enabled = enabled;

        public int Priority = priority;

        public string CharacterName = charactername;

        public uint HomeWorld = homeworld;

        //TOSETUP: add new dictionary here
        public Dictionary<uint, ActionTransientsConfig> ActionTransientsWriter { get; set; } = actiontransientswriter;
        public Dictionary<uint, CompanionDetourConfig> CompanionDetourWriter { get; set; } = Companiondetourwriter;
        public Dictionary<uint, CompanionTransientsConfig> CompanionTransientsWriter { get; set; } = companiontransientswriter;
        public Dictionary<uint, JobTransientsConfig> JobTransientsWriter { get; set; } = jobtransientswriter;
        public Dictionary<uint, MountDetourConfig> MountDetourWriter { get; set; } = mountdetourwriter;
        public Dictionary<uint, MountLightConfig> MountLightWriter { get; set; } = mountlightwriter;
        public Dictionary<uint, MountTransientsConfig> MountTransientsWriter { get; set; } = mounttransientswriter;
        public Dictionary<uint, OrnamentTransientsConfig> OrnamentTransientsWriter { get; set; } = ornamenttransientswriter;
        public Dictionary<uint, StatusTransientsConfig> StatusTransientsWriter { get; set; } = statustransientswriter;
        public Dictionary<uint, TraitTransientsConfig> TraitTransientsWriter { get; set; } = traittransientswriter;
        
        public static ReplacementSet? Load(string jsonFile)
        {
            try
            {
                ReplacementSet? replacements =
                    JsonConvert.DeserializeObject<ReplacementSet>(
                        File.ReadAllText(jsonFile));

                if (replacements == null) return null;

                replacements.CharacterName = Service.PlayerState.CharacterName;
                replacements.HomeWorld = Service.PlayerState.HomeWorld.RowId;

                return replacements;
            }
            catch
            {
                return null;
            }
        }

        public static bool Save(string jsonFile, int index)
        {
            //scrub out player name and homeworld to prevent leakage
            ReplacementSet replacements = Service.Config.ReplacementSets[index];
            replacements.CharacterName = "";
            replacements.HomeWorld = 0;
            try
            {
                File.WriteAllText(jsonFile, JsonConvert.SerializeObject(replacements));
                return true;
            }
            catch
            {
                Service.Log.Error("savefailed for set" + index);
                return false;
            }
        }
    }

    public int Version { get; set; } = 1;

    public bool EnableReplacement = true;

    public bool devLogging = false;

    public List<ReplacementSet> ReplacementSets { get; set; } = [];

    internal void Load()
    {
        Service.Config = Service.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
    }
    internal void Save()
    {
        //File.WriteAllText(Service.PluginInterface.ConfigFile.FullName, JsonConvert.SerializeObject(Service.Config));

        Service.PluginInterface.SavePluginConfig(this);
    }
}