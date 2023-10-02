using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Notes
{
    public class ManageClinicalNote
    {
        public List<ClinicalNote> infoList = new List<ClinicalNote>();
        private int value = 1;
        public int GetNewId()
        {

            foreach (ClinicalNote patient in infoList)
            {
                if (patient.Id >= value)
                {
                    value = patient.Id + 1;
                }

            }
            return value;
        }
        public void AddNewNote(ClinicalNote patient)
        {
            infoList.Add(patient);
            using (StreamWriter writer = new StreamWriter("Patient.txt",true))
            {

                writer.WriteLine(patient.ToString());


            }

        }
        public void LoadNotesFromFile()
        {
            using (StreamReader reader = new StreamReader("Patient.txt"))
            {
                string info;
                while ((info = reader.ReadLine()) != null)
                {
                    string[] parts = info.Split('|');
                    int id = int.Parse(parts[0]);
                    string patientName = parts[1];
               
                    DateTime dob= DateTime.Parse(parts[2]);
                    string[] ProblemArray =  parts[3].Split(';') ;

                    string note = parts[4].Replace(';','\n');

                    ClinicalNote noteToAdd = new ClinicalNote (id.ToString(),patientName, dob, ProblemArray, note );
                    infoList.Add(noteToAdd);
                }
            }
        }
      
        public void DeleteNote(ClinicalNote note)
        {
            infoList.Remove(note);

            
            var tempFile = Path.GetTempFileName();
            using (var sr = new StreamReader("Patient.txt"))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line !=note.ToString())
                        sw.WriteLine(line);
                }
            }
            File.Delete("Patient.txt");
            File.Move(tempFile, "Patient.txt");
        }

     
        public bool UpdateNote(ClinicalNote updatedNote)
        {
          
           
            int index = infoList.FindIndex(n => n.Id == updatedNote.Id);        
        
            infoList[index] = updatedNote;

        
            RewriteFile();

            return true;
        }

        private void RewriteFile()
        {
           
            using (StreamWriter writer = new StreamWriter("Patient.txt"))
            {
                foreach (ClinicalNote note in infoList)
                {
                    writer.WriteLine(note.ToString());
                }
            }
        }


    }
}
