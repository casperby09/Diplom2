using System.Linq;
using Diplom2.Data;
using Diplom2.Models;

namespace Diplom2
{
    public class SampleData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Themes.Any())
            {
                context.Themes.AddRange(
                    new Theme
                    {
                        ThemeName = "Book"
                    },
                    new Theme
                    {
                        ThemeName = "Alcohol"
                    },
                    new Theme
                    { 
                        ThemeName = "Badges"
                    }
                );
                context.SaveChanges();

            }
        }
    }
}
