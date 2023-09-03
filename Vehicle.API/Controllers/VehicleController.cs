using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vehicle.API.Application.Commands;
using Vehicle.API.Application.Models;

namespace Vehicle.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(
            IMediator mediator,
            ILogger<VehicleController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
