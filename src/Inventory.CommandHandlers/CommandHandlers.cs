using AgileWorkshop.Bus;
using AgileWorkshop.Cqrs.Core;
using Inventory.Commands;
using Inventory.Domain;
using Inventory.EventStore;

namespace Inventory.CommandHandlers
{
    public class InventoryCommandHandlers : IHandle<CreateInventoryItem>, IHandle<DeactivateInventoryItem>, IHandle<RemoveItemsFromInventory>, IHandle<CheckInItemsToInventory>, IHandle<RenameInventoryItem>
    {
        private readonly IDomainRepository<InventoryItem> domainRepository;
        public InventoryCommandHandlers(IDomainRepository<InventoryItem> domainRepository)
        {
            this.domainRepository = domainRepository;
        }
        public void Handle(CreateInventoryItem message)
        {
            var item = new InventoryItem(message.InventoryItemId, message.Name);
            domainRepository.Save(item, -1);
        }
        public void Handle(DeactivateInventoryItem message)
        {
            var item = domainRepository.GetById(message.InventoryItemId);
            item.Deactivate();
            domainRepository.Save(item, message.OriginalVersion);
        }
        public void Handle(RemoveItemsFromInventory message)
        {
            var item = domainRepository.GetById(message.InventoryItemId);
            item.Remove(message.Count);
            domainRepository.Save(item, message.OriginalVersion);
        }
        public void Handle(CheckInItemsToInventory message)
        {
            var item = domainRepository.GetById(message.InventoryItemId);
            item.CheckIn(message.Count);
            domainRepository.Save(item, message.OriginalVersion);
        }
        public void Handle(RenameInventoryItem message)
        {
            var item = domainRepository.GetById(message.InventoryItemId);
            item.ChangeName(message.NewName);
            domainRepository.Save(item, message.OriginalVersion);
        }
    }

}
