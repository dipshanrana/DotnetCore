namespace ConfigSetting;


// It is a Poco class
// Poco : plain old clr object
// clr : common language runtime 
//i.e. model
public class MapApiOptions
{
    public string? ApiKey { get; set; }
    public string? BaseUrl { get; set; }
    public int TimeOut { get; set; }
}