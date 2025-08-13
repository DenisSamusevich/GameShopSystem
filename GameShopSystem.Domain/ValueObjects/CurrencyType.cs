namespace GameShopSystem.Domain.ValueObjects
{
    [Flags]
    public enum CurrencyType
    {
        Gold = 1 << 0,
        Gems = 1 << 1,
        RealMoney = 1 << 2,
    }
}
