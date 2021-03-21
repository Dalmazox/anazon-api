using Anazon.Domain.Entities;
using System;

namespace Anazon.Domain.Models.Create
{
    public class CreateUserModel
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public DateTime Birthdate { get; set; }
        public char Sex { get; set; }

        public static User Map(CreateUserModel model)
        {
            return new User()
            {
                Name = model.Name,
                CPF = model.CPF,
                RG = model.RG,
                Birthdate = model.Birthdate,
                Sex = model.Sex
            };
        }
    }
}
