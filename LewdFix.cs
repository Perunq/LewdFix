using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static TextureReplace;

namespace LewdFix
{
    internal static class ModInfo
    {
        internal const string Guid = "perunq.elinplugins.lewdfix";
        internal const string Name = "LewdFix";
        internal const string Version = "1.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class LewdFix : BaseUnityPlugin
    {
        internal static LewdFix Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            Log("My mod...loaded?!");
            var harmony = new Harmony("perunq.elinplugins.lewdfix");
            harmony.PatchAll();

        }

        public static void Log(object payload)
        {
            Instance.Logger.LogInfo(payload);
        }
    }


    [HarmonyPatch]
    internal class FuckConsequences
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(AI_Fuck), nameof(AI_Fuck.Finish))]
        internal static IEnumerable<CodeInstruction> OnFinishIl(IEnumerable<CodeInstruction> instructions)
        {
            //instructions.Do(Debug.Log);
            return new CodeMatcher(instructions)
                .MatchEndForward(
                    new CodeMatch(o => o.opcode == OpCodes.Callvirt &&
                                       o.operand.ToString().Contains(nameof(ConDisease))))
                .RemoveInstruction()
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Pop),
                    new CodeInstruction(OpCodes.Pop))
                .InstructionEnumeration();
        }
    }


    [HarmonyPatch]
    internal class HobbiesAndFavorites
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Chara), nameof(Chara.GetFavCat))]
        static void PostFix1(Chara __instance, ref SourceCategory.Row __result)
        {
            if (!__instance.IsPC)
            {
                return;
            }
            var result=SourceManager.sources.categories.GetRow("sword");
            __result = result;

        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(Chara), nameof(Chara.GetFavFood))]
        static void PostFix2(Chara __instance, ref SourceThing.Row __result)
        {
            if (!__instance.IsPC)
            {
                return;
            }
            var result = SourceManager.sources.things.GetRow("110");
            __result = result;

        }


        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(Biography), nameof(Biography.Generate))]
        //static void PostFix3(Biography __instance)
        //{



        //}



        //public void EditAppearance(UICharaMaker uiCM)
        //{
        //    Dialog.InputName("dialogChangeAppearance", uiCM.chara.c_altName.IsEmpty(uiCM.chara.NameSimple), delegate (bool cancel, string text)
        //    {
        //        if (!cancel)
        //        {
        //            int height = text.ToInt();
        //            uiCM.bio.height=(height);
        //            uiCM.bio.weight= (height * height * (EClass.rnd(6) + 18) / 10000);
        //            uiCM.Refresh();
        //        }
        //    }, Dialog.InputType.Default).SetOnKill(delegate
        //    {
        //        EMono.ui.hud.hint.Show("hintEmbarkTop".lang(), false);
        //    });
        //}
        ////public void OnEndEditAppearance(UICharaMaker uiCM)
        ////{
        ////    uiCM.chara.c_altName = uiCM.inputName.text;
       
    }


}
