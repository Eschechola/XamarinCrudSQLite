using CRUD_Xamarin2.Administrador;
using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.ModeloFilme;
using CRUD_Xamarin2.ModeloUsuarios;
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
	public partial class Excluir : PopupPage
	{
        //variaveis e objetos auxiliares
        ModeloBanco banco = new ModeloBanco();
        Usuario _user;
        PadraoADM admAux;
        bool _noturno = false;

        public Excluir (Usuario user, PadraoADM adms, bool noturno)
		{
			InitializeComponent ();
            _user = user;
            admAux = adms;
            _noturno = noturno;
            //modo noturno XAML
            if (noturno)
            {
                lbl_deletar.TextColor = Color.Black;
                stk_1.BackgroundColor = Color.White;
                lbl_deletar.PlaceholderColor = Color.Gray;
                btn_1.BackgroundColor = Color.Gray;
            }

		}

        //funçao para limpar campos
        public void limparCampos()
        {
            lbl_deletar.Text = String.Empty;
        }

        //funçao para excluir o filme
        private async void excluirFilme(object sender, EventArgs e)
        {
            //filme auxiliar
            Filme filme = new Filme();
            //pega o nome do filme
            filme.Nome = lbl_deletar.Text;
            //tenta deletar o filme do banco
            var sucesso = banco.DeletarFilme(filme,_user);
            //se conseguir exibe uma mensagem e volta atualiza a lista
            if (sucesso)
            {
                await DisplayAlert("Parabéns", "\""+filme.Nome+"\" foi deletado com sucesso. Atualize a lista de filmes!", "Entendi!");
                App.Current.MainPage = new NavigationPage(new Exibir(admAux, _user, _noturno));
                await PopupNavigation.PopAllAsync();
            }
            else
            {
                DisplayAlert("Erro!", "Filme não encontrado!", "Ok");
            }
        }
    }
}