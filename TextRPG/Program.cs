using System;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace TextRPG
{
    class Player
    {
        public int level { get; set; }
        string name;
        string job;
        public int damage { get; set; }
        public int defense { get; set; }
        public int hp { get; set; }
        public int gold { get; set; }
        public List<Item> inventory { get; set; }

        //장착여부를 판단하는 장비
        public Weapon eWeapon { get; set; }
        public Armor eArmor { get; set; }

        //레벨업 관리를 위한 경험치
        public int exp;
        

        public Player(string name, string job)
        {
            level = 1;
            this.name = name;
            this.job = job;
            damage = 10;
            defense = 5;
            hp = 100;
            gold = 1500;
            inventory = new List<Item>();
            eWeapon = new Weapon();
            eArmor = new Armor();
            exp = 1;
        }

        public void playerInfo()
        {

            Console.WriteLine("Lv. " + level.ToString("D2"));
            Console.WriteLine("Chad( {0} )", job);
            if (eWeapon.damage > 0) Console.WriteLine("공격력 : {0} (+{1})", damage, eWeapon.damage);
            else Console.WriteLine("공격력 : {0}", damage);
            if (eArmor.defense > 0) Console.WriteLine("방어력 : {0} (+{1})", defense, eArmor.defense);
            else Console.WriteLine("방어력 : {0}", defense);
            Console.WriteLine("체 력 : " + hp);
            Console.WriteLine("Gold : {0} G", gold);
        }

        public void showInventory(bool type)
        {
            if (type)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    string eqi = "";
                    if (inventory[i] == eWeapon) eqi = "[E]";
                    if (inventory[i] == eArmor) eqi = "[E]";

                    Console.Write("- {0} {1}", i + 1, eqi);
                    inventory[i].itemInfo();
                }
            }
            else
            {
                foreach (Item item in inventory)
                {
                    string eqi = "";
                    if (item == eWeapon) eqi = "[E]";
                    if (item == eArmor) eqi = "[E]";

                    Console.Write("- {0}",eqi);
                    item.itemInfo();
                }
            }
        }

        public void showInventory()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                string price = inventory[i].price / 100 * 85 + " G";
                Console.Write("- {0} ", i + 1);
                inventory[i].itemInfo(price);
            }
        }

        public void equip(Weapon weapon) 
        {
            unequip(eWeapon);
            damage += weapon.damage;
            eWeapon = weapon;
        }
        public void unequip(Weapon weapon) 
        {
            damage -= weapon.damage;
            eWeapon = new Weapon();
        }
        public void equip(Armor armor)
        {
            unequip(eWeapon);
            defense += armor.defense;
            eArmor = armor;
        }
        public void unequip(Armor armor)
        {
            defense -= armor.defense;
            eArmor = new Armor();
        }

        public void itemEquipped(int num)
        {
            if (inventory[num-1].GetType() == typeof(Weapon))
            {
                equip((Weapon)inventory[num - 1]);
            }
            else
            {
                equip((Armor)inventory[num - 1]);
            }
        }

        public bool levelUp(int dungeonClear)
        {
            if(exp == dungeonClear)
            {
                level++;
                exp++;
                return true;
            }
            return false;
        }

    }

    class Item
    {
        public string name { get; set; }
        public string showing { get; set; }
        public int price { get; set; }

        public virtual void itemInfo() { }
        public virtual void itemInfo(string pri) { }

    }
    class Weapon : Item
    {

        public int damage { get; set; }
        public Weapon(string name, int damage, string showing,int price)
        {
            this.name = name;
            this.damage = damage;
            this.showing = showing;
            this.price = price;
        }
        public Weapon() { }

        public override void itemInfo()
        {
            Console.WriteLine("{0}\t| 공격력 +{1}\t| {2}",name, damage, showing);
        }
        public override void itemInfo(string pri)
        {
            Console.WriteLine("{0}\t| 공격력 +{1}\t| {2}\t{3}", name, damage, showing, pri);
        }
    }
    class Armor : Item
    {
        public int defense { get; set; }
        public Armor(string name, int defense, string showing,int price)
        {
            this.name = name;
            this.defense = defense;
            this.showing = showing;
            this.price = price;
        }
        public Armor() { }
        public override void itemInfo()
        {
            Console.WriteLine("{0}\t| 방어력 +{1}\t| {2}", name, defense, showing);
        }
        public override void itemInfo(string pri)
        {
            Console.WriteLine("{0}\t| 방어력 +{1}\t| {2}\t{3}", name, defense, showing, pri);
        }
    }

    class Store
    {
        public List<Item> items { get; set; }
        public bool[] sale { get; set; }

        public Store()
        {
            items = new List<Item>();
            sale = new bool[20];
        }
        public void addItem(Item item)
        {
            items.Add(item);
            sale[items.Count-1] = false;
        }
        public void showItems(bool type)
        {
            if (type)
            {
                for (int i = 0; i < items.Count; i++)
                {

                    string price = "";
                    if (sale[i]) price = "판매완료";
                    else price = items[i].price + "G";

                    Console.Write("- {0} ", i + 1);
                    items[i].itemInfo(price);
                }
            }
            else
            {
                foreach (Item item in items)
                {
                    string price = "";
                    if (sale[items.IndexOf(item)]) price = "판매완료";
                    else price = item.price + "G";

                    Console.Write("- ");
                    item.itemInfo(price);
                }
            }
        }

        public void buyItem(Player user,int num)
        {
            if(sale[num-1])
            {
                Console.WriteLine("이미 구매한 상품입니다.");
                Console.ReadLine();
            }
            else if(!sale[num-1] && user.gold >= items[num-1].price)
            {
                user.gold -= items[num - 1].price;
                user.inventory.Add(items[num - 1]);
                sale[num - 1] = true;
                Console.WriteLine("구매를 완료했습니다.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("골드가 부족합니다");
                Console.ReadLine();
            }

        }

        public void sellItem(Player user, int num)
        {
            //무기인지 방어구인지 판단 후 장비 해제
            if(user.inventory[num - 1].GetType() == typeof(Weapon))
            {
                user.unequip((Weapon)user.inventory[num - 1]);
            }
            else
            {
                user.unequip((Armor)user.inventory[num - 1]);
            }
            //골드 85퍼 환산
            user.gold += user.inventory[num - 1].price / 100 * 85;
            //다시 구매 가능
            for(int i=0;i<items.Count;i++)
            {
                if (user.inventory[num-1] == items[i])
                {
                    sale[i] = false;
                }
            }
            //인벤토리에서 제거
            user.inventory.RemoveAt(num - 1);

        }
    }

    class Dungeon
    {
        public string name { get; set; }
        public int recDefence { get; set; }
        public int clearGold { get; set; }
        public Random random { get; set; }

        public Dungeon(string name, int recDefence, int clearGold)
        {
            this.name = name;
            this.recDefence = recDefence;
            this.clearGold = clearGold;
            random = new Random();
        }

        public int tryDungeon(Player user, int dungeonClear)
        {
            if (user.defense < recDefence)
            {
                int rand = random.Next(1, 101);
                if (rand <= 40)
                {
                    failDungeon(user);
                    return dungeonClear;
                }
                else
                {
                    return clearDungeon(user, dungeonClear);
                }
            }
            else
            {
                return clearDungeon(user, dungeonClear);
            }
        }

        private int clearDungeon(Player user, int dungeonClear)
        {
            Console.Clear();

            Console.WriteLine("던전 클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine("{0}을 클리어 하셨습니다.\n", name);


            Console.WriteLine("[탐험 결과]");
            dungeonClear++; //level up 계산
            Console.WriteLine("{0}, {1}",user.exp,dungeonClear);
            if (user.levelUp(dungeonClear))
            {
                dungeonClear = 0;
                Console.WriteLine("Level {0} -> {1}", user.level - 1, user.level);
            }
            //유저가 받는 데미지 계산
            int def = user.defense - recDefence;
            int dunDamage = random.Next(20 + def, 35 + def);
            //0미만으로 못 내려가게 조정
            int userHp = user.hp - dunDamage;
            if (userHp < 0) userHp = 0;

            Console.WriteLine("체력 {0} -> {1}", user.hp, userHp);

            user.hp = userHp; //체력 반영

            //보상 계산
            int newGold = clearGold + (clearGold * random.Next(user.damage, user.damage * 2)) / 100;
            Console.WriteLine("Gold {0} G -> {1} G", user.gold, user.gold + newGold);

            user.gold += newGold;//보상 반영

            Console.WriteLine("\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요");
            Console.Write(">>");
            Console.ReadLine();

            return dungeonClear;
        }

        private void failDungeon(Player user)
        {
            Console.Clear();

            Console.WriteLine("던전 실패");
            Console.WriteLine("{0}의 공략을 실패 하셨습니다.\n", name);

            Console.WriteLine("[탐험 결과]");
            Console.WriteLine("체력 {0} -> {1}",user.hp, user.hp/2);

            user.hp /= 2;

            Console.WriteLine("\n0. 나가기\n");
            Console.WriteLine("원하시는 행동을 입력해 주세요");
            Console.Write(">>");
            Console.ReadLine();
        }

    }

    class GameManager
    {
        int dungeonClear = 0;
        Player user;
        Store store;
        Dungeon[] dunjeons;
        public void GameStart()
        {
            string name = nameCreate();
            string job = jobSelect();
            user = new Player(name, job);
        }

        string nameCreate()
        {
            string name = "";
            while (true)
            {
                Console.Clear();

                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.WriteLine("원하시는 이름을 설정해 주세요\n");

                name = Console.ReadLine();

                int command;
                while (true)
                {
                    Console.WriteLine("\n입력하신 이름은 {0} 입니다.\n", name);
                    Console.WriteLine("1. 저장\n2. 취소\n");
                    Console.WriteLine("원하시는 행동을 입력해 주세요.");

                    command = int.Parse(Console.ReadLine());

                    if (command == 1)
                    {
                        break;
                    }
                    else if (command == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Console.ReadLine();
                        continue;
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
                int command = int.Parse(Console.ReadLine());

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
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine();
                    continue;
                }


            }
            return job;
        }
        public void GamePlay()
        {
            //test code
            store = new Store();
            store.addItem(new Armor("수련자 갑옷", 5, "수련에 도움을 주는 갑옷입니다.", 1000));
            store.addItem(new Armor("그래도 좋은 갑옷", 7, "적당한 선능에 그럭저럭 쓸만한 갑옷입니다.", 1800));
            store.addItem(new Armor("무쇠갑옷", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2200));
            store.addItem(new Armor("스파르타의 갑옷", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500));
            store.addItem(new Weapon("낡은 검", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600));
            store.addItem(new Weapon("좋은 검", 4, "잘 다듬어져 있는 가성비 좋은 검 입니다.", 1000));
            store.addItem(new Weapon("청동 도끼", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500));
            store.addItem(new Weapon("스파르타의 창", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3200));

            dunjeons = new Dungeon[3];

            Dungeon easy = new Dungeon("쉬운 던전", 5, 1000);
            Dungeon normal = new Dungeon("일반 던전", 11, 1700);
            Dungeon hard = new Dungeon("어려운 던전", 17, 2500);
            dunjeons[0] = easy;
            dunjeons[1] = normal;
            dunjeons[2] = hard;


            while (true)
            {
                Console.Clear();

                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전입장\n5. 휴식하기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");

                int command = int.Parse(Console.ReadLine());

                if (command == 1) playInfo();
                else if (command == 2) openInventory();
                else if (command == 3) openStore(store);
                else if (command == 4) openDungeon(dunjeons);
                else if (command == 5) rest();

            }
        }
        void playInfo()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

                user.playerInfo();

                Console.WriteLine("\n0. 나가기\n");

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int Command = int.Parse(Console.ReadLine());

                if (Command == 0) break;
                else continue;
            }
        }

        void openInventory()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

                Console.WriteLine("[아이템 목록]");
                user.showInventory(false);

                Console.WriteLine("1. 장착 관리\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = int.Parse(Console.ReadLine());

                if (command == 1) equipmentMng();
                if (command == 0) break;
            }
        }

        void equipmentMng()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

                Console.WriteLine("[아이템 목록]");
                user.showInventory(true);

                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = int.Parse(Console.ReadLine());

                if (command == 0) break;
                else if (0 < command && command <= user.inventory.Count)
                {
                    user.itemEquipped(command);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine();
                }
            }


        }

        void openStore(Store store)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\n", user.gold);

                Console.WriteLine("[아이템 목록]");
                store.showItems(false);

                Console.WriteLine("\n1. 아이템 구매\n2. 아이템 판매\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = int.Parse(Console.ReadLine());

                if (command == 0) break;
                else if (command == 1) buyStore(store);
                else if (command == 2) sellStore(store);

            }
        }

        void buyStore(Store store)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점 - 아이템 구매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\n", user.gold);

                Console.WriteLine("[아이템 목록]");
                store.showItems(true);

                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = int.Parse(Console.ReadLine());

                if (command == 0) break;
                else if (0 < command && command <= store.items.Count)
                {
                    store.buyItem(user, command);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine();
                }
            }
        }

        void sellStore(Store store)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("상점 - 아이템 판매");
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

                Console.WriteLine("[보유 골드]");
                Console.WriteLine("{0}G\n", user.gold);

                Console.WriteLine("[아이템 목록]");
                user.showInventory();

                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = int.Parse(Console.ReadLine());

                if (command == 0) break;
                else if (0 < command && command <= user.inventory.Count)
                {
                    store.sellItem(user, command);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.ReadLine();
                }
            }
        }

        void openDungeon(Dungeon[] dungeons)
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("던전입장");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

                for (int i = 0; i < dungeons.Length; i++)
                {
                    Console.WriteLine("{0}. {1}\t| 방어력 {2} 이상 권장", i + 1, dungeons[i].name, dungeons[i].recDefence);
                }

                Console.WriteLine("0. 나가기\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">>");
                int command = int.Parse(Console.ReadLine());

                if (command == 0) break;
                else if (0 < command && command <= dungeons.Length && user.hp > 0)
                {
                    dungeonClear = dungeons[command - 1].tryDungeon(user, dungeonClear);
                }
                else if (0 < command && command <= dungeons.Length && user.hp == 0)
                {
                    Console.WriteLine("체력이 부족합니다.");
                    Console.ReadLine();
                }
            }
        }

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
                int command = int.Parse(Console.ReadLine());

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
    }




    internal class Program
    {
  
        static void Main(string[] args)
        {
            GameManager gameManager = new GameManager();

            gameManager.GameStart();
            gameManager.GamePlay();
        }

       
    }

}
