using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes
{
    public abstract class VitalMeasurement
    {

        public abstract string Type { get; }
        public abstract string Units { get; }
        public abstract string Measurement { get; set; }
        public abstract string GetStatus();

    }
    public class BloodPressure : VitalMeasurement
    {
        public override string Type { get { return "BP"; } }
        public override string Units { get { return "mmHg"; } }
        public override string Measurement { get; set; }
      
        public override string GetStatus()
        {
            int systolic = int.Parse(Measurement.Split('/')[0]);
            int diastolic = int.Parse(Measurement.Split('/')[1]);
            if (systolic < 90 && diastolic < 60)
            {
                return "(Low)";

            }
            else if (systolic > 130 && diastolic > 80)
            {
                return "(High)";
            }
            else
            {
                return "";
            }
        }
    }
    public class HeartRate : VitalMeasurement
    {
        public override string Type { get { return "HR"; } }
        public override string Units { get { return "bpm"; } }
        public override string Measurement { get; set; }
        public override string GetStatus()
        {
            
          
            if (int.Parse(Measurement) < 60)
            {
                return "(Low)";

            }
            else if (int.Parse(Measurement) > 100)
            {
                return "(High)";
            }
            else
            {
                return "";
            }
        }

    }
    public class RespiratoryRate : VitalMeasurement
    {

        public override string Type { get { return "RR"; } }
        public override string Units { get { return "bpm"; } }
        public override string Measurement { get; set; }
        public override string GetStatus()
        {


            if (int.Parse(Measurement) < 12)
            {
                return "(Low)";

            }
            else if (int.Parse(Measurement) > 16)
            {
                return "(High)";
            }
            else
            {
                return "";
            }
        }
    }
    public class Temperature : VitalMeasurement
    {

        public override string Type { get { return "T"; } }
        public override string Units { get { return "Degrees Celsius"; } }
        public override string Measurement { get; set; }
        public override string GetStatus()
        {


            if (double.Parse(Measurement) < 36.5)
            {
                return "(Low)";

            }
            else if (double.Parse(Measurement) > 37.2)
            {
                return "(High)";
            }
            else
            {
                return "";
            }
        }
    }
}
