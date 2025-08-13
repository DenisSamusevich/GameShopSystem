namespace GameShopSystem.Application.DTOs
{
    public record PaymentResultDto
    {
        public bool IsSuccess { get; }
        public string TransactionId { get; }

        public PaymentResultDto(bool isSuccess, string transactionId)
        {
            IsSuccess = isSuccess;
            TransactionId = transactionId;
        }
    }
}
