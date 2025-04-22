using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextRPG
{
    class GameManager2
    {
        //저장 경로
        string path = AppDomain.CurrentDomain.BaseDirectory;
        //게임 진행에 필요한 3가지 클래스
        private Character player;
        private Item[] itemDb;
        private Dungeon[] dungeons;

        private Random random;

        public GameManager2()
        {
            random = new Random();
        }

        public void SetData()   //게임 첫 시작시 생성되는 정보들
        {
            player = new Character(1, nameCreate(), jobSelect(), 1500);
            dungeons = new Dungeon[3];

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

            dungeons = new Dungeon[]
            {
                new Dungeon("쉬운 던전", 5, 1000),
                new Dungeon("일반 던전", 11, 1700),
                new Dungeon("어려운 던전", 17, 2500)
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
                    DisplayDungeonUI();//던전 열기
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

        //입장할 수 있는 던전을 표시하는 메서드
        void DisplayDungeonUI()
        {
            Console.Clear();
            Console.WriteLine("던전입장");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

            for (int i = 0; i < dungeons.Length; i++)
            {   //던전의 정보를 표시
                Console.WriteLine($"{i + 1}. {dungeons[i].DungeonInfo()}");
            }

            Console.WriteLine("0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int command = inputCommand(0, dungeons.Length);

            switch (command)
            {
                case 0:
                    DisplayMainUI();
                    break;
                default:
                    if (player.Hp > 0)
                    {
                        int dungeonNum = command - 1;
                        DungeonTry(dungeonNum);
                    }
                    else
                    {
                        Console.WriteLine("Console.WriteLine(\"체력이 부족합니다.\");");
                    }
                    break;

            }

        }

        void DungeonTry(int dungeonNum)
        {
            if (player.Def < dungeons[dungeonNum].recDef)
            {   //권장 방어력 보다 낮으면 40퍼 확률로 실패
                int rand = random.Next(1, 101);
                if (rand <= 40)
                {
                    failDungeon(dungeonNum);
                }
                else
                {
                    //clearDungeon(dungeonNum);
                }
            }
            else
            {
                //clearDungeon(dungeonNum);
            }
        }

        void failDungeon(int dungeonNum)
        {
            Console.Clear();
            Console.WriteLine("던전 실패");
            Console.WriteLine("{0}의 공략을 실패 하셨습니다.\n", dungeons[dungeonNum].name);
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine("체력 {0} -> {1}", player.Hp, player.Hp / 2);

            player.Hp /= 2;

            Console.WriteLine("\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요");

            int command = inputCommand(0, 0);

            if (command == 0) DisplayDungeonUI();
        }

        //void clearDungeon(int dungeonNum)
        //{
        //    Console.Clear();
        //    Console.WriteLine("던전 클리어");
        //    Console.WriteLine("축하합니다!!");
        //    Console.WriteLine("{0}을 클리어 하셨습니다.", dungeons[dungeonNum].name);
        //    Console.WriteLine();
        //    Console.WriteLine("[탐험 결과]");

        //    if (player.LevelUp())
        //    {
        //        Console.WriteLine("Level {0} -> {1}", player.Level - 1, player.Level);
        //    }

        //    int userHp = dungeons[dungeonNum].dungeonDamage(player);

        //    Console.WriteLine("체력 {0} -> {1}", player.Hp, userHp);

        //    player.Hp = userHp; //체력 반영

        //    //보상 계산
        //    int newGold = dungeons[dungeonNum].DungeonClearGold(player);
        //    Console.WriteLine("Gold {0} G -> {1} G", player.Gold, player.Gold + newGold);

        //    player.Gold += newGold;//보상 반영

        //    Console.WriteLine("\n0. 나가기\n");
        //    Console.WriteLine("원하시는 행동을 입력해 주세요");

        //    int command = inputCommand(0, 0);

        //    if (command == 0) DisplayDungeonUI();
        //}
        // 휴식하기 기능
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

        //데이터를 저장하는 메서드
        void saveData()
        {
            //유저 데이터 저장
            string playerData = JsonConvert.SerializeObject(player);
            File.WriteAllText(path + "\\playerData.json", playerData);

            //상점 데이터를 저장
            //여기는 판매여부를 판단하는 정보만 저장
            string itemDbData = JsonConvert.SerializeObject(itemDb);
            File.WriteAllText(path + "\\itemDbData.json", itemDbData);

            //던전의 정보를 저장
            //없어도 되는 과정
            string dungeonData = JsonConvert.SerializeObject(dungeons);
            File.WriteAllText(path + "\\dungeonData.json", dungeonData);
        }

        //저장된 정보를 가져오는 메서드
        //public void loadData()
        //{
        //    //유저 데이터가 있는지 확인
        //    if (!File.Exists(path + "\\playerData.json"))
        //    {   //데이터가 없으면 새로 생성
        //        SetData();
        //        return;
        //    }

        //    //유저의 정보를 가져오기
        //    string playerLData = File.ReadAllText(path + "\\playerData.json");
        //    Character userLoadData = JsonConvert.DeserializeObject<Character>(playerLData);
        //    player = userLoadData;

        //    //상점 정보 가져오기
        //    string itemDbData = File.ReadAllText(path + "\\itemDbData.json");
        //    Item[] storeLoadData = JsonConvert.DeserializeObject<Item[]>(itemDbData);
        //    itemDb = storeLoadData;

        //    //던전 데이터 가져오기
        //    //없어도 되는 과정
        //    //DungeonCreate 메서드를 구현해서 사용해도 됨.
        //    string dungeonData = File.ReadAllText(path + "\\dungeonData.json");
        //    Dungeon[] dungeonLoadData = JsonConvert.DeserializeObject<Dungeon[]>(dungeonData);
        //    if (dungeons == null)
        //    {
        //        dungeons = new Dungeon[3];
        //    }
        //    for (int i = 0; i < 3; i++)
        //    {
        //        dungeons[i] = dungeonLoadData[i];
        //    }
        //}
    }
}
