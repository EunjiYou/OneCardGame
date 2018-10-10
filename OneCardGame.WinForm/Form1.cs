using OneCardGame.Engine;
using OneCardGame.WinForm.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OneCardGame.WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            playersNumber = 2;
            playingCardCount = 7;
            isInitialPrint = true;
            isChangeAbility = false;

            textBox = txtBox;

            playerLabels[0] = lblName0;
            playerLabels[1] = lblName1;

            cardLabels[0] = lblCardCnt0;
            cardLabels[1] = lblCardCnt1;
            cardLabels[2] = lblCardCnt2;

            curCardCountLabels[0] = lblCurCnt0;
            curCardCountLabels[1] = lblCurCnt1;
            curCardCountLabels[2] = lblCurCnt2;
            curCardCountLabels[3] = lblCurCnt3;

            directionLabel = lblDir;
            abilityLabel = lblAbility;
            
            fieldImage = ptbField;
            boardImage = ptbBoard;
            
            cardImages[0] = ptb0;
            cardImages[1] = ptb1;
            cardImages[2] = ptb2;
            cardImages[3] = ptb3;
            cardImages[4] = ptb4;
            cardImages[5] = ptb5;
            cardImages[6] = ptb6;
            cardImages[7] = ptb7;
            cardImages[8] = ptb8;
            cardImages[9] = ptb9;
            cardImages[10] = ptb10;
            cardImages[11] = ptb11;
            cardImages[12] = ptb12;
            cardImages[13] = ptb13;
            cardImages[14] = ptb14;
            cardImages[15] = ptb15;
            cardImages[16] = ptb16;
            cardImages[17] = ptb17;
            cardImages[18] = ptb18;
            cardImages[19] = ptb19;

            //Spade, Clover, Diamond, Hart, Joker
            images[0] = Resources.ace_of_spades2;
            images[1] = Resources._2_of_spades;
            images[2] = Resources._3_of_spades;
            images[3] = Resources._4_of_spades;
            images[4] = Resources._5_of_spades;
            images[5] = Resources._6_of_spades;
            images[6] = Resources._7_of_spades;
            images[7] = Resources._8_of_spades;
            images[8] = Resources._9_of_spades;
            images[9] = Resources._10_of_spades;
            images[10] = Resources.king_of_spades2;
            images[11] = Resources.queen_of_spades2;
            images[12] = Resources.jack_of_spades2;
            images[13] = Resources.ace_of_clubs;
            images[14] = Resources._2_of_clubs;
            images[15] = Resources._3_of_clubs;
            images[16] = Resources._4_of_clubs;
            images[17] = Resources._5_of_clubs;
            images[18] = Resources._6_of_clubs;
            images[19] = Resources._7_of_clubs;
            images[20] = Resources._8_of_clubs;
            images[21] = Resources._9_of_clubs;
            images[22] = Resources._10_of_clubs;
            images[23] = Resources.king_of_clubs2;
            images[24] = Resources.queen_of_clubs2;
            images[25] = Resources.jack_of_clubs2;
            images[26] = Resources.ace_of_diamonds;
            images[27] = Resources._2_of_diamonds;
            images[28] = Resources._3_of_diamonds;
            images[29] = Resources._4_of_diamonds;
            images[30] = Resources._5_of_diamonds;
            images[31] = Resources._6_of_diamonds;
            images[32] = Resources._7_of_diamonds;
            images[33] = Resources._8_of_diamonds;
            images[34] = Resources._9_of_diamonds;
            images[35] = Resources._10_of_diamonds;
            images[36] = Resources.king_of_diamonds2;
            images[37] = Resources.queen_of_diamonds2;
            images[38] = Resources.jack_of_diamonds2;
            images[39] = Resources.ace_of_hearts;
            images[40] = Resources._2_of_hearts;
            images[41] = Resources._3_of_hearts;
            images[42] = Resources._4_of_hearts;
            images[43] = Resources._5_of_hearts;
            images[44] = Resources._6_of_hearts;
            images[45] = Resources._7_of_hearts;
            images[46] = Resources._8_of_hearts;
            images[47] = Resources._9_of_hearts;
            images[48] = Resources._10_of_hearts;
            images[49] = Resources.king_of_hearts2;
            images[50] = Resources.queen_of_hearts2;
            images[51] = Resources.jack_of_hearts2;
            images[52] = Resources.black_joker;
            
            SetGame();
        }
        
        public void SetGame()
        {
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

            player = players[dealer.GetCurrentPlayer()];
            PrintBoard(board, dealer, players, player);
        }

        
        int playersNumber;
        int playingCardCount;
        bool isInitialPrint;
        bool isChangeAbility;

        Dealer dealer = new Dealer();
        Board board = new Board();
        List<Player> players = new List<Player>();

        TextBox textBox = new TextBox();
        
        String str = "";
        int num = 0;
        Player player = new Player(0);

        private void btnOK_Click(object sender, EventArgs e)
        {
            player = players[dealer.GetCurrentPlayer()];

            if (isChangeAbility)
            {
                str = textBox.Text;
                if (int.TryParse(str, out num))
                {
                    num = Int32.Parse(str);
                    if(num <= (int)Card.Pattern.Joker)
                    {
                        dealer.ChangePattern((Card.Pattern)(num - 1));

                        isChangeAbility = false;
                        lblQuestion.Text = "놓을 카드 번호를 입력해주세요.";
                        board.GetFieldCard().SpecialAbility(dealer);
                        dealer.ResetPlayCount();
                        GoToNextTurn();
                    }
                    else
                    {
                        MessageBox.Show("잘못된 입력입니다.");
                    }
                }
                else
                {
                    MessageBox.Show("숫자만 입력해주세요.");
                }
            }
            else
            { 
                str = textBox.Text;
                if (int.TryParse(str, out num))
                {
                    if (num <= player.GetCardAmount())
                    {
                        if (dealer.CanPlayCard(player.GetCard(num)))
                        {
                            //카드 개수가 0개인지 확인
                            if (!GameIsEnd(players))
                            {
                                //처음 필드에 올려지는 카드는 능력을 발휘하지 않음
                                if (isInitialPrint) isInitialPrint = false;

                                //플레이어의 카드를 보드의 필드카드로 보냄
                                Card card = player.PlayCard(num);
                                SetCardToField(card, board, dealer);

                                //문양을 바꾸는 능력은 사용자의 입력을 미리 받아둬야함
                                if (IsChangeAbilityCard(card))
                                {
                                    lblQuestion.Text = "어떤 문양으로 변경?  1:♠  2:♣  3:◆  4:♥";
                                    isChangeAbility = true;

                                }
                                else
                                {
                                    dealer.ResetPlayCount();
                                    //카드의 스페셜 능력 발동, 딜러를 건내줌
                                    card.SpecialAbility(dealer);
                                }

                                textBox.Clear();

                                if (GameIsEnd(players))
                                {
                                    GameEnd(players);
                                }
                                
                                if (dealer.GetPlayCount() == 0)
                                {
                                    GoToNextTurn();
                                }
                                else
                                {
                                    DrawBoard();
                                }
                            }
                        }
                        else
                        {

                            MessageBox.Show("낼 수 없는 카드입니다.");
                            //낼 수 없는 카드인데도 턴이 넘겨지는 버그
                        }
                    }
                    else
                    {
                        MessageBox.Show("잘못된 입력입니다.");
                    }
                }
                else
                {
                    MessageBox.Show("숫자만 입력해주세요.");
                }
            }
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            //플레이어가 카드를 새로 받음
            if (dealer.GetDamage() > 0)
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
                GameEnd(players);
            }
            else
            {
                GoToNextTurn();
            }
        }

        private void GameEnd(List<Player> players)
        {
            DrawBoard();
            textBox.Enabled = false;
            btnOK.Enabled = false;
            btnNew.Enabled = false;
            Player winner = GetWinner(players);
            lblQuestion.Text = "승자 : Player" + winner.no.ToString();
        }

        private void GoToNextTurn()
        {
            dealer.SetNextTurn();
            DrawBoard();
        }

        private void DrawBoard()
        {
            player = players[dealer.GetCurrentPlayer()];
            PrintBoard(board, dealer, players, player);
        }

        private bool IsChangeAbilityCard(Card card)
        {
            if (card.number == 7)
            {
                return true;
            }
            return false;
        }

        //해당 카드를 필드카드로 보내고 딜러에게 정보를 저장하게 함
        private void SetCardToField(Card card, Board board, Dealer dealer)
        {
            board.FieldCardSetting(card);
            dealer.SetCurrentFieldCard(board);
        }

        private void SendCardToPlayer(Board board, Player player)
        {
            Card card = board.GetBoardCard();
            player.TakeCard(card);
        }

        private Player GetWinner(List<Player> players)
        {
            if (players[0].GetCardAmount() == 0)
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

        private Label[] cardLabels = new Label[3];
        private Label[] playerLabels = new Label[2];
        private Label directionLabel = new Label();
        private Label abilityLabel = new Label();
        private Label[] curCardCountLabels = new Label[4];
        private PictureBox fieldImage = new PictureBox();
        private PictureBox boardImage = new PictureBox();
        private PictureBox[] cardImages = new PictureBox[20];
        private Image[] images = new Image[53];

        private void PrintBoard(Board board, Dealer dealer, List<Player> players, Player player)
        {
            boardImage.Image = Resources.backcard;

            //특수능력 프린트
            if (!isInitialPrint)
                PrintSpecialCardAbility(board, dealer);
            
            //필드카드 프린트
            cardLabels[2].Text = board.GetBoardCardsAmount().ToString();
            fieldImage.Image = FindImage(board.GetFieldCard());
            directionLabel.Text = dealer.GetCurrentDirection().ToString();
            
            //플레이어 상태 프린트
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == player)
                {
                    playerLabels[0].Text = players[i].no.ToString();
                    cardLabels[0].Text = players[i].GetCardAmount().ToString();

                    for (int j = 1; j <= cardImages.Length; j++)
                    {
                        cardImages[j - 1].Visible = false;

                        if (j <= players[i].GetCardAmount())
                        {
                            cardImages[j-1].Image = FindImage(player.GetCard(j));
                            cardImages[j-1].Visible = true;
                            cardImages[j-1].BringToFront();
                        }
                        else
                        {
                            cardImages[j - 1].Image = null;
                        }
                    }

                    for(int j = 0; j < curCardCountLabels.Length; j++)
                    {
                        curCardCountLabels[j].Visible = false;

                        if(j <= (int)(players[i].GetCardAmount()/5))
                        {
                            curCardCountLabels[j].Visible = true;
                        }
                    }
                }
                else
                {
                    playerLabels[1].Text = players[i].no.ToString();
                    cardLabels[1].Text = players[i].GetCardAmount().ToString();
                }
            }
        }

        private Image FindImage(Card card)
        {
            int num = (int)card.pattern * 13 + card.number;
            num = card.pattern == Card.Pattern.Joker ? num + 2 : num;
            return images[num-1];
        }

        private void PrintSpecialCardAbility(Board board, Dealer dealer)
        {
            String text = "";

            if (dealer.GetDamage() > 0)
            {
                text = "패널티 +" + dealer.GetDamage().ToString();
            }

            if (board.GetFieldCard().GetAbilityName().Equals(new NoAbility().ToString()))
            {
                
            }
            if (board.GetFieldCard().GetAbilityName().Equals(new OneMoreTimeAbility().ToString()))
            {
                text = "한 번 더!";
            }
            if (board.GetFieldCard().GetAbilityName().Equals(new ReverseAbility().ToString()))
            {
                text = "방향 전환";
            }
            if (board.GetFieldCard().GetAbilityName().Equals(new JumpAbility().ToString()))
            {
                text = "점프!";
            }
            if (board.GetFieldCard().GetAbilityName().Equals(new ChangePatternAbility().ToString()))
            {
                text = "제시된 문양 :" + (Card.Pattern)dealer.changePattern;
            }

            abilityLabel.Text = text;
        }
    }
}