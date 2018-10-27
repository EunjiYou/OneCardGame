using OneCardGame.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneCardGame.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //플레이하는 인원 수와 인원 수 당 제시할 카드 수 지정
            int playersNumber = 2;
            int playingCardCount = 7;
            bool isPlayCountEnd = false;

            //클래스들 선언
            Dealer dealer = new Dealer();
            Board board = new Board();
            List<Player> players = new List<Player>();

            //지정된 플레이어 수만큼 생성 후 리스트에 추가
            for (int i = 0; i < playersNumber; i++)
            {
                Player player = new Player(i);
                players.Add(player);
            }

            //먼저 시작할 플레이어 결정
            dealer.SetCurrentPlayer(0);
            
            //보드에서 카드 셔플
            board.SetCard();
            
            //플레이어들에게 지정한 만큼의 카드 배분
            for (int i = 0; i < playersNumber; i++)
            {
                for (int j = 0; j < playingCardCount; j++)
                {
                    SendCardToPlayer(board, players[i]);
                }
            }

            //필드에 하나 카드를 올려놓음. 이 카드를 시작으로 게임을 진행
            Card firstCard = board.GetBoardCard();
            SetCardToField(firstCard, board, dealer);


            String str = "";
            int num = 0;
            //게임은 한 명이 파산하거나 카드 갯수가 0개가 될 때까지 계속된다.
            while (!GameIsEnd(players))
            {
                //시작하면 플레이어 한 명이 한 턴을 시작한다.
                Player player = players[dealer.GetCurrentPlayer()];

                //반복가능 횟수만큼 플레이 가능(특수카드 능력에 대비)
                for (int i = 0; i < dealer.GetPlayCount(); i++)
                {
                    isPlayCountEnd = false;
                    
                    //화면을 보여줌
                    PrintBoard(board, dealer, players, player);

                    //반복가능 횟수 한 번을 썼으면(카드를 내던가 받던가 다 했으면)
                    while (!isPlayCountEnd)
                    {
                        //가진 카드 중 몇 번째 카드를 선택할 건지 선택지를 주고 플레이어가 선택
                        System.Console.WriteLine("몇 번째 카드를 선택? (카드 받기 : 0)");
                        str = System.Console.ReadLine();
                        try
                        {
                            num = Int32.Parse(str);
                        }
                        catch (FormatException e)
                        {
                            System.Console.WriteLine(e.Message);
                            continue;
                        }
                        if (num > player.GetCardAmount())
                        {
                            System.Console.WriteLine("잘못된 입력입니다.");
                            continue;
                        }

                        //새 카드를 뽑는 경우
                        if (num == 0)
                        {
                            //플레이어가 카드를 새로 받음
                            if(dealer.GetDamage() > 0)
                            {
                                for (int j = 1; j <= dealer.GetDamage(); j++)
                                {
                                    SendCardToPlayer(board, player);
                                }

                                dealer.ResetDamage();
                            }
                            else
                            {
                                SendCardToPlayer(board, player);
                            }
                            
                            //카드 수가 초과할 경우를 대비해 확인
                            if (GameIsEnd(players))
                            {
                                break;
                            }
                        }
                        //카드를 내는 경우
                        else
                        {
                            //카드가 낼 수 있는지 딜러가 현재 문양과 숫자를 보고 판단
                            //맞으면 그대로 제출하도록, 아니면 사용 가능한 카드를 제출하거나 새카드를 뽑을 때까지 재질문
                            if (!dealer.CanPlayCard(player.GetCard(num)))
                            {
                                System.Console.WriteLine("낼 수 없는 카드입니다.");
                                continue;
                            }

                            //카드 개수가 0개인지 확인
                            if (GameIsEnd(players))
                            {
                                break;
                            }

                            //플레이어의 카드를 보드의 필드카드로 보냄
                            Card card = player.PlayCard(num);
                            SetCardToField(card, board, dealer);

                            //문양을 바꾸는 능력은 사용자의 입력을 미리 받아둬야함
                            if(IsChangeAbilityCard(card))
                            {
                                while(true)
                                {
                                    System.Console.WriteLine("어떤 문양으로 변경?  1:♠  2:♣  3:◆  4:♥");
                                    str = System.Console.ReadLine();
                                    try
                                    {
                                        num = Int32.Parse(str);
                                    }
                                    catch (FormatException e)
                                    {
                                        System.Console.WriteLine(e.Message);
                                        continue;
                                    }

                                    dealer.ChangePattern((Card.Pattern)(num - 1));
                                    break;
                                }
                                    
                            }

                            //카드의 스페셜 능력 발동, 딜러를 건내줌
                            card.SpecialAbility(dealer);
                        }

                        isPlayCountEnd = true;
                    }
                }

                //게임이 계속 되면 다음 턴으로 넘김
                dealer.SetNextTurn();
            }

            Player winner = GetWinner(players);
            System.Console.WriteLine($"Winner is Player{winner.no}");
        }

        private static bool IsChangeAbilityCard(Card card)
        {
            if(card.number == 7)
            {
                return true;
            }
            return false;
        }

        //해당 카드를 필드카드로 보내고 딜러에게 정보를 저장하게 함
        private static void SetCardToField(Card card, Board board, Dealer dealer)
        {
            board.FieldCardSetting(card);
            dealer.SetCurrentFieldCard(board);
        }

        private static void SendCardToPlayer(Board board, Player player)
        {
            Card card = board.GetBoardCard();
            player.TakeCard(card);
        }

        private static Player GetWinner(List<Player> players)
        {
            if (players[0].GetCardAmount() < players[1].GetCardAmount())
            {
                return players[0];
            }
            else
            {
                return players[1];
            }
        }

        private static bool GameIsEnd(List<Player> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].GetCardAmount() == 0 || players[i].GetCardAmount() >= 20)
                {
                    return true;
                }
            }

            return false;
        }

        private static void PrintBoard(Board board, Dealer dealer, List<Player> players, Player player)
        {
            System.Console.Clear();
            //보드 상황을 프린트해주는 함수
            System.Console.WriteLine();
            //최상단은 특수능력이 발동될때마다 프린트를 다르게 해줌  
            PrintSpecialCardAbility(board, dealer);
            System.Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");

            //필드카드 프린트
            System.Console.WriteLine($"남은 카드 {board.GetBoardCardsAmount()}장         필드 : {board.GetFieldCard().ToText()}        방향 : {dealer.GetCurrentDirection().ToString()}");
            System.Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");


            //플레이어 상태 프린트
            for (int i = 0; i < players.Count; i++)
            {
                System.Console.WriteLine($"Player{players[i].no} : {players[i].GetCardAmount()}장 보유");
                if (players[i] == player)
                {
                    for (int j = 1; j <= players[i].GetCardAmount(); j++)
                    {
                        if (j < 10)
                        {
                            System.Console.Write($"0{j}: {player.GetCard(j).ToText()}    ");
                        }
                        else
                        {
                            System.Console.Write($"{j}: {player.GetCard(j).ToText()}    ");
                        }

                        if (j == 10)
                        {
                            System.Console.WriteLine();
                        }
                    }
                    System.Console.WriteLine();
                    System.Console.WriteLine();
                }
                else
                {
                    System.Console.WriteLine();
                }
            }

            System.Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            System.Console.WriteLine();
        }

        private static void PrintSpecialCardAbility(Board board, Dealer dealer)
        {
            if (dealer.GetDamage() > 0)
            {
                System.Console.WriteLine($"Damage : {dealer.GetDamage()}    ");
            }

            if (board.GetFieldCard().GetAbilityName().Equals(new NoAbility().ToString()))
            {
                System.Console.WriteLine();
            }
            if (board.GetFieldCard().GetAbilityName().Equals(new OneMoreTimeAbility().ToString()))
            {
                System.Console.WriteLine("한 번 더!");
            }
            if (board.GetFieldCard().GetAbilityName().Equals(new ReverseAbility().ToString()))
            {
                System.Console.WriteLine("방향 전환");
            }
            if (board.GetFieldCard().GetAbilityName().Equals(new JumpAbility().ToString()))
            {
                System.Console.WriteLine("점프!");
            }
            if (board.GetFieldCard().GetAbilityName().Equals(new ChangePatternAbility().ToString()))
            {
                System.Console.WriteLine($"제시된 문양 : {(Card.Pattern)dealer.changePattern}");
            }
        }
    }
}
