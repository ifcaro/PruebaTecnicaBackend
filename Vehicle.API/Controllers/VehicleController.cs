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

        #region Location

        /// <summary>
        /// Obtiene la ubicación de todos los vehiculos registrados
        /// </summary>
        /// <returns>Returns list of <see cref="Application.Queries.Vehicle"/>Vehicle</returns>
        [HttpGet]
        [Route("location")]
        public async Task<ActionResult<IEnumerable<Application.Queries.Vehicle>>> GetAllVehiclesLocation()
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

        /// <summary>
        /// Obtiene la ubicación de un vehiculo
        /// </summary>
        /// <param name="vehicleId">Identificador del vehiculo</param>
        /// <returns><see cref="Application.Queries.Vehicle"/>Vehicle</returns>
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

        /// <summary>
        /// Obtiene el historial de ubicaciones de un vehiculo
        /// </summary>
        /// <param name="vehicleId">Identificador del vehiculo</param>
        /// <returns><see cref="VehicleLocationHistory"/>VehicleLocationHistory</returns>
        [HttpGet]
        [Route("{vehicleId}/location/history")]
        public async Task<ActionResult<VehicleLocationHistory>> GetVehicleLocationHistory(Guid vehicleId)
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

        /// <summary>
        /// Establece la ubicación de un vehiculo. Se creará el vehiculo si este no figura en el sistema
        /// </summary>
        /// <param name="vehicleId">Identificador del vehiculo</param>
        /// <param name="vehicleLocation">Ubicación del vehiculo</param>
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

        #endregion

        #region Order

        /// <summary>
        /// Obtiene el vehiculo que contiene un pedido
        /// </summary>
        /// <param name="orderId">Identificador del pedido</param>
        /// <returns><see cref="Application.Queries.Vehicle"/>Vehicle</returns>
        [HttpGet]
        [Route("order/{orderId}")]
        public async Task<ActionResult<Application.Queries.Vehicle>> GetVehicleByOrder(Guid orderId)
        {
            try
            {
                var vehicleOrderList = await _vehicleQueries.GetVehicleByOrderAsync(orderId);

                return Ok(vehicleOrderList);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Obtiene todos los pedidos de un vehiculo
        /// </summary>
        /// <param name="vehicleId">Identificador del vehiculo</param>
        /// <returns><see cref="VehicleOrders"/>VehicleOrders</returns>
        [HttpGet]
        [Route("{vehicleId}/order")]
        public async Task<ActionResult<VehicleOrders>> GetAllVehicleOrders(Guid vehicleId)
        {
            try
            {
                var vehicleOrderList = await _vehicleQueries.GetVehicleOrdersAsync(vehicleId);

                return Ok(vehicleOrderList);
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Agrega un pedido a un vehiculo
        /// </summary>
        /// <param name="vehicleId">Identificador del vehiculo</param>
        /// <param name="order">Información del pedido</param>
        [HttpPost]
        [Route("{vehicleId}/order")]
        public async Task<IActionResult> AddOrder(Guid vehicleId, [FromBody] Order order)
        {
            bool commandResult = false;

            if (vehicleId != Guid.Empty)
            {
                var addOrderCommand = new AddOrderCommand(vehicleId, order.OrderId);

                _logger.LogInformation(
                    "Sending command: {CommandName} - {VehicleId}: {OrderId} ({@Command})",
                    addOrderCommand.GetType().Name,
                    addOrderCommand.VehicleId,
                    addOrderCommand.OrderId,
                    addOrderCommand);

                commandResult = await _mediator.Send(addOrderCommand);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Elimina un pedido de un vehiculo
        /// </summary>
        /// <param name="vehicleId">Identificador del vehiculo</param>
        /// <param name="orderId">Identificador del pedido</param>
        [HttpDelete]
        [Route("{vehicleId}/order/{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid vehicleId, Guid orderId)
        {
            bool commandResult = false;

            if (vehicleId != Guid.Empty)
            {
                var removeOrderCommand = new RemoveOrderCommand(vehicleId, orderId);

                _logger.LogInformation(
                    "Sending command: {CommandName} - {VehicleId}: {OrderId} ({@Command})",
                    removeOrderCommand.GetType().Name,
                    removeOrderCommand.VehicleId,
                    removeOrderCommand.OrderId,
                    removeOrderCommand);

                commandResult = await _mediator.Send(removeOrderCommand);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        #endregion
    }
}
