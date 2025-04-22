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
    class GameManager
    {
        //저장 경로
        string path = AppDomain.CurrentDomain.BaseDirectory;
        //게임 진행에 필요한 3가지 클래스
        private Character player;
        private Item[] itemDb;
        private Dungeon[] dungeons;
        Monster[] monsters;

        private Random random;

        public GameManager()
        {
            random = new Random();
        }

        public void SetData()   //게임 첫 시작시 생성되는 정보들
        {
            player = new Character(1, nameCreate(), jobSelect(), 1500);
            monsters = new Monster[3];

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


            monsters = new Monster[]
            {
                    new Monster(2, "미니언", 5, 15),
                    new Monster(3,"공허충",9,10),
                    new Monster(5,"대포미니언",10,20)
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

                        //   소지금이 부족핟
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
            //유저 정보
            Console.WriteLine();

            Console.WriteLine("1. 공격");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(1, 1);

            switch (command)
            {
                case 1:
                    DisplayAttackUI();
                    break;
            }


        }

        void DisplayAttackUI()
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            //몬스터 생성되는 함수

            //몬스터 정보 표시되는 함수
            for (int i = 0; i < monsters.Length; i++) {
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
                    DisplayMonsterDamageUI(targetMonster);
                    break;



            }
        }

        void DisplayMonsterDamageUI(int targetMonster)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            Console.WriteLine($"{player.Name}의 공격!");
            Console.WriteLine($"Lv{monsters[targetMonster].level} {monsters[targetMonster].name} 을(를) 맞췄습니다. " +
                $"[대미지 : {monsters[targetMonster].MonsterGetDamage(player.Atk)}]");
            Console.WriteLine();
            Console.WriteLine($"Lv{monsters[targetMonster].level} {monsters[targetMonster].name} ");

            int monsterHp = monsters[targetMonster].Hp - monsters[targetMonster].MonsterGetDamage(player.Atk);

            Console.WriteLine("HP {0} -> {1}", monsters[targetMonster].Hp, monsters[targetMonster].isDie() ? "Dead": monsterHp);

            //대미지 입히는거 계산
            monsters[targetMonster].Hp = monsterHp;

            Console.WriteLine("0. 다음");

            int command = inputCommand(0, 0);

            switch (command)
            {
                case 0:
                    int aliveMon = monsters.Length;
                    foreach(Monster monster in monsters)
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

        void DisplayEnemyPhaseUI()
        {
            foreach (Monster monster in monsters) 
            {
                if (!monster.isDie())
                {
                    Console.Clear();
                    Console.WriteLine("Battle!!\n");

                    Console.WriteLine($"Lv.{monster.level} {monster.name} 의 공격!");
                    Console.WriteLine($"{player.Name} 을(를) 맞췄습니다. [데미지 : {player.PlayerGetDamage(monster.Atk)}]");
                    Console.WriteLine();
                    Console.WriteLine($"Lv.{player.Level} {player.Name}");
                    Console.WriteLine($"HP {player.Hp} -> {player.Hp - player.PlayerGetDamage(monster.Atk)}");

                    player.Hp -= player.PlayerGetDamage(monster.Atk);

                    Console.WriteLine("0. 다음\n");
                    Console.WriteLine("대상을 선택해주세요.");

                    int command = inputCommand(0, 0);

                    switch (command)
                    {
                        case 0:
                            if (player.isDie())
                                DisplayLoseUI();
                        
                            if (monster == monsters[monsters.Length - 1])
                                DisplayAttackUI();
                            break;
                    }


                }
            }
        }


        void DisplayVictoryUI()
        {
            Console.Clear();
            Console.WriteLine("Battle!! - Result\n");
            Console.WriteLine("Victory");
            Console.WriteLine();
            Console.WriteLine($"던전에서 몬스터 {monsters.Length}마리를 잡았습니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.maxHp} -> {player.Hp}");
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
            Console.WriteLine($"HP {player.maxHp} -> {player.Hp}");
            Console.WriteLine();
            Console.WriteLine("0. 다음");
            Console.WriteLine();

            int command = inputCommand(0,0);

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
                        player.Hp = 100;
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

       
    }
}
