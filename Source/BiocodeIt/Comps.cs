using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using Verse.AI;

namespace Krelinos_BiocodeIt
{
    // Then next two classes used the Infused mod as a base and guide
    // https://steamcommunity.com/sharedfiles/filedetails/?id=731287727
    public class CompTargetable_Equipment : CompTargetable
    {
        protected override bool PlayerChoosesTarget => true;
        public override IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null)
        {
            yield return targetChosenByPlayer;
        }

        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            return base.CompFloatMenuOptions(selPawn);
        }

        protected override TargetingParameters GetTargetingParameters()
        {
            return new TargetingParameters
            {
                canTargetPawns = false,
                canTargetBuildings = false,
                canTargetAnimals = false,
                canTargetHumans = false,
                canTargetMechs = false,
                mapObjectTargetsMustBeAutoAttackable = false,

                canTargetItems = true,
                mustBeSelectable = true,

                validator = EquipmentValidator
            };
        }

        public static bool IsBiocodable(Thing thing)
        {
            return thing.TryGetComp<CompBiocodable>() != null;
        }

        bool EquipmentValidator(TargetInfo tInfo)
        {
            Thing targetedThing = tInfo.Thing;
            
            if (targetedThing == null) { return false; }

            if (IsBiocodable(targetedThing))
            {
                // If the thing is already biocodeable we can only code it if it isn't yet.
                // That's the whole point. This tool is meant to biocode your weapons, not to re-biocode them.
                return !CompBiocodable.IsBiocoded(targetedThing);
            }

            return false;
        }
    }

    public class CompTargetEffect_Biocode : CompTargetEffect
    {
        public override void DoEffectOn(Pawn user, Thing target)
        {
            if (user == null || target == null) { return; }
            if( !user.CanReserveAndReach(target, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false)) { return; }

            Job job = JobMaker.MakeJob(BiocodeIt_JobDefOf.Biocode, target, this.parent);
            job.count = 1;
            user.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
        }
    }
}
