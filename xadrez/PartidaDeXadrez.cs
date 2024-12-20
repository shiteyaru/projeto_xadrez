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
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;


        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
            
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino) {

            Peca p = tab.retirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null) {
                Capturadas.Add(pecaCapturada);
            }
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

        public HashSet<Peca> PecasCapturadas(Cor cor) {

            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas) { 

                if (x.Cor == cor) {
                    aux.Add(x);
                }

            }

            return aux;


        }

        public HashSet<Peca> PecasEmJogo(Cor cor) {

            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas) {

                if (x.Cor == cor) {
                    aux.Add(x);
                }

            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }


        public void ColocarNovaPeca(char coluna, int linha, Peca peca) {

            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);

        }

        private void ColocarPecas() {

            ColocarNovaPeca('c', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('c', 2, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('d', 2, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('e', 2, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Rei(tab, Cor.Branca));

            ColocarNovaPeca('c', 7, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('d', 7, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('e', 7, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Rei(tab, Cor.Preta));
        }

    }
}
