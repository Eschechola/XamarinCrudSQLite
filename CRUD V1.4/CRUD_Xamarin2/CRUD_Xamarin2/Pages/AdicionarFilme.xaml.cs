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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRUD_Xamarin2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AdicionarFilme : PopupPage
	{
        //variaveis e objetos auxiliares
        Usuario _user;
        Exibir exibir;
        bool _adm;
        string caminhoImagem = "";
        bool _noturno;
		public AdicionarFilme (Usuario user, bool adm, bool noturno)
		{
			InitializeComponent ();
            _user = user;
            _adm = adm;
            _noturno = noturno;

            //Preferencia de Usuario em Modo Noturno
            if (noturno)
            {
                lbl_nome.TextColor = Color.Black;
                lbl_genero.TextColor = Color.Black;
                lbl_ano.TextColor = Color.Black;
                btn1.BackgroundColor = Color.Gray;
                btn1.TextColor = Color.White;
                stk1.BackgroundColor = Color.White;
                imgSelecionada.Source = "photo_black.png";
            }
		}

        //Pegar Imagem da Galeria
        private async void importarImagem(object sender, EventArgs e)
        {
            //verifica se tem foto com formatos compativeis
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Erro!", "Nenhuma foto disponível!", "OK");
                return;
            }
            else
            {
                //variavel pra pegar o endereço do armazenamento
                var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                //verifica se o app tem permissao de acessar o armazenamento
                if (storageStatus != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    storageStatus = results[Permission.Storage];
                }

                //se  tiver permissao
                if (storageStatus == PermissionStatus.Granted)
                {
                    //abre a galeria para pegar um foto
                    var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();

                    try
                    {
                        //mexe no layout XAML
                        imgSelecionada.Source = file.Path;
                        imgSelecionada.Source = "cameraok.png";
                        //atribui uma variavel que vai pegar o caminho da imagem
                        caminhoImagem = file.Path;
                    }
                    catch (Exception)
                    {
                        //modo noturno XAML
                        if (!_noturno)
                        {
                            imgSelecionada.Source = "photo.png";
                        }
                        else
                        {
                            imgSelecionada.Source = "photo_black.png";
                        }
                        
                        DisplayAlert("Aviso!", "Nenhuma imagem foi selecionada!", "Ok");
                    }


                }
                else
                {
                    await DisplayAlert("Permissão Negada", "Não foi possível acessar as fotos", "OK");
                }
            }
        }

        //Deixar as Variaveis do ENTRY sem Conteudo
        public void limparCampos()
        {
            lbl_nome.Text = String.Empty;
            lbl_ano.Text = String.Empty;
            lbl_genero.Text = String.Empty;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        //Realizar Cadastro de um filme
        private async void cadastrarFilme(object sender, EventArgs e)
        {
            //Verificação, Caso tenha algum espaço em branco das Entry´s
            if (string.IsNullOrWhiteSpace(lbl_nome.Text))
            {
                DisplayAlert("Aviso!", "Insira um nome para o filme!", "Ok");
            }
            else if (string.IsNullOrWhiteSpace(lbl_ano.Text)|| Int32.Parse(lbl_ano.Text) < 1895)
            {
                DisplayAlert("Aviso!", "Insira um ano válido para o filme!", "Ok");
            }
            else if (string.IsNullOrWhiteSpace(lbl_genero.Text))
            {
                DisplayAlert("Aviso!", "Insira um genero para o filme!", "Ok");
            }
            else
            {
                //Caso Contrario a Inserção ocorrerá normalmente
                ModeloBanco banco = new ModeloBanco();
                Filme filme = new Filme();
                filme.Cpf = _user.Cpf;
                filme.Nome = lbl_nome.Text;
                filme.Genero = lbl_genero.Text;
                filme.Ano = lbl_ano.Text;
                filme.Cor = "White";

                if (string.IsNullOrEmpty(caminhoImagem))
                {
                    filme.Imagem = "none.png";
                }
                else
                {
                    filme.Imagem = caminhoImagem;
                }
                
                var sucesso = banco.InserirFilme(filme);
                if (sucesso)
                {
                    PadraoADM admAux = new PadraoADM();
                    admAux.Adm = _adm;
                    DisplayAlert("Parabéns", "\""+filme.Nome + "\" foi inserido com sucesso, atualize sua lista de filmes", "Ok");
                    App.Current.MainPage = new NavigationPage(new Exibir(admAux, _user, _noturno));
                    await PopupNavigation.PopAllAsync();
                }
                else
                {
                    DisplayAlert("Erro!", "Não foi possível inserir o filme, por - favor, entre em contato com os desenvolvedores", "Ok");
                }
            }
        }
    }
}