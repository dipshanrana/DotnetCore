using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace ConfigSetting;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IConfiguration _configuration;

    private readonly IOptions<MapApiOptions> _options;

    public ConfigController(IConfiguration configuration, IOptions<MapApiOptions> options)
    {
        _configuration = configuration;
        _options = options;
    }

    [HttpGet("iconfig")]
    public  IActionResult GetviaIConfig()
    {
       var apikey = _configuration["ApiKey"];
        var baseurl = _configuration["BaseUrl"];
        var timeout = _configuration["TimeOut"];
        return Ok(new { 
            Source = "IConfiguration",
            apikey, 
            baseurl, 
            timeout 
        });
    }

 //Endpoint: read setting data using IOptions

 
    [HttpGet("options")]
    public IActionResult GetviaOptions()
    {
       MapApiOptions info = _options.Value;
       return Ok(new { 
        Source = "IOptions",
        info.ApiKey,
        info.BaseUrl,
        info.TimeOut
       }); 
    }
}
 