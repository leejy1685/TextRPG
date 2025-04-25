using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRPG
{
    class GameManager4
    {
        //저장 경로
        string path = AppDomain.CurrentDomain.BaseDirectory;
        //게임 진행에 필요한 3가지 클래스
        private Character player;
        private Item[] itemDb;
        private Dungeon[] dungeons;
        Monster[] monstersDb;
        Monster[] monsters;

        private Random random;

        public GameManager4()
        {
            random = new Random();
        }

        public void SetData()   //게임 첫 시작시 생성되는 정보들
        {
            player = new Character(1, nameCreate(), jobSelect(), 50, 1500);
            monsters = new Monster[4];

            itemDb = new Item[]
            {
            new Item("수련자의 갑옷", 1, 5,"수련에 도움을 주는 갑옷입니다. ",1000),
            new Item("그래도 좋은 갑옷", 1,7, "적당한 선능에 그럭저럭 쓸만한 갑옷입니다.", 1800),
            new Item("무쇠갑옷", 1, 9,"무쇠로 만들어져 튼튼한 갑옷입니다. ",2000),
            new Item("스파르타의 갑옷", 1, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",3500),
            new Item("낣은 검", 0, 2,"쉽게 볼 수 있는 낡은 검 입니다. ",600),
            new Item("좋은 검", 0,4, "잘 다듬어져 있는 가성비 좋은 검 입니다.", 1000),
            new Item("청동 도끼", 0, 5,"어디선가 사용됐던거 같은 도끼입니다. ",1500),
            new Item("스파르타의 창", 0, 7,"스파르타의 전사들이 사용했다는 전설의 창입니다. ",2500)
            };


            monstersDb = new Monster[]
            {
                    new Monster(2, "미니언", 5, 15,itemDb[4],100),
                    new Monster(3,"공허충",9,10,itemDb[5],200),
                    new Monster(5,"대포미니언",10,20,itemDb[6],300)
            };
        }

        //플레이어 캐릭터의 이름을 만드는 메서드
        string nameCreate()
        {
            Console.Clear();

            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정해 주세요\n");

            string name = Console.ReadLine();


            Console.WriteLine("\n입력하신 이름은 {0} 입니다.\n", name);
            Console.WriteLine("1. 저장\n2. 취소\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요.");

            int command = inputCommand(1, 2);

            switch (command)
            {
                case 1:
                    break;
                case 2:
                    nameCreate();
                    break;
            }

            return name;
        }

        //플레이어 캐릭터의 직업을 선택하는 메서드
        Job jobSelect()
        {
            Job job = Job.Warrior;
            Console.Clear();

            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 직업을 설정해 주세요.");
            Console.WriteLine();
            Console.WriteLine("1. 전사");
            Console.WriteLine("2. 도적");
            Console.WriteLine("3. 바바리안");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해 주세요.");
            int command = inputCommand(1, 3);

            switch (command)
            {
                case 1:
                    job = Job.Warrior;
                    break;

                case 2:
                    job = Job.Thief;
                    break;

                case 3:
                    job = Job.Barbarian;
                    break;
            }

            return job;
        }
        //게임을 진행하는 메서드
        public void DisplayMainUI()
        {

            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            //Console.WriteLine("0. 저장 후 종료");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");


            int command = inputCommand(1, 5);

            switch (command)
            {
                case 1:
                    DisplayStatUI();//캐릭터 정보 표시
                    break;

                case 2:
                    DisplayInventoryUI();//인벤토리 열기
                    break;

                case 3:
                    DisplayShopUI();//상점 열기
                    break;
                case 4:
                    player.beforeSave(); // 전투 시작 시점 hp 저장
                    DisplayBattleUI();//던전 열기
                    break;
                case 5:
                    DisplayRestUI();//휴식
                    break;
                    //case 0:
                    //    saveData();
                    //    return;
                    //    break;
            }



        }
        void DisplayStatUI() //캐릭터의 정보 보기
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();

            player.DisplayCharacterInfo();

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(0, 0);

            switch (command)
            {
                case 0:
                    DisplayMainUI();
                    break;
            }
        }

        //인벤토리 확인
        void DisplayInventoryUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.DisplayInventory(false, false);

            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = inputCommand(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;

                case 1:
                    DisplayEquipUI();
                    break;
            }
        }

        //장비 장착 관리하는 메서드
        void DisplayEquipUI()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.DisplayInventory(true, false);

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(0, player.InventoryCount);

            switch (command)
            {
                case 0:
                    DisplayInventoryUI();
                    break;

                default:

                    int itemIdx = command - 1;
                    Item targetItem = itemDb[itemIdx];
                    player.EquipItem(targetItem);

                    DisplayEquipUI();
                    break;
            }


        }

        //상점을 여는 메서드
        void DisplayShopUI()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < itemDb.Length; i++)
            {
                Item curItem = itemDb[i];

                string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
                Console.WriteLine($"- {curItem.ItemInfoText()}  |  {displayPrice}");
            }

            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = inputCommand(0, 2);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;

                case 1:
                    DisplayBuyUI();
                    break;

                case 2:
                    DisplaySellUI();
                    break;
            }
        }

        //아이템 구매창을 띄우는 메서드
        void DisplayBuyUI()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < itemDb.Length; i++)
            {
                Item curItem = itemDb[i];

                string displayPrice = (player.HasItem(curItem) ? "구매완료" : $"{curItem.Price} G");
                Console.WriteLine($"- {i + 1} {curItem.ItemInfoText()}  |  {displayPrice}");
            }

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(0, itemDb.Length);

            switch (command)
            {
                case 0:
                    DisplayShopUI();
                    break;

                default:
                    int itemIdx = command - 1;
                    Item targetItem = itemDb[itemIdx];

                    // 이미 구매한 아이템이라면?
                    if (player.HasItem(targetItem))
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        Console.WriteLine("Enter 를 눌러주세요.");
                        Console.ReadLine();
                    }
                    else // 구매가 가능할떄
                    {
                        //   소지금이 충분하다
                        if (player.Gold >= targetItem.Price)
                        {
                            Console.WriteLine("구매를 완료했습니다.");
                            player.BuyItem(targetItem);
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다.");
                            Console.WriteLine("Enter 를 눌러주세요.");
                            Console.ReadLine();
                        }
                    }

                    DisplayBuyUI();
                    break;
            }
        }

        //아이템 판매창을 띄우는 메서드
        void DisplaySellUI()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            player.DisplayInventory(true, true);

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            int command = inputCommand(0, player.InventoryCount);

            switch (command)
            {
                case 0:
                    DisplayShopUI();
                    break;

                default:
                    int itemIdx = command - 1;
                    Item targetItem = player.Inventory[itemIdx];
                    if (player.IsEquipped(targetItem))
                    {
                        player.EquipItem(targetItem);
                        player.SellItem(targetItem);
                    }
                    else
                    {
                        player.SellItem(targetItem);
                    }

                    DisplaySellUI();
                    break;
            }

        }

        void DisplayBattleUI()
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            //몬스터 생성되는 함수

            //몬스터 정보 표시되는 함수
            foreach (Monster monster in monsters)
            {
                Console.WriteLine(monster.monsterInfo());
            }
            Console.WriteLine();
            Console.WriteLine("[내정보]");
            player.DisplayBattlePlayerInfo();
            Console.WriteLine();

            Console.WriteLine("1. 알파 스트라이크");
            Console.WriteLine("2. 더블 스트라이크");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(1, 2);

            switch (command)
            {
                case 1:
                    AlphaStrike(command);
                    //DisplayAttackUI(false);
                    break;
                case 2:
                    DoubleStrike(command);
                    break;

            }


        }

        void DisplaySkillUI() // 전투 - 스킬 목록 확인
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            //몬스터 생성되는 함수

            //몬스터 정보 표시되는 함수
            foreach (Monster monster in monsters)
            {
                Console.WriteLine(monster.monsterInfo());
            }
            Console.WriteLine();
            Console.WriteLine("[내정보]");
            player.DisplayBattlePlayerInfo();
            Console.WriteLine();

            //스킬 목록 출력
            for (int i = 0; i < player.skillDb.Length; i++)
            {
                Console.Write($"{i + 1}. ");
                player.skillDb[i].skillInfo();
            }

            Console.WriteLine("0. 취소");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(0, 2);

            switch (command)
            {
                case 0:
                    DisplayBattleUI(); // 공격 or 스킬 선택 화면으로 돌아가기
                    break;
                case 1:
                    AlphaStrike(command);
                    //DisplayAttackUI(false);
                    break;
                case 2:
                    DoubleStrike(command);
                    break;

            }
        }



        int DisplayAttackUI(bool skill)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            //몬스터 생성되는 함수

            //몬스터 정보 표시되는 함수
            for (int i = 0; i < monsters.Length; i++)
            {
                Console.WriteLine($"{i + 1} {monsters[i].monsterInfo()}");
            }

            Console.WriteLine();
            Console.WriteLine("[내정보]");

            // 캐릭터 정보
            Console.WriteLine();
            Console.WriteLine("0. 취소");
            Console.WriteLine("대상을 선택해주세요.");

            int command = inputCommand(0, monsters.Length);

            switch (command)
            {
                case 0:
                    DisplayBattleUI();
                    break;
                default:
                    int targetMonster = command - 1;
                    if (skill)
                        return targetMonster;
                    DisplayMonsterDamageUI(targetMonster);
                    break;



            }
            return -1;
        }

        void DisplayMonsterDamageUI(int targetMonster)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            Console.WriteLine($"{player.Name}의 공격!");

            if (monsters[targetMonster].isEvasion()) // 몬스터의 회피 성공 시
            {
                Console.WriteLine($"Lv{monsters[targetMonster].level} {monsters[targetMonster].name} 을(를) 공격했지만 아무일도 일어나지 않았습니다.\n");
            }
            else // 몬스터의 회피 실패 시
            {
                Console.WriteLine($"Lv{monsters[targetMonster].level} {monsters[targetMonster].name} 을(를) 맞췄습니다. " +
                $"[대미지 : {player.PlayerDamage()}]");
                Console.WriteLine();
                Console.WriteLine($"Lv{monsters[targetMonster].level} {monsters[targetMonster].name} ");

                int monsterHp = monsters[targetMonster].Hp - player.PlayerDamage();

                Console.WriteLine("HP {0} -> {1}", monsters[targetMonster].Hp, monsterHp <= 0 ? "Dead" : monsterHp);

                //대미지 입히는거 계산
                monsters[targetMonster].Hp = monsterHp;
            }

            Console.WriteLine("0. 다음");

            int command = inputCommand(0, 0);

            switch (command)
            {
                case 0:
                    int aliveMon = monsters.Length;
                    foreach (Monster monster in monsters)
                    {
                        if (monster.isDie())
                            aliveMon--;
                    }
                    if (aliveMon == 0)
                        DisplayVictoryUI();

                    DisplayEnemyPhaseUI();
                    break;
            }

        }

        void DisplaySkillDamageUI(int target, int skillNum)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            Console.WriteLine($"{player.Name}의 공격!");
            Console.WriteLine($"Lv{monsters[target].level} {monsters[target].name} 을(를) 맞췄습니다. " +
                $"[대미지 : {player.PlayerDamage(player.skillDb[skillNum].Value)}]");
            Console.WriteLine();
            Console.WriteLine($"Lv{monsters[target].level} {monsters[target].name} ");

            int monsterHp = monsters[target].Hp - player.PlayerDamage(player.skillDb[skillNum].Value);

            Console.WriteLine("HP {0} -> {1}", monsters[target].Hp, monsterHp <= 0 ? "Dead" : monsterHp);

            //대미지 입히는거 계산
            monsters[target].Hp = monsterHp;

            Console.WriteLine("0. 다음");

            int command = inputCommand(0, 0);

            switch (command)
            {
                case 0:
                    int aliveMon = monsters.Length;
                    foreach (Monster monster in monsters)
                    {
                        if (monster.isDie())
                            aliveMon--;
                    }
                    if (aliveMon == 0)
                        DisplayVictoryUI();


                    break;
            }

        }


        void DisplayEnemyPhaseUI()
        {
            foreach (Monster monster in monsters)
            {
                if (!monster.isDie())
                {
                    Console.Clear();
                    Console.WriteLine("Battle!!\n");

                    Console.WriteLine($"Lv.{monster.level} {monster.name} 의 공격!");
                    Console.WriteLine($"{player.Name} 을(를) 맞췄습니다. [데미지 : {monster.MonsterDamage()}]");
                    Console.WriteLine();
                    Console.WriteLine($"Lv.{player.Level} {player.Name}");
                    Console.WriteLine($"HP {player.Hp} -> {player.Hp - monster.MonsterDamage()}");

                    player.Hp -= monster.MonsterDamage();

                    Console.WriteLine("0. 다음\n");
                    Console.WriteLine("대상을 선택해주세요.");

                    int command = inputCommand(0, 0);

                    switch (command)
                    {
                        case 0:
                            if (player.isDie())
                                DisplayLoseUI();
                            else
                                DisplayBattleUI();
                            break;
                    }


                }
            }
        }


        void DisplayVictoryUI()
        {
            //캐릭터 마나 회복, 캐릭터가 여러번 마나회복을
            //하는 것을 막기 위해서 턴 종료 시점에 회복
            player.recoveryMp();

            int totalGetGold = 0; // 총합 획득 골드

            Console.Clear();
            Console.WriteLine("Battle!! - Result\n");
            Console.WriteLine("Victory");
            Console.WriteLine();
            Console.WriteLine($"던전에서 몬스터 {monsters.Length}마리를 잡았습니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.Beforehp} -> {player.Hp}");
            Console.WriteLine();
            Console.WriteLine("[획득 아이템]");

            for (int i = 0; i < monsters.Length; i++) // 골드 드롭 여부 체크
            {
                if (monsters[i].goldDrop() != 0) // 골드가 드롭됐을 경우
                {
                    totalGetGold += monsters[i].goldDrop(); // 총합 드롭 골드에 더하기
                }
            }

            if (totalGetGold != 0) // 드롭 골드가 0이 아니라면
            {
                Console.WriteLine($"{totalGetGold} Gold");
                player.Gold += totalGetGold; // 골드 획득
            }

            for (int i = 0; i < monsters.Length; i++) // 아이템 드롭 여부 체크
            {
                if (monsters[i].dropItem() != null) // 아이템이 드롭됐을 경우
                {
                    Console.WriteLine($"{monsters[i].item.Name} - 1"); // 아이템 획득 메시지
                    player.Inventory.Add(monsters[i].item); // 아이템을 인벤토리에 추가
                }
            }

            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();

            int command = inputCommand(0, 0);

            switch (command)
            {
                case 0:
                    DisplayMainUI();
                    break;
            }
        }

        void DisplayLoseUI()
        {
            Console.Clear();
            Console.WriteLine("Battle!! - Result\n");
            Console.WriteLine("You Lose");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.Beforehp} -> {player.Hp}");
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();

            int command = inputCommand(0, 0);

            switch (command)
            {
                case 0:
                    DisplayMainUI();
                    break;
            }
        }

        void DisplayRestUI()
        {
            Console.Clear();

            Console.WriteLine("휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복 할 수 있습니다. (보유 골드 : {player.Gold} G)");
            Console.WriteLine();
            Console.WriteLine("1. 휴식 하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(0, 1);

            switch (command)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    if (player.Gold >= 500)
                    {
                        player.Gold -= 500;
                        player.Hp = player.Maxhp; // 최대 체력으로 회복
                        Console.WriteLine("휴식을 완료했습니다.");
                    }
                    else
                    {
                        Console.WriteLine("Gold 가 부족합니다.");
                    }
                    Console.ReadLine();
                    DisplayRestUI();
                    break;
            }

        }

        int inputCommand(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine();
                bool isNumber = int.TryParse(input, out result);
                if (isNumber)
                {
                    if (result >= min && result <= max)
                        return result;
                }
                Console.WriteLine("잘못된 입력입니다!!!!");
            }
        }

        void AlphaStrike(int skillNum)
        {
            int target = DisplayAttackUI(true);
            int skill = skillNum - 1;
            DisplaySkillDamageUI(target, skill);
            DisplayEnemyPhaseUI();
        }

        void DoubleStrike(int skillNum)
        {
            int skill = skillNum - 1;

            Random random = new Random();

            int target = random.Next(monsters.Length);
            while (monsters[target].isDie())
            {
                target = random.Next(monsters.Length);
            }
            DisplaySkillDamageUI(target, skill);

            target = random.Next(monsters.Length);
            while (monsters[target].isDie())
            {
                target = random.Next(monsters.Length);
            }
            DisplaySkillDamageUI(target, skill);
            DisplayEnemyPhaseUI();
        }



    }
}
