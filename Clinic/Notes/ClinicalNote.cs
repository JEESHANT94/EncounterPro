using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Notes
{
    public class ClinicalNote
    {
        public int Id;
        public string Name;
        public DateTime Dob;
        public string Problems;
        public string Notes;
        public string[] ProbArray;
        public string newName;

        public ClinicalNote()
        {
        }

        public ClinicalNote(string id, string name, DateTime dob, string[] problems, string notes)
        {
         
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Name must not be blank");
            }

            else if (dob > DateTime.Now)
            {
                throw new Exception("Time must not be in future");
            }
            else if (problems.Length==0)
            {
                throw new Exception("Please add problem");
            }
            else if (string.IsNullOrEmpty(notes))
            {
                throw new Exception("Notes must not be blank");
            }
            Id = int.Parse(id);
            Name = name;
            Dob = dob;

          foreach(string problem in problems)
            {
                Problems += problem;
            }
          
            Notes = notes;
            ProbArray = problems;
            newName = name + $" (Note:{id})";
        }

        public override string ToString()
        {
            return $"{Id}|{Name}|{Dob.ToString("dd MMMM yyyy")}|{string.Join(";", ProbArray)}|{Notes.Replace("\n",";")}";
        }
    }
}
