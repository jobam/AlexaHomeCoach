namespace HomeCoach.Business.Models
{
    public class HomeCoachData
    {
        public string DeviceName { get; set; }

        public double Temperature { get; set; }
        
        public double HumidityPercent { get; set; }     
        
        public int Co2 { get; set; }
        
        public double Noise { get; set; }
        
        public double Pressure { get; set; }
        
    }
}