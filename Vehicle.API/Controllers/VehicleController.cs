using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vehicle.API.Application.Commands;
using Vehicle.API.Application.Models;
using Vehicle.API.Application.Queries;

namespace Vehicle.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IVehicleQueries _vehicleQueries;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(
            IMediator mediator,
            IVehicleQueries vehicleQueries,
            ILogger<VehicleController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _vehicleQueries = vehicleQueries ?? throw new ArgumentNullException(nameof(vehicleQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("location")]
        public async Task<ActionResult<Application.Queries.Vehicle>> GetAllVehiclesLocation()
        {
            try
            {
                var vehicleLocationList = await _vehicleQueries.GetAllVehicleLocationAsync();

                return Ok(vehicleLocationList);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{vehicleId}/location")]
        public async Task<ActionResult<Application.Queries.Vehicle>> GetVehicleLocation(Guid vehicleId)
        {
            try
            {
                var vehicleLocation = await _vehicleQueries.GetVehicleLocationAsync(vehicleId);

                return Ok(vehicleLocation);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{vehicleId}/location/history")]
        public async Task<ActionResult<Application.Queries.Vehicle>> GetVehicleLocationHistory(Guid vehicleId)
        {
            try
            {
                var vehicleLocationHistory = await _vehicleQueries.GetVehicleLocationHistoryAsync(vehicleId);

                return Ok(vehicleLocationHistory);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("{vehicleId}/location")]
        public async Task<IActionResult> SetVehicleLocation(Guid vehicleId, [FromBody] VehicleLocation vehicleLocation)
        {
            bool commandResult = false;

            if (vehicleId != Guid.Empty)
            {
                var setVehicleLocationCommand = new SetVehicleLocationCommand(vehicleId, vehicleLocation.Location);

                _logger.LogInformation(
                    "Sending command: {CommandName} - {VehicleId}: {OrderId} ({@Command})",
                    setVehicleLocationCommand.GetType().Name,
                    setVehicleLocationCommand.VehicleId,
                    setVehicleLocationCommand.Location,
                    setVehicleLocationCommand);

                commandResult = await _mediator.Send(setVehicleLocationCommand);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
