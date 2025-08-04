using System;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;


namespace DestructiveWars{
    [ModEntry]
    class Main : MonoBehaviour{

        public const string VERSION = "1.1";

        public static Main instance;

        internal static Harmony harmony;

        void Awake(){
            Debug.Log($"{Mod.Info.Name} loaded!");
            //Harmony
            harmony = new Harmony(Mod.Info.Name);
            Harmony.CreateAndPatchAll(typeof(Effects));
            Debug.Log(" ----------------- Patching done");

            Config.Init();
            Effects.init();
            Debug.Log(" \n\n\n\n\n\nEffects initialized");
        }
    }
}