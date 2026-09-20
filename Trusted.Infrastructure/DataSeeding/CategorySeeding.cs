using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities.Games;

namespace Trusted.Infrastructure.DataSeeding
{
    public class CategorySeeding
    {
        public static class CategoriesSeeding
        {
            public static async Task SeedAsync(ApplicationDbContext context)
            {
                var categories = new[]
            {
                new Game { EnName = "Fortnite", ArName = "فورتنايت", Logo = "logos/fortnite.svg" },
                new Game { EnName = "Valorant", ArName = "فالورانت", Logo = "logos/valorant.png" },
                new Game { EnName = "League of Legends", ArName = "ليج أوف ليجيندز", Logo = "logos/league-of-legends.png" },
                new Game { EnName = "Counter-Strike 2", ArName = "كونتر سترايك 2", Logo = "logos/counter-strike-2.png" },
                new Game { EnName = "Roblox", ArName = "روبلوكس", Logo = "logos/roblox.png" },
                new Game { EnName = "Minecraft", ArName = "ماينكرافت", Logo = "logos/minecraft.png" },
                new Game { EnName = "EA Sports FC", ArName = "إي إيه سبورتس إف سي", Logo = "logos/ea-sports-fc.png" },
                new Game { EnName = "Call of Duty", ArName = "كول أوف ديوتي", Logo = "logos/call-of-duty.png" },
                new Game { EnName = "Apex Legends", ArName = "أبيكس ليجندز", Logo = "logos/apex-legends.png" },
                new Game { EnName = "Overwatch 2", ArName = "أوفر واتش 2", Logo = "logos/overwatch-2.png" },
                new Game { EnName = "Rainbow Six Siege", ArName = "رينبو سيكس سيج", Logo = "logos/rainbow-six-siege.png" },
                new Game { EnName = "PUBG", ArName = "ببجي", Logo = "logos/pubg.png" },
                new Game { EnName = "Rocket League", ArName = "روكيت ليج", Logo = "logos/rocket-league.png" },
                new Game { EnName = "Dota 2", ArName = "دوتا 2", Logo = "logos/dota-2.png" },
                new Game { EnName = "Genshin Impact", ArName = "جينشن إمباكت", Logo = "logos/genshin-impact.png" },
                new Game { EnName = "Grand Theft Auto V", ArName = "جراند ثفت أوتو 5", Logo = "logos/gta-v.png" },
                new Game { EnName = "Rust", ArName = "رست", Logo = "logos/rust.png" },
                new Game { EnName = "Escape from Tarkov", ArName = "إسكيب فروم تاركوف", Logo = "logos/escape-from-tarkov.png" },
                new Game { EnName = "Destiny 2", ArName = "ديستني 2", Logo = "logos/destiny-2.png" },
                new Game { EnName = "Dead by Daylight", ArName = "ديد باي دايلايت", Logo = "logos/dead-by-daylight.png" }
            }; foreach (var category in categories) { if (!await context.Games.AnyAsync(x => x.EnName == category.EnName)) { await context.Games.AddAsync(category); } }
                await context.SaveChangesAsync();
            }
        }
    }
}
