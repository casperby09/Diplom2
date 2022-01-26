using Diplom2.Models;
using System.Collections.Generic;

namespace Diplom2.ViewModels
{
    public class HomeIndexView
    {
        public IEnumerable<Collection> CollectionsIndex { get; set; }

        public IEnumerable<Item> ItemsIndex { get; set; }

        public IEnumerable<Tag> TagsIndex { get; set; }
    }
}
