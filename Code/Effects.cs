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
        
        private static bool RandomChance(double chance)
        {
            return UnityEngine.Random.value < chance;
        }

        public static bool ArrowSmallExplosion(BaseSimObject pSelf, BaseSimObject pTarget = null, WorldTile pTile = null)
        {
            if (!Config.enableExplosiveArrow || !RandomChance(Config.explosiveArrowChance)) return true;
            MapAction.damageWorld(pTile, 2, AssetManager.terraform.get("grenade"), null);
            EffectsLibrary.spawnAtTileRandomScale("fx_explosion_small", pTile, 0.02f, 0.03f);
            return true;
        }

        public static bool ArrowImpact(BaseSimObject pSelf, BaseSimObject pTarget = null, WorldTile pTile = null){
            //random
            if (!Config.enableFlamingArrow || !RandomChance(Config.flamingArrowChance)) return true;
            return ActionLibrary.burnTile(pSelf, pTarget, pTile);
        }
    	
        [HarmonyPostfix]
        [HarmonyPatch(typeof(City), "tryToMakeWarrior")]
        public static void TryToMakeWarriorPatch(ref bool __result, Actor pActor){
            if (!__result) {
                return;
            }
            
            bool set_bomberman = Config.enableBomberman && RandomChance(Config.bombermanChance);
            if (set_bomberman)
            {
                pActor.addTrait("bomberman");
            }

            __result = true;
        }

        public static void init(){
            //Get arrow projectile
            ProjectileAsset t = AssetManager.projectiles.get("arrow");
            t.impact_actions = (AttackAction)Delegate.Combine(t.impact_actions, new AttackAction(ArrowImpact));
            t.impact_actions = (AttackAction)Delegate.Combine(t.impact_actions, new AttackAction(ArrowSmallExplosion));
            if (Config.removeShake)
                t.hit_shake = false;
        } 


        //Debug

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActionLibrary), nameof(ActionLibrary.throwBombAtTile))]
        public static bool bombermanEffectPatch(BaseSimObject pSelf, WorldTile pTile){
            if (pSelf.a.has_attack_target) return true;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), nameof(MapBox.startShake))]
        public static bool RemoveShake(float pDuration = 0.3f, float pInterval = 0.01f, float pIntensity = 2f, bool pShakeX = false, bool pShakeY = true)
        {
            if (Config.removeShake)
            {
                return false;
            }
            return true;
        }
    }
}