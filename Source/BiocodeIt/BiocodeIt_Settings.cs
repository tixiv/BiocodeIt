﻿using System;
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
        public static int biocodedStealChancePercent = 0;
        public static int biocodedMarketValuePercent = 0;

        public static bool notifyPlayerOfSpite;

        public static bool biocodeAllTheThings = true;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref biocodedMarketValuePercent, "biocodedMarketValuePercent", 35);
            Scribe_Values.Look(ref biocodedStealChancePercent, "biocodedStealChancePercent", 2);

            Scribe_Values.Look(ref notifyPlayerOfSpite, "notifyPlayerOfSpite", true);

            Scribe_Values.Look(ref biocodeAllTheThings, "biocodeAllTheThings", true);
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
                    (1 - Math.Pow((100 - BiocodeIt_Settings.biocodedStealChancePercent) / 100f, 6)) * 100,
                1)
            ));

            listingStandard.Gap();

            listingStandard.CheckboxLabeled("BiocodedNotifyPlayerOfSpiteLabel".Translate(), ref BiocodeIt_Settings.notifyPlayerOfSpite);

            listingStandard.Gap();

            listingStandard.Label("Make all weapons and apperel biocodable. This mod is pretty safe if you leave this turned off. If you turn it on there is a little bit of danger because we are adding a CompBiocodable to things that normally don't have it. It should not break your savegame and you should be able to remove the mod without breaking anything still.");
            listingStandard.CheckboxLabeled("Biocode all the things", ref BiocodeIt_Settings.biocodeAllTheThings);

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "BiocodeIt_SettingsCategory".Translate();
        }
    }

}
