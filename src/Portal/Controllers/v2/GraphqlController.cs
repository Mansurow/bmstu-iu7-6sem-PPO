using HotChocolate.Execution;
using HotChocolate.Language;
using Microsoft.AspNetCore.Mvc;
using Portal.Common.Dto;

namespace Portal.Controllers.v2;

[ApiController]
[Route("/graphql")]
public class GraphqlController: ControllerBase
{
    private readonly IRequestExecutor _documentExecuter;
    private readonly ISchema _schema;

    public GraphqlController(IRequestExecutor documentExecuter, ISchema schema)
    {
        _documentExecuter = documentExecuter;
        _schema = schema;
    }
    
    /*[HttpPost]
    public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
    {
        if (query == null) { throw new ArgumentNullException(nameof(query)); }
        var inputs = query.Variables.ToInputs();
        var executionOptions = new ExecutionOptions
        {
            Schema = _schema,
            Query = query.Query,
            Inputs = inputs
        };

        var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

        if (result.Errors?.Count > 0)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }*/
}