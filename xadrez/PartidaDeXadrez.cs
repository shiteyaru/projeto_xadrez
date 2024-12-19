using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tabuleiro;
using xadrex_console;

namespace xadrez {
    internal class PartidaDeXadrez {

        public Tabuleiro tab { get; private set; } 
        public int Turno { get; private set;     }
        public Cor JogadorAtual {  get; private set; }
        public bool Terminada {  get; private set; }

        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            ColocarPecas();
            Terminada = false;
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino) {

            Peca p = tab.retirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);

        }

        public void RealizaJogada(Posicao origem, Posicao destino) {

            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();

        }

        public void ValidarPosicaoOrigem(Posicao pos) {

            if(tab.peca(pos) == null) {
                throw new TabuleiroException($"{Tela.Espaco()}Não existe peça na \n{Tela.Espaco()}posição escolhida!");
            }
            if (JogadorAtual != tab.peca(pos).Cor) {
                throw new TabuleiroException($"{Tela.Espaco()}A peça de origem escolhida \n{Tela.Espaco()}não é sua!");
            }
            if (!tab.peca(pos).ExisteMovimentosPossiveis()) {
                throw new TabuleiroException($"{Tela.Espaco()}Não há movimentos possíveis para a \n{Tela.Espaco()}peça de origem escolhida!");
            }

        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino) {
            if (!tab.peca(origem).PodeMoverPara(destino)) {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }


        private void MudaJogador() {

            if (JogadorAtual == Cor.Branca) {
                JogadorAtual = Cor.Preta;
            }
            else {
                JogadorAtual = Cor.Branca;
            }

        }

        private void ColocarPecas() {

            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c', 1).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c', 2).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('d', 2).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('e', 2).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('e', 1).ToPosicao());
            tab.colocarPeca(new Rei(tab, Cor.Branca), new PosicaoXadrez('d', 1).ToPosicao());

            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('c', 7).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('c', 8).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('d', 7).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('e', 7).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('e', 8).ToPosicao());
            tab.colocarPeca(new Rei(tab, Cor.Preta), new PosicaoXadrez('d', 8).ToPosicao());
        }

    }
}
