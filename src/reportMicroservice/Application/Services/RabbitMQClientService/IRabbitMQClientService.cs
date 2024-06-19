using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.RabbitMQClientService;
public interface IRabbitMQClientService
{
    Task<CounterDetailsDto> GetCounterDetailsAsync(Guid counterId);
}
