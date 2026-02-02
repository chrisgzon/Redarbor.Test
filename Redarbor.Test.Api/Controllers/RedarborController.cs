using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Redarbor.Test.Application.Commands.CreateEmployee;
using Redarbor.Test.Application.Commands.DeleteEmployee;
using Redarbor.Test.Application.Commands.UpdateEmployee;
using Redarbor.Test.Application.Queries.GetAllEmployees;
using Redarbor.Test.Application.Queries.GetEmployeeByIdQuery;

namespace Redarbor.Test.Api.Controllers
{
    [ApiController]
    [Route("api/redarbor")]
    [Authorize]
    public class RedarborController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RedarborController> _logger;

        public RedarborController(IMediator mediator, ILogger<RedarborController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>Array of employee items</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all employees");
            var query = new GetAllEmployeesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get an employee by ID
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>Employee item</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Getting employee with ID: {EmployeeId}", id);
            var query = new GetEmployeeByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound(new { message = $"Employee with ID {id} not found" });

            return Ok(result);
        }

        /// <summary>
        /// Add a new employee
        /// </summary>
        /// <param name="command">Employee data</param>
        /// <returns>Created employee item</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        {
            _logger.LogInformation("Creating new employee: {Username}", command.Username);
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <param name="command">Updated employee data</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                _logger.LogWarning("ID mismatch: URL ID {UrlId} != Body ID {BodyId}", id, command.Id);
                return BadRequest(new { message = "ID in URL does not match ID in body" });
            }

            _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Delete an employee
        /// </summary>
        /// <param name="id">Employee ID</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);
            var command = new DeleteEmployeeCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
