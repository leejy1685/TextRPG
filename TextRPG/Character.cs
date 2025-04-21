using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Character
    {
        public int Level { get; private set; }
        public string Name { get; }
        public string Job { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; set; }
        public int Gold { get; set; }

        public int Exp { get; private set; }
        public int ExpBar { get; private set; }

        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        public List<Item> Inventory = new List<Item>();
        public Item[] EquipList = new Item[2];

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }

        public Character(int level, string name, string job, int atk, int def, int hp, int gold)
        {
            Level = level;
            Name = name;
            Job = job;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
            Exp = 0;
            ExpBar = 1;
        }

        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            Console.WriteLine($"{Name} {{ {Job} }}");
            Console.WriteLine(ExtraAtk == 0 ? $"공격력 : {Atk}" : $"공격력 : {Atk + ExtraAtk} (+{ExtraAtk})");
            Console.WriteLine(ExtraDef == 0 ? $"방어력 : {Def}" : $"방어력 : {Def + ExtraDef} (+{ExtraDef})");
            Console.WriteLine($"체력 : {Hp}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        public void DisplayInventory(bool showIdx,bool showPrice)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];

                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";
                string displayPrice = showPrice ? $"{targetItem.Price / 100 * 85} G" : "";
                Console.WriteLine($"- {displayIdx}{displayEquipped} {targetItem.ItemInfoText()} {displayPrice}");
            }
        }

        public void EquipItem(Item item)
        {
            if (IsEquipped(item))
            {
                if (item.Type == 0)
                {
                    EquipList[item.Type] = new Item();
                    ExtraAtk -= item.Value;
                }
                else
                {
                    EquipList[item.Type] = new Item();
                    ExtraDef -= item.Value;
                }
            }
            else
            {
                if (item.Type == 0)
                {
                    EquipList[item.Type] = item;
                    ExtraAtk += item.Value;
                }
                else
                {
                    EquipList[item.Type] =item;
                    ExtraDef += item.Value;
                }
            }
        }

        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }

        public void BuyItem(Item item)
        {
            Gold -= item.Price;
            Inventory.Add(item);
        }

        public void SellItem(Item item)
        {
            Gold += item.Price/100*85;
            Inventory.Remove(item);
        }

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }

        public bool LevelUp()
        {
            Exp++;
            if(ExpBar == Exp)
            {
                Level++;
                ExpBar++;
                Exp = 0;
                return true;    
            }
            return false;
        }
    }
}
