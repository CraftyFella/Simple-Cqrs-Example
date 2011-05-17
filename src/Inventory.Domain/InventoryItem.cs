using System;
using AgileWorkshop.Cqrs.Core;
using Inventory.Events;

namespace Inventory.Domain
{
    public class InventoryItem : AggregateRoot
    {
        private bool _activated;
        private Guid _id;
        private int _stockLevel;

        private void Apply(InventoryItemCreated e)
        {
            _id = e.Id;
            _activated = true;
            _stockLevel = 0;
        }

        private void Apply(InventoryItemDeactivated e)
        {
            _activated = false;
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new InventoryItemRenamed(_id, newName));
        }

        public void Remove(int count)
        {
            if (count <= 0) throw new InvalidOperationException("cant remove negative count from inventory");
            if ((_stockLevel - count) < 0) throw new InvalidOperationException("cant remove more than wotz in the inventory");
            ApplyChange(new ItemsRemovedFromInventory(_id, count));
        }


        public void CheckIn(int count)
        {
            if(count <= 0) throw new InvalidOperationException("must have a count greater than 0 to add to inventory");
            ApplyChange(new ItemsCheckedInToInventory(_id, count));
        }

        public void Deactivate()
        {
            if(!_activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new InventoryItemDeactivated(_id));
        }

        public override Guid Id
        {
            get { return _id; }
        }

        public InventoryItem()
        {
            // used to create in repository ... many ways to avoid this, eg making private constructor
        }

        public InventoryItem(Guid id, string name)
        {
            ApplyChange(new InventoryItemCreated(id, name));
        }
    }
}

