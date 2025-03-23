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
        //public new CompProperties_Equipment Props => (CompProperties_Equipment)this.props;

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

    /*
        So this is now obsolete since I replaced comp properties with mod settings.
        Keeping it here as legacy code for those curious.
    
    public class CompProperties_Equipment : CompProperties_Targetable
    {
        public bool includeMeleeWeapons;        // These bools are okay to be left uninitialized.
        public bool includeRangedWeapons;       // When CompTargetable_Equipment is defined in an XML,
        public bool includeApparel;             // the relevant ones will be set to true.
        public bool industrialTierAndUpOnly;    // It will probably look something like this in some
        public bool spacerTierAndUpOnly;        // XML somewhere:
        public bool nonBiocodedEquipmentOnly;
    }                                           
//                                                <ThingDef>
//                                                    ...
//                                                    <comps>
//                                                        <li Class="Krelinos_BiocodeIt.CompProperties_Equipment">
//                                                            <compClass>Krelinos_BiocodeIt.CompTargetable_Equipment</compClass>
//                                                            <includeMeleeWeapons>true</includeMeleeWeapons>
//                                                            <spacerTierAndUpOnly>true</spacerTierAndUpOnly>
//                                                        </li>
//                                                    </comps>
//                                                </ThingDef>
                                                
    */

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
