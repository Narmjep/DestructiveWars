using System.Threading;
using life;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Config;
using System.Reflection;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEngine;
using System;
using HarmonyLib;



namespace DestructiveWars{
    public class Effects{

        public static float flamingArrowChance = 0.2f;
        public static float pyromaniacChance = 0.3f;
        public static float grenadeChance = 0.1f;

        public static bool ArrowSmallExplosion(BaseSimObject pSelf, BaseSimObject pTarget = null, WorldTile pTile = null){
            MapAction.damageWorld(pTile, 2, AssetManager.terraform.get("grenade"), null);
            EffectsLibrary.spawnAtTileRandomScale("fx_explosion_small", pTile, 0.03f, 0.06f);
            return true;
        }

        public static bool ArrowImpact(BaseSimObject pSelf, BaseSimObject pTarget = null, WorldTile pTile = null){
            //random
            if (Toolbox.randomChance(flamingArrowChance)) return true;
            return ActionLibrary.burnTile(pSelf, pTarget, pTile);
        }
    	
        [HarmonyPostfix]
        [HarmonyPatch(typeof(City), "tryToMakeWarrior")]
        public static void tryToMakeWarriorPatch(ref bool __result, Actor pActor){
            if (!__result) {
                return;
            }

            if (Toolbox.randomChance(pyromaniacChance)){
                pActor.addTrait("pyromaniac");
            } else if (Toolbox.randomChance(grenadeChance)){
                pActor.addTrait("bomberman");
            }
            __result = true;
        }

        public static void init(){
            //Get arrow projectile
            ProjectileAsset t = AssetManager.projectiles.get("arrow");
            t.impact_actions = (AttackAction)Delegate.Combine(t.impact_actions, new AttackAction(ArrowImpact));
            t.hitShake = false;
        } 


        //Debug

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActionLibrary), nameof(ActionLibrary.pyromaniacEffect))]
        public static bool pyromaniacEffectPatch(BaseSimObject pTarget, WorldTile pTile){
            if (pTarget.a.has_attack_target) return true;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActionLibrary), nameof(ActionLibrary.bombermanEffect))]
        public static bool bombermanEffectPatch(BaseSimObject pTarget, WorldTile pTile){
            if (pTarget.a.has_attack_target) return true;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), nameof(MapBox.startShake))]
        public static bool removeShake(float pDuration = 0.3f, float pInterval = 0.01f, float pIntensity = 2f, bool pShakeX = false, bool pShakeY = true){
            return false;
        }
    }
}