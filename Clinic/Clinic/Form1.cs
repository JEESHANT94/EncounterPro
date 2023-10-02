using Notes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ClinicalNoteClass = Notes.ClinicalNote;
using ManageClinicalNoteClass = Notes.ManageClinicalNote;
using VitalMeasurementClass = Notes.VitalMeasurement;


namespace Assignment_3
{
    public partial class Form1 : Form
    {
        ManageClinicalNoteClass _manageClinicalNoteClass = new ManageClinicalNoteClass();
        private List<VitalMeasurementClass> vitals = new List<VitalMeasurementClass>();
   
        public Form1()        {
            InitializeComponent();
        }
        private void Enable()
        {
            btnAddNote.Enabled = true;
            txtName.Enabled = true;
            txtProblem.Enabled = true;
            rtbNote.Enabled = true;
            btnAdd.Enabled = true;
            dateTimePicker1.Enabled = true;
            btnRemoveProblem.Enabled = true;

        }
        private void NotEnabled()
        {
            btnAdd.Enabled = false;
            btnAddNote.Enabled = false;
            txtName.Enabled = false;
            txtProblem.Enabled = false;
            rtbNote.Enabled = false;
            dateTimePicker1.Enabled = false;
            btnRemoveProblem.Enabled = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            _manageClinicalNoteClass.LoadNotesFromFile();
            foreach(var patient in _manageClinicalNoteClass.infoList)
            {
                if(patient.Name != null)
                {
                    lstNotes.Items.Add(patient.Name + $" (Note:{patient.Id})");
                }
            }
          

            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            NotEnabled();
           

        }


        private void btnStartNewNote_Click(object sender, EventArgs e)
        {
            ClearField();
            txtId.Text= _manageClinicalNoteClass.GetNewId().ToString();
            btnDelete.Enabled= false;
            btnUpdate.Enabled= false;
            Enable();
            lblErrors.Text = "";

        } 
        private void ClearField()
        {
            rtbNote.Text = "";
            txtName.Text = "";
            txtId.Text = "";
            lstProblems.Items.Clear();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            lstProblems.Items.Add(txtProblem.Text);
            txtProblem.Text = "";
            lblErrors.Text = "";

        }

        private void btnRemoveProblem_Click(object sender, EventArgs e)
        {
            if (lstProblems.SelectedIndex == -1)
            {
                lblErrors.Text = "Please select a problem first";
                lblErrors.ForeColor= Color.Red; 
            }
            else
            {
                lstProblems.Items.Remove(lstProblems.SelectedItem);
                lblErrors.Text = "Successfully removed a problem";
                lblErrors.ForeColor= Color.Green;
            }
         
        }

        private void btnAddNote_Click(object sender, EventArgs e)
        {
            string id;
            string name;
            DateTime dob;
            string notes;

            id=txtId.Text;
            name=txtName.Text;
            dob=dateTimePicker1.Value;
            notes = rtbNote.Text;


            string[] problems = new string[lstProblems.Items.Count];
            for (int i = 0; i < lstProblems.Items.Count; i++)
            {
                problems[i] = lstProblems.Items[i].ToString();
            }
          
            try
            {
                ClinicalNoteClass patient = new ClinicalNoteClass(id, name, dob, problems, notes);
                _manageClinicalNoteClass.AddNewNote(patient);
                lstNotes.Items.Add(name+ $" (Note:{id})");
                lblErrors.Text = "New Note was added";
                lblErrors.ForeColor = Color.Green;
                NotEnabled();
                ClearField();
            }
            catch (Exception ex)
            {
                lblErrors.Text = ex.Message;
                lblErrors.ForeColor = Color.Red;
            }
        

        }

        private void lstNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearField();
            string Name = lstNotes.GetItemText(lstNotes.SelectedItem);
            Enable();
            btnAddNote.Enabled = false;
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            lblErrors.Text = "";
            foreach (var name in _manageClinicalNoteClass.infoList)
            {
                if(Name == name.newName)
                {
                    txtName.Text = name.Name;
                    txtId.Text = name.Id.ToString();
                    rtbNote.Text=name.Notes;
                  dateTimePicker1.Value = name.Dob;
                    foreach(var problem in name.ProbArray)
                    {
                        lstProblems.Items.Add(problem);
                    }

                }
            }
        }

