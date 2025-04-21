using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class GameManager
    {
        //저장 경로
        string path = AppDomain.CurrentDomain.BaseDirectory;
        //게임 진행에 필요한 3가지 클래스
        Player user;
        Store store;
        Dungeon[] dungeons;

        void createDate()   //게임 첫 시작시 생성되는 정보들
        {
            user = new Player(nameCreate(), jobSelect());
            store = new Store();
            dungeons = new Dungeon[3];

            store.addItem(new Armor("수련자 갑옷", 5, "수련에 도움을 주는 갑옷입니다.", 1000));
            store.addItem(new Armor("그래도 좋은 갑옷", 7, "적당한 선능에 그럭저럭 쓸만한 갑옷입니다.", 1800));
            store.addItem(new Armor("무쇠갑옷", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2200));
            store.addItem(new Armor("스파르타의 갑옷", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500));
            store.addItem(new Weapon("낡은 검", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600));
            store.addItem(new Weapon("좋은 검", 4, "잘 다듬어져 있는 가성비 좋은 검 입니다.", 1000));
            store.addItem(new Weapon("청동 도끼", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500));
            store.addItem(new Weapon("스파르타의 창", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3200));

            Dungeon easy = new Dungeon("쉬운 던전", 5, 1000);
            Dungeon normal = new Dungeon("일반 던전", 11, 1700);
            Dungeon hard = new Dungeon("어려운 던전", 17, 2500);
            dungeons[0] = easy;
            dungeons[1] = normal;
            dungeons[2] = hard;

        }

        //플레이어 캐릭터의 이름을 만드는 메서드
        string nameCreate()
        {
            string name = "";
            while (true)
            {
                Console.Clear();

                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("원하시는 이름을 설정해 주세요\n");

                name = Console.ReadLine();

                int command = 0;
                while (true)
                {
                    Console.WriteLine("\n입력하신 이름은 {0} 입니다.\n", name);
                    Console.WriteLine("1. 저장\n2. 취소\n");
                    Console.WriteLine("원하시는 행동을 입력해 주세요.");

                    command = inputCommand();


                    if (command == 1)
                    {
                        break;
                    }
                    else if (command == 2)
                    {
                        break;
                    }
                }

                if (command == 1)
                {
                    break;
                }
                else if (command == 2)
                {
                    continue;
                }
            }

            return name;
        }

        //플레이어 캐릭터의 직업을 선택하는 메서드
        string jobSelect()
        {
            string job;
            while (true)
            {
                Console.Clear();

                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("원하시는 직업을 설정해 주세요\n");
                Console.WriteLine("1. 전사\n2. 도적\n");

                Console.WriteLine("원하시는 행동을 입력해 주세요.");
                int command = inputCommand();

                if (command == 1)
                {
                    job = "전사";
                    break;
                }
                else if (command == 2)
                {
                    job = "도적";
                    break;
                }


            }
            return job;
        }
        //게임을 진행하는 메서드
        public void GamePlay()
        {

            while (true)
            {
                Console.Clear();

                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전입장\n5. 휴식하기\n0. 저장 후 종료\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                int command = inputCommand();   // 입력

                if (command == 1) playInfo();   //캐릭터 정보 표시
                else if (command == 2) openInventory(); //인벤토리 열기
                else if (command == 3) openStore(); //상점 열기
                else if (command == 4) openDungeon();   //던전 열기
                else if (command == 5) rest();  //휴식
                else if (command == 0)
                {
                    saveData(); //데이터 저장
                    break;  //및 게임 종료
                }

            }
        }
        void playInfo() //캐릭터의 정보 보기
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                user.playerInfo();  //캐릭터의 정보를 표시하는 메서드

                Console.WriteLine("\n0. 나가기\n");

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = inputCommand();

                if (command == 0) break;
            }
        }

        //인벤토리 확인
        void openInventory()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

                Console.WriteLine("[아이템 목록]");
                //인벤토리를 확인하는 메서드
                //false 시 앞에 숫자 표시 안함
                user.showInventory(false);

                Console.WriteLine("1. 장착 관리\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = inputCommand();

                if (command == 1) equipmentMng();   //장비 장착 관리
                if (command == 0) break;
            }
        }

        //장비 장착 관리하는 메서드
        void equipmentMng()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

                Console.WriteLine("[아이템 목록]");
                //true 시 앞에 숫자 표시
                user.showInventory(true);

                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = inputCommand();

                if (command == 0) break;
                else if (0 < command && command <= user.inventory.Count)
                {   //장비를 장착
                    user.itemEquipped(command);
                }
            }


        }

        //상점을 여는 메서드
        void openStore()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\n", user.gold);

                Console.WriteLine("[아이템 목록]");
                //상점에 있는 아이템을 조회하는 메서드
                //false 시 아이템 앞에 번호 표시 안함
                store.showItems(false);

                Console.WriteLine("\n1. 아이템 구매\n2. 아이템 판매\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = inputCommand();

                if (command == 0) break;
                else if (command == 1) buyStore();  //아이템 구매창을 띄우는 메서드
                else if (command == 2) sellStore(); //아이템 판매창을 띄우는 메서드

            }
        }

        //아이템 구매창을 띄우는 메서드
        void buyStore()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\n", user.gold);

                Console.WriteLine("[아이템 목록]");
                //true 시 아이템 앞에 번호 표시
                store.showItems(true);

                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = inputCommand();

                if (command == 0) break;
                else if (0 < command && command <= store.items.Count)
                {   // 아이템을 구매 처리하는 메서드
                    store.buyItem(user, command);
                }
            }
        }

        //아이템 판매창을 띄우는 메서드
        void sellStore()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\n", user.gold);

                Console.WriteLine("[아이템 목록]");
                //인벤토리를 확인하는 메서드
                //bool 타입 값을 넣지 않으면 판매 가격이 표시 됨
                user.showInventory();

                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = inputCommand();

                if (command == 0) break;
                else if (0 < command && command <= user.inventory.Count)
                {   //아이템을 판매 처리하는 메서드
                    store.sellItem(user, command);
                }
            }
        }

        //입장할 수 있는 던전을 표시하는 메서드
        void openDungeon()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("던전입장");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

                for (int i = 0; i < dungeons.Length; i++)
                {   //던전의 정보를 표시
                    //Dungeon 클래스에 구현해도 될 것 같음.
                    Console.WriteLine("{0}. {1}\t| 방어력 {2} 이상 권장", i + 1, dungeons[i].name, dungeons[i].recDefence);
                }

                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = inputCommand();

                if (command == 0) break;
                else if (0 < command && command <= dungeons.Length && user.hp > 0)
                {   //던전 시도
                    dungeons[command - 1].tryDungeon(user);
                }
                else if (0 < command && command <= dungeons.Length && user.hp == 0)
                {   //던전에 입장 할 수 있느 조건 
                    Console.WriteLine("체력이 부족합니다.");
                    Console.ReadLine();
                }
            }
        }
        // 휴식하기 기능
        void rest()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("휴식하기");
                Console.WriteLine("500 G 를 내면 체력을 회복 할 수 있습니다. (보유 골드 : {0} G)\n", user.gold);

                Console.WriteLine("1. 휴식 하기\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = inputCommand();


                if (command == 0) break;
                else if (command == 1 && user.gold >= 500)
                {
                    user.gold -= 500;
                    user.hp = 100;
                    Console.WriteLine("휴식을 완료했습니다.");
                    Console.ReadLine();
                }
                else if (command == 1 && user.gold < 500)
                {
                    Console.WriteLine("Gold 가 부족합니다.");
                    Console.ReadLine();
                }

            }
        }

        //명령어를 입력받는 메서드
        //많이 사용되어서 메서드 화
        int inputCommand()
        {
            int command = 0;
            try
            {
                command = int.Parse(Console.ReadLine());
                return command;
            }
            catch (Exception)
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.ReadLine();
            }
            return -1;
        }

        //데이터를 저장하는 메서드
        void saveData()
        {
            //유저 데이터 저장
            string userData = JsonConvert.SerializeObject(user);
            File.WriteAllText(path + "\\UserData.json", userData);

            //유저안에 인벤토리가 Item List로 구현되어 있기 때문에
            //직렬화로 저장 시 Weapon과 Armor가 잘못된 방식으로 저장됨
            //해결을 위해서 무기와 방어구 리스트를 만들어 저장
            List<Armor> armors = new List<Armor>();
            List<Weapon> weapons = new List<Weapon>();
            foreach (Item item in user.inventory)
            {
                if (item.GetType() == typeof(Armor))
                {
                    armors.Add((Armor)item);
                }
                else
                {
                    weapons.Add((Weapon)item);
                }
            }

            string armorsData = JsonConvert.SerializeObject(armors);
            File.WriteAllText(path + "\\userArmorsData.json", armorsData);

            string weaponsData = JsonConvert.SerializeObject(weapons);
            File.WriteAllText(path + "\\userweaponsData.json", weaponsData);

            //상점 데이터를 저장
            //여기는 판매여부를 판단하는 정보만 저장
            string storeData = JsonConvert.SerializeObject(store);
            File.WriteAllText(path + "\\storeData.json", storeData);

            //상점도 유저와 마찬가지로 같은 버그 발생
            //해결을 위해서 같은 방법을 사용
            armors = new List<Armor>();
            weapons = new List<Weapon>();
            foreach (Item item in store.items)
            {
                if (item.GetType() == typeof(Armor))
                {
                    armors.Add((Armor)item);
                }
                else
                {
                    weapons.Add((Weapon)item);
                }
            }

            armorsData = JsonConvert.SerializeObject(armors);
            File.WriteAllText(path + "\\storeArmorsData.json", armorsData);

            weaponsData = JsonConvert.SerializeObject(weapons);
            File.WriteAllText(path + "\\storeWeaponsData.json", weaponsData);

            //던전의 정보를 저장
            //없어도 되는 과정
            string dungeonData = JsonConvert.SerializeObject(dungeons);
            File.WriteAllText(path + "\\dungeonData.json", dungeonData);
        }

        //저장된 정보를 가져오는 메서드
        public void loadData()
        {
            //유저 데이터가 있는지 확인
            if (!File.Exists(path + "\\UserData.json"))
            {   //데이터가 없으면 새로 생성
                createDate();
                return;
            }

            //유저의 정보를 가져오기
            string userLData = File.ReadAllText(path + "\\UserData.json");
            Player userLoadData = JsonConvert.DeserializeObject<Player>(userLData);
            user = userLoadData;
            user.inventory = new List<Item>();  //인벤토리는 비우고 새로 채우기

            //방어구 가져오기
            string userArmorsData = File.ReadAllText(path + "\\userArmorsData.json");
            List<Armor> armorsLoadData = JsonConvert.DeserializeObject<List<Armor>>(userArmorsData);
            foreach (Armor armor in armorsLoadData)
            {
                user.inventory.Add(armor);
            }

            //무기 가져오기
            string userWeaponsData = File.ReadAllText(path + "\\userWeaponsData.json");
            List<Weapon> WeaponsLoadData = JsonConvert.DeserializeObject<List<Weapon>>(userWeaponsData);
            foreach (Weapon weapon in WeaponsLoadData)
            {
                user.inventory.Add(weapon);
            }

            //상점 정보 가져오기
            string storeData = File.ReadAllText(path + "\\storeData.json");
            Store storeLoadData = JsonConvert.DeserializeObject<Store>(storeData);
            store = storeLoadData;
            store.items = new List<Item>(); //아이템 리스트는 비워주기

            //방어구 리스트 가져오기
            string storeArmorsData = File.ReadAllText(path + "\\storeArmorsData.json");
            armorsLoadData = JsonConvert.DeserializeObject<List<Armor>>(storeArmorsData);
            foreach (Armor armor in armorsLoadData)
            {
                store.items.Add(armor);
            }

            //무기 리스트 가져오기
            string storeWeaponsData = File.ReadAllText(path + "\\storeWeaponsData.json");
            WeaponsLoadData = JsonConvert.DeserializeObject<List<Weapon>>(storeWeaponsData);
            foreach (Weapon weapon in WeaponsLoadData)
            {
                store.items.Add(weapon);
            }

            //던전 데이터 가져오기
            //없어도 되는 과정
            //DungeonCreate 메서드를 구현해서 사용해도 됨.
            string dungeonData = File.ReadAllText(path + "\\dungeonData.json");
            Dungeon[] dungeonLoadData = JsonConvert.DeserializeObject<Dungeon[]>(dungeonData);
            if (dungeons == null)
            {
                dungeons = new Dungeon[3];
            }
            for (int i = 0; i < 3; i++)
            {
                dungeons[i] = dungeonLoadData[i];
            }
        }
    }
}
