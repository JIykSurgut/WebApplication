using Microsoft.AspNet.Identity;
using System;

namespace Models
{
    public class AppRole : IRole<int>
    {
        public int Id { get; set; }      //Идентификатор
        public string Name { get; set; }    //Имя роли

        public AppRole()
        {
            Id = 0; //отсутствие ID
        }
        public AppRole(string name) : this()
        {
            Name = name;
        }
        public AppRole(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}