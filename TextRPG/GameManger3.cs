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
        Monster[] monstersDb;
        Monster[] monsters;

        //미니언 퀘스트
        int minionKill = 0;
        bool minionQuest = false;

        private Random random;

        public GameManager()
        {
            random = new Random();
        }

        public void SetData()   //게임 첫 시작시 생성되는 정보들
        {
            player = new Character(1, nameCreate(), jobSelect(), 50, 1500);
            monsters = new Monster[4];

            itemDb = new Item[]
            {
            new Item("포션",2,30,"체력을 회복시키는 포션입니다.",200),
            new Item("수련자의 갑옷", 1, 5,"수련에 도움을 주는 갑옷입니다. ",1000),
            new Item("쓸만한 방패", 1,7, "나무로 만들어진 쓸만한 방패입니다.", 1800),
            new Item("무쇠갑옷", 1, 9,"무쇠로 만들어져 튼튼한 갑옷입니다. ",2000),
            new Item("스파르타의 갑옷", 1, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",3500),
            new Item("낣은 검", 0, 2,"쉽게 볼 수 있는 낡은 검 입니다. ",600),
            new Item("청동 도끼", 0, 5,"어디선가 사용됐던거 같은 도끼입니다. ",1500),
            new Item("질풍 검", 0, 10,"야스오의 무기입니다.",10000)
            };


            monstersDb = new Monster[]
            {
                    new Monster(2, "미니언", 5, 15,itemDb[0],100),
                   // new Monster(3,"공허충",9,10,itemDb[0],200),
                   // new Monster(5,"대포미니언",10,20,itemDb[0],300),
                   // new Monster(7,"칼날 부리",10,30,itemDb[5],400),
                   // new Monster(8,"어스름 늑대",12,30,itemDb[6],500),
                   // new Monster(10,"야스오",30,50,itemDb[7],600)
            };

            player.SkillSet();
        }

        public Monster[] createMonsters()
        {
            Random random = new Random();

            // 1마리에서 4마리까지 랜덤 생성
            int numberOfMonsters = random.Next(1, 5);
            Monster[] monsters = new Monster[numberOfMonsters];

            // 랜덤으로 몬스터 선택하여 배열에 추가
            for (int i = 0; i < numberOfMonsters; i++)
            {
                int randomIndex = random.Next(monstersDb.Length);
                monsters[i] = new Monster(monstersDb[randomIndex]);
            }

            return monsters;
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
            Console.WriteLine("3. 미니언 퀘스트");
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
                    DisplayQuesUI();//퀘스트 목록
                    // MinionQuesUI();
                    //DisplayShopUI();//상점 열기
                    break;
                case 4:
                    player.beforeHpSave();
                    monsters = createMonsters();
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
                if (monster.isDie()) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(monster.monsterInfo());
                if (monster.isDie()) Console.ResetColor();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("[내정보]");

            player.DisplayBattlePlayerInfo();

            Console.WriteLine();
            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 스킬");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(1, 2);

            switch (command)
            {
                case 1:
                    DisplayAttackUI(false);
                    break;
                case 2:
                    DisplaySkillUI();
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
                if (monsters[i].isDie()) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{i + 1} {monsters[i].monsterInfo()}");
                if (monsters[i].isDie()) Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            player.DisplayBattlePlayerInfo();

            // 캐릭터 정보
            Console.WriteLine();
            Console.WriteLine("0. 취소");
            Console.WriteLine();
            Console.WriteLine("대상을 선택해주세요.");

            int command = inputCommand(0, monsters.Length);

            switch (command)
            {
                case 0:
                    DisplayBattleUI();
                    break;
                default:
                    int targetMonster = command - 1;
                    if (monsters[targetMonster].isDie())
                    {   //이미 죽은 몬스터를 공격 지정 할 때
                        Console.WriteLine("이미 죽은 몬스터 입니다.");
                        DisplayAttackUI(false);
                    }
                    else
                    {
                        if (skill)  //현재 공격이 스킬 일 때
                            return targetMonster;
                        //일반 공격 UI
                        DisplayMonsterDamageUI(targetMonster);
                    }

                    DisplayEnemyPhaseUI();
                    break;
            }
            return -1;
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

        void DisplayMonsterDamageUI(int target)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            // 1. 치명타 여부 판단
            bool isCritical = player.isCrit();
            // 2. 기본 데미지 계산 (치명타 포함)
            int baseDamage = player.PlayerDamage();
            if (isCritical)
            {
                baseDamage = (int)(baseDamage * 1.6f); // 치명타일 경우 1.6배
            }
            // 3. 체력 변화 전 상태 저장
            int hpBefore = monsters[target].Hp;
            // 4. 데미지 적용
            monsters[target].Hp = Math.Max(0, monsters[target].Hp - baseDamage);
            string hpAfter = monsters[target].isDie() ? "Dead" : monsters[target].Hp.ToString();
            // 5. 출력
            Console.WriteLine($"{player.Name}의 공격!");
            Console.WriteLine($"Lv.{monsters[target].level} {monsters[target].name} 을(를) 맞췄습니다. [대미지 : {baseDamage}]" +
                              (isCritical ? " - 치명타 공격!!" : ""));

            Console.WriteLine();
            Console.WriteLine($"Lv.{monsters[target].level} {monsters[target].name}");
            Console.WriteLine($"HP {hpBefore} -> {hpAfter}");
            Console.WriteLine();
            Console.WriteLine("0. 다음");

            CheckMinionQuest(monsters[target]);

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
                    else
                        DisplayEnemyPhaseUI();
                    break;

            }

        }

        void DisplaySkillDamageUI(int target, int skillNum)
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            int damage = player.PlayerDamage(player.skillDb[skillNum].Value);

            Console.WriteLine($"{player.Name}의 공격!");
            Console.WriteLine($"Lv{monsters[target].level} {monsters[target].name} 을(를) 맞췄습니다. " +
                $"[대미지 : {damage}]");
            Console.WriteLine();
            Console.WriteLine($"Lv{monsters[target].level} {monsters[target].name} ");

            int monsterHp = monsters[target].Hp - damage;

            Console.WriteLine("HP {0} -> {1}", monsters[target].Hp, monsterHp <= 0 ? "Dead" : monsterHp);

            //대미지 입히는거 계산
            monsters[target].Hp = monsterHp;

            //미니언 퀘스트
            CheckMinionQuest(monsters[target]);

            Console.WriteLine();
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
            //캐릭터 마나 회복, 캐릭터가 여러번 마나회복을
            //하는 것을 막기 위해서 턴 종료 시점에 회복
            player.recoveryMp();

            foreach (Monster monster in monsters)
            {
                if (!monster.isDie())
                {
                    Console.Clear();
                    Console.WriteLine("Battle!!\n");

                    int damage = monster.MonsterDamage(); //몬스터 고유 데미지
                    int hpBefore = player.Hp; //플레이어 피해전 체력
                    player.Hp = Math.Max(0, player.Hp - damage); // 체력 감소처리
                    int hpAfter = player.Hp;

                    // 4. 출력
                    Console.WriteLine($"Lv.{monster.level} {monster.name}의 공격!");
                    Console.WriteLine($"{player.Name}을(를) 맞췄습니다. [대미지: {damage}]");
                    Console.WriteLine();
                    Console.WriteLine($"Lv.{player.Level} {player.Name}");
                    Console.WriteLine($"HP {hpBefore} -> {hpAfter}");

                    Console.WriteLine();
                    Console.WriteLine("0. 다음");

                    int command = inputCommand(0, 0);

                    if (player.isDie())
                    {
                        DisplayLoseUI();
                        return;
                    }
                }
            }
            // 모든 몬스터(살아있는 기준)가 공격 후 다시 공격 기회
            DisplayBattleUI();
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
            Console.WriteLine($"HP {player.Maxhp} -> {player.Hp}");
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

        void DisplayQuesUI() //2025.04.24 퀘스트 UI
        {
            Console.Clear();
            Console.WriteLine("[Quest!!]\n");
            Console.WriteLine(" 1. 마을을 위협하는 미니언 처치");
            Console.WriteLine(" 2. 장비를 장착해보자");
            Console.WriteLine(" 3. 더욱 더 강해지기!\n");
            Console.WriteLine("원하시는 퀘스트를 선택해주세요.");
            Console.Write(">> ");

            int choice = inputCommand(1, 3);

            switch (choice)
            {
                case 1:
                    MinionQuesUI();
                    break;
                case 2:
                   // EquipQuestUI();
                    break;
                case 3:
                    StrongQuestUI();
                    break;
            }
        }

        void MinionQuesUI()
        {
            Console.Clear();
            Console.WriteLine("Quest!!");
            Console.WriteLine();
            Console.WriteLine("마을을 위협하는 미니언 처치");
            Console.WriteLine();
            Console.WriteLine("이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나??");
            Console.WriteLine("마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!");
            Console.WriteLine("자네가 좀 처치해주게!");
            Console.WriteLine();
            Console.WriteLine($"- 미니언 5마리 처치 ({minionKill}/5)");
            Console.WriteLine();
            Console.WriteLine("- 보상 -");
            Console.WriteLine($"{itemDb[1].Name} x 1");
            Console.WriteLine("5G");
            Console.WriteLine();
            if (minionQuest)
            {
                Console.WriteLine("1. 보상 받기");
                Console.WriteLine("2. 돌아가기");
            }
            else
            {
                Console.WriteLine("1. 수락");
                Console.WriteLine("2. 거절");
            }
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(1, 2);

            if (minionQuest)
            {
                switch (command)
                {
                    case 1:
                        if (minionKill >= 5)
                        {
                            minionQuest = false;
                            minionKill = 0;
                            player.Inventory.Add(itemDb[1]);
                            player.Gold += 5;
                            Console.WriteLine("퀘스트 클리어!!");
                            Console.ReadLine();
                        }
                        MinionQuesUI();
                        break;
                    case 2:
                        DisplayMainUI();
                        //DisplayQuestUI();
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case 1:
                        minionQuest = true;
                        MinionQuesUI();
                        break;
                    case 2:
                        DisplayMainUI();
                        //DisplayQuestUI();
                        break;
                }
            }
        }

        void StrongQuestUI() //레벨업 퀘스트 //날리기
        {
            Console.Clear();
            Console.WriteLine("Quest!!\n");
            Console.WriteLine("더욱 더 강해지기");

            Console.WriteLine("자네, 강해지고 싶지 않나?");
            Console.WriteLine("레벨 5만 되어도 새로운 힘을 얻을 수 있다네!\n");

            Console.WriteLine($"- 현재 레벨 : {player.Level} / 5");
            Console.WriteLine("- 보상 : 질풍 검 x1\n");

            Console.WriteLine("1. 보상 받기");
            Console.WriteLine("2. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(1, 2);

            switch (command)
            {
                case 1:
                    if (player.Level >= 5)
                    {
                        player.Inventory.Add(itemDb[7]); // 질풍 검 보상 지급
                        Console.WriteLine("퀘스트 클리어!!");
                        Console.ReadLine();
                        StrongQuestUI();
                    }
                    else
                    {
                        Console.WriteLine("레벨이 부족합니다! 더 성장하세요!");
                        Console.ReadLine();
                        StrongQuestUI();
                    }
                    break;
                case 2:
                    DisplayQuesUI();
                    break;
            }
        }


        void CheckMinionQuest(Monster monster)
        {
            if (minionQuest && monster.name == monstersDb[0].name &&
                monster.isDie())
            {
                minionKill++;
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

        void AlphaStrike(int skillNum)
        {
            player.Mp -= 10;
            int target = DisplayAttackUI(true);
            int skill = skillNum - 1;
            DisplaySkillDamageUI(target, skill);
            DisplayEnemyPhaseUI();
        }

        void DoubleStrike(int skillNum)
        {
            player.Mp -= 15;
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
