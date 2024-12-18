using tabuleiro;
using xadrex_console;
using tabuleiro;
using xadrez;

namespace Course {
    class Program {
        static void Main(string[] args) {

            PosicaoXadrez pos = new PosicaoXadrez('c', 6);
            Console.WriteLine(pos);
            Console.WriteLine(pos.ToPosicao());

            Console.ReadLine();

        }
    }
}
