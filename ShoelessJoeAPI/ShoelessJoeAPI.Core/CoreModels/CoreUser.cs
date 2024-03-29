﻿namespace ShoelessJoeAPI.Core.CoreModels
{
    public class CoreUser
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhoneNumb { get; set; }
        public bool IsAdmin { get; set; }

        public List<CoreManufacter> Manufacters { get; set; }
        public List<CorePotentialBuy> PotentialBuys { get; set; }
        public List<CoreComment> Comments { get; set; }
    }
}
