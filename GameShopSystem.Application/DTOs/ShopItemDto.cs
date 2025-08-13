namespace GameShopSystem.Application.DTOs
{
    public record ShopItemDto
    {
        public Guid ItemGuid { get; }
        public int Price { get; }
        public int DiscountPrice { get; }
        public ShopItemDto(Guid itemGuid, int price, int discountPrice)
        {
            ItemGuid = itemGuid;
            Price = price;
            DiscountPrice = discountPrice;
        }
    }
}
