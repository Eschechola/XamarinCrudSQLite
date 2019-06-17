
using CRUD_Xamarin2.ModeloFilme;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CRUD_Xamarin2.Administrador;
using CRUD_Xamarin2.ModeloUsuarios;
using CRUD_Xamarin2.CRUD;

namespace CRUD_Xamarin2
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Exibir : ContentPage
    {
        //carrega a pagina de excluir
        Excluir excluir;
        //carrega a pagina de adicionar filme
        AdicionarFilme addfilme;
        //carrega o banco de dados
        ModeloBanco banco = new ModeloBanco();
        //carrega o usuario auxiliar
        Usuario _user;
        //carrega a pagina de alterar filme
        Alterar alterar;
        //carrega o filme auxiliar
        Filme enviarAlterar = new Filme();
        //carrega as permissoes de adm
        PadraoADM _adm;
        //variavel para verificar quando remover a pagina de adicionar
        bool add = false;
        //variavel para verificar quando remover a pagina de alterar
        bool alt = false;
        //variavel para verificar quando remover a pagina de deletar
        bool remove = false;
        //variavel para verificar quando adicionar o modo noturno
        bool _noturno = false;

        //construtor da pagina
        public Exibir(PadraoADM adm, Usuario usuario, bool noturno)
        {
            InitializeComponent();
            //define o valor de noturno como global
            _noturno = noturno;

            //se verdadeiro, muda a cor das paginas
            if (noturno)
            {
                abs_fundo1.BackgroundColor = Color.White;
                stk_fundo1.BackgroundColor = Color.White;
                stk_fundo2.BackgroundColor = Color.White;
                stk_fundo3.BackgroundColor = Color.Gray;
                btn1.BackgroundColor = Color.Gray;
                btn2.BackgroundColor = Color.Gray;
                btn3.BackgroundColor = Color.Gray;
                btn4.BackgroundColor = Color.Gray;
                btn5.BackgroundColor = Color.Gray;
                listaExibir.BackgroundColor = Color.White;
                listaExibir.SeparatorColor = Color.White;
            }

            //define que nao tera navigationbar na pagina
            NavigationPage.SetHasNavigationBar(this, false);

            //constroi a pagina de adicionar com os parametros ja passados
            addfilme = new AdicionarFilme(usuario, adm.Adm, _noturno);
            //constroi a pagina de excluir com os parametros ja passados
            excluir = new Excluir(usuario, adm, _noturno);

            //fala que o usuario global é igual ao usuario passado no construtor
            _user = usuario;
            //define o padrao de admnistrador é igual ao passado no construtor
            _adm = adm;

            //lista que vai pegar todos os filmes cadastrados naquela conta
            List<Filme> itens = new List<Filme>();
            //chama a funçao pra pegar todos os filmes
            itens = banco.GetFilmes(usuario);
            //adiciona +2 filmes vazios na list para deixar o layout mais visivel
            itens.Add(new Filme() { Imagem = "" });
            itens.Add(new Filme() { Imagem = "" });
            //adiciona todos os filmes pegos na list view
            listaExibir.ItemsSource = itens;

            //se o usuario for admnistrador chama o aviso de ADM
            if (adm.Adm)
            {
                avisoADM();
            }
        }

        //função para remover os POPUPs que serao chamados
        protected override bool OnBackButtonPressed()
        {
            if (remove)
            {
                PopupNavigation.RemovePageAsync(excluir);
                remove = false;
            } 
            else if (add)
            {
                PopupNavigation.RemovePageAsync(addfilme);
                add = false;
            }
            else if (alt)
            {
                PopupNavigation.RemovePageAsync(alterar);
                alt = false;
            }
            else
            {
                voltarPreview();
            }
            return true;
        }

        //funçao para o usuario sair
        private async void voltarPreview()
        {
            //pergunta se o usuario deseja realmente voltaar para pagina inicial
            var voltar = await DisplayAlert("Voltar", "Deseja mesmo voltar para a tela inicial?", "Ok", "Cancelar");
            if (voltar)
            {
                //define que a mainpage é a preview e chama ela 
                App.Current.MainPage = new NavigationPage(new Preview());
                Navigation.PushModalAsync(new Preview());
            }
        }

        //funçao para dar um aviso aos adms
        private async void avisoADM()
        {
            //chama o aviso para os admnistradores
            await PopupNavigation.PushAsync(new AvisoAdm(_user, _adm, _noturno));
        }

        //funçao para adicionar um filme
        private async void adicionarFilme(object sender, EventArgs e)
        {
            //limpa os campos da pagina filme
            addfilme.limparCampos();
            //declara a variavel de verificaçao para remover pagina como true
            add = true;
            //chama a pagina de adicionar filme
            await PopupNavigation.PushAsync(addfilme);
        }

        //funçao para deletar um filme
        private async void deletarFilme(object sender, EventArgs e)
        {
            //limpa os campos da pagina excluir
            excluir.limparCampos();
            //declara a variavel de verificaçao para remover pagina como true
            remove = true;
            //chama a pagina de excluir filme
            await PopupNavigation.PushAsync(excluir);
        }

        //funçao para alterar um filme
        private async void alterarFilme(object sender, EventArgs e)
        {
            //constroi a pagina alterar com os parametros ja passados
            alterar = new Alterar(_user, enviarAlterar, _adm, _noturno);
            //declara a variavel de verificaçao para remover pagina como true
            alt = true;
            //chama a pagina de alterar filme
            await PopupNavigation.PushAsync(alterar);
        }

        //funçao para ordenar a lista de filmes
        async void mostrarOpcoes(object sender, EventArgs e)
        {
            //abre um modal com 2 opçoes de ordenação
            string action = await DisplayActionSheet("Ordenar por:", null, "Cancelar", "Nome", "Ano");
            //lista que serao guardados os filmes depois de ordenados
            List<Filme> itens = new List<Filme>();

            //se a opção escolhida for nome
            if (action == "Nome")
            {
                //ele pega todos os filmes do usuario
                itens = banco.GetFilmes(_user);
                //ordena por nome e guarda na lista atraves de LINQ
                itens = itens.OrderBy(a => a.Nome).ToList();
                //adiciona um novo filme por questoes de layout
                itens.Add(new Filme() { Imagem = "" });
                //atribui a lista ja ordenada no listview
                listaExibir.ItemsSource = itens;
                await DisplayAlert("!", "Sua lista de filmes foi ordenada por nome!", "OK");
            }
            else if (action == "Ano")
            {
                //ele pega todos os filmes do usuario
                itens = banco.GetFilmes(_user);
                //ordena por ano e guarda na lista atraves de LINQ
                itens = itens.OrderBy(a => a.Ano).ToList();
                //adiciona um novo filme por questoes de layout
                itens.Add(new Filme() { Imagem = "" });
                //atribui a lista ja ordenada no listview
                listaExibir.ItemsSource = itens;
                await DisplayAlert("!", "Sua lista de filmes foi ordenada por ano!", "OK");
            }
            else { }
        }

        //funçao para abrir a pagina de configuraçao do usuario
        private async void abrirConfirguracoes(object sender, EventArgs e)
        {
            //chama a pagina de configuração do usuario
            await Navigation.PushModalAsync(new Configuracoes(_user, _adm, _noturno));
        }

        //funçao para atualizar a lista de filmes
        private void atualizarPaginaXAML(object sender, EventArgs e)
        {
            //lista que vai pegar os filmes
            var lista = banco.GetFilmes(_user);
            //filme adicionado por questao de layout
            lista.Add(new Filme() { Imagem = "" });
            //atribui a lista atualizada ao listview
            listaExibir.ItemsSource = lista;
            listaExibir.IsRefreshing = false;
        }

        //funçao para pesquisar os filmes na searchbar
        private void pesquisarFilme(object sender, TextChangedEventArgs e)
        {
            //guarda o conteudo da searchbar
            var texto = searchBar.Text;
            //pega todos os filmes do usuario
            var lista = banco.GetFilmes(_user);

            //se estiver nulo a lista fica sem filtro
            if (string.IsNullOrEmpty(texto) || string.IsNullOrWhiteSpace(texto))
            {
                lista.Add(new Filme() { Imagem = "" });
                listaExibir.ItemsSource = lista;
            }
            //senao
            else
            {
                //cria uma nova lista 
                var listaColocar = new List<Filme>();
                //adiciona na lista se tiver algum filme com o nome informado no filtro
                foreach (var x in lista)
                {
                    if (x.Nome.ToLower().Contains(texto.ToLower()))
                    {
                        listaColocar.Add(x);
                    }
                }
                listaColocar.Add(new Filme() { Imagem = "" });
                //adiciona a nova list filtrada ja no listview
                listaExibir.ItemsSource = listaColocar;
            }
        }

        //funçao para alterar o filme quando o usuario segurar no filme selecionado
        private async void iconeAlterarBanco(object sender, EventArgs e)
        {
            //define como verdadeiro para fechar o modal
            alt = true;
            //define o o item de menu como uma variavel
            var menuItem = sender as MenuItem;
            //define a variavel como um objeto Filme
            var pessoa = menuItem.CommandParameter as Filme;
            //define as propriedades dda classe filme auxiliar ccmo as do menuitem
            enviarAlterar.Nome = pessoa.Nome;
            enviarAlterar.Ano = pessoa.Ano;
            enviarAlterar.Genero = pessoa.Genero;
            //chama o modal de alterar com os parametros ja passados
            alterar = new Alterar(_user, enviarAlterar, _adm, _noturno);
            await PopupNavigation.PushAsync(alterar);

        }

        //funçao para excluir o filme quando o usuario segurar no filme selecionado
        private async void iconeDeletarBanco(object sender, EventArgs e)
        {
            //define o o item de menu como uma variavel
            var menuItem = sender as MenuItem;
            //define a variavel como um objeto Filme
            var produto = menuItem.CommandParameter as Filme;
            //define as propriedades dda classe filme auxiliar ccmo as do menuitem
            enviarAlterar.Nome = produto.Nome;
            //deleta o filme e guarda o retorno em uma variavel
            var sucesso = banco.DeletarFilme(enviarAlterar, _user);
            if (sucesso)
            {
                //se funcionar ele atualiza a pagina definindo ela como mainpage
                DisplayAlert("Sucesso!", enviarAlterar.Nome+" excluído com sucesso!", "Ok");
                App.Current.MainPage = new NavigationPage(new Exibir(_adm, _user, _noturno));
            }
            else
            {
                //senao da apenas um aviso
                DisplayAlert("Erro!", "Aconteceu algum erro. Por - favor contate os admnistradores!", "Entendi");
            }

        }
    }
}