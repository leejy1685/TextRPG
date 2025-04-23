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
        public int Mp { get; set; } // MP - 스킬 사용에 필요한 마나
        public int Maxhp { get; set; } // 최대 hp
        public int Maxmp { get; set; } // 최대 mp
        public int Beforehp { get; set; } // 전투 시작 시점의 hp
        public int Gold { get; set; }

        public int Exp { get; private set; }
        public int ExpBar { get; private set; }

        public int ExtraAtk { get; private set; }
        public int ExtraDef { get; private set; }

        public List<Item> Inventory = new List<Item>();
        public Item[] EquipList = new Item[2];

        public Skill[] skillDb; // 스킬 DB

        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }

        public Character(int level, string name, Job job, int mp, int gold)
        {
            Level = level;
            Name = name;
            Mp = mp; // mp 초기화
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
            Maxhp = Hp; // 최대 hp 저장
            Maxmp = Mp; // 최대 mp 저장
            Mp = mp;
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

        public void DisplayInventory(bool showIdx, bool showPrice)
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
                    EquipList[item.Type] = item;
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
            Gold += item.Price / 100 * 85;
            Inventory.Remove(item);
        }

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }

        public bool LevelUp(Monster monster)
        {
            // 경험치 획득량 계산 : 몬스터들의 레벨을 합친 값
            int sumExp = monster.level;

            Exp += sumExp; // 경험치 획득

            if (Exp >= ExpBar) // 현재 Exp가 ExpBar 이상일 경우
            {
                Level++; // 레벨업
                Exp -= ExpBar; // 레벨업 후 잔존 경험치 계산

                switch (Level) // 레벨업에 따른 ExpBar 증가
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

        public void DisplayBattlePlayerInfo() // 전투 중 플레이어 정보 표시
        {
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
            Console.WriteLine($"Lv.{Level} {Name} ({jobStr})\nHP {Hp}/{Maxhp}\nMP {Mp}/{Maxmp}");
        }

        public void SkillSet() // 스킬 목록
        {
            skillDb = new Skill[]
            {
                // 이름, 비용, 공격력 배율, 설명
                new Skill("알파 스트라이크", 10, 2.0f, $"공격력 * 2 로 하나의 적을 공격합니다."),
                new Skill("더블 스트라이크", 15, 1.5f, $"공격력 * 1.5 로 적을 랜덤으로 공격합니다.")
            };
        }

        public int PlayerDamage() // 몬스터가 입을 피해량 계산 (일반공격)
        {
            float damageRandom; // 1차적으로 계산된 데미지 (실수)
            int GetDamage; // 소숫점 이하 올림 처리된 데미지

            // ## 플레이어측이 몬스터에게 가할 피해량 계산 ##
            Random random = new Random(); // 랜덤 클래스 인스턴스 생성

            // 플레이어 공격력 * 최소(0.9) ~ 최대(1.1) 랜덤값 계산
            damageRandom = Atk * random.Next(9, 12) / 10.0f;

            GetDamage = (int)Math.Ceiling(damageRandom); // 랜덤값 올림 처리

            return GetDamage; // 데미지 return
        }

        public int PlayerDamage(float skillValue) // 몬스터가 입을 피해량 계산 (스킬공격)
        {
            float damageRandom; // 1차적으로 계산된 데미지 (실수)
            int GetDamage; // 소숫점 이하 올림 처리된 데미지

            // ## 플레이어측이 몬스터에게 가할 피해량 계산 ##
            Random random = new Random(); // 랜덤 클래스 인스턴스 생성

            // 플레이어 공격력 * 스킬 배율 * 최소(0.9) ~ 최대(1.1) 랜덤값 계산
            damageRandom = Atk * skillValue * random.Next(9, 12) / 10.0f;

            GetDamage = (int)Math.Ceiling(damageRandom); // 랜덤값 올림 처리

            return GetDamage; // 데미지 return
        }

        public bool isCrit() // 치명타 발동 여부 체크
        {
            Random random = new Random(); // 랜덤 클래스 인스턴스 생성
            int critCheck = random.Next(1, 101);
            if(critCheck <= 15) // 치명타 발생 : 랜덤값이 1 ~ 15
            {
                return true;
            }
            else // 치명타 미발생 : 랜덤값이 16 ~ 100
            {
                return false;
            }
        }

        public bool isDie() // 플레이어 사망 여부 체크
        {
            if (Hp <= 0)
            {
                Hp = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void recoveryMp()
        {
            Mp += 10;
            Mp = Mp >= Maxmp ? Maxmp : Mp;
        }
    }
}
