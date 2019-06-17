using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CRUD_Xamarin2.Administrador;
using CRUD_Xamarin2.ModeloUsuarios;
using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.Pages;

namespace CRUD_Xamarin2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Configuracoes : ContentPage
    {
        //chama as variaveis auxiliares
        Usuario _user;
        PadraoADM _adm;
        ModeloBanco banco = new ModeloBanco();
        bool _noturno;
        public Configuracoes(Usuario usuario, PadraoADM adm, bool noturno)
        {
            InitializeComponent();
            _user = usuario;
            _adm = adm;
            _noturno = noturno;

            //verifica se esta no modo noturno
            if (noturno)
            {
                abs1.BackgroundColor = Color.White;
                btn1.BackgroundColor = Color.DimGray;
                btn2.BackgroundColor = Color.DimGray;
            }

            //verifica se pode ou nao habilitar o botao de admnistrador
            if (!_adm.Adm)
            {
                btn_adm.IsVisible = false;
                btn_adm.IsEnabled = false;
            }
            else
            {
                btn_adm.IsVisible = true;
                btn_adm.IsEnabled = true;
            }
        }

        //funçao para chamar os previlegios de adm
        private void ChamarPrivilegios(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PaginaAdmnistrador(_noturno));
        }

        //funçao que ativa o modo noturno
        private async void AtivarModoNoturno(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Exibir(_adm, _user, true));
            DisplayAlert("Concluído", "Modo noturno ativado!", "Ok");
        }

        //funçao que ativa o modo normal
        private void AtivarModoNormal(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new Exibir(_adm, _user, false));
            DisplayAlert("Concluído", "Modo normal ativado!", "Ok");
        }

        //função para sair da conta e desativar o salvamento de senha
        private async void SairDaConta(object sender, EventArgs e)
        {
            //usuario auxiliar
            Usuario auxiliar = new Usuario();
            var voltar = await DisplayAlert("Voltar", "Deseja mesmo voltar para a tela inicial?", "Ok", "Cancelar");
            //define o cpf da conta salva como o do adm raiz (O QUE NAO PODE SER SALVO)
            auxiliar.Cpf = "000.000.000-00";
            //altera os dados no banco
            var sucesso = banco.AlterarSenhaSalva(auxiliar, false);

            //se tudo ocorrer bem volta pra pagina inicial de login/cadastro
            if (voltar||sucesso)
            {
                App.Current.MainPage = new NavigationPage(new Preview());
                Navigation.PushModalAsync(new Preview());
            }
        }

        //chama a pagina para alterar os dados pessoais
        private async void AlterarDados(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AlterarUsuario(_user,_noturno));
        }
    }
}