using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace TextRPG
{
    class Player
    {
        //기본 정보 
        public int level { get; set; }
        public string name;
        public string job;
        public int damage { get; set; }
        public int defense { get; set; }
        public int hp { get; set; }
        public int gold { get; set; }
        //아이템 인벤토리
        public List<Item> inventory { get; set; }

        //장착여부를 판단하는 장비
        public Weapon eWeapon { get; set; }
        public Armor eArmor { get; set; }

        //레벨업 관리를 위한 경험치
        public int exp;
        public int dungeonClear { get; set; }

        // 플레이어 생성자
        public Player(string name, string job)
        {
            level = 1;
            this.name = name;
            this.job = job;
            damage = 10;
            defense = 5;
            hp = 100;
            gold = 1500;
            //인벤토리
            inventory = new List<Item>();
            //장착 여부를 판단
            eWeapon = new Weapon();
            eArmor = new Armor();
            //레벨업을 판단하는 경험치
            exp = 1;
            dungeonClear = 0;
        }

        public void playerInfo()
        {
            //플레이어 정보 표시하는 함수
            Console.WriteLine("Lv. " + level.ToString("D2"));
            Console.WriteLine("Chad( {0} )", job);

            //장착된 장비가 있으면 다르게 표시
            if (eWeapon.damage > 0) Console.WriteLine("공격력 : {0} (+{1})", damage, eWeapon.damage);
            else Console.WriteLine("공격력 : {0}", damage);
            if (eArmor.defense > 0) Console.WriteLine("방어력 : {0} (+{1})", defense, eArmor.defense);
            else Console.WriteLine("방어력 : {0}", defense);

            Console.WriteLine("체 력 : " + hp);
            Console.WriteLine("Gold : {0} G", gold);
        }

        //인벤토리 아이템을 조회하는 함수
        public void showInventory(bool type)    //type에 따라 앞에 숫자가 붙는지 판단
        {
            if (type)
            {
                for (int i = 0; i < inventory.Count; i++)
                {
                    // 장착된 아이템은 앞에 [E] 표시
                    string eqi = "";
                    if (inventory[i].name == eWeapon.name) eqi = "[E]";
                    if (inventory[i].name == eArmor.name) eqi = "[E]";

                    Console.Write("- {0} {1}", i + 1, eqi);
                    inventory[i].itemInfo();
                }
            }
            else
            {
                foreach (Item item in inventory)
                {
                    // 장착된 아이템은 앞에 [E] 표시
                    string eqi = "";
                    if (item.name == eWeapon.name) eqi = "[E]";
                    if (item.name == eArmor.name) eqi = "[E]";

                    Console.Write("- {0}",eqi);
                    item.itemInfo();
                }
            }
        }

        //판매 할 때 이용되는 showInventory
        public void showInventory() 
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                string price = inventory[i].price / 100 * 85 + " G";
                Console.Write("- {0} ", i + 1);
                inventory[i].itemInfo(price);
            }
        }

        //장비 장착, 무기 버전
        public void equip(Weapon weapon) 
        {
            //이미 장착되어 있던 무기라면 해제
            if (eWeapon.Equals(weapon))
            {
                unequip(eWeapon);
                return;
            }
            //장착되어 있는 무기 해제 선택한 무기 장착
            unequip(eWeapon);
            damage += weapon.damage;
            eWeapon = weapon;
        }
        //장비 해제, 무기 버전
        public void unequip(Weapon weapon) 
        {
            damage -= weapon.damage;
            eWeapon = new Weapon();
        }

        //무기와 설명 동일
        public void equip(Armor armor)
        {
            if (eArmor.Equals(armor))
            {
                unequip(eArmor);
                return;
            }
            unequip(eArmor);
            defense += armor.defense;
            eArmor = armor;
        }
        public void unequip(Armor armor)
        {
            defense -= armor.defense;
            eArmor = new Armor();
        }
        
        //아이템 장착 관리 하는 메서드
        //입력 받은 숫자를 장비로 변환해서 실행
        //입력 받은 숫자가 무기인지 방어구인지 판단
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

        //레벨업, 경험치 바도 늘어난다.
        public bool levelUp()
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
        //아이템이 공통으로 가지고 있는 정보
        public string name { get; set; }
        public string showing { get; set; }
        public int price { get; set; }
        
        //아이템 정보를 표시하는 가상 메서드
        public virtual void itemInfo() { }
        public virtual void itemInfo(string pri) { }

    }

    class Weapon : Item
    {
        public int damage { get; set; }//무기만 가지고 있는 정보

        //무기 생성자
        public Weapon(string name, int damage, string showing,int price)
        {
            this.name = name;
            this.damage = damage;
            this.showing = showing;
            this.price = price;
        }
        public Weapon() { }

        //아이템 정보를 표시하는 오버라이드 메서드
        public override void itemInfo()
        {
            Console.WriteLine("{0}\t| 공격력 +{1}\t| {2}",name, damage, showing);
        }
        //아이템 가격을 표시하는 오버라이드 메서드
        //판매할 땐 원래 가격의 85퍼를 표시하기 때문에 이렇게 구현
        public override void itemInfo(string pri)
        {
            Console.WriteLine("{0}\t| 공격력 +{1}\t| {2}\t{3}", name, damage, showing, pri);
        }
    }

    class Armor : Item
    {
        //설명이 무기와 같음
        public int defense { get; set; }    //방어구만 가지고 있는 정보
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
        //장비를 파는 상점
        public List<Item> items { get; set; }
        //판매 여부
        public bool[] sale { get; set; }

        //상점 생성자
        public Store()
        {
            items = new List<Item>();
            sale = new bool[20];
        }
        //상점에 아이템 추가하는 메서드
        public void addItem(Item item)
        {
            items.Add(item);
            sale[items.Count-1] = false;
        }
        //상점의 아이템을 표시하는 메서드
        //인벤토리와 마찬가지로 타입에 따라 앞에 숫자를 표시
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

        //아이템을 판매할 때 
        public void buyItem(Player user,int num)
        {
            if(sale[num-1])
            {//아이템의 구매 여부를 판단
                Console.WriteLine("이미 구매한 상품입니다.");
                Console.ReadLine();
            }
            else if(!sale[num-1] && user.gold >= items[num-1].price)
            {//유저가 소지한 골드가 충분하면 아이템 구매 성공
                user.gold -= items[num - 1].price;
                user.inventory.Add(items[num - 1]);
                sale[num - 1] = true;
                Console.WriteLine("구매를 완료했습니다.");
                Console.ReadLine();
            }
            else
            {//유저가 소지한 골드가 충분하지 않으면 구매 실패
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
        //던전의 이름, 권장 방어력, 클리어 골드
        public string name { get; set; }
        public int recDefence { get; set; }
        public int clearGold { get; set; }
        //클리어 확률 계산에 필요한 랜덤 클래스
        //이건 게임 매니저에 있어도 될거 같음
        public Random random { get; set; }

        //던전 생성자 
        public Dungeon(string name, int recDefence, int clearGold)
        {
            this.name = name;
            this.recDefence = recDefence;
            this.clearGold = clearGold;
            random = new Random();
        }

        //던전 시도, 권장 방어력보다 낮으면 40퍼 확률로 클리어
        //이 메서드는 Random 클래스 때문에 던전 클래스에 있으므로 개선의 여지 있음.
        public void tryDungeon(Player user)
        {
            if (user.defense < recDefence)
            {
                int rand = random.Next(1, 101);
                if (rand <= 40)
                {
                    failDungeon(user);
                }
                else
                {
                    clearDungeon(user);
                }
            }
            else
            {
                clearDungeon(user);
            }
        }

        //던전 클리어시 표시되는 창
        //인터페이스 역할을 하는 GameManager 클래스에서 하고 싶었으나
        //이전 정보와 변경된 정보를 표시해야 하기 때문에 이곳에 작성
        private void clearDungeon(Player user)
        {
            Console.Clear();

            Console.WriteLine("던전 클리어");
            Console.WriteLine("축하합니다!!");
            Console.WriteLine("{0}을 클리어 하셨습니다.\n", name);


            Console.WriteLine("[탐험 결과]");
            user.dungeonClear++; //level up 계산
            if (user.levelUp())
            {
                user.dungeonClear = 0;
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

        }

        //던전 실패시 표시되는 창
        //클리어 메서드와 마찬가지로 아쉬움이 남음.
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
                else if (command == 0){
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
        void openStore( )
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
        void buyStore( )
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
        void sellStore( )
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
            foreach(Item item in user.inventory)
            {
                if(item.GetType() == typeof(Armor))
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




    internal class Program
    {
  
        static void Main(string[] args)
        {
            //저장 데이터 로드 후 실행
            GameManager gameManager = new GameManager();
            gameManager.loadData();
            gameManager.GamePlay();

        }

       
    }

}
