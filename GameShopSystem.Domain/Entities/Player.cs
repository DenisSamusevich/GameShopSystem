using GameShopSystem.Domain.DomainEvemts;

namespace GameShopSystem.Domain.Entities
{
    public class Player
    {
        public Guid Id { get; }
        public int BalanceGold { get; set; }
        public int BalanceGems { get; set; }
        public IList<PurchaseEvent> PurchaseEvents { get; private set; } = new List<PurchaseEvent>();
        public IList<RealMoneyPurchaseEvent> RealMoneyPurchaseEvents { get; private set; } = new List<RealMoneyPurchaseEvent>();
        public Player(Guid id, int balanceGold, int balanceGems)
        {
            Id = id;
            BalanceGold = balanceGold;
            BalanceGems = balanceGems;
        }

        public void AddPurchasedEvent(PurchaseEvent purchaseEvent)
        {
            PurchaseEvents.Add(purchaseEvent);
        }

        public void AddRealMoneyPurchasedEvent(RealMoneyPurchaseEvent realMoneyPurchaseEvent)
        {
            RealMoneyPurchaseEvents.Add(realMoneyPurchaseEvent);
        }
    }
}
