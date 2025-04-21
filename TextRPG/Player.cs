using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine("{0}( {1} )",name, job);

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

                    Console.Write("- {0}", eqi);
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
            if (inventory[num - 1].GetType() == typeof(Weapon))
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
            if (exp == dungeonClear)
            {
                level++;
                exp++;
                return true;
            }
            return false;
        }
    }
}
