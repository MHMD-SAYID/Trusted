using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Trusted.Core.Entities.Games;

namespace Trusted.Infrastructure.DataSeeding;

public static class gamesSeeding
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        var games = new[]
        {
            new Game { EnName = "Fortnite", ArName = "فورتنايت", Logo = "logos/fortnite.png", IsPopular = true },
            new Game { EnName = "Valorant", ArName = "فالورانت", Logo = "logos/valorant.png", IsPopular = false },
            new Game { EnName = "League of Legends", ArName = "ليج أوف ليجيندز", Logo = "logos/league-of-legends.png", IsPopular = true },
            new Game { EnName = "Counter-Strike 2", ArName = "كونتر سترايك 2", Logo = "logos/counter-strike-2.png", IsPopular = false },
            new Game { EnName = "Roblox", ArName = "روبلوكس", Logo = "logos/roblox.png", IsPopular = false },
            new Game { EnName = "Minecraft", ArName = "ماينكرافت", Logo = "logos/minecraft.png", IsPopular = false },
            new Game { EnName = "EA Sports FC", ArName = "إي إيه سبورتس إف سي", Logo = "logos/ea-sports-fc.png", IsPopular = false },
            new Game { EnName = "Call of Duty", ArName = "كول أوف ديوتي", Logo = "logos/call-of-duty.png", IsPopular = false },
            new Game { EnName = "Apex Legends", ArName = "أبيكس ليجندز", Logo = "logos/apex-legends.png", IsPopular = false },
            new Game { EnName = "Overwatch 2", ArName = "أوفر واتش 2", Logo = "logos/overwatch-2.png", IsPopular = false },
            new Game { EnName = "Rainbow Six Siege", ArName = "رينبو سيكس سيج", Logo = "logos/rainbow-six-siege.png", IsPopular = false },
            new Game { EnName = "PUBG", ArName = "ببجي", Logo = "logos/pubg.png", IsPopular = false },
            new Game { EnName = "Rocket League", ArName = "روكيت ليج", Logo = "logos/rocket-league.png", IsPopular = false },
            new Game { EnName = "Dota 2", ArName = "دوتا 2", Logo = "logos/dota-2.png", IsPopular = false },
            new Game { EnName = "Genshin Impact", ArName = "جينشن إمباكت", Logo = "logos/genshin-impact.png", IsPopular = true },
            new Game { EnName = "Grand Theft Auto V", ArName = "جراند ثفت أوتو 5", Logo = "logos/gta-v.png", IsPopular = true },
            new Game { EnName = "Rust", ArName = "رست", Logo = "logos/rust.png", IsPopular = false },
            new Game { EnName = "Escape from Tarkov", ArName = "إسكيب فروم تاركوف", Logo = "logos/escape-from-tarkov.png", IsPopular = false },
            new Game { EnName = "Destiny 2", ArName = "ديستني 2", Logo = "logos/destiny-2.png", IsPopular = false },
            new Game { EnName = "Dead by Daylight", ArName = "ديد باي دايلايت", Logo = "logos/dead-by-daylight.png", IsPopular = false }
        };

        foreach (var game in games)
        {
            var existing = await context.Games.FirstOrDefaultAsync(x => x.EnName == game.EnName);
            if (existing is null)
            {
                await context.Games.AddAsync(game);
            }
            else if (existing.Logo != game.Logo)
            {
                existing.Logo = game.Logo;
                existing.IsPopular = game.IsPopular;
            }
        }

        await context.SaveChangesAsync();
    }
}