        private void rtbNote_TextChanged(object sender, EventArgs e)
        {
            vitals.Clear();
            Regex regexBP = new Regex(@"BP:? ?(\d{2,3}/\d{2,3})");
            Regex regexHR = new Regex(@"HR:? ?(\d{2,3})");
            Regex regexRR = new Regex(@"RR:? ?(\d{2,3})");
            Regex regexT = new Regex(@"T:? ?(\d{2,3})");

            MatchCollection matchesBP = regexBP.Matches(rtbNote.Text);
            MatchCollection matchesHR = regexHR.Matches(rtbNote.Text);
            MatchCollection matchesRR = regexRR.Matches(rtbNote.Text);
            MatchCollection matchesT =  regexT.Matches(rtbNote.Text);


            foreach (Match match in matchesBP)
            {
                BloodPressure bp = new BloodPressure();

                  bp.Measurement = match.Groups[1].Value;
                  vitals.Add(bp);          
            }
            foreach (Match match in matchesHR)
            {
                 HeartRate hr = new HeartRate();
                hr.Measurement = match.Groups[1].Value; 
                vitals.Add(hr);
            }
            foreach (Match match in matchesRR)
            {
                RespiratoryRate rr = new RespiratoryRate();
                rr.Measurement = match.Groups[1].Value;
                vitals.Add(rr);
            }
            foreach (Match match in matchesT)
            {
                Temperature t = new Temperature();
                t.Measurement = match.Groups[1].Value;
                vitals.Add(t);
            }

            UpdateVitalsListBox();
        }
        private void UpdateVitalsListBox()
        {

            lstBp.Items.Clear();
            foreach (VitalMeasurementClass vital in  vitals)
            {   
               
               
                lstBp.Items.Add($"{vital.Type}: {vital.Measurement} {vital.Units} {vital.GetStatus()}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (lstNotes.SelectedItem != null)
            {

                string selectedItem = lstNotes.SelectedItem.ToString();

              
                ClinicalNote note = _manageClinicalNoteClass.infoList.Find(n => n.newName == selectedItem);
                _manageClinicalNoteClass.DeleteNote(note);
                lstNotes.Items.Remove(selectedItem);

                NotEnabled();
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                lblErrors.Text = "successfully deleted";
                lblErrors.ForeColor= Color.Green;
             
            }
            else
            {
                lblErrors.Text = "please select note first";
            }

        }
       

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstNotes.SelectedIndex == -1)
              {
                lblErrors.Text = "Please select a Note first";
                lblErrors.ForeColor = Color.Red;
               }
             string selectedItem = lstNotes.SelectedItem.ToString();

            
              ClinicalNote noteToUpdate = _manageClinicalNoteClass.infoList.Find(n => n.newName == selectedItem);
              noteToUpdate.Name = txtName.Text;
              noteToUpdate.Dob = dateTimePicker1.Value;
             string[] problems = new string[lstProblems.Items.Count];
             for (int i = 0; i < lstProblems.Items.Count; i++)
            {
                problems[i] = lstProblems.Items[i].ToString();
            }
            noteToUpdate.ProbArray = problems;
            noteToUpdate.Notes= rtbNote.Text;
            
              ClinicalNoteClass f = new ClinicalNoteClass(txtId.Text,txtName.Text,dateTimePicker1.Value,noteToUpdate.ProbArray,rtbNote.Text);
            if (_manageClinicalNoteClass.UpdateNote(noteToUpdate))
            {

                lblErrors.Text = "Note updated successfully.";
                    NotEnabled();
                    btnDelete.Enabled = false;
                    btnUpdate.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblErrors.Text = ex.Message;
                lblErrors.ForeColor = Color.Red;
            }
        }      
}
}
