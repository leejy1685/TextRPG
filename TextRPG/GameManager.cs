using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRPG
{
    class GameManager
    {
        public void SaveData(Character player) // 저장
        {
            string savepath = path;

            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonConvert.SerializeObject(player, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(path, json);            
        }

        public Character LoadData() // 불러오기
        {
            string FilePath = path;
            string json = File.ReadAllText(FilePath);
            Character loadplayer = JsonConvert.DeserializeObject<Character>(json);

            return loadplayer;
        }

        //저장 경로
        string path = "SaveData/savefile.json";

        //string path = AppDomain.CurrentDomain.BaseDirectory;

        //게임 진행에 필요한 3가지 클래스
        private Character player;
        private Item[] itemDb;
        Monster[] monstersDb;
        Monster[] monsters;

        //미니언 퀘스트
        int minionKill = 0;
        bool minionQuest = false;

        //장비 퀘스트
        bool equipQuest = false;

        //스테이지
        private int stage = 1; // 기본 스테이지 1로 시작

        public GameManager()    {}  //생성자

        public void SetData()   //게임 첫 시작시 생성되는 정보들
        {
            player = new Character(1, nameCreate(), jobSelect(), 50, 1500);  //캐릭터 생성
            player.SkillSet();  //스킬 저장
            itemDb = new Item[] //아이템 DB
            {
            new Item("포션",2,30,"체력을 회복시키는 포션입니다.",200),
            new Item("수련자의 갑옷", 1, 5,"수련에 도움을 주는 갑옷입니다. ",1000),
            new Item("쓸만한 방패", 1,7, "나무로 만들어진 쓸만한 방패입니다.", 1800),
            new Item("무쇠갑옷", 1, 9,"무쇠로 만들어져 튼튼한 갑옷입니다. ",2000),
            new Item("스파르타의 갑옷", 1, 15,"스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",3500),
            new Item("낣은 검", 0, 2,"쉽게 볼 수 있는 낡은 검 입니다. ",600),
            new Item("청동 도끼", 0, 5,"어디선가 사용됐던거 같은 도끼입니다. ",1500),
            new Item("질풍 검", 0, 20,"야스오의 무기입니다.",10000)
            };
            monstersDb = new Monster[]  //몬스터 DB
            {
                    new Monster(2, "미니언", 5, 15,itemDb[0],100),
                    new Monster(3,"공허충",9,10,itemDb[0],200),
                    new Monster(5,"대포미니언",10,20,itemDb[0],300),
                    new Monster(7,"칼날 부리",10,30,itemDb[5],400),
                    new Monster(8,"어스름 늑대",12,30,itemDb[6],500),
                    new Monster(10,"야스오",30,100,itemDb[7],600)
            };
            //게임 시작 시 포션 3개 지급
            for (int i = 0; i < 8; i++)
            {
                player.Inventory.Add(itemDb[i]);
            }
        }

        public void createMonsters()
        {
            Random random = new Random();

            // 1마리에서 4마리까지 랜덤 생성
            int numberOfMonsters = random.Next(1, 5);
            monsters = new Monster[numberOfMonsters];

            // 랜덤으로 몬스터 선택하여 배열에 추가
            for (int i = 0; i < numberOfMonsters; i++)
            {
                int randomIndex = random.Next(2 + stage);   //스테이지 따라서 몬스터 종류가 많아짐
                monsters[i] = new Monster(monstersDb[randomIndex]);
            }

            //보스 스테이지
            if (stage == 4)
            {
                monsters = new Monster[1];
                monsters[0] = new Monster(monstersDb[5]);
            }
        }//전투에 필요한 몬스터를 생성하는 메서드

        string nameCreate() //캐릭터 이름 생성
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.ResetColor();

            Console.WriteLine("원하시는 이름을 설정해 주세요\n");

            string name = Console.ReadLine();

            Console.WriteLine();
            Console.Write("입력하신 이름은 ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(name);
            Console.ResetColor();
            Console.WriteLine(" 입니다.");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("1");
            Console.ResetColor();
            Console.WriteLine(". 저장");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("2");
            Console.ResetColor();
            Console.WriteLine(". 취소");
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

        Job jobSelect()
        {
            Job job = Job.Warrior;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.ResetColor();
            Console.WriteLine("원하시는 직업을 설정해 주세요.");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("1");
            Console.ResetColor();
            Console.WriteLine(". 전사");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("2");
            Console.ResetColor();
            Console.WriteLine(". 도적");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("3");
            Console.ResetColor();
            Console.WriteLine(". 바바리안");

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
        }//캐릭터 직업 선택

        public void DisplayMainUI() //게임을 진행하는 메서드
        {

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("이제 전투를 시작할 수 있습니다.");

            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("1. ");
            Console.ResetColor();
            Console.WriteLine("▶▶ 상태 보기 ◀◀");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("2. ");
            Console.ResetColor();
            Console.WriteLine("▶▶  인벤토리 ◀◀");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("3. ");
            Console.ResetColor();
            Console.WriteLine("▶▶   퀘스트  ◀◀");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("4. ");
            Console.ResetColor();
            Console.Write("▶▶ 던전 입장 ◀◀ ( ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Stage ");
            Console.Write(stage);
            Console.ResetColor();
            Console.WriteLine(" )");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("5. ");
            Console.ResetColor();
            Console.WriteLine("▶▶   회 복   ◀◀");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("6. ");
            Console.ResetColor();
            Console.WriteLine("▶▶   저 장   ◀◀");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("7. ");
            Console.ResetColor();
            Console.WriteLine("▶▶ 불러 오기 ◀◀");

            //Console.WriteLine("0. 저장 후 종료");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");


            int command = inputCommand(1, 7);

            switch (command)
            {
                case 1:
                    DisplayStatUI();//캐릭터 정보 표시
                    break;

                case 2:
                    DisplayInventoryUI();//인벤토리 열기
                    break;

                case 3:
                    DisplayQuestUI();//퀘스트 확인
                    break;
                case 4:
                    player.beforeSave();    //체력과 마나 저장
                    createMonsters();   //몬스터 생성
                    DisplayBattleUI();  //전투 시작
                    break;
                case 5:
                    DisplayPotionUI();  //물약
                    break;

                case 6: // 저장
                    SaveData(player);
                    DisplayMainUI();
                    break;

                case 7: // 불러오기
                    player = LoadData();
                    DisplayMainUI();
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("상태 보기");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine();

            player.DisplayCharacterInfo();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 나가기");
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

        void DisplayInventoryUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("인벤토리");
            Console.ResetColor();

            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[아이템 목록]");
            Console.ResetColor();

            player.DisplayInventory(false, false);  //순서표시, 판매금액 표시 없음

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("1.");
            Console.ResetColor();
            Console.WriteLine(" 장착 관리");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int result = inputCommand(0, 1);

            switch (result)
            {
                case 0:
                    DisplayMainUI();
                    break;

                case 1:
                    DisplayEquipUI();   //장착 관리
                    break;
            }
        }//인벤토리 확인

        void DisplayEquipUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("인벤토리 - 장착관리");
            Console.ResetColor();

            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[아이템 목록]");
            Console.ResetColor();


            player.DisplayInventory(true, false);   //순서 표시 있음, 판매 금액 표시 안함

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 나가기");
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
                    Item targetItem = player.Inventory[itemIdx];
                    player.EquipItem(targetItem);

                    DisplayEquipUI();
                    break;
            }


        }//장비 장착 관리

        void DisplayBattleUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Battle!!\n");
            Console.ResetColor();

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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("1.");
            Console.ResetColor();
            Console.WriteLine(" 공격");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("2.");
            Console.ResetColor();
            Console.WriteLine(" 스킬");
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


        }//전투 UI

        int DisplayAttackUI(bool skill)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Battle!!\n");
            Console.ResetColor();

            //몬스터 생성되는 함수

            //몬스터 정보 표시되는 함수
            for (int i = 0; i < monsters.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{i + 1}");
                Console.ResetColor();
                if (monsters[i].isDie()) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" {monsters[i].monsterInfo()}");
                if (monsters[i].isDie()) Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            player.DisplayBattlePlayerInfo();

            // 캐릭터 정보
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 취소");
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
                        Console.ReadLine();
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
        }//일반 공격 몬스터 선택 UI

        void DisplaySkillUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Battle!!\n");
            Console.ResetColor();

            //몬스터 정보 표시되는 함수
            foreach (Monster monster in monsters)
            {
                if(monster.isDie()) Console.ForegroundColor= ConsoleColor.DarkGray;
                Console.WriteLine(monster.monsterInfo());
                if (monster.isDie()) Console.ResetColor();
            }
            Console.WriteLine();
            Console.WriteLine("[내정보]");
            player.DisplayBattlePlayerInfo();
            Console.WriteLine();

            //스킬 목록 출력
            for (int i = 0; i < player.skillDb.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{i + 1}. ");
                Console.ResetColor();
                player.skillDb[i].skillInfo();
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 취소");
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
        }// 전투 - 스킬 목록 확인

        void DisplayMonsterDamageUI(int target)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Battle!!\n");
            Console.ResetColor();

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
            Console.Write($"Lv.{monsters[target].level} {monsters[target].name} 을(를) 맞췄습니다. ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"[대미지 : {baseDamage}]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(isCritical ? " - 치명타 공격!!" : "");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"Lv.{monsters[target].level} {monsters[target].name}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HP {hpBefore} -> {hpAfter}");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 다음");

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

        }//일반 공격 시 대미지 UI

        void DisplaySkillDamageUI(int target, int skillNum)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Battle!!\n");
            Console.ResetColor();

            int damage = player.PlayerDamage(player.skillDb[skillNum].Value);
            int hpBefore = monsters[target].Hp; //플레이어 피해전 체력
            monsters[target].Hp = Math.Max(0, monsters[target].Hp - damage);
            string hpAfter = monsters[target].isDie() ? "Dead" : monsters[target].Hp.ToString();

            Console.WriteLine($"{player.Name}의 공격!");
            Console.Write($"Lv{monsters[target].level} {monsters[target].name} 을(를) 맞췄습니다. ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[대미지 : {damage}]");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"Lv{monsters[target].level} {monsters[target].name} ");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HP {hpBefore} -> {hpAfter}");
            Console.ResetColor();

            //미니언 퀘스트
            CheckMinionQuest(monsters[target]);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 다음");

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

        }//스킬 공격 시 대미지 UI

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
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Battle!!\n");
                    Console.ResetColor();

                    int damage = monster.MonsterDamage(); //몬스터 고유 데미지
                    int hpBefore = player.Hp; //플레이어 피해전 체력
                    player.Hp = Math.Max(0, player.Hp - damage); // 체력 감소처리
                    int hpAfter = player.Hp;

                    // 4. 출력
                    Console.WriteLine($"Lv.{monster.level} {monster.name}의 공격!");
                    Console.Write($"{player.Name}을(를) 맞췄습니다. ");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"[대미지: {damage}]");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine($"Lv.{player.Level} {player.Name}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"HP {hpBefore} -> {hpAfter}");
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("0.");
                    Console.ResetColor();
                    Console.WriteLine(" 다음");

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
        }//몬스터 공격 UI

        void DisplayVictoryUI()
        {
            //캐릭터 마나 회복, 캐릭터가 여러번 마나회복을
            //하는 것을 막기 위해서 턴 종료 시점에 회복
            player.recoveryMp();

            int totalGetGold = 0; // 총합 획득 골드

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Battle!! - Result\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Victory");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"던전에서 몬스터 {monsters.Length}마리를 잡았습니다.");
            Console.WriteLine();
            Console.WriteLine("[캐릭터 정보]");

            // 몬스터 처치 후 경험치 획득 및 레벨업 확인
            bool LevelUp = false;
            foreach (var monster in monsters)
            {
                if (player.LevelUp(monster)) // 레벨업 체크
                {
                    LevelUp = true;
                }
            }

            // 레벨업이 되었으면 레벨업 UI 출력
            if (LevelUp)
            {
                Console.WriteLine($"Lv.{player.Level - 1} -> Lv.{player.Level}");
            }
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HP {player.Beforehp} -> {player.Hp}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"HP {player.Beforemp} -> {player.Mp}");
            Console.ResetColor();
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

            if (stage < 4) //클리어시 +1씩 증가 최대 3
            {
                stage++;
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 다음");
            Console.WriteLine();

            int command = inputCommand(0, 0);

            switch (command)
            {
                case 0:
                    DisplayMainUI();
                    break;
            }
        }//승리 UI

        void DisplayLoseUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Battle!! - Result\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("You Lose");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"HP {player.Maxhp} -> {player.Hp}");
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 다음");
            Console.WriteLine();

            int command = inputCommand(0, 0);

            switch (command)
            {
                case 0:
                    DisplayMainUI();
                    break;
            }
        }//패배 UI

        void DisplayQuestUI() 
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Quest!!]");
            Console.ResetColor();
            Console.WriteLine();
            // 퀘스트 상태에 따른 색상 표시 Yellow/Green
            DisplayQuestStatus(1, "마을을 위협하는 미니언 처치", minionQuest, minionKill >= 5);
            DisplayQuestStatus(2, "장비를 장착해보자", false, false); 
            DisplayQuestStatus(3, "더욱 더 강해지기!", false, player.Level >= 5);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0.");
            Console.ResetColor();
            Console.WriteLine(" 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 퀘스트를 선택해주세요.");
            Console.Write(">> ");

            int choice = inputCommand(0, 3);

            switch (choice)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    MinionQuestUI();
                    break;
                case 2:
                    EquipQuestUI();
                    break;
                case 3:
                    StrongQuestUI();
                    break;
            }
        }//퀘스트 UI

        void DisplayQuestStatus(int index, string questName, bool isAccepted, bool isComplete)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"{index}. ");
            if (isComplete)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (isAccepted)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ResetColor();

            Console.WriteLine(questName);
            Console.ResetColor();
        }//퀘스트 상태를 색으로 나타내는 함수

        void MinionQuestUI()
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Quest!!");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("마을을 위협하는 미니언 처치");
            Console.WriteLine();
            Console.WriteLine("이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나??");
            Console.WriteLine("마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!");
            Console.WriteLine("자네가 좀 처치해주게!");
            Console.WriteLine();

            Console.Write("- 미니언 5마리 처치 (");
            Console.ForegroundColor = minionKill >= 5 ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{minionKill}");
            Console.ResetColor();
            Console.WriteLine("/5)");
            Console.WriteLine();

            Console.WriteLine("- 보상 -");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(itemDb[1].Name);
            Console.ResetColor();
            Console.Write(" x ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("1");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("5G");
            Console.ResetColor();
            Console.WriteLine();

            if (minionQuest)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1. ");
                Console.ResetColor();

                if (minionKill >= 5)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("보상 받기");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2. ");
                Console.ResetColor();
                Console.WriteLine("돌아가기");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1. ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("수락");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2. ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("거절");
                Console.ResetColor();
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

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("퀘스트 클리어!!");
                            Console.ResetColor();
                            Console.ReadLine();
                        }
                        MinionQuestUI();
                        break;

                    case 2:
                        DisplayQuestUI();
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case 1:
                        minionQuest = true;
                        MinionQuestUI();
                        break;
                    case 2:
                        DisplayQuestUI();
                        break;
                }
            }
        }//미니언 퀘스트 UI

        void CheckMinionQuest(Monster monster)
        {
            if (minionQuest && monster.name == monstersDb[0].name &&
                monster.isDie())
            {
                minionKill++;
            }
        }//미니언 퀘스트 진행을 돕는 메서드

        void EquipQuestUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Quest!!");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("장비를 장착해보자");
            Console.WriteLine();
            Console.WriteLine("아니? 자네 그것도 무기라고 들고 댕기나?");
            Console.WriteLine("저기 칼날 부리만 잡아도 낡은 검이 나오니 그거라도 들고 댕기게");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{itemDb[5].Name} 장착 : ");
            Console.ResetColor();
            Console.Write("(");
            Console.ForegroundColor = player.IsEquipped(itemDb[5]) ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(player.IsEquipped(itemDb[5]));
            Console.ResetColor();
            Console.WriteLine(")");
            Console.WriteLine();

            Console.WriteLine("- 보상 -");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("포션");
            Console.ResetColor();
            Console.Write(" x ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("3");
            Console.ResetColor();
            Console.WriteLine();

            if (equipQuest)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1. ");
                Console.ResetColor();

                Console.ForegroundColor = player.IsEquipped(itemDb[5]) ? ConsoleColor.Green : ConsoleColor.DarkGray;
                Console.WriteLine("보상 받기");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2. ");
                Console.ResetColor();
                Console.WriteLine("돌아가기");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("1. ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("수락");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("2. ");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red; // 수정: 더 잘 보이는 반대 색상으로
                Console.WriteLine("거절");
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(1, 2);

            if (equipQuest)
            {
                switch (command)
                {
                    case 1:
                        if (player.IsEquipped(itemDb[5]))
                        {
                            equipQuest = false;
                            player.Inventory.Add(itemDb[0]);
                            player.Inventory.Add(itemDb[0]);
                            player.Inventory.Add(itemDb[0]);

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("퀘스트 클리어!!");
                            Console.ResetColor();
                            Console.ReadLine();
                        }
                        EquipQuestUI();
                        break;
                    case 2:
                        DisplayQuestUI();
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case 1:
                        equipQuest = true;
                        EquipQuestUI();
                        break;
                    case 2:
                        DisplayQuestUI();
                        break;
                }
            }
        }//장비 장착 퀘스트 UI

        void StrongQuestUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Quest!!\n");
            Console.ResetColor();

            Console.WriteLine("더욱 더 강해지기\n");
            Console.WriteLine("자네, 강해지고 싶지 않나?");
            Console.WriteLine("레벨 5만 되어도 새로운 힘을 얻을 수 있다네!\n");

            Console.Write("- 현재 레벨 : ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(player.Level);
            Console.ResetColor();
            Console.WriteLine(" / 5");

            Console.WriteLine("- 보상 : ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("질풍 검");
            Console.ResetColor();
            Console.Write(" x ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("1\n");
            Console.ResetColor();

            // 선택지 색상
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("1. ");
            Console.ResetColor();

            if (player.Level >= 5)
                Console.ForegroundColor = ConsoleColor.Green; // 퀘스트 완료
            else
                Console.ForegroundColor = ConsoleColor.DarkGray; // 퀘스트 미완료
            Console.WriteLine("보상 받기");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("2. ");
            Console.ResetColor();
            Console.WriteLine("나가기\n");

            Console.ResetColor();

            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(1, 2);

            switch (command)
            {
                case 1:
                    if (player.Level >= 5)
                    {
                        player.Inventory.Add(itemDb[7]); // 질풍 검 보상 지급
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("퀘스트 클리어!!");
                        Console.ResetColor();
                        Console.ReadLine();
                        StrongQuestUI();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("레벨이 부족합니다! 더 성장하세요!");
                        Console.ResetColor();
                        Console.ReadLine();
                        StrongQuestUI();
                    }
                    break;
                case 2:
                    DisplayQuestUI();
                    break;
            }
        }//레벨업 퀘스트

        void DisplayPotionUI()
        {
            Console.Clear();
            //제목
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("회복");
            Console.ResetColor();

            // 포션 설명
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("포션");
            Console.ResetColor();
            Console.Write("을 사용하면 체력을 ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("30");
            Console.ResetColor();
            Console.Write(" 회복 할 수 있습니다. (남은 포션 : ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(player.numberOfPotion());
            Console.ResetColor();
            Console.WriteLine(" )");

            Console.WriteLine();

            // 선택지 출력
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("1. ");
            Console.ResetColor();
            Console.WriteLine("사용하기");

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("0. ");
            Console.ResetColor();
            Console.WriteLine("나가기");

            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(0, 1);

            switch (command)
            {
                case 0:
                    DisplayMainUI();
                    break;
                case 1:
                    player.UsePotion(itemDb[0]);
                    DisplayPotionUI();
                    break;
            }

        }//포션을 먹을 수 있는 UI

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
        }//명령어 입력 메서드

        void AlphaStrike(int skillNum)
        {
            player.Mp -= 10;
            int target = DisplayAttackUI(true);
            int skill = skillNum - 1;
            DisplaySkillDamageUI(target, skill);
            DisplayEnemyPhaseUI();
        }//스킬 1

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
        }// 스킬 2

    }
}
