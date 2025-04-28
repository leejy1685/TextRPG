using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        public int Level { get; private set; }//레벨
        public string Name { get; }//이름
        public Job job { get; set; }//직업
        public float Atk { get; set; }//공격력
        public int Def { get; set; }//방어력
        public int Hp { get; set; }//체력
        public int Mp { get; set; } //마나
        public int Maxhp { get; set; } // 최대 hp
        public int Maxmp { get; set; } // 최대 mp
        public int Beforehp { get; set; } // 전투 시작 시점의 hp
        public int Beforemp { get; set; } // 전투 시작 시점의 mp
        public int Gold { get; set; }//골드

        public int Exp { get; private set; }//경험치
        public int ExpBar { get; private set; }//경험치 바

        public int ExtraAtk { get; private set; }//장비 공격력
        public int ExtraDef { get; private set; }//장비 방어력

        public List<Item> Inventory = new List<Item>(); //인벤토리
        public Item[] EquipList = new Item[2];  //장비 장착 리스트

        public Skill[] skillDb; // 스킬 DB
        public int InventoryCount
        {
            get
            {
                return Inventory.Count;
            }
        }   //인벤토리 길이

        public int critical {  get; set; } //치트용 크확

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
            critical = 15;
        }//생성자

        public void DisplayCharacterInfo()
        {
            Console.Write($"Lv. ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"{Level:D2}");
            Console.ResetColor();

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

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Name);
            Console.ResetColor();
            Console.Write(" { ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(jobStr);
            Console.ResetColor();
            Console.WriteLine(" }");

            if (ExtraAtk == 0)
            {
                Console.Write("공격력 : ");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(Atk);
                Console.ResetColor();
            }
            else
            {
                Console.Write("공격력 : ");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(Atk);
                Console.ResetColor();

                Console.Write(" (+");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(ExtraAtk);
                Console.ResetColor();
                Console.WriteLine(")");
            }

            if (ExtraDef == 0)
            {
                Console.Write("방어력 : ");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine(Def);
                Console.ResetColor();
            }
            else
            {
                Console.Write("방어력 : ");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(Def);
                Console.ResetColor();

                Console.Write(" (+");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(ExtraDef);
                Console.ResetColor();
                Console.WriteLine(")");
            }

            Console.Write($"체력 : ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(Hp);
            Console.ResetColor();

            Console.Write($"마나 : ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(Mp);
            Console.ResetColor();

            Console.Write($"Gold : ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(Gold);
            Console.ResetColor();
            Console.WriteLine(" G");
        }//캐릭터 상태창

        public void DisplayInventory(bool showIdx, bool showPrice)
        {
            for (int i = 0; i < Inventory.Count; i++)
            {
                Item targetItem = Inventory[i];
                string displayIdx = showIdx ? $"{i + 1} " : "";
                string displayEquipped = IsEquipped(targetItem) ? "[E]" : "";

                string displayPrice = showPrice ? $"{targetItem.Price / 100 * 85} G" : "";

                Console.Write($"- ");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write(displayIdx);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(displayEquipped);
                Console.ResetColor();

                Console.Write(" ");
                targetItem.ItemInfoText();
                Console.WriteLine($" {displayPrice}");

            }
        }//인벤토리 아이템 확인

        public void EquipItem(Item item)
        {
            if (item.Type >= 2)
            {
                Console.WriteLine("장착할 수 없는 아이템입니다.");
                Console.ReadLine();
                return;
            }

            if (IsEquipped(item))
            {
                if (item.Type == 0)
                {
                    Atk -= ExtraAtk;
                    ExtraAtk -= EquipList[item.Type].Value;
                    EquipList[item.Type] = new Item();
                }
                else
                {
                    Def -= ExtraDef;
                    ExtraDef -= EquipList[item.Type].Value;
                    EquipList[item.Type] = new Item();
                }
            }
            else
            {
                if (item.Type == 0)
                {
                    if (EquipList[item.Type] != null)
                    {
                        Atk -= ExtraAtk;
                        ExtraAtk -= EquipList[item.Type].Value;
                    }
                    EquipList[item.Type] = item;
                    ExtraAtk += item.Value;
                    Atk += ExtraAtk;
                }
                else
                {
                    if (EquipList[item.Type] != null)
                    {
                        Def -= ExtraDef;
                        ExtraDef -= EquipList[item.Type].Value;
                    }
                    EquipList[item.Type] = item;
                    ExtraDef += item.Value;
                    Def += ExtraDef;
                }
            }
        }//장비 장착

        public bool IsEquipped(Item item)
        {
            return EquipList.Contains(item);
        }//해당 아이템 장착 여부

        public bool HasItem(Item item)
        {
            return Inventory.Contains(item);
        }//아이템을 지니고 있는지 판단

        public bool LevelUp(Monster monster)
        {
            // 경험치 획득량 계산 : 몬스터들의 레벨을 합친 값
            int sumExp = monster.level;

            Exp += sumExp; // 경험치 획득

            if (Exp >= ExpBar) // 현재 Exp가 ExpBar 이상일 경우
            {
                Level++; // 레벨업
                Exp -= ExpBar; // 레벨업 후 잔존 경험치 계산
                Atk += 0.5f;
                Def += 1;

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
        }//경험치 습득 혹은 레벨업

        public void DisplayBattlePlayerInfo()
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
            Console.WriteLine($"Lv.{Level} {Name} ({jobStr})");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HP {Hp}/{Maxhp}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"MP {Mp}/{Maxmp}");
            Console.ResetColor();
        }// 전투 중 플레이어 정보 표시

        public void SkillSet()
        {
            skillDb = new Skill[]
            {
                // 이름, 비용, 공격력 배율, 설명
                new Skill("알파 스트라이크", 10, 2.0f, $"공격력 * 2 로 하나의 적을 공격합니다."),
                new Skill("더블 스트라이크", 15, 1.5f, $"공격력 * 1.5 로 적을 랜덤으로 공격합니다.")
            };
        }// 스킬 목록

        public int PlayerDamage() 
        {
            float damageRandom; // 1차적으로 계산된 데미지 (실수)
            int GetDamage; // 소숫점 이하 올림 처리된 데미지

            // ## 플레이어측이 몬스터에게 가할 피해량 계산 ##
            Random random = new Random(); // 랜덤 클래스 인스턴스 생성

            // 플레이어 공격력 * 최소(0.9) ~ 최대(1.1) 랜덤값 계산
            damageRandom = Atk * random.Next(9, 12) / 10.0f;

            GetDamage = (int)Math.Ceiling(damageRandom); // 랜덤값 올림 처리

            return GetDamage; // 데미지 return
        }// 몬스터가 입을 피해량 계산 (일반공격)

        public int PlayerDamage(float skillValue) 
        {
            float damageRandom; // 1차적으로 계산된 데미지 (실수)
            int GetDamage; // 소숫점 이하 올림 처리된 데미지

            // ## 플레이어측이 몬스터에게 가할 피해량 계산 ##
            Random random = new Random(); // 랜덤 클래스 인스턴스 생성

            // 플레이어 공격력 * 스킬 배율 * 최소(0.9) ~ 최대(1.1) 랜덤값 계산
            damageRandom = Atk * skillValue * random.Next(9, 12) / 10.0f;

            GetDamage = (int)Math.Ceiling(damageRandom); // 랜덤값 올림 처리

            return GetDamage; // 데미지 return
        }// 몬스터가 입을 피해량 계산 (스킬공격)

        public bool isCrit()
        {
            Random random = new Random(); // 랜덤 클래스 인스턴스 생성
            int critCheck = random.Next(1, 101);
            return critCheck <= critical; // 치명타 발생 : 랜덤값이 1 ~ 15
        }// 치명타 발동 여부 체크

        public void beforeSave()
        {
            Beforehp = Hp;  //전투 시작 시점의 체력 저장
            Beforemp = Mp;  //전투 시작 시점의 마나 저장
        }//전투 입장 전 체력 마나 체크

        public bool isDie() 
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
        }// 플레이어 사망 여부 체크

        public void recoveryMp()
        {
            Mp += 10;
            Mp = Mp >= Maxmp ? Maxmp : Mp;
        }//마나 회복 메서드

        public void UsePotion(Item item)
        {
            Hp += item.Value; // item value 값만큼 체력 회복
            if (Hp > Maxhp) // 현재 체력이 최대 체력을 초과할 경우
            {
                Hp = Maxhp; // 현재 체력을 최대 체력으로 설정
            }
            Inventory.Remove(item); // 인벤토리에서 대상 아이템 - 포션 제거
        }// 포션 사용

        public int numberOfPotion() 
        {
            int num = 0; // 포션 개수를 담을 변수

            foreach (Item item in Inventory) // 인벤토리 내 아이템 체크
            {
                if (item.Type == 2) // 아이템의 타입이 2라면 = 포션이라면
                {
                    num += 1; // 개수 + 1
                }
            }

            return num; // 포션 개수 return
        }// 포션 개수 파악
    }
}
