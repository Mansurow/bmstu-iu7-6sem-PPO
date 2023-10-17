using HotChocolate.Execution;
using HotChocolate.Language;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Dto;

namespace Portal.Controllers.v2;

[ApiController]
[Route("/api/v2/")]
public class GraphqlController: ControllerBase
{
    private readonly ILogger<GraphqlController> _logger;
    private readonly IRequestExecutor _executor;

    public GraphqlController(IRequestExecutor executor, 
        ILogger<GraphqlController> logger)
    {
        _executor = executor;
        _logger = logger;
    }

    /*[HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] GraphQLRequest request)
    {
        try
        {
            /*var result = await _executor.ExecuteAsync(request);
            return Ok(result);#1#
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse(e));
        }
    }*/
}