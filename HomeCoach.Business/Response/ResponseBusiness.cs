namespace HomeCoach.Business.Response
{
    using Models;

    public class ResponseBusiness : IResponseBusiness
    {
        public string BuildResponse(HomeCoachData data, string intent)
        {
            switch (intent)
            {
                case "temperature":
                    return $"La température de {data.DeviceName} est de {data.Temperature}°C";

                case "humidity":
                    return $"L'humidité de {data.DeviceName} est de {data.HumidityPercent}%";                
                case "pressure":
                    return $"La pression au niveau de {data.DeviceName} est de {data.Pressure} hecto pascals";     
                case "noise":
                    return $"Le bruit au niveau de {data.DeviceName} est de {data.Noise}%";             
                case "ppm":
                    return $"L'air au niveau de {data.DeviceName} est chargé de {data.Co2} particules par mètre cube";
                default:
                    return
                        $"La température de {data.DeviceName} est de {data.Temperature}°C, l'humidité est de {data.HumidityPercent}%, la pression est de {data.Pressure} hectoPascal, le niveau de CO2 est de {data.Pressure} particule par mètre cube et le bruit est de {data.Noise} décibels";
            }
        }
    }
}