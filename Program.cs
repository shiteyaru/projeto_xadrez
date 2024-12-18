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
                    Tela.ImprimirTabuleiro(partida.tab);
                }
                
            } 
            catch (TabuleiroException e) {
                
                Console.WriteLine(e.Message);

            }

            Console.ReadLine();

        }
    }
}
