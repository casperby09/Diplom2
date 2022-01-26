using Diplom2.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Diplom2.ViewModels
{
    public class AdminIndexView
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public IList<string> Role { get; set; }
        public int QuantityCollectionsUser { get; set; }
    }
}
