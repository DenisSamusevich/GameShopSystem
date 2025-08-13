using GameShopSystem.Application.DTOs;

namespace GameShopSystem.Application.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentResultDto> ChargeAsync(Guid playerId, int amount);
    }
}
