using CRUD_Xamarin2.Administrador;
using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.ModeloFilme;
using CRUD_Xamarin2.ModeloUsuarios;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRUD_Xamarin2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Alterar : PopupPage
	{
        //variaveis auxiliares
        ModeloBanco banco = new ModeloBanco();
        Usuario _user;
        Filme filme = new Filme();
        Filme filme_anterior = new Filme();
        PadraoADM _adm;
        string caminhoimagem;
        bool _noturno = false;

        //funçao para alterar dados de um filme
        public Alterar (Usuario user, Filme _filme, PadraoADM adm, bool noturno)
		{
			InitializeComponent ();
            boxAlterar.IsEnabled = false;
            boxAlterar.IsVisible = false;
            _user = user;
            _adm = adm;
            _noturno = noturno;

            //se o paramentro _filme nao estiver vazio ele ja pesquisa o nome do filme para poder alterar
            if (!string.IsNullOrEmpty(_filme.Nome))
            {
                lbl_nome_pesquisar.Text = _filme.Nome;
            }

            //modo noturno
            if (noturno)
            {
                stk1.BackgroundColor = Color.White;
                btn1.BackgroundColor = Color.DimGray;
                btn1.TextColor = Color.White;
                btn2.BackgroundColor = Color.DimGray;
                btn2.TextColor = Color.White;
                lbl_nome_pesquisar.PlaceholderColor = Color.DimGray;
                lbl_nome_alterar.TextColor = Color.Black;
                lbl_nome_alterar.PlaceholderColor = Color.DimGray;
                lbl_nome_pesquisar.TextColor = Color.Black;
                lbl_ano_alterar.PlaceholderColor = Color.DimGray;
                lbl_ano_alterar.TextColor = Color.Black;
                lbl_genero_alterar.PlaceholderColor = Color.DimGray;
                lbl_genero_alterar.TextColor = Color.Black;
            }
        }

        //funçao para pegar os dados do novo filme
        private async void abrirAlterar(object sender, EventArgs e)
        {
            //se o nome passado como parametro nao for nulo ele pesquisa no banco
            if (!string.IsNullOrEmpty(lbl_nome_pesquisar.Text))
            {
                var filmeExiste = banco.GetFilme(_user, lbl_nome_pesquisar.Text);

                //se ele nao existir no banco ele nao aparece o modal pra alteração
                if (filmeExiste.Count < 1)
                {
                    boxAlterar.IsEnabled = false;
                    boxAlterar.IsVisible = false;
                    DisplayAlert("Erro!", "Filme não encontrado! Tente novamente.", "Ok");
                }
                else
                {
                    //atribui os valores as Entrys e as variaveis auxiliares
                    filme_anterior = filmeExiste[0];
                    boxAlterar.IsEnabled = true;
                    boxAlterar.IsVisible = true;
                    lbl_nome_alterar.Text = filmeExiste[0].Nome;
                    lbl_ano_alterar.Text = filmeExiste[0].Ano;
                    lbl_genero_alterar.Text = filmeExiste[0].Genero;
                    filme_anterior.Nome = filmeExiste[0].Nome;
                    filme_anterior.Genero = filmeExiste[0].Genero;
                    filme_anterior.Ano = filmeExiste[0].Ano;
                }
            }
            else
            {
                DisplayAlert("Erro!", "Insira um nome para o filme! Tente novamente.", "Ok");
            }
        }

        //função para limpar as entrys do filme
        public void limparCampos()
        {
            lbl_nome_pesquisar.Text = String.Empty;
            lbl_nome_alterar.Text = String.Empty;
            lbl_ano_alterar.Text = String.Empty;
            lbl_genero_alterar.Text = String.Empty;
            boxAlterar.IsEnabled = false;
            boxAlterar.IsVisible = false;
        }

        //funçao para atualizar os dados no banco
        private async void atualizarFilme(object sender, EventArgs e)
        {
            //atribui os valores da entry aos valores do filme auxiliar
            filme.Nome = lbl_nome_alterar.Text;
            filme.Genero = lbl_genero_alterar.Text;
            filme.Ano = lbl_ano_alterar.Text;
            //verifica se a imagem foi pega ou nao
            if (string.IsNullOrEmpty(caminhoimagem) || string.IsNullOrWhiteSpace(caminhoimagem))
            {
                caminhoimagem = "none.png";
            }
            else
            {
                //se foi pega ele coloca a imagem, senao uma imagem padrao do celular
                filme.Imagem = caminhoimagem;
            }

            //atualiza os dados do filme no banco
            var sucesso = banco.AtualizarFilme(filme, _user, filme_anterior);

            //se ocorreu tudo bem ele da um alert e atualiza a lista de filmes
            if (sucesso)
            {
                DisplayAlert("Parabéns", "O filme foi atualizado com sucesso!", "Ok");
                App.Current.MainPage = new NavigationPage(new Exibir(_adm, _user, _noturno));
                await PopupNavigation.PopAllAsync();
            }
            else
            {
                await DisplayAlert("Erro!", "Não foi possível atualizar o filme! Por - favor, tente novamente.", "Entendi!");
            }
        }

        //função para importar imagem
        private async void ImportarImagem(object sender, EventArgs e)
        {
            //ve se existe um formato suportado
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Erro!", "Nenhuma foto disponível!", "OK");
                return;
            }
            else
            {
                //entra no cacho
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                //verifica se tem permissao para acessar o armazenamento, senao tiver, pede a permissao
                if (storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    storageStatus = results[Permission.Storage];
                }

                if (storageStatus == PermissionStatus.Granted)
                {
                    //abre a galeria pra guardar a foto na variavel file
                    var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();

                    try
                    {
                        //atribui a imagem para o XAML
                        imgSelecionada.Source = file.Path;
                        imgSelecionada.Source = "cameraok.png";
                        //atribui a imagem caminho imagem pra uma variavel global
                        caminhoimagem = file.Path;
                    }
                    catch (Exception)
                    {
                        imgSelecionada.Source = "photo.png";
                        DisplayAlert("Aviso!", "Nenhuma imagem foi selecionada!", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Permissão Negada", "Não foi possível acessar as fotos", "OK");
                }
            }
        }
    }
}