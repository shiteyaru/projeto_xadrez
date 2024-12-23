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
        public bool Xeque {  get; private set; }
        public Peca VulneravelEnPassant { get; private set; }


        public PartidaDeXadrez() {
            tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
            
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino) {

            Peca p = tab.retirarPeca(origem);
            p.IncrementarQtdMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null) {
                Capturadas.Add(pecaCapturada);
            }

            // #jogada especial roque pequeno
            
            if (p is Rei && destino.Coluna == origem.Coluna + 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(origemT);
                T.IncrementarQtdMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // #jogada especial roque grande

            if (p is Rei && destino.Coluna == origem.Coluna - 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(origemT);
                T.IncrementarQtdMovimentos();
                tab.colocarPeca(T, destinoT);
            }

            // #jogada especial en passant

            if (p is Peao) {

                if (origem.Coluna != destino.Coluna && pecaCapturada == null) {
                    Posicao posP;

                    if (p.Cor == Cor.Branca) {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = tab.retirarPeca(posP);
                    Capturadas.Add(pecaCapturada);
                }

            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada) {
            Peca p = tab.retirarPeca(destino);
            p.DecrementarQtdMovimentos();
            if (pecaCapturada != null) {
                tab.colocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // #jogada especial roque pequeno

            if (p is Rei && destino.Coluna == origem.Coluna + 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.retirarPeca(destinoT);
                T.DecrementarQtdMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogada especial roque grande

            if (p is Rei && destino.Coluna == origem.Coluna - 2) {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.retirarPeca(destinoT);
                T.DecrementarQtdMovimentos();
                tab.colocarPeca(T, origemT);
            }

            // #jogada especial en passant

            if (p is Peao) {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant) {

                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    
                    if (p.Cor == Cor.Branca) {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    
                    else {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    
                    tab.colocarPeca(peao, posP);
                }
            }

        }


        public void RealizaJogada(Posicao origem, Posicao destino) {

            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EstaEmXeque(JogadorAtual)) {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocqar em xeque!");
            }

            if (EstaEmXeque(Adversaria(JogadorAtual))) {
                Xeque = true;
            }
            else {
                Xeque = false;
            }

            if (TesteXequeMate(Adversaria(JogadorAtual))) {
                Terminada = true;
            }
            else {
                Turno++;
                MudaJogador();
            }

            Peca p = tab.peca(destino);

            // #jogada especial en passant

            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2)) {
                VulneravelEnPassant = p;
            }
            else {
                VulneravelEnPassant = null;
            }


        }

        public void ValidarPosicaoOrigem(Posicao pos) {

            if(tab.peca(pos) == null) {
                throw new TabuleiroException($"Não existe peça na \nposição escolhida!");
            }
            if (JogadorAtual != tab.peca(pos).Cor) {
                throw new TabuleiroException($"A peça de origem escolhida \nnão é sua!");
            }
            if (!tab.peca(pos).ExisteMovimentosPossiveis()) {
                throw new TabuleiroException($"Não há movimentos possíveis para a \npeça de origem escolhida!");
            }

        }

        public void ValidarPosicaoDestino(Posicao origem, Posicao destino) {
            if (!tab.peca(origem).MovimentoPossivel(destino)) {
                throw new TabuleiroException($"Posição de destino inválida!");
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
            foreach (Peca x in Pecas) {

                if (x.Cor == cor) {
                    aux.Add(x);
                }

            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }


        private Cor Adversaria(Cor cor) {

            if (cor == Cor.Branca) {
                return Cor.Preta;
            }
            else {
                return Cor.Branca;
            }

        }

        private Peca Rei(Cor cor) {

            foreach(Peca x in PecasEmJogo(cor)) {

                if (x is Rei) {
                    return x;
                }

            }
            return null;
        }

        private bool EstaEmXeque(Cor cor) {
            Peca R = Rei(cor);
            if (R == null) {
                throw new TabuleiroException($"Não há rei da cor {cor} no tabuleiro!");
            }

            foreach (Peca x in PecasEmJogo(Adversaria(cor))) {

                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna]) {
                    return true;
                }

            }
            return false;
        }

        public bool TesteXequeMate(Cor cor) {
            if (!EstaEmXeque(cor)) {
                return false;
            }
            foreach(Peca x in PecasEmJogo(cor)) {
                bool[,] mat = x.MovimentosPossiveis();
                for(int i = 0; i < tab.Linhas; i++) {
                    for (int j = 0; j < tab.Colunas; j++) {
                        if (mat[i, j]) {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        public void ColocarNovaPeca(char coluna, int linha, Peca peca) {

            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);

        }

        private void ColocarPecas() {

            ColocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            ColocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            ColocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            ColocarNovaPeca('a', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(tab, Cor.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(tab, Cor.Branca, this));

            ColocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('d', 8, new Dama(tab, Cor.Preta));
            ColocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            ColocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            ColocarNovaPeca('a', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(tab, Cor.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(tab, Cor.Preta, this));

        }

    }
}
