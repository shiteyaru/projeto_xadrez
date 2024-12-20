using tabuleiro;
using xadrex_console;
using tabuleiro;
using xadrez;

namespace Course {
    class Program {
        static void Main(string[] args) {

            try {

                PartidaDeXadrez partida = new PartidaDeXadrez();

                while (!partida.Terminada) {

                    try {
                        Console.Clear();
                        Tela.ImprimirPartida(partida);

                        Console.Write($"\n{Tela.Espaco()}Origem: ");
                        Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoOrigem(origem);

                        bool[,] posicoesPossiveis = partida.tab.peca(origem).MovimentosPossiveis();
                        Console.Clear();
                        Tela.ImprimirTabuleiro(partida.tab, posicoesPossiveis);


                        Console.Write($"\n{Tela.Espaco()}Destino: ");
                        Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                        partida.ValidarPosicaoDestino(origem, destino);

                        partida.RealizaJogada(origem, destino);
                    }
                    catch (TabuleiroException e) {
                        Console.WriteLine(e.Message);
                        Console.Write($"{Tela.Espaco()}");
                        Console.ReadLine();
                    }
                }
            } 
            catch (TabuleiroException e) {
                
                Console.WriteLine(e.Message);

            }

            Console.ReadLine();

        }

        
    }
}
