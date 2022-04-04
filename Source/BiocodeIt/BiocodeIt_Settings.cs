using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace Krelinos_BiocodeIt
{

    // Just so you know, I basically Copy & Pasted these two classes from the Mod Settings section of the Rimworld Wiki Modding Tutorials
    public class BiocodeIt_Settings : ModSettings
    {
        public static int biocodedMarketValuePercent = 35;
        public static int biocodedStealChancePercent = 0;
        public static bool notifyPlayerOfSpite = true;
        public static int minTechLevelRequired = 4;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref biocodedMarketValuePercent, "biocodedMarketValuePercent", 35);
            Scribe_Values.Look(ref biocodedStealChancePercent, "biocodedStealChancePercent", 0);
            Scribe_Values.Look(ref notifyPlayerOfSpite, "notifyPlayerOfSpite", true);
            Scribe_Values.Look(ref minTechLevelRequired, "minTechLevelRequired", 4);
            base.ExposeData();
        }
    }

    public class BiocodeIt : Mod
    {
        BiocodeIt_Settings settings;
        public BiocodeIt(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<BiocodeIt_Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label(String.Format("BiocodedMarketValuePercentLabel".Translate(), BiocodeIt_Settings.biocodedMarketValuePercent));
            BiocodeIt_Settings.biocodedMarketValuePercent = (int)listingStandard.Slider(BiocodeIt_Settings.biocodedMarketValuePercent, 0f, 100f);
            
            listingStandard.Label(String.Format("BiocodedStealChancePercentLabel".Translate(), BiocodeIt_Settings.biocodedStealChancePercent));
            BiocodeIt_Settings.biocodedStealChancePercent = (int)listingStandard.Slider(BiocodeIt_Settings.biocodedStealChancePercent, 0f, 100f);

            listingStandard.Label(String.Format("BiocodedStealChancePercentExplanation".Translate(),
                Math.Round(BiocodeIt_Settings.biocodedStealChancePercent / 100f, 2),
                Math.Round(
                    (1 - Math.Pow((100 - BiocodeIt_Settings.biocodedStealChancePercent) / 100f, 5)) * 100,
                1)
            ));
            listingStandard.Gap();
            listingStandard.CheckboxLabeled("BiocodedNotifyPlayerOfSpiteLabel".Translate(), ref BiocodeIt_Settings.notifyPlayerOfSpite);
            listingStandard.Gap(25f);
            listingStandard.Label("BiocodedMinTechLevelRequiredLabel".Translate());
            if( listingStandard.RadioButton("BiocodedMinTechLevelRequiredArcho".Translate(), BiocodeIt_Settings.minTechLevelRequired == 7, 10) )
            {
                BiocodeIt_Settings.minTechLevelRequired = 7;
            }
            if( listingStandard.RadioButton("BiocodedMinTechLevelRequiredUltra".Translate(), BiocodeIt_Settings.minTechLevelRequired == 6, 10) )
            {
                BiocodeIt_Settings.minTechLevelRequired = 6;
            }
            if( listingStandard.RadioButton("BiocodedMinTechLevelRequiredSpacer".Translate(), BiocodeIt_Settings.minTechLevelRequired == 5, 10) )
            {
                BiocodeIt_Settings.minTechLevelRequired = 5;
            }
            if( listingStandard.RadioButton("BiocodedMinTechLevelRequiredIndustrial".Translate(), BiocodeIt_Settings.minTechLevelRequired == 4, 10) )
            {
                BiocodeIt_Settings.minTechLevelRequired = 4;
            }
            Text.Font = GameFont.Tiny;
            listingStandard.Label("BiocodedMinTechLevelRequiredYNoMedievalAndBelow".Translate());
            Text.Font = GameFont.Small;
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "BiocodeIt_SettingsCategory".Translate();
        }
    }

}
