using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextRPG
{
    class GameManager2
    {
        string path = AppDomain.CurrentDomain.BaseDirectory; // 저장 경로
        private Character player = null!;                     // 플레이어 객체
        private Item[] itemDb = null!;                        // 아이템 DB
        private Dungeon[] dungeons = null!;                   // 던전 정보(아직 미구현?)
        private Random random;

        public GameManager2()
        {
            random = new Random(); // 랜덤 초기화
        }

        public void SetData() // 게임 시작 시 초기 세팅
        {
            player = new Character(1, nameCreate(), jobSelect(), 1500); // 이름, 직업, 초기 골드

            // 아이템 데이터 초기화
            itemDb = new Item[]
            {
                new Item("수련자의 갑옷", 1, 5, "수련에 도움을 주는 갑옷입니다. ", 1000),
                new Item("그래도 좋은 갑옷", 1, 7, "적당한 선능에 그럭저럭 쓸만한 갑옷입니다.", 1800),
                new Item("무쇠갑옷", 1, 9, "무쇠로 만들어져 튼튼한 갑옷입니다. ", 2000),
                new Item("스파르타의 갑옷", 1, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ", 3500),
                new Item("낣은 검", 0, 2, "쉽게 볼 수 있는 낡은 검 입니다. ", 600),
                new Item("좋은 검", 0, 4, "잘 다듬어져 있는 가성비 좋은 검 입니다.", 1000),
                new Item("청동 도끼", 0, 5, "어디선가 사용됐던거 같은 도끼입니다. ", 1500),
                new Item("스파르타의 창", 0, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다. ", 2500)
            };

            // 던전 초기화 //아직 미구현?
            dungeons = new Dungeon[]
            {
                new Dungeon("쉬운 던전", 5, 1000),
                new Dungeon("일반 던전", 11, 1700),
                new Dungeon("어려운 던전", 17, 2500)
            };
        }

        // 이름 입력 메서드
        string nameCreate()
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("원하시는 이름을 설정해 주세요\n");
            string name = Console.ReadLine();
            Console.WriteLine("\n입력하신 이름은 {0} 입니다.\n", name);
            Console.WriteLine("1. 저장\n2. 취소\n");
            int command = inputCommand(1, 2);
            if (command == 2) return nameCreate();
            return name;
        }

        // 직업 선택 메서드
        Job jobSelect()
        {
            Console.Clear();
            Console.WriteLine("직업을 선택하세요.\n1. 전사\n2. 도적\n3. 바바리안");
            int command = inputCommand(1, 3);
            return (Job)(command - 1);
        }

        // 메인 메뉴 표시
        public void DisplayMainUI()
        {
            Console.Clear();
            Console.WriteLine("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 전투\n5. 휴식하기");
            int command = inputCommand(1, 5);
            switch (command)
            {
                case 1: player.DisplayCharacterInfo(); break;
                case 2: Console.WriteLine("[인벤토리 기능 생략됨]"); break;
                case 3: Console.WriteLine("[상점 기능 생략됨]"); break;
                case 4: DisplayBattleUI(); break;
                case 5: Console.WriteLine("[휴식 기능 생략됨]"); break;
            }
            Console.WriteLine("\n0. 나가기");
            if (inputCommand(0, 0) == 0) DisplayMainUI();
        }


        // 전투 시스템 (미니언 1마리만 임시 구현)
        void DisplayBattleUI()
        {
            Console.Clear();
            Console.WriteLine("Battle!!\n");

            Monster monster = new Monster(3, "미니언", 7, 30); // 미니언 1마리 생성
            bool playerTurn = true;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!!\n");
                Console.WriteLine($"Lv.{monster.level} {monster.name} {(monster.isDie() ? "Dead" : "HP " + monster.Hp)}");
                Console.WriteLine($"\nLv.{player.Level} {player.Name} HP {player.Hp}/100\n");

                if (playerTurn)
                {
                    Console.WriteLine("1. 공격\n>> ");
                    if (Console.ReadLine() != "1") continue;

                    int damage = (int)Math.Ceiling(player.Atk * (random.NextDouble() * 0.2 + 0.9));
                    monster.playergetdamage(player);
                    Console.WriteLine($"{player.Name} 의 공격! {monster.name}에게 {damage} 데미지!");
                    Console.ReadLine();
                    playerTurn = false;
                }
                else
                {
                    if (!monster.isDie())
                    {
                        int damage = (int)Math.Ceiling(monster.Atk * (random.NextDouble() * 0.2 + 0.9));
                        player.Hp -= damage;
                        if (player.Hp < 0) player.Hp = 0;
                        Console.WriteLine($"{monster.name} 의 공격! {player.Name}에게 {damage} 데미지!");
                        Console.ReadLine();
                    }
                    playerTurn = true;
                }

                if (monster.isDie())
                {
                    Console.WriteLine("\nVictory! 몬스터 처치 완료.");
                    Console.ReadLine();
                    break;
                }
                else if (player.Hp <= 0)
                {
                    Console.WriteLine("\nYou Lose... 플레이어 사망.");
                    Console.ReadLine();
                    break;
                }
            }
            DisplayMainUI();
        }

        // 사용자 입력 처리
        int inputCommand(int min, int max)
        {
            int result;
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out result) && result >= min && result <= max)
                    return result;
                Console.WriteLine("잘못된 입력입니다!");
            }
        }           
    }
}