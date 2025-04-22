using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    enum Job
    {
        Warrior,
        Thief,
        Barbarian
    };

    class Character
    {
        public int Level { get; private set; }
        public string Name { get; }
        public Job job { get; set; }
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

        public Character(int level, string name, Job job, int gold)
        {
            Level = level;
            Name = name;
            this.job = job;
            switch (this.job)   //직업에 따른 스텟 분배
            {
                case Job.Warrior:
                    Atk = 10;
                    Def = 5;
                    Hp = 100;
                    break;

                case Job.Thief:
                    Atk = 12;
                    Def = 5;
                    Hp = 90;
                    break;

                case Job.Barbarian:
                    Atk = 8;
                    Def = 5;
                    Hp = 120;
                    break;
            }
            Gold = gold;
            Exp = 0;
            ExpBar = 10;
        }

        public void DisplayCharacterInfo()
        {
            Console.WriteLine($"Lv. {Level:D2}");
            string jobStr = "";
            switch (job)
            {
                case Job.Warrior:
                    jobStr = "전사";
                    break;
                case Job.Thief:
                    jobStr = "도적";
                    break;
                case Job.Barbarian:
                    jobStr = "바바리안";
                    break;
            }
            Console.WriteLine($"{Name} {{ {jobStr} }}");
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

        public bool LevelUp(Monster monster)
        {
            // 경험치 획득량 계산 : 몬스터들의 레벨을 합친 값
            int sumExp = monster.Level;

            Exp += sumExp; // 경험치 획득

            if(Exp >= ExpBar) // 현재 Exp가 ExpBar 이상일 경우
            {
                Level++; // 레벨업
                Exp -= ExpBar; // 레벨업 후 잔존 경험치 계산

                switch(Level) // 레벨업에 따른 ExpBar 증가
                {
                    case 2: // 레벨이 2로 올랐을 때
                        ExpBar = 35; // 2 -> 3을 위한 값
                        break;
                    case 3: // 레벨이 3으로 올랐을 때
                        ExpBar = 65; // 3 -> 4를 위한 값
                        break;
                    case 4: // 레벨이 4로 올랐을 때
                        ExpBar = 100; // 4 -> 5를 위한 값
                        break;
                    default: // 레벨이 5이상일 때
                        ExpBar = 100;
                        break;
                }
                return true;    
            }
            return false;
        }

        //public void PlayerGetDamage(Monster monster) // 플레이어가 입는 피해 계산
        //{
        //    // ## 플레이어가 입을 피해량 계산 ##
        //    Random rand = new Random(); // 랜덤 클래스 인스턴스 생성

        //    // 몬스터 공격력의 최소(0.9) ~ 최대(1.1) 랜덤값 계산
        //    // double damageRandom = rand.NextDouble(monster.Atk * 0.9, monster.Atk * 1.1); 
        //    // int GetDamage = (int)Math.Ceiling(damageRandom); // 랜덤값 올림 처리

        //    // ## 플레이어 체력 차감 ##
        //    // Hp -= GetDamage; // 체력에서 데미지 차감
        //    if (Hp <= 0) // 만약 체력이 0 이하라면
        //    {
        //        Hp = 0; // 체력을 0으로 설정 - 사망
        //    }
        //}
    }
}
