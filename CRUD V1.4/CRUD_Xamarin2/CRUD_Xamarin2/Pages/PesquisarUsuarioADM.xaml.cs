using CRUD_Xamarin2.CRUD;
using CRUD_Xamarin2.Funcionalidades;
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

namespace CRUD_Xamarin2.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PesquisarUsuarioADM : PopupPage
    {
        ModeloBanco banco = new ModeloBanco();
        bool _noturno;
		public PesquisarUsuarioADM (bool noturno)
		{
			InitializeComponent ();
            _noturno = noturno;
		}

        private void pesquisarUsuarioParaAlterar(object sender, EventArgs e)
        {
            Usuario user = new Usuario();
            user.Cpf = lbl_pesquisar.Text;
            var listaComUsuario = banco.GetUsuarioSemSenha(user);
            if (user.Cpf == "000.000.000-00")
            {
                DisplayAlert("Erro", "Você não pode alterar o ADMNISTRADOR PADRÃO!", "Entendi");
            }
            else if(listaComUsuario.Count < 1)
            {
                DisplayAlert("Aviso!", "Usuário não encontrado!", "Ok");
            }
            else
            {
                Criptografia criptar = new Criptografia();
                user.Nome = listaComUsuario[0].Nome;
                user.Senha = criptar.Decrypt(listaComUsuario[0].Senha);
                user.Adm = false;
                chamarAlterarRegistros(user);
            }
        }

        private async void chamarAlterarRegistros(Usuario user)
        {
            PopupNavigation.RemovePageAsync(this);
            await Navigation.PushModalAsync(new AlterarUsuario(user, _noturno));
        }
    }
}