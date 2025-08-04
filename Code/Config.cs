using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

namespace DestructiveWars
{
    public static class Config
    {
        private const string CONFIG_PATH = "destructivewars_config.json";

        public const string FLAMING_ARROW_CHANCE = "flamingArrowChance";
        public const string ENABLE_FLAMING_ARROW = "enableFlamingArrow";
        public const string EXPLOSIVE_ARROW_CHANCE = "explosiveArrowChance";
        public const string ENABLE_EXPLOSIVE_ARROW = "enableExplosiveArrow";
        public const string BOMBERMAN_CHANCE = "bombermanChance";
        public const string ENABLE_BOMBERMAN = "enableBomberman";
        public const string REMOVE_SHAKE = "removeShake";

        public static float flamingArrowChance = 0.1f;
        public static float explosiveArrowChance = 0.1f;
        public static float bombermanChance = 0.2f;
        public static bool enableFlamingArrow = true;
        public static bool enableExplosiveArrow = true;
        public static bool enableBomberman = true;
        public static bool removeShake = true;

        [System.Serializable]
        private class ConfigData
        {
            public string version = Main.VERSION;
            public float flamingArrowChance = 0.1f;
            public float explosiveArrowChance = 0.1f;
            public float bombermanChance = 0.1f;

            public bool enableFlamingArrow = true;
            public bool enableExplosiveArrow = true;
            public bool enableBomberman = true;
            public bool removeShake = true;
        }

        public static void Init()
        {
            string path = Path.Combine(Application.streamingAssetsPath, CONFIG_PATH);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"Config file not found at {path}, creating default config.");
                SaveDefaults(path);
                return;
            }
            string json = File.ReadAllText(path);

            try
            {
                ConfigData data = JsonUtility.FromJson<ConfigData>(json);

                if (VersionToNumber(data.version) < VersionToNumber(Main.VERSION))
                {
                    Debug.LogWarning($"Config version {data.version} is outdated. Updating to {Main.VERSION}.");
                }

                flamingArrowChance = data.flamingArrowChance;
                explosiveArrowChance = data.explosiveArrowChance;
                bombermanChance = data.bombermanChance;
                enableFlamingArrow = data.enableFlamingArrow;
                enableExplosiveArrow = data.enableExplosiveArrow;
                enableBomberman = data.enableBomberman;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse config file at {path}: {e.Message}");
                SaveDefaults(path);
            }     
        }

        private static void SaveDefaults(string path)
        {
            var defaultConfig = new ConfigData();
            try
            {
               string json = JsonUtility.ToJson(defaultConfig, true);
                File.WriteAllText(path, json); 
            } catch (Exception e)
            {
                Debug.LogError($"Failed to save default config to {path}: {e.Message}");
            } 
        }

        public static int VersionToNumber(string version)
        {
            // remove all dots and convert to int
            try
            {
                int result = int.Parse(version.Replace(".", ""));
                return result;
            } catch (Exception e)
            {
                Debug.LogError($"Failed to convert version '{version}' to number: {e.Message}");
                return 0; // return 0 if conversion fails
            }
        }
    }
}