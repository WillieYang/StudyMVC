using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace StudyMVC.Models
{
    public class Person
    {
        [HiddenInput(DisplayValue = false)]
        [Key]
        public int PersonID { get; set; }
        [Required]
        public string UniqName { get; set; }
        [DataType(DataType.Date)]
        [UIHint("DateOnly")]
        [Required]
        public DateTime Birthday { get; set; }
    }
    public class PersonContext : DbContext{
           public PersonContext() : base() {}
           public DbSet<Person> BirthdayDB { get; set; }
}
    public static class MyExtensionmodels
    {
        public static Person FindByNumber(
            this IEnumerable<Person> people, int id)
        {
            return (from p in people where p.PersonID == id select p).First(); }
    }

}